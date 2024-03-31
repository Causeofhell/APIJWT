using Application.Common.Interfaces;
using Application.Contracts;
using Application.DTOs.User;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class UserRepository(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator) : IUser
    {
        private readonly AppDbContext _dbContext = context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        public async Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO)
        {
            var getUser = await FindUserByEmail(loginDTO.Email!);
            if (getUser is null) return new LoginResponse(false, "User not found.");

            var checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successfully", _jwtTokenGenerator.GenerateToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");
        }

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var getUser = await FindUserByEmail(registerUserDTO.Email!);
            if (getUser is not null)
                return new RegistrationResponse(false, "User alredy exist.");

            _dbContext.Users.Add(new User()
            {
                Name = registerUserDTO.Name,
                Email = registerUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
            });

            await _dbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration complete.");
        }

        private async Task<User> FindUserByEmail(string email) =>
            await this._dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}
