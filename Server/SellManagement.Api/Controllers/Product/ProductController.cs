using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using SellManagement.Api.Controllers.Product;
using SellManagement.Api.Messages;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductController : Controller
    {
        IProductFunction _productFunction;
        IClassifyNameFunction _classifyNameFunction;
        public ProductController(IProductFunction productFunction, IClassifyNameFunction classifyNameFunction)
        {
            _productFunction = productFunction;
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetProductByCd")]
        [Authorize]
        public async Task<IActionResult> GetProductByCd([FromBody] string productCd)
        {
            var response = new GetProductByCdResponse
            {
                ProductData = await _productFunction.GetProductByCd(productCd)
            };
            return Ok(response);
        }

        [HttpPost("GetListProduct")]
        [Authorize]
        public async Task<IActionResult> GetListProduct()
        {
            var response = new GetListProductResponse
            {
                ListProduct = await _productFunction.GetListProduct(),
                ListCategory = await _classifyNameFunction.GetListNameByGroupId(1),
                ListTradeMark = await _classifyNameFunction.GetListNameByGroupId(2),
                ListOrigin = await _classifyNameFunction.GetListNameByGroupId(3),
            };
            return Ok(response);
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddRequest request)
        {
            var response = new ProductAddResponse
            {
                ProductData = await _productFunction.AddProduct(request.ProductData)
            };
            return Ok(response);
        }

        [HttpPost("UpdateProduct")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            var response = new ProductUpdateResponse
            {
                UpdRecCount = await _productFunction.UpdateProduct(request.ProductData),
            };
            return Ok(response);
        }

        [HttpPost("DeleteProduct")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromBody] string productCd)
        {
            var response = new ProductDeleteResponse
            {
                DelRecCount = await _productFunction.DeleteProduct(productCd)
            };
            return Ok(response);
        }
    }
}
