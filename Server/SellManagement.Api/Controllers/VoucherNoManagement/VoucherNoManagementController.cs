using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using SellManagement.Api.Controllers.VoucherNoManagement;
using SellManagement.Api.Messages;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VoucherNoManagementController : Controller
    {
        IVoucherNoManagementFunction _voucherNoManagementFunction;
        public VoucherNoManagementController(IVoucherNoManagementFunction voucherNoManagementFunction)
        {
            _voucherNoManagementFunction = voucherNoManagementFunction;
        }

        [HttpPost("GetVoucherNo")]
        [Authorize]
        public async Task<IActionResult> GetVoucherNoManagementByCd([FromBody] int categoryCd)
        {
            var response = new GetVoucherNoResponse
            {
                VoucherNo = await _voucherNoManagementFunction.GetVoucherNo(categoryCd, false)
            };
            return Ok(response);
        }
    }
}
