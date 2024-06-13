using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MyBankApp.Models;
using MyBankApp.Services;

namespace MyBankApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", model.Username);

                var user = await _userService.Authenticate(model.Username, model.Password);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for user: {Username}", model.Username);
                    return Unauthorized(new { message = "Username or password is incorrect" });
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });

                _logger.LogInformation("Login successful for user: {Username}", model.Username);
                return Ok(new { user.Id, user.Username });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for user: {Username}", model.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                _logger.LogInformation("Registration attempt for user: {Username}", model.Username);

                var user = await _userService.Register(model.Username, model.Password);
                if (user == null)
                {
                    _logger.LogWarning("Registration failed for user: {Username}. User already exists.");
                    return BadRequest("User already exists");
                }

                _logger.LogInformation("Registration successful for user: {Username}", model.Username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for user: {Username}", model.Username);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("account")]
        public async Task<IActionResult> GetAccount()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    _logger.LogWarning("GetAccount failed: User not logged in.");
                    return Unauthorized(new { message = "User not logged in" });
                }

                var user = await _userService.GetByIdAsync(userId.Value);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return NotFound(new { message = "User not found" });
                }

                var result = new
                {
                    user.Username,
                    user.Checking,
                    user.Savings
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching account data.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("account/update")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountModel model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    _logger.LogWarning("UpdateAccount failed: User not logged in.");
                    return Unauthorized(new { message = "User not logged in" });
                }

                var user = await _userService.GetByIdAsync(userId.Value);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return NotFound(new { message = "User not found" });
                }

                user.Checking = model.Checking;
                user.Savings = model.Savings;
                await _userService.UpdateUserAsync(user);

                return Ok(new { message = "Account updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating account data.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UpdateAccountModel
    {
        public decimal Checking { get; set; }
        public decimal Savings { get; set; }
    }
}
