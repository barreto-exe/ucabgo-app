using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UcabGo.App.ApiAccess.Tools
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
