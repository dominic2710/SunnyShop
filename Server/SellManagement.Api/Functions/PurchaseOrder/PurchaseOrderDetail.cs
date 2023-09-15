using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ProductCd { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int CostPrice { get; set; }
        public int Cost { get; set; }
        public string Note { get; set; }
    }
}
