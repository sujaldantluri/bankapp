using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using MyBankApp.Data;
using MyBankApp.Models;
namespace MyBankApp.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        try
        {
            _logger.LogDebug("Authenticating user: {Username}", username);
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                return null;

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication.");
            throw;
        }
    }

    public async Task<User> Register(string username, string password)
    {
        try
        {
            _logger.LogDebug("Registering user: {Username}", username);
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return null;

            var user = new User { Username = username, PasswordHash = CreatePasswordHash(password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration.");
            throw;
        }
    }

    private string CreatePasswordHash(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        using (var hmac = new HMACSHA512())
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash) == storedHash;
        }
    }
}
