﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblShippingCompany
    {
        public int Id { get; set; }
        public string ShippingCompanyCd { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
