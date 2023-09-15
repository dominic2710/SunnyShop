using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.Customer
{
    public class GetListCustomerResponse
    {
        public IEnumerable<Functions.Customer> ListCustomer { get; set; }
        public IEnumerable<Functions.ClassifyName> ListCategory { get; set; }
    }
}
