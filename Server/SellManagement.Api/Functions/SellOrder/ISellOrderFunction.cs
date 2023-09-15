using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface ISellOrderFunction
    {
        Task<SellOrder> GetSellOrderByNo(string SellOrderNo);
        Task<IEnumerable<SellOrder>> GetListSellOrder();
        Task<SellOrder> AddSellOrder(SellOrder SellOrder);
        Task<int> UpdateSellOrder(SellOrder SellOrder);
        Task<int> UpdateSellOrderStatus(string SellOrderNo, int status);
        Task<int> DeleteSellOrder(string SellOrderNo);
    }
}
