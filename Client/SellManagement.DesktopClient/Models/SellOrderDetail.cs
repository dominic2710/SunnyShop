using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Models
{
    public class SellOrderDetail:BaseModel
    {
        public int Id { get; set; }
        public string SellOrderNo { get; set; }
        public string ProductCd { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        private int costPrice;
        public int CostPrice
        {
            get { return costPrice; }
            set 
            { 
                costPrice = value;
                Cost = Quantity * costPrice;
                OnPropertyChanged();
            }
        }

        private int cost;
        public int Cost
        {
            get { return cost; }
            set { cost = value; OnPropertyChanged(); }
        }

        public string Note { get; set; }

    }
}
