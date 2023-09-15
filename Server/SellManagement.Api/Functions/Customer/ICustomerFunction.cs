using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public interface ICustomerFunction
    {
        Task<Customer> GetCustomerByCd(string customerCd);
        Task<IEnumerable<Customer>> GetListCustomer();
        Task<Customer> AddCustomer(Customer customer);
        Task<int> UpdateCustomer(Customer customer);
        Task<int> DeleteCustomer(string customerCd);

    }
}
