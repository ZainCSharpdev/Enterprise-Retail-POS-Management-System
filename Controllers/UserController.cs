using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.User;
using POSbackend.Service.Interface.Users;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService _userService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(new {user.UserName});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginDto login)
        {
            var user = await _userService.LoginAsync(login);
            if (user == null) return Unauthorized("Invalid credentials");
            return Ok(user);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUp)
        {
            var user = await _userService.SignUpAsync(signUp);
            if (user == null) return BadRequest("User already exists");
            return Ok(user);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string username, string newPassword)
        {
            var user = await _userService.ResetPasswordAsync(email, newPassword, username);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

