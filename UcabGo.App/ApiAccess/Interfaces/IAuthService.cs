using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.ApiAccess.Models;

namespace UcabGo.App.ApiAccess.Interfaces
{
    public interface IAuthService
    {
        Task<Login> LoginAsync(string email, string password);
    }
}
