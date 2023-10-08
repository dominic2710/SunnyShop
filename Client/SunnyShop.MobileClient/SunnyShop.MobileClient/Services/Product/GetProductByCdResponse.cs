using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Services.Product
{
    public class GetProductByCdResponse : BaseResponse
    {
        public Models.Product ProductData { get; set; }
    }
}
