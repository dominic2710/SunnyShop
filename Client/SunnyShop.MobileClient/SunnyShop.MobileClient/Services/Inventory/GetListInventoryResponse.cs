using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SunnyShop.MobileClient.Services.Inventory
{
    public class GetListInventoryResponse: BaseResponse
    {
        public IEnumerable<Inventory> ListInventory { get; set; }
    }
}
