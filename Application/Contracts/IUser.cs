using Application.DTOs.User;

namespace Application.Contracts
{
    public interface IUser
    {
        Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO);

        Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO);
    }
}
