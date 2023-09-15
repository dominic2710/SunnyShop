using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class VoucherNoManagement
    {
        public int Id { get; set; }
        public int CategoryCd { get; set; }
        public string CategoryName { get; set; }
        public int VoucherNo { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
