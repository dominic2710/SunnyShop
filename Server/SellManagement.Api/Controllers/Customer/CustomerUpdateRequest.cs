using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.Customer
{
    public class CustomerUpdateRequest
    {
        public Functions.Customer CustomerData { get; set; }
    }
}
