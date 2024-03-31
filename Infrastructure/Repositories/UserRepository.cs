using Application.Contracts;
using Application.DTOs.User;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    internal class UserRepository(AppDbContext context, IConfiguration configuration) : IUser
    {
        private readonly IConfiguration configuration = configuration;
        private readonly AppDbContext _dbContext = context;

        public async Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO)
        {
            var getUser = await FindUserByEmail(loginDTO.Email!);
            if (getUser is null) return new LoginResponse(false, "User not found.");

            var checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginResponse(true, "Login successfully", GenerateJWTToken(getUser));
            else
                return new LoginResponse(false, "Invalid credentials");
        }

        private string GenerateJWTToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var getUser = await FindUserByEmail(registerUserDTO.Email!);
            if (getUser is not null)
                return new RegistrationResponse(false, "User alredy exist.");

            _dbContext.Users.Add(new ApplicationUser()
            {
                Name = registerUserDTO.Name,
                Email = registerUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
            });

            await _dbContext.SaveChangesAsync();
            return new RegistrationResponse(true, "Registration complete.");
        }

        private async Task<ApplicationUser> FindUserByEmail(string email) =>
            await this._dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}
