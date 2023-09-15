using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Supplier
{
    public class GetListSupplierResponse:BaseResponse
    {
        public IEnumerable<Models.Supplier> ListSupplier { get; set; }
        public IEnumerable<Models.ClassifyName> ListCategory { get; set; }
    }
}
