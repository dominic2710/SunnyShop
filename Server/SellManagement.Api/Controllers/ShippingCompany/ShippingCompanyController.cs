using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Controllers.ShippingCompany;
using SellManagement.Api.Functions;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ShippingCompanyController : Controller
    {
        IShippingCompanyFunction _ShippingCompanyFunction;
        IClassifyNameFunction _classifyNameFunction;
        public ShippingCompanyController(IShippingCompanyFunction ShippingCompanyFunction, IClassifyNameFunction classifyNameFunction)
        {
            _ShippingCompanyFunction = ShippingCompanyFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetShippingCompanyByCd")]
        [Authorize]
        public async Task<IActionResult> GetShippingCompanyByCd([FromBody] string ShippingCompanyCd)
        {
            var response = new GetShippingCompanyByCdResponse
            {
                ShippingCompanyData = await _ShippingCompanyFunction.GetShippingCompanyByCd(ShippingCompanyCd)
            };
            return Ok(response);
        }

        [HttpPost("GetListShippingCompany")]
        [Authorize]
        public async Task<IActionResult> GetListShippingCompany()
        {
            var response = new GetListShippingCompanyResponse
            {
                ListShippingCompany = await _ShippingCompanyFunction.GetListShippingCompany(),
            };
            return Ok(response);
        }

        [HttpPost("AddShippingCompany")]
        [Authorize]
        public async Task<IActionResult> AddShippingCompany([FromBody] ShippingCompanyAddRequest request)
        {
            var response = new ShippingCompanyAddResponse
            {
                ShippingCompanyData = await _ShippingCompanyFunction.AddShippingCompany(request.ShippingCompanyData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateShippingCompany")]
        [Authorize]
        public async Task<IActionResult> UpdateShippingCompany([FromBody] ShippingCompanyUpdateRequest request)
        {
            var response = new ShippingCompanyUpdateResponse
            {
                UpdRecCount = await _ShippingCompanyFunction.UpdateShippingCompany(request.ShippingCompanyData),
            };
            return Ok(response);
        }

        [HttpPost("DeleteShippingCompany")]
        [Authorize]
        public async Task<IActionResult> DeleteShippingCompany([FromBody] string ShippingCompanyCd)
        {
            var response = new ShippingCompanyDeleteResponse
            {
                DelRecCount = await _ShippingCompanyFunction.DeleteShippingCompany(ShippingCompanyCd)
            };
            return Ok(response);
        }
    }
}
