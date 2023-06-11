using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.SosContacts
{
    public class SosContactsApi : BaseRestJsonApi, ISosContactsApi
    {
        public SosContactsApi(ISettingsService settingsService) : base(settingsService)
        {
        }

        public async Task<ApiResponse<List<SosContact>>> GetSosContacts()
        {
            return await GetAsync<List<SosContact>>(ApiRoutes.SOS_CONTACTS);
        }
        public async Task<ApiResponse<SosContact>> AddSosContact(SosContact sosContact)
        {
            var input = new
            {
                name = sosContact.Name,
                phone = sosContact.Phone
            };
            return await PostAsync<SosContact>(ApiRoutes.SOS_CONTACTS, input);
        }
        public async Task<ApiResponse<SosContact>> UpdateSosContact(SosContact sosContact)
        {
            var input = new
            {
                id = sosContact.Id,
                name = sosContact.Name,
                phone = sosContact.Phone
            };
            return await PutAsync<SosContact>(ApiRoutes.SOS_CONTACTS, input);
        }
        public async Task<ApiResponse<SosContact>> DeleteSosContact(int id)
        {
            return await DeleteAsync<SosContact>(ApiRoutes.SOS_CONTACTS + "/" + id);
        }
    }
}
