using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Functions
{
    public class ShippingCompanyFunction:IShippingCompanyFunction
    {
        SellManagementContext _context;
        public ShippingCompanyFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<ShippingCompany> GetShippingCompanyByCd(string ShippingCompanyCd)
        {
            var entity = await _context.TblShippingCompanys.Where(x => x.ShippingCompanyCd == ShippingCompanyCd).ToListAsync();
            return entity.Select(ToShippingCompanyModel).FirstOrDefault();
        }
        public async Task<IEnumerable<ShippingCompany>> GetListShippingCompany()
        {
            var enties = await _context.TblShippingCompanys.ToListAsync();
            return enties.Select(ToShippingCompanyModel).ToList();
        }
        public async Task<ShippingCompany> AddShippingCompany(ShippingCompany ShippingCompany)
        {
            TblShippingCompany entity = new TblShippingCompany
            {
                ShippingCompanyCd = ShippingCompany.ShippingCompanyCd,
                Name = ShippingCompany.Name,
                Address1 = ShippingCompany.Address1,
                Address2 = ShippingCompany.Address2,
                PhoneNumber = ShippingCompany.PhoneNumber,
                Email = ShippingCompany.Email,
                Note = ShippingCompany.Note
            };
            await _context.TblShippingCompanys.AddAsync(entity);
            await _context.SaveChangesAsync();

            ShippingCompany.Id = entity.Id;
            return ShippingCompany;
        }
        public async Task<int> UpdateShippingCompany(ShippingCompany ShippingCompany)
        {
            var entity = await _context.TblShippingCompanys.Where(x => x.ShippingCompanyCd == ShippingCompany.ShippingCompanyCd).FirstOrDefaultAsync();
            if (entity == null) return 0;

            entity.ShippingCompanyCd = ShippingCompany.ShippingCompanyCd;
            entity.Name = ShippingCompany.Name;
            entity.Address1 = ShippingCompany.Address1;
            entity.Address2 = ShippingCompany.Address2;
            entity.PhoneNumber = ShippingCompany.PhoneNumber;
            entity.Email = ShippingCompany.Email;
            entity.Note = ShippingCompany.Note;

            _context.TblShippingCompanys.Update(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }
        public async Task<int> DeleteShippingCompany(string ShippingCompanyCd)
        {
            var entity = await _context.TblShippingCompanys.Where(x => x.ShippingCompanyCd == ShippingCompanyCd).FirstOrDefaultAsync();
            if (entity == null) return 0;
            _context.Remove(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }

        private ShippingCompany ToShippingCompanyModel(TblShippingCompany entity)
        {
            return entity == null ? new ShippingCompany() : new ShippingCompany
            {
                Id = entity.Id,
                ShippingCompanyCd = entity.ShippingCompanyCd,
                Name = entity.Name,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                PhoneNumber =entity.PhoneNumber,
                Email = entity.Email,
                Note = entity.Note
            };
        }
    }
}
