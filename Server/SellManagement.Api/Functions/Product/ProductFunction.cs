using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Functions
{
    public class ProductFunction : IProductFunction
    {
        const int GROUPID_CATEGORY = 1;
        const int GROUPID_TRADEMARK = 2;
        const int GROUPID_ORIGIN = 3;

        SellManagementContext _context;
        public ProductFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductByCd(string productCd)
        {
            var entity = await _context.TblProducts.Where(x => x.ProductCd == productCd).ToListAsync();
            return entity.Select(ToProductModel).FirstOrDefault();
        }
        public async Task<IEnumerable<Product>> GetListProduct()
        {
            var enties = await _context.TblProducts.ToListAsync();

            return enties.Select(ToProductModel).ToList();
        }

        public async Task<Product> AddProduct(Product product)
        {
            TblProduct entity = new TblProduct
            {
                ProductCd = product.ProductCd,
                Barcode = product.Barcode,
                Name = product.Name,
                CategoryCd = product.CategoryCd,
                TradeMarkCd = product.TradeMarkCd,
                OriginCd = product.OriginCd,
                CostPrice = product.CostPrice,
                SoldPrice = product.SoldPrice,
                Detail = product.Detail
            };
            await _context.TblProducts.AddAsync(entity);
            await _context.SaveChangesAsync();

            product.Id = entity.Id;
            return product;
        }
        public async Task<int> UpdateProduct(Product product)
        {
            var entity = await _context.TblProducts.Where(x => x.ProductCd == product.ProductCd).FirstOrDefaultAsync();
            if (entity == null) return 0;

            entity.ProductCd = product.ProductCd;
            entity.Barcode = product.Barcode;
            entity.Name = product.Name;
            entity.CategoryCd = product.CategoryCd;
            entity.TradeMarkCd = product.TradeMarkCd;
            entity.OriginCd = product.OriginCd;
            entity.CostPrice = product.CostPrice;
            entity.SoldPrice = product.SoldPrice;
            entity.Detail = product.Detail;

            _context.TblProducts.Update(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }
        public async Task<int> DeleteProduct(string productCd)
        {
            var entity = await _context.TblProducts.Where(x => x.ProductCd == productCd).FirstOrDefaultAsync();
            if (entity == null) return 0;

            _context.Remove(entity);
            var count = await _context.SaveChangesAsync();

            return count;
        }

        private Product ToProductModel(TblProduct entity)
        {
            var categories = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_CATEGORY).ToList();
            var trademarks = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_TRADEMARK).ToList();
            var origins = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_ORIGIN).ToList();
            var productInventories = _context.TblProductInventories.Where(x => x.ProductCd == entity.ProductCd).FirstOrDefault();
            if (productInventories == null) 
                productInventories = new TblProductInventory
                {
                    PlannedInpStock = 0,
                    PlannedOutStock = 0,
                    InStock = 0,
                    AvailabilityInStock = 0,
                };

            return entity == null ? new Product() : new Product
            {
                Id = entity.Id,
                ProductCd = entity.ProductCd,
                Barcode = entity.Barcode,
                Name = entity.Name,
                CategoryCd = entity.CategoryCd,
                CategoryName = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_CATEGORY)
                                                .Where(x => x.Code == entity.CategoryCd)
                                                .SingleOrDefault().Name,
                TradeMarkCd = entity.TradeMarkCd,
                TradeMarkName = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_TRADEMARK)
                                                .Where(x => x.Code == entity.TradeMarkCd)
                                                .SingleOrDefault().Name,
                OriginCd = entity.OriginCd,
                OriginName = _context.TblClassifiesName.Where(x => x.GroupId == GROUPID_ORIGIN)
                                                .Where(x => x.Code == entity.OriginCd)
                                                .SingleOrDefault().Name,
                PlannedInpStock = productInventories.PlannedInpStock,
                PlannedOutStock = productInventories.PlannedOutStock,
                InStock = productInventories.InStock,
                AvailabilityInStock = productInventories.AvailabilityInStock,
                CostPrice = entity.CostPrice,
                SoldPrice = entity.SoldPrice,
                Detail = entity.Detail
            };
        }
    }
}
