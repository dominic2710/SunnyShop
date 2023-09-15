using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblSupplier
    {
        public int Id { get; set; }
        public string SupplierCd { get; set; }
        public string Name { get; set; }
        public int CategoryCd { get; set;}
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Note { get; set; }
    }
}
