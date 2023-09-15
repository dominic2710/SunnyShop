using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.Overview
{
    public class GetOverviewInfoResponse:BaseResponse
    {
        public Models.Overview OverviewData { get; set; }
    }
}
