using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.ClassifyName
{
    public class GetListClassifyNameByGroupIdResponse
    {
        public IEnumerable<Functions.ClassifyName> ListClassifyName { get; set; }
    }
}
