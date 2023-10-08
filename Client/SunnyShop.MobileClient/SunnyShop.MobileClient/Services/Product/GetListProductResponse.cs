using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Services.Product
{
    public class GetListProductResponse:BaseResponse
    {
        public IEnumerable<Models.Product> ListProduct { get; set; }
    }
}
