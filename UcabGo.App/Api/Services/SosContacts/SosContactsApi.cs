using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.SosContacts
{
    public class SosContactsApi : BaseRestJsonApi, ISosContactsApi
    { 
        public SosContactsApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<List<SosContact>>> GetSosContacts()
        {
            return await GetAsync<List<SosContact>>(ApiRoutes.SOS_CONTACTS);
        }
        public async Task<ApiResponse<SosContact>> AddSosContact(SosContact sosContact)
        {
            return await PostAsync<SosContact>(ApiRoutes.SOS_CONTACTS, sosContact);
        }
        public async Task<ApiResponse<SosContact>> UpdateSosContact(SosContact sosContact)
        {
            return await PutAsync<SosContact>(ApiRoutes.SOS_CONTACTS, sosContact);
        }
        public async Task<ApiResponse<SosContact>> DeleteSosContact(int id)
        {
            return await DeleteAsync<SosContact>(ApiRoutes.SOS_CONTACTS + "/" + id);
        }
    }
}
