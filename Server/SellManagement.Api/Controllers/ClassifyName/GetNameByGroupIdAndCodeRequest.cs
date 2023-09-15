using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.ClassifyName
{
    public class GetNameByGroupIdAndCodeRequest
    {
        public int GroupId { get; set; }
        public int Code { get; set; }

    }
}
