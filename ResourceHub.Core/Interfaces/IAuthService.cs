namespace ResourceHub.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string username, string email, string password);
        Task<string> LoginAsync(string username, string password);
    }
}