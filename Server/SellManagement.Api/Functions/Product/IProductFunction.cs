using SellManagement.Api.Controllers.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IProductFunction
    {
        Task<Product> GetProductByCd(string productCd);
        Task<IEnumerable<Product>> GetListProduct();
        Task<Product> AddProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(string productCd);
    }
}
