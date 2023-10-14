using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SellManagement.Api.Const;
using SellManagement.Api.Entities;
using SellManagement.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class InventoryFunction : IInventoryFunction
    {
        SellManagementContext _context;
        IVoucherNoManagementFunction _voucherNoManagementFunction;
        UserOperator _userOperator;

        public InventoryFunction(SellManagementContext context, IVoucherNoManagementFunction voucherNoManagementFunction, UserOperator userOperator)
        {
            _context = context;
            _voucherNoManagementFunction = voucherNoManagementFunction;
            _userOperator = userOperator;
        }

        public async Task<Inventory> GetInventoryByNo(string inventoryNo)
        {
            var entity = await _context.TblInventoryHeads.Where(x => x.InventoryNo == inventoryNo).ToListAsync();
            return entity.Select(ToInventoryModel).FirstOrDefault();
        }
        public async Task<IEnumerable<Inventory>> GetListInventory()
        {
            var entity = await _context.TblInventoryHeads.ToListAsync();
            return entity.Select(ToInventoryModel);
        }
        public async Task<Inventory> AddInventory(Inventory inventory)
        {
            inventory.InventoryNo = await _voucherNoManagementFunction.GetVoucherNo(VoucherCategoryCd.INVENTORY, true);
            TblInventoryHead inventoryHead = new TblInventoryHead
            {
                InventoryNo = inventory.InventoryNo,
                InventoryDate = inventory.InventoryDate,
                Note = inventory.Note,
                CreateDate = System.DateTime.Now,
                CreateUserId = _userOperator.GetRequestUserId(),
                UpdateDate = System.DateTime.Now,
                UpdateUserId = _userOperator.GetRequestUserId(),
            };
            await _context.TblInventoryHeads.AddAsync(inventoryHead);
            await _context.SaveChangesAsync();

            foreach (var item in inventory.InventoryDetails)
            {
                TblInventoryDetail inventoryDetail = new TblInventoryDetail
                {
                    InventoryNo = inventory.InventoryNo,
                    ExpiryDate = item.ExpiryDate,
                    ProductCd = item.ProductCd,
                    Quantity = item.Quantity,
                    CreateDate = DateTime.Now,
                    CreateUserId = _userOperator.GetRequestUserId(),
                    UpdateDate = DateTime.Now,
                    UpdateUserId = _userOperator.GetRequestUserId(),
                };
                await _context.TblInventoryDetails.AddAsync(inventoryDetail);
                await _context.SaveChangesAsync();
            }

            inventory.Id = inventoryHead.Id;
            return inventory;
        }
        public async Task<int> UpdateInventory(Inventory inventory)
        {
            var entity = await _context.TblInventoryHeads.Where(x=>x.InventoryNo == inventory.InventoryNo).FirstOrDefaultAsync();

            if (entity == null) return 0;

            entity.InventoryDate = inventory.InventoryDate;
            entity.Note = inventory.Note;
            entity.UpdateDate = inventory.UpdateDate;
            entity.UpdateUserId = _userOperator.GetRequestUserId();

            _context.Update(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblInventoryDetails.Where(x=>x.InventoryNo == inventory.InventoryNo).ToListAsync();
            _context.TblInventoryDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();

            foreach (var item in inventory.InventoryDetails)
            {
                TblInventoryDetail inventoryDetail = new TblInventoryDetail
                {
                    InventoryNo = item.InventoryNo,
                    ExpiryDate = item.ExpiryDate,
                    ProductCd = item.ProductCd,
                    Quantity = item.Quantity,
                    CreateDate = DateTime.Now,
                    CreateUserId = _userOperator.GetRequestUserId(),
                    UpdateDate = DateTime.Now,
                    UpdateUserId = _userOperator.GetRequestUserId(),
                };
                await _context.TblInventoryDetails.AddAsync(inventoryDetail);
                await _context.SaveChangesAsync();
            }

            return count;
        }
        //Task<int> UpdateInventoryStatus(string inventoryNo, int status);
        public async Task<int> DeleteInventory(string inventoryNo)
        {
            var entity = await _context.TblInventoryHeads.Where(x => x.InventoryNo == inventoryNo).FirstOrDefaultAsync();
            if (entity == null) return 0;

            _context.TblInventoryHeads.RemoveRange(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblInventoryDetails.Where(x => x.InventoryNo == inventoryNo).ToListAsync();
            _context.TblInventoryDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();

            return count;
        }

        private Inventory ToInventoryModel(TblInventoryHead entity)
        {
            return entity == null ? null : new Inventory
            {
                Id = entity.Id,
                InventoryNo = entity.InventoryNo,
                InventoryDate = entity.InventoryDate,
                Note = entity.Note,
                InventoryDetails = _context.TblInventoryDetails.ToList().Where(x => x.InventoryNo == entity.InventoryNo)
                                                                .Select(ToInventoryDetailModel),
                CreateDate = entity.CreateDate,
                CreateUserId = entity.CreateUserId,
                UpdateDate = entity.UpdateDate,
                UpdateUserId = entity.UpdateUserId,
            };
        }

        private InventoryDetail ToInventoryDetailModel(TblInventoryDetail entity)
        {
            return entity == null ? null : new InventoryDetail
            {
                Id = entity.Id,
                InventoryNo = entity.InventoryNo,
                ProductCd = entity.ProductCd,
                ProductName = _context.TblProducts.Where(x => x.ProductCd == entity.ProductCd).FirstOrDefault()?.Name,
                Barcode = _context.TblProducts.Where(x => x.ProductCd == entity.ProductCd).FirstOrDefault()?.Barcode,
                ExpiryDate = entity.ExpiryDate,
                Quantity = entity.Quantity,
                CreateDate = entity.CreateDate,
                CreateUserId = entity.CreateUserId,
                UpdateDate = entity.UpdateDate,
                UpdateUserId = entity.UpdateUserId
            };
        }
    }
}
