using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using SellManagement.Api.Controllers.PurchaseOrder;
using SellManagement.Api.Messages;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PurchaseOrderController : Controller
    {
        IPurchaseOrderFunction _purchaseOrderFunction;
        IClassifyNameFunction _classifyNameFunction;
        public PurchaseOrderController(IPurchaseOrderFunction purchaseOrderFunction, IClassifyNameFunction classifyNameFunction)
        {
            _purchaseOrderFunction = purchaseOrderFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetPurchaseOrderByNo")]
        [Authorize]
        public async Task<IActionResult> GetPurchaseOrderByNo([FromBody] string purchaseOrderNo)
        {
            var response = new GetPurchaseOrderByCdResponse
            {
                PurchaseOrderData = await _purchaseOrderFunction.GetPurchaseOrderByNo(purchaseOrderNo)
            };
            return Ok(response);
        }

        [HttpPost("GetListPurchaseOrder")]
        [Authorize]
        public async Task<IActionResult> GetListPurchaseOrder()
        {
            var response = new GetListPurchaseOrderResponse
            {
                ListPurchaseOrder = await _purchaseOrderFunction.GetListPurchaseOrder(),
            };
            return Ok(response);
        }

        [HttpPost("AddPurchaseOrder")]
        [Authorize]
        public async Task<IActionResult> AddPurchaseOrder([FromBody] PurchaseOrderAddRequest request)
        {
            var UserName = User.Identity.Name;

            var response = new PurchaseOrderAddResponse
            {
                PurchaseOrderData = await _purchaseOrderFunction.AddPurchaseOrder(request.PurchaseOrderData)
            };
            return Ok(response);
        }

        [HttpPost("UpdatePurchaseOrder")]
        [Authorize]
        public async Task<IActionResult> UpdatePurchaseOrder([FromBody] PurchaseOrderUpdateRequest request)
        {
            var response = new PurchaseOrderUpdateResponse
            {
                UpdRecCount = await _purchaseOrderFunction.UpdatePurchaseOrder(request.PurchaseOrderData),
            };
            return Ok(response);
        }

        [HttpPost("UpdatePurchaseOrderStatus")]
        [Authorize]
        public async Task<IActionResult> UpdatePurchaseOrderStatus([FromBody] PurchaseOrderUpdateStatusRequest request)
        {
            var response = new PurchaseOrderUpdateStatusResponse
            {
                UpdRecCount = await _purchaseOrderFunction.UpdatePurchaseOrderStatus(request.PurchaseOrderNo, request.Status),
            };
            return Ok(response);
        }

        [HttpPost("DeletePurchaseOrder")]
        [Authorize]
        public async Task<IActionResult> DeletePurchaseOrder([FromBody] string purchaseOrderNo)
        {
            var response = new PurchaseOrderDeleteResponse
            {
                DelRecCount = await _purchaseOrderFunction.DeletePurchaseOrder(purchaseOrderNo)
            };
            return Ok(response);
        }
    }
}
