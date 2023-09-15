using SellManagement.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Product
{
    public class ProductAddResponse: BaseResponse
    {
        public Models.Product ProductData { get; set; }
    }
}
