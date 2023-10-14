using System;

namespace SellManagement.Api.Functions
{
    public class InventoryDetail
    {
        public int Id { get; set; }
        public string InventoryNo { get; set; }
        public string ProductCd { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
