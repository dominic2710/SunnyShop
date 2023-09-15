using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Functions
{
    public class VoucherNoManagementFunction:IVoucherNoManagementFunction
    {
        SellManagementContext _context;
        public VoucherNoManagementFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<string> GetVoucherNo(int categoryCd, bool updateFlg)
        {
            var entity = _context.TblVoucherNoManagements.Where(x => x.CategoryCd == categoryCd).FirstOrDefault();
            if (entity == null)
            {
                entity = new TblVoucherNoManagement
                {
                    CategoryCd = categoryCd,
                    CategoryName = "",
                    VoucherNo = 1,
                    CreateUserId = "Admin",
                    CreateDate = DateTime.Now,
                    UpdateUserId = "Admin",
                    UpdateDate = DateTime.Now
                };

                await _context.TblVoucherNoManagements.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            var returnVoucherNo = entity.VoucherNo;

            if (updateFlg)
            {
                entity.VoucherNo++;
                _context.TblVoucherNoManagements.Update(entity);
                await _context.SaveChangesAsync();
            }

            return returnVoucherNo.ToString("00000000");
        }
    }
}
