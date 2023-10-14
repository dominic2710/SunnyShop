using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Models
{
    public class Inventory
    {
        public string InventoryNo { get; set; }
        public DateTime InventoryDate { get; set; }
        public string Note { get; set; }
        public int ProductCount { get; set; }
    }
}
