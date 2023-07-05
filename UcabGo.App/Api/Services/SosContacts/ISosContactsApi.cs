using UcabGo.App.Api.Tools;
using UcabGo.App.Models;

namespace UcabGo.App.Api.Services.SosContacts
{
    public interface ISosContactsApi
    {
        Task<ApiResponse<IEnumerable<SosContact>>> GetSosContacts();
        Task<ApiResponse<SosContact>> AddSosContact(SosContact sosContact);
        Task<ApiResponse<SosContact>> UpdateSosContact(SosContact sosContact);
        Task<ApiResponse<SosContact>> DeleteSosContact(int id);
    }
}
