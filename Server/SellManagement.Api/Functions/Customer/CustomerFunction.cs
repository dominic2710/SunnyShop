using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Functions
{
    public class CustomerFunction:ICustomerFunction
    {
        const int GROUPID_CUSTOMER_CATEGORY = 4;
        SellManagementContext _context;
        public CustomerFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetCustomerByCd(string customerCd)
        {
            var entity = await _context.TblCustomers.Where(x => x.CustomerCd == customerCd).ToListAsync();
            return entity.Select(ToCustomerModel).FirstOrDefault();
        }
        public async Task<IEnumerable<Customer>> GetListCustomer()
        {
            var enties = await _context.TblCustomers.ToListAsync();
            return enties.Select(ToCustomerModel).ToList();
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            TblCustomer entity = new TblCustomer
            {
                CustomerCd = customer.CustomerCd,
                Name = customer.Name,
                CategoryCd = customer.CategoryCd,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Facebook = customer.Facebook,
                Note = customer.Note
            };
            await _context.TblCustomers.AddAsync(entity);
            await _context.SaveChangesAsync();

            customer.Id = entity.Id;
            return customer;
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {
            var entity = await _context.TblCustomers.Where(x => x.CustomerCd == customer.CustomerCd).FirstOrDefaultAsync();
            if (entity == null) return 0;

            entity.CustomerCd = customer.CustomerCd;
            entity.Name = customer.Name;
            entity.CategoryCd = customer.CategoryCd;
            entity.Address1 = customer.Address1;
            entity.Address2 = customer.Address2;
            entity.PhoneNumber = customer.PhoneNumber;
            entity.Email = customer.Email;
            entity.Facebook = customer.Facebook;
            entity.Note = customer.Note;

            _context.TblCustomers.Update(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }
        public async Task<int> DeleteCustomer(string customerCd)
        {
            var entity = await _context.TblCustomers.Where(x => x.CustomerCd == customerCd).FirstOrDefaultAsync();
            if (entity == null) return 0;
            _context.Remove(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }

        private Customer ToCustomerModel(TblCustomer entity)
        {
            return entity == null ? new Customer() : new Customer
            {
                Id = entity.Id,
                CustomerCd = entity.CustomerCd,
                Name = entity.Name,
                CategoryCd = entity.CategoryCd,
                CategoryName = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_CUSTOMER_CATEGORY)
                                                .Where(x => x.Code == entity.CategoryCd)
                                                .SingleOrDefault().Name,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                PhoneNumber =entity.PhoneNumber,
                Email = entity.Email,
                Facebook = entity.Facebook,
                Note = entity.Note
            };
        }
    }
}
