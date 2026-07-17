using POSbackend.DTO.User;
using POSbackend.Models;
using Microsoft.EntityFrameworkCore;
using POSbackend.Repository.Interface.Users;
using POSbackend.Security;

namespace POSbackend.Repository.implement.Users
{
    public class UserRepo(PosdbContext _context) : IUserRepo
    {
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                Email = user.Email,
                Role = user.Role,
                LastLogin = user.LastLogin
            };
        }
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                Email = user.Email,
                Role = user.Role,
                LastLogin = user.LastLogin
            };
        }

        public async Task<UserDto?> LoginAsync(loginDto login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == login.UserName);
            if (user == null) return null;
            if (!PasswordHelper.VerifyPassword(login.Password, user.Password)) return null;

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                Email = user.Email,
                Role = user.Role,
                LastLogin = user.LastLogin
            };
        }

        public async Task<UserDto?> SignUpAsync(SignUpDto signUp)
        {
            var existingUser = await _context.Users
                .AnyAsync(u => u.Username == signUp.UserName || u.Email == signUp.Email);
            if (existingUser) return null;

            var Newuser = new User
            {
                Username = signUp.UserName,
                Email = signUp.Email,
                Password = PasswordHelper.HashPassword(signUp.Password),
                Role = signUp.Role,
                LastLogin = DateTime.Now
            };
            _context.Users.Add(Newuser);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserId = Newuser.UserId,
                UserName = Newuser.Username,
                Email = Newuser.Email,
                Role = Newuser.Role,
                LastLogin = Newuser.LastLogin
            };
        }

        public async Task<UserDto?> ResetPasswordAsync(string email, string newPassword, string Username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Username == Username);
            if (user == null) return null;
            user.Password = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                Email = user.Email,
                Role = user.Role,
                LastLogin = user.LastLogin
            };
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
