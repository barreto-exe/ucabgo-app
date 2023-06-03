using UcabGo.App.Api.Models;

namespace UcabGo.App.Api.Interfaces
{
    public interface IAuthApi
    {
        Task<Login> LoginAsync(string email, string password);
    }
}
