using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunnyShop.MobileClient.Models
{
    public class InventoryDetail:BaseModel
    {
        private int quantity;

        public string ProductCd { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string LotNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity 
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }
        public string PageNumber { get; set; }
        public bool IsEditable { get; set; }
        public bool IsDisplay { get; set; }
        public string SapoProductId { get; set; }
    }
}
