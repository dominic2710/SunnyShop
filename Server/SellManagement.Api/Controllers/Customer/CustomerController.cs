using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Controllers.Customer;
using SellManagement.Api.Functions;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CustomerController : Controller
    {
        ICustomerFunction _customerFunction;
        IClassifyNameFunction _classifyNameFunction;
        public CustomerController(ICustomerFunction customerFunction, IClassifyNameFunction classifyNameFunction)
        {
            _customerFunction = customerFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetCustomerByCd")]
        [Authorize]
        public async Task<IActionResult> GetCustomerByCd([FromBody] string CustomerCd)
        {
            var response = new GetCustomerByCdResponse
            {
                CustomerData = await _customerFunction.GetCustomerByCd(CustomerCd)
            };
            return Ok(response);
        }

        [HttpPost("GetListCustomer")]
        [Authorize]
        public async Task<IActionResult> GetListCustomer()
        {
            var response = new GetListCustomerResponse
            {
                ListCustomer = await _customerFunction.GetListCustomer(),
                ListCategory = await _classifyNameFunction.GetListNameByGroupId(4),
            };
            return Ok(response);
        }

        [HttpPost("AddCustomer")]
        [Authorize]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerAddRequest request)
        {
            var response = new CustomerAddResponse
            {
                CustomerData = await _customerFunction.AddCustomer(request.CustomerData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateCustomer")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateRequest request)
        {
            var response = new CustomerUpdateResponse
            {
                UpdRecCount = await _customerFunction.UpdateCustomer(request.CustomerData),
            };
            return Ok(response);
        }

        [HttpPost("DeleteCustomer")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer([FromBody] string CustomerCd)
        {
            var response = new CustomerDeleteResponse
            {
                DelRecCount = await _customerFunction.DeleteCustomer(CustomerCd)
            };
            return Ok(response);
        }
    }
}
