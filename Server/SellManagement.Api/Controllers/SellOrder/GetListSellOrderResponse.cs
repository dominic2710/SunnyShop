using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.SellOrder
{
    public class GetListSellOrderResponse
    {
        public IEnumerable<Functions.SellOrder> ListSellOrder { get; set; }
    }
}
