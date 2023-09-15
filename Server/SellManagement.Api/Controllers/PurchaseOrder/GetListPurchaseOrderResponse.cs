using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.PurchaseOrder
{
    public class GetListPurchaseOrderResponse
    {
        public IEnumerable<Functions.PurchaseOrder> ListPurchaseOrder { get; set; }
    }
}
