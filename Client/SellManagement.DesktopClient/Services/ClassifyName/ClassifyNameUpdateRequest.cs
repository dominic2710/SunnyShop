using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.Services.ClassifyName
{
    public class ClassifyNameUpdateRequest
    {
        public IEnumerable<Models.ClassifyName> ClassifyNameData { get; set; }
    }
}
