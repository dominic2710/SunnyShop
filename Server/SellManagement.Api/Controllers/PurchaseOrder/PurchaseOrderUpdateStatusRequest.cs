using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.PurchaseOrder
{
    public class PurchaseOrderUpdateStatusRequest
    {
        public string PurchaseOrderNo { get; set; }
        public int Status { get; set; }
    }
}
