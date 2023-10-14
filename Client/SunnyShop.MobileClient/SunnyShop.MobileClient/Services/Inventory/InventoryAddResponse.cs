using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Services.Inventory
{
    public class InventoryAddResponse : BaseResponse
    {
        public Inventory InventoryData { get; set; }
    }
}
