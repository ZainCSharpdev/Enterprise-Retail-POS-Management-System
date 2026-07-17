using POSbackend.DTO.User;

namespace POSbackend.Repository.Interface.Users
{
    public interface IUserRepo
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> LoginAsync(loginDto login);
        Task<UserDto?> SignUpAsync(SignUpDto signUp);
        Task<UserDto?> ResetPasswordAsync(string email, string newPassword, string Username);
        Task<bool> DeleteUserAsync(int userId);
    }
}
