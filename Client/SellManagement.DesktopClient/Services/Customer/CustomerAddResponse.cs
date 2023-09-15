using SellManagement.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Customer
{
    public class CustomerAddResponse : BaseResponse
    {
        public Models.Customer CustomerData { get; set; }
    }
}
