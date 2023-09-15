using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Models
{
    public class PurchaseOrder:BaseModel
    {
        public int Id { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public DateTime PlannedImportDate { get; set; }
        public string SupplierCd { get; set; }
        public string SupplierName { get; set; }
        public int Status { get; set; }
        public int SummaryCost { get; set; }
        public int SaleOffCost { get; set; }
        public int PaidCost { get; set; }
        public int PurchaseCost { get; set; }
        public string Note { get; set; }
        public IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
