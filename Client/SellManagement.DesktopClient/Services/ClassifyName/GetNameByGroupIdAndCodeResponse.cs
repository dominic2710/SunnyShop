using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.ClassifyName
{
    public class GetNameByGroupIdAndCodeResponse: BaseResponse
    {
        public Models.ClassifyName ClassifyNameData { get; set; }
    }
}
