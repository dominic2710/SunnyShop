using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface IShippingCompanyFunction
    {
        Task<ShippingCompany> GetShippingCompanyByCd(string ShippingCompanyCd);
        Task<IEnumerable<ShippingCompany>> GetListShippingCompany();
        Task<ShippingCompany> AddShippingCompany(ShippingCompany ShippingCompany);
        Task<int> UpdateShippingCompany(ShippingCompany ShippingCompany);
        Task<int> DeleteShippingCompany(string ShippingCompanyCd);

    }
}
