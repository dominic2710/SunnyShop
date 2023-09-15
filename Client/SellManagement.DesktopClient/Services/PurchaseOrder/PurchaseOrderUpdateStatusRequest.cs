using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.PurchaseOrder
{
    public class PurchaseOrderUpdateStatusRequest
    {
        public string PurchaseOrderNo { get; set; }
        public int Status { get; set; }
    }
}
