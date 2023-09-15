using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.SellOrder
{
    public class SellOrderUpdateStatusRequest
    {
        public string SellOrderNo { get; set; }
        public int Status { get; set; }
    }
}
