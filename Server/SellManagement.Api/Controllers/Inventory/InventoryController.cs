using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Controllers.Inventory;
using SellManagement.Api.Functions;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        IInventoryFunction _InventoryFunction;
        IClassifyNameFunction _classifyNameFunction;
        public InventoryController(IInventoryFunction InventoryFunction, IClassifyNameFunction classifyNameFunction)
        {
            _InventoryFunction = InventoryFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetInventoryByNo")]
        [Authorize]
        public async Task<IActionResult> GetInventoryByNo([FromBody] string inventoryNo)
        {
            var response = new GetInventoryByNoResponse
            {
                InventoryData = await _InventoryFunction.GetInventoryByNo(inventoryNo)
            };
            return Ok(response);
        }

        [HttpPost("GetListInventory")]
        [Authorize]
        public async Task<IActionResult> GetListInventory()
        {
            var response = new GetListInventoryResponse
            {
                ListInventory = await _InventoryFunction.GetListInventory(),
            };
            return Ok(response);
        }

        [HttpPost("AddInventory")]
        [Authorize]
        public async Task<IActionResult> AddInventory([FromBody] InventoryAddRequest request)
        {
            var UserName = User.Identity.Name;

            var response = new InventoryAddResponse
            {
                InventoryData = await _InventoryFunction.AddInventory(request.InventoryData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateInventory")]
        [Authorize]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryUpdateRequest request)
        {
            var response = new InventoryUpdateResponse
            {
                UpdRecCount = await _InventoryFunction.UpdateInventory(request.InventoryData),
            };
            return Ok(response);
        }

        //[HttpPost("UpdateInventoryStatus")]
        //[Authorize]
        //public async Task<IActionResult> UpdateInventoryStatus([FromBody] InventoryUpdateStatusRequest request)
        //{
        //    var response = new InventoryUpdateStatusResponse
        //    {
        //        UpdRecCount = await _InventoryFunction.UpdateInventoryStatus(request.InventoryNo, request.Status),
        //    };
        //    return Ok(response);
        //}

        [HttpPost("DeleteInventory")]
        [Authorize]
        public async Task<IActionResult> DeleteInventory([FromBody] string inventoryNo)
        {
            var response = new InventoryDeleteResponse
            {
                DelRecCount = await _InventoryFunction.DeleteInventory(inventoryNo)
            };
            return Ok(response);
        }
    }
}
