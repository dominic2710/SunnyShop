using System;
using System.Collections.Generic;

namespace SellManagement.Api.Functions
{
    public class Inventory
    {
        public int Id { get; set; }
        public string InventoryNo { get; set; }
        public DateTime InventoryDate { get; set; }
        public string Note { get; set; }
        public IEnumerable<InventoryDetail> InventoryDetails { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
