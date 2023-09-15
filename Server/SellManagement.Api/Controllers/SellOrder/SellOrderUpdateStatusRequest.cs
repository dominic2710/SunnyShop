using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.SellOrder
{
    public class SellOrderUpdateStatusRequest
    {
        public string SellOrderNo { get; set; }
        public int Status { get; set; }
    }
}
