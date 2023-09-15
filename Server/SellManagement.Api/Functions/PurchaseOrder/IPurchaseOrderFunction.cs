using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IPurchaseOrderFunction
    {
        Task<PurchaseOrder> GetPurchaseOrderByNo(string purchaseOrderNo);
        Task<IEnumerable<PurchaseOrder>> GetListPurchaseOrder();
        Task<PurchaseOrder> AddPurchaseOrder(PurchaseOrder purchaseOrder);
        Task<int> UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        Task<int> UpdatePurchaseOrderStatus(string purchaseOrderNo, int status);
        Task<int> DeletePurchaseOrder(string purchaseOrderNo);
    }
}
