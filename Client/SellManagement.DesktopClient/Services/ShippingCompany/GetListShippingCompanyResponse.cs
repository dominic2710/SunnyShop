using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.ShippingCompany
{
    public class GetListShippingCompanyResponse:BaseResponse
    {
        public IEnumerable<Models.ShippingCompany> ListShippingCompany { get; set; }
        public IEnumerable<Models.ClassifyName> ListCategory { get; set; }
    }
}
