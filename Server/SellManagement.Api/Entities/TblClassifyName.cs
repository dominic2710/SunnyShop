using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblClassifyName
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int Code { get; set; }
        public string Name { get; set;}
    }
}
