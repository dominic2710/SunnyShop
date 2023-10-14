using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using SellManagement.Api.Controllers.SellOrder;
using SellManagement.Api.Messages;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SellOrderController : Controller
    {
        ISellOrderFunction _SellOrderFunction;
        IClassifyNameFunction _classifyNameFunction;
        public SellOrderController(ISellOrderFunction SellOrderFunction, IClassifyNameFunction classifyNameFunction)
        {
            _SellOrderFunction = SellOrderFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetSellOrderByNo")]
        [Authorize]
        public async Task<IActionResult> GetSellOrderByNo([FromBody] string sellOrderNo)
        {
            var response = new GetSellOrderByCdResponse
            {
                SellOrderData = await _SellOrderFunction.GetSellOrderByNo(sellOrderNo)
            };
            return Ok(response);
        }

        [HttpPost("GetListSellOrder")]
        [Authorize]
        public async Task<IActionResult> GetListSellOrder()
        {
            var response = new GetListSellOrderResponse
            {
                ListSellOrder = await _SellOrderFunction.GetListSellOrder(),
            };
            return Ok(response);
        }

        [HttpPost("AddSellOrder")]
        [Authorize]
        public async Task<IActionResult> AddSellOrder([FromBody] SellOrderAddRequest request)
        {
            var UserName = User.Identity.Name;

            var response = new SellOrderAddResponse
            {
                SellOrderData = await _SellOrderFunction.AddSellOrder(request.SellOrderData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateSellOrder")]
        [Authorize]
        public async Task<IActionResult> UpdateSellOrder([FromBody] SellOrderUpdateRequest request)
        {
            var response = new SellOrderUpdateResponse
            {
                UpdRecCount = await _SellOrderFunction.UpdateSellOrder(request.SellOrderData),
            };
            return Ok(response);
        }

        [HttpPost("UpdateSellOrderStatus")]
        [Authorize]
        public async Task<IActionResult> UpdateSellOrderStatus([FromBody] SellOrderUpdateStatusRequest request)
        {
            var response = new SellOrderUpdateStatusResponse
            {
                UpdRecCount = await _SellOrderFunction.UpdateSellOrderStatus(request.SellOrderNo, request.Status),
            };
            return Ok(response);
        }

        [HttpPost("DeleteSellOrder")]
        [Authorize]
        public async Task<IActionResult> DeleteSellOrder([FromBody] string sellOrderNo)
        {
            var response = new SellOrderDeleteResponse
            {
                DelRecCount = await _SellOrderFunction.DeleteSellOrder(sellOrderNo)
            };
            return Ok(response);
        }
    }
}
