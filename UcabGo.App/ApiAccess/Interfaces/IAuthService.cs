using UcabGo.App.ApiAccess.Models;

namespace UcabGo.App.ApiAccess.Interfaces
{
    public interface IAuthService
    {
        Task<Login> LoginAsync(string email, string password);
    }
}
