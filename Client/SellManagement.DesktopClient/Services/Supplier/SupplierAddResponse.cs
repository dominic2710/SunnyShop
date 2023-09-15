using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Supplier
{
    public class SupplierAddResponse:BaseResponse
    {
        public Models.Supplier SupplierData { get; set; }
    }
}
