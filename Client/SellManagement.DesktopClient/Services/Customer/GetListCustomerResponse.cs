using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Customer
{
    public class GetListCustomerResponse : BaseResponse
    {
        public IEnumerable<Models.Customer> ListCustomer { get; set; }
        public IEnumerable<Models.ClassifyName> ListCategory { get; set; }

    }
}
