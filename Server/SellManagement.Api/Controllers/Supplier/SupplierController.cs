using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Controllers.Supplier;
using SellManagement.Api.Functions;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SupplierController : Controller
    {
        ISupplierFunction _supplierFunction;
        IClassifyNameFunction _classifyNameFunction;
        public SupplierController(ISupplierFunction supplierFunction, IClassifyNameFunction classifyNameFunction)
        {
            _supplierFunction = supplierFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetSupplierByCd")]
        [Authorize]
        public async Task<IActionResult> GetSupplierByCd([FromBody] string SupplierCd)
        {
            var response = new GetSupplierByCdResponse
            {
                SupplierData = await _supplierFunction.GetSupplierByCd(SupplierCd)
            };
            return Ok(response);
        }

        [HttpPost("GetListSupplier")]
        [Authorize]
        public async Task<IActionResult> GetListSupplier()
        {
            var response = new GetListSupplierResponse
            {
                ListSupplier = await _supplierFunction.GetListSupplier(),
                ListCategory = await _classifyNameFunction.GetListNameByGroupId(5),
            };
            return Ok(response);
        }

        [HttpPost("AddSupplier")]
        [Authorize]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierAddRequest request)
        {
            var response = new SupplierAddResponse
            {
                SupplierData = await _supplierFunction.AddSupplier(request.SupplierData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateSupplier")]
        [Authorize]
        public async Task<IActionResult> UpdateSupplier([FromBody] SupplierUpdateRequest request)
        {
            var response = new SupplierUpdateResponse
            {
                UpdRecCount = await _supplierFunction.UpdateSupplier(request.SupplierData),
            };
            return Ok(response);
        }

        [HttpPost("DeleteSupplier")]
        [Authorize]
        public async Task<IActionResult> DeleteSupplier([FromBody] string SupplierCd)
        {
            var response = new SupplierDeleteResponse
            {
                DelRecCount = await _supplierFunction.DeleteSupplier(SupplierCd)
            };
            return Ok(response);
        }
    }
}
