using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.ShippingCompany
{
    public class GetShippingCompanyByCdResponse:BaseResponse
    {
        public Models.ShippingCompany ShippingCompanyData { get; set; }
    }
}
