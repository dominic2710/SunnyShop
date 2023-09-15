using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface ISupplierFunction
    {
        Task<Supplier> GetSupplierByCd(string supplierCd);
        Task<IEnumerable<Supplier>> GetListSupplier();
        Task<Supplier> AddSupplier(Supplier supplier);
        Task<int> UpdateSupplier(Supplier supplier);
        Task<int> DeleteSupplier(string supplierCd);

    }
}
