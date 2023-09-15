using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.ClassifyName
{
    public class GetListClassifyNameByGroupIdResponse : BaseResponse
    {
        public IEnumerable<Models.ClassifyName> ListClassifyName { get; set; }
    }
}
