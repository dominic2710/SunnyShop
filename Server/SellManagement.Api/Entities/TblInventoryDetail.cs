using System;

namespace SellManagement.Api.Entities
{
    public class TblInventoryDetail
    {
        public int Id { get; set; }
        public string InventoryNo { get; set; }
        public string ProductCd { get; set; }
        public DateTime ExpiryDate { get; set;} 
        public int Quantity { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
