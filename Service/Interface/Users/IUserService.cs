using POSbackend.DTO.User;

namespace POSbackend.Service.Interface.Users
{
    public interface IUserService
    {
        Task<UserDto?> LoginAsync(loginDto login);
        Task<UserDto?> SignUpAsync(SignUpDto signUp);
        Task<UserDto?> ResetPasswordAsync(string email, string newPassword, string username);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);
    }
}
