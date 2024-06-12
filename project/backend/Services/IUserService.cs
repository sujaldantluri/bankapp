
public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<User> Register(string username, string password);
}
