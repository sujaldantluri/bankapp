using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using MyBankApp.Data; // Adjust namespace as needed
using MyBankApp.Models; // Adjust namespace as needed
using Microsoft.Extensions.Logging;
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
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        if (user == null)
        {
            _logger.LogWarning($"User '{username}' not found.");
            return null;
        }

        _logger.LogInformation($"User '{username}' found. Verifying password.");

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning($"Password for user '{username}' does not match.");
            return null;
        }

        _logger.LogInformation($"Password for user '{username}' verified successfully.");
        return user;
    }

    public async Task<User> Register(string username, string password)
    {
        if (await _context.Users.AnyAsync(x => x.Username == username))
            return null;

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        var user = new User { Username = username, PasswordHash = Convert.ToBase64String(passwordHash), PasswordSalt = passwordSalt };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, string storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            _logger.LogInformation($"Computed hash: {Convert.ToBase64String(computedHash)}");
            _logger.LogInformation($"Stored hash: {storedHash}");
            return Convert.ToBase64String(computedHash) == storedHash;
        }
    }
}
