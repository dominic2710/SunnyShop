using SellManagement.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.SellOrder
{
    public class SellOrderAddResponse : BaseResponse
    {
        public Models.SellOrder SellOrderData { get; set; }
    }
}
