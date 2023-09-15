using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblPurchaseOrderHead
    {
        public int Id { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public DateTime PlannedImportDate { get; set; }
        public string SupplierCd { get; set; }
        public int Status { get; set; }
        public int SummaryCost { get; set; }
        public int SaleOffCost { get; set; }
        public int PaidCost { get; set; }
        public int PurchaseCost { get; set; }
        public string Note { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
