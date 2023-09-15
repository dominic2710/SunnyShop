using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Login
{
    public class AuthenticateResponse:BaseResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
    }
}
