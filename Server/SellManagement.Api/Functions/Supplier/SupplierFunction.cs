using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Functions
{
    public class SupplierFunction:ISupplierFunction
    {
        const int GROUPID_SUPPLIER_CATEGORY = 5;
        SellManagementContext _context;
        public SupplierFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<Supplier> GetSupplierByCd(string supplierCd)
        {
            var entity = await _context.TblSuppliers.Where(x => x.SupplierCd == supplierCd).ToListAsync();
            return entity.Select(ToSupplierModel).FirstOrDefault();
        }
        public async Task<IEnumerable<Supplier>> GetListSupplier()
        {
            var enties = await _context.TblSuppliers.ToListAsync();
            return enties.Select(ToSupplierModel).ToList();
        }
        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            TblSupplier entity = new TblSupplier
            {
                SupplierCd = supplier.SupplierCd,
                Name = supplier.Name,
                CategoryCd = supplier.CategoryCd,
                Address1 = supplier.Address1,
                Address2 = supplier.Address2,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Facebook = supplier.Facebook,
                Note = supplier.Note
            };
            await _context.TblSuppliers.AddAsync(entity);
            await _context.SaveChangesAsync();

            supplier.Id = entity.Id;
            return supplier;
        }
        public async Task<int> UpdateSupplier(Supplier supplier)
        {
            var entity = await _context.TblSuppliers.Where(x => x.SupplierCd == supplier.SupplierCd).FirstOrDefaultAsync();
            if (entity == null) return 0;

            entity.SupplierCd = supplier.SupplierCd;
            entity.Name = supplier.Name;
            entity.CategoryCd = supplier.CategoryCd;
            entity.Address1 = supplier.Address1;
            entity.Address2 = supplier.Address2;
            entity.PhoneNumber = supplier.PhoneNumber;
            entity.Email = supplier.Email;
            entity.Facebook = supplier.Facebook;
            entity.Note = supplier.Note;

            _context.TblSuppliers.Update(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }
        public async Task<int> DeleteSupplier(string supplierCd)
        {
            var entity = await _context.TblSuppliers.Where(x => x.SupplierCd == supplierCd).FirstOrDefaultAsync();
            if (entity == null) return 0;
            _context.Remove(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }

        private Supplier ToSupplierModel(TblSupplier entity)
        {
            return entity == null ? new Supplier() : new Supplier
            {
                Id = entity.Id,
                SupplierCd = entity.SupplierCd,
                Name = entity.Name,
                CategoryCd = entity.CategoryCd,
                CategoryName = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_SUPPLIER_CATEGORY)
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
