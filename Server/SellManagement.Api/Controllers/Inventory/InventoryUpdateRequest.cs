﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.Inventory
{
    public class InventoryUpdateRequest
    {
        public Functions.Inventory InventoryData { get; set; }
    }
}