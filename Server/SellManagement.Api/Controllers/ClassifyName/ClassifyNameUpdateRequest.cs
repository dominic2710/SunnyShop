﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers.ClassifyName
{
    public class ClassifyNameUpdateRequest
    {
        public IEnumerable<Functions.ClassifyName> ClassifyNameData { get; set; }
    }
}
