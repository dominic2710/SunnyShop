using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Product
{
    public class GetListProductResponse: BaseResponse
    {
        public IEnumerable<Models.Product> ListProduct { get; set; }
        public IEnumerable<Models.ClassifyName> ListCategory { get; set; }
        public IEnumerable<Models.ClassifyName> ListTradeMark { get; set; }
        public IEnumerable<Models.ClassifyName> ListOrigin { get; set; }

    }
}
