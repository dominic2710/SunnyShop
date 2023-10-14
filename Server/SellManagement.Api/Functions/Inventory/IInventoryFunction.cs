using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IInventoryFunction
    {
        Task<Inventory> GetInventoryByNo(string inventoryNo);
        Task<IEnumerable<Inventory>> GetListInventory();
        Task<Inventory> AddInventory(Inventory inventory);
        Task<int> UpdateInventory(Inventory inventory);
        //Task<int> UpdateInventoryStatus(string inventoryNo, int status);
        Task<int> DeleteInventory(string inventoryNo);
    }
}
