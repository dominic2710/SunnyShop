using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IOverviewFunction
    {
        Task<Overview> GetOverviewInfo();
    }
}
