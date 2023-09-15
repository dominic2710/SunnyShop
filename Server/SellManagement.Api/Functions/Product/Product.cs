using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCd { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int CategoryCd { get; set; }
        public string CategoryName { get; set; }
        public int TradeMarkCd { get; set; }
        public string TradeMarkName { get; set; }
        public int OriginCd { get; set; }
        public string OriginName { get; set; }
        public int Quantity { get; set; }
        public int PlannedInpStock { get; set; }
        public int PlannedOutStock { get; set; }
        public int InStock { get; set; }
        public int AvailabilityInStock { get; set; }
        public int CostPrice { get; set; }
        public int SoldPrice { get; set; }
        public string Detail { get; set; }
    }
}
