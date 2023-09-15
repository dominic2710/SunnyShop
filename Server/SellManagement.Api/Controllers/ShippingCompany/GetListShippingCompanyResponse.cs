using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.ShippingCompany
{
    public class GetListShippingCompanyResponse
    {
        public IEnumerable<Functions.ShippingCompany> ListShippingCompany { get; set; }
        public IEnumerable<Functions.ClassifyName> ListCategory { get; set; }
    }
}
