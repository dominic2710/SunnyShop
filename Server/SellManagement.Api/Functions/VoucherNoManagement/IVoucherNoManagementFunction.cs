using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SellManagement.Api.Const;

namespace SellManagement.Api.Functions
{
    public interface IVoucherNoManagementFunction
    {
        Task<string> GetVoucherNo(int categoryCd, bool updateFlg);
    }
}
