using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.Supplier
{
    public class GetListSupplierResponse
    {
        public IEnumerable<Functions.Supplier> ListSupplier { get; set; }
        public IEnumerable<Functions.ClassifyName> ListCategory { get; set; }
    }
}
