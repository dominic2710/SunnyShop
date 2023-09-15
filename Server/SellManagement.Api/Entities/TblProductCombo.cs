using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblProductCombo
    {
        public int Id { get; set; }
        public string ProductComboCd { get; set; }
        public string ProductCd { get; set; }
        public int Quatity { get; set; }
    }
}
