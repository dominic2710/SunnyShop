using SellManagement.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.PurchaseOrder
{
    public class PurchaseOrderAddResponse : BaseResponse
    {
        public Models.PurchaseOrder PurchaseOrderData { get; set; }
    }
}
