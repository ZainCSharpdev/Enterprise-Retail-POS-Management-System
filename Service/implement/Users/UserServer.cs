using POSbackend.DTO.User;
using POSbackend.Repository.Interface.Users;
using POSbackend.Service.Interface.Users;

namespace POSbackend.Service.implement.Users
{
    public class UserServer(IUserRepo _UserRepo) : IUserService
    {
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _UserRepo.DeleteUserAsync(userId);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            return await _UserRepo.GetUserByIdAsync(userId);
        }

        public async Task<UserDto?> LoginAsync(loginDto login)
        {
            return await _UserRepo.LoginAsync(login);
        }

        public async Task<UserDto?> ResetPasswordAsync(string email, string newPassword, string username)
        {
            return await _UserRepo.ResetPasswordAsync(email, newPassword, username);
        }

        public async Task<UserDto?> SignUpAsync(SignUpDto signUp)
        {
            return await _UserRepo.SignUpAsync(signUp);
        }
    }
}
