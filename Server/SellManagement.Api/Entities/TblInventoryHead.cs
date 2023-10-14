using System;

namespace SellManagement.Api.Entities
{
    public class TblInventoryHead
    {
        public int Id { get; set; }
        public string InventoryNo { get; set; }
        public DateTime InventoryDate { get; set; }
        public string Note { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
