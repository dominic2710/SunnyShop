using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.Product
{
    public class GetListProductResponse
    {
        public IEnumerable<Functions.Product> ListProduct { get; set; }
        public IEnumerable<Functions.ClassifyName> ListCategory { get; set; }
        public IEnumerable<Functions.ClassifyName> ListTradeMark { get; set; }
        public IEnumerable<Functions.ClassifyName> ListOrigin { get; set; }
    }
}
