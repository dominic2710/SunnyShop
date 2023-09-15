using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Const;
using SellManagement.Api.Entities;
using SellManagement.Api.Helpers;

namespace SellManagement.Api.Functions
{
    public class PurchaseOrderFunction: IPurchaseOrderFunction
    {
        SellManagementContext _context;
        IVoucherNoManagementFunction _voucherNoManagementFunction;
        UserOperator _userOperator;
        public PurchaseOrderFunction(SellManagementContext context, IVoucherNoManagementFunction voucherNoManagementFunction, UserOperator userOperator)
        {
            _context = context;
            _voucherNoManagementFunction = voucherNoManagementFunction;
            _userOperator = userOperator;
        }
        public async Task<PurchaseOrder> GetPurchaseOrderByNo(string purchaseOrderNo)
        {
            var entity = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseOrderNo == purchaseOrderNo).ToListAsync();
            return entity.Select(ToPurchaseOrderModel).FirstOrDefault();
        }
        public async Task<IEnumerable<PurchaseOrder>> GetListPurchaseOrder()
        {
            var entity = await _context.TblPurchaseOrderHeads.ToListAsync();
            return entity.Select(ToPurchaseOrderModel).ToList();
        }
        public async Task<PurchaseOrder> AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.PurchaseOrderNo =await _voucherNoManagementFunction.GetVoucherNo(VoucherCategoryCd.PURCHASE_ORDER, true);

            TblPurchaseOrderHead orderHead = new TblPurchaseOrderHead
            {
                PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                PurchaseOrderDate = purchaseOrder.PurchaseOrderDate,
                PlannedImportDate = purchaseOrder.PlannedImportDate,
                SupplierCd = purchaseOrder.SupplierCd,
                Status = purchaseOrder.Status,
                SummaryCost = purchaseOrder.SummaryCost,
                SaleOffCost = purchaseOrder.SaleOffCost,
                PaidCost = purchaseOrder.PaidCost,
                PurchaseCost = purchaseOrder.PurchaseCost,
                Note = purchaseOrder.Note,
                CreateDate = DateTime.Now,
                CreateUserId = _userOperator.GetRequestUserId(),
                UpdateDate = DateTime.Now,
                UpdateUserId = _userOperator.GetRequestUserId()
            };
            await _context.TblPurchaseOrderHeads.AddAsync(orderHead);
            await _context.SaveChangesAsync();

            foreach (var detail in purchaseOrder.PurchaseOrderDetails)
            {
                TblPurchaseOrderDetail orderDetail = new TblPurchaseOrderDetail
                {
                    PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                    ProductCd = detail.ProductCd,
                    Quantity = detail.Quantity,
                    CostPrice = detail.CostPrice,
                    Cost = detail.Cost,
                    Note = detail.Note
                };
                await _context.TblPurchaseOrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();

                //Add PlannedInpStock
                int addInStock = orderHead.Status == 2 ? detail.Quantity : 0;
                int addPlannedInpStock = orderHead.Status == 0 ? detail.Quantity : 0;

                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == orderDetail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null)
                {
                    productInventory = new TblProductInventory
                    {
                        ProductCd = orderDetail.ProductCd,
                        AvailabilityInStock = addInStock,
                        InStock = addInStock,
                        PlannedInpStock = addPlannedInpStock,
                        PlannedOutStock = 0,
                        AveragePurchasePrice = orderDetail.CostPrice,
                        CreateDate = DateTime.Now,
                        CreateUserId = _userOperator.GetRequestUserId(),
                        UpdateDate = DateTime.Now,
                        UpdateUserId = _userOperator.GetRequestUserId()
                    };
                    await _context.TblProductInventories.AddAsync(productInventory);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    productInventory.AveragePurchasePrice = CalcAveragePurchasePriceAfterAdd(
                                                                        productInventory.AveragePurchasePrice,
                                                                        productInventory.InStock,
                                                                        orderDetail.CostPrice,
                                                                        addInStock);
                    productInventory.InStock += addInStock;
                    productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                    productInventory.PlannedInpStock += addPlannedInpStock;
                    productInventory.UpdateDate = DateTime.Now;
                    productInventory.UpdateUserId = "Admin";
                    _context.Update(productInventory);
                    await _context.SaveChangesAsync();
                }
            }

            purchaseOrder.Id = orderHead.Id;
            return purchaseOrder;
        }
        public async Task<int> UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            var entity = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseOrderNo == purchaseOrder.PurchaseOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0;

            int prevStatus = entity.Status;

            entity.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;
            entity.PurchaseOrderDate = purchaseOrder.PurchaseOrderDate;
            entity.PlannedImportDate = purchaseOrder.PlannedImportDate;
            entity.SupplierCd = purchaseOrder.SupplierCd;
            entity.Status = purchaseOrder.Status;
            entity.SummaryCost = purchaseOrder.SummaryCost;
            entity.SaleOffCost = purchaseOrder.SaleOffCost;
            entity.PaidCost = purchaseOrder.PaidCost;
            entity.PurchaseCost = purchaseOrder.PurchaseCost;
            entity.Note = purchaseOrder.Note;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateUserId = "Admin";

            _context.Update(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblPurchaseOrderDetails.Where(x => x.PurchaseOrderNo == purchaseOrder.PurchaseOrderNo).ToListAsync();

            //Minus InStock, PlannedInpStock
            foreach (var detail in entities)
            {
                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == detail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null) continue;

                int minusInStock = prevStatus == 2 ? detail.Quantity : 0;
                int minusPlannedInpStock = prevStatus == 0 ? detail.Quantity : 0;

                productInventory.AveragePurchasePrice = CalcAveragePurchasePriceAfterMinus(
                                            productInventory.AveragePurchasePrice,
                                            productInventory.InStock,
                                            detail.CostPrice,
                                            minusInStock);
                productInventory.InStock -= minusInStock;
                productInventory.PlannedInpStock -= minusPlannedInpStock;
                _context.Update(detail);
                await _context.SaveChangesAsync();
            }

            _context.TblPurchaseOrderDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();

            foreach (var detail in purchaseOrder.PurchaseOrderDetails)
            {
                TblPurchaseOrderDetail orderDetail = new TblPurchaseOrderDetail
                {
                    PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                    ProductCd = detail.ProductCd,
                    Quantity = detail.Quantity,
                    CostPrice = detail.CostPrice,
                    Cost = detail.Cost,
                    Note = detail.Note,
                };
                await _context.TblPurchaseOrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();

                //Add PlannedInpStock
                int addInStock = purchaseOrder.Status == 2 ? detail.Quantity : 0;
                int addPlannedInpStock = purchaseOrder.Status == 0 ? detail.Quantity : 0;

                var productInventory =await _context.TblProductInventories.Where(x => x.ProductCd == orderDetail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null)
                {
                    productInventory = new TblProductInventory
                    {
                        ProductCd = orderDetail.ProductCd,
                        InStock = addInStock,
                        AvailabilityInStock = addInStock,
                        PlannedInpStock = addPlannedInpStock,
                        PlannedOutStock = 0,
                        AveragePurchasePrice = orderDetail.CostPrice,
                        CreateDate = DateTime.Now,
                        CreateUserId = _userOperator.GetRequestUserId(),
                        UpdateDate = DateTime.Now,
                        UpdateUserId = _userOperator.GetRequestUserId()
                    };
                    await _context.TblProductInventories.AddAsync(productInventory);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    productInventory.AveragePurchasePrice = CalcAveragePurchasePriceAfterAdd(
                                                                        productInventory.AveragePurchasePrice,
                                                                        productInventory.InStock,
                                                                        orderDetail.CostPrice,
                                                                        addInStock);
                    productInventory.InStock += addInStock;
                    productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                    productInventory.PlannedInpStock += addPlannedInpStock;
                    productInventory.UpdateDate = DateTime.Now;
                    productInventory.UpdateUserId = "Admin";
                    _context.Update(productInventory);
                    await _context.SaveChangesAsync();
                }
            }

            return count;
        }
        public async Task<int> UpdatePurchaseOrderStatus(string purchaseOrderNo, int status)
        {
            var entity = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseOrderNo == purchaseOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0; 
            if (entity.Status == 2) return 0;

            entity.Status = status;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateUserId = "Admin";

            _context.Update(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblPurchaseOrderDetails.Where(x => x.PurchaseOrderNo == purchaseOrderNo).ToListAsync();

            //Minus PlannedInpStock
            foreach (var detail in entities)
            {
                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == detail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null) continue;

                switch (status)
                {
                    case 0:
                        productInventory.PlannedInpStock += detail.Quantity;
                        productInventory.InStock -= detail.Quantity;
                        break;
                    case 2:
                        productInventory.PlannedInpStock -= detail.Quantity;
                        productInventory.InStock += detail.Quantity;
                        break;
                    case 9:
                        productInventory.PlannedInpStock -= detail.Quantity;
                        productInventory.InStock -= detail.Quantity;
                        break;
                    default:
                        break;
                }

                productInventory.UpdateDate = DateTime.Now;
                productInventory.UpdateUserId = "Admin";
                _context.Update(productInventory);
                await _context.SaveChangesAsync();
            }

            return count;
        }
        public async Task<int> DeletePurchaseOrder(string purchaseOrderNo)
        {
            var entity = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseOrderNo == purchaseOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0;

            _context.TblPurchaseOrderHeads.Remove(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblPurchaseOrderDetails.Where(x => x.PurchaseOrderNo == purchaseOrderNo).ToListAsync();

            //Minus PlannedInpStock
            foreach (var detail in entities)
            {
                int minusInStock = entity.Status == 2 ? detail.Quantity : 0;
                int minusPlannedInpStock = entity.Status == 0 ? detail.Quantity : 0;

                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == detail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null) continue;

                productInventory.AveragePurchasePrice = CalcAveragePurchasePriceAfterMinus(
                                                            productInventory.AveragePurchasePrice,
                                                            productInventory.InStock,
                                                            detail.CostPrice,
                                                            minusInStock);

                productInventory.InStock -= minusInStock;
                productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                productInventory.PlannedInpStock -= minusPlannedInpStock;
                productInventory.UpdateDate = DateTime.Now;
                productInventory.UpdateUserId = "Admin";
                _context.Update(productInventory);
                await _context.SaveChangesAsync();
            }

            _context.TblPurchaseOrderDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();


            return count;
        }
        private PurchaseOrder ToPurchaseOrderModel(TblPurchaseOrderHead entity)
        {
            return entity == null ? new PurchaseOrder() : new PurchaseOrder
            {
                Id = entity.Id,
                PurchaseOrderNo = entity.PurchaseOrderNo,
                PurchaseOrderDate = entity.PurchaseOrderDate,
                PlannedImportDate = entity.PlannedImportDate,
                SupplierCd = entity.SupplierCd,
                SupplierName = _context.TblSuppliers.Where(x=>x.SupplierCd == entity.SupplierCd).FirstOrDefault()?.Name,
                Status = entity.Status,
                SummaryCost = entity.SummaryCost,
                SaleOffCost = entity.SaleOffCost,
                PaidCost = entity.PaidCost,
                PurchaseCost = entity.PurchaseCost,
                Note = entity.Note,
                PurchaseOrderDetails = _context.TblPurchaseOrderDetails.ToList().Where(x=>x.PurchaseOrderNo == entity.PurchaseOrderNo)
                                                                        .Select(ToPurchaseOrderDetailModel).ToList(),
                CreateDate = entity.CreateDate,
                CreateUserId = entity.CreateUserId,
                UpdateDate = entity.UpdateDate,
                UpdateUserId = "Admin"
            };
        }
        private PurchaseOrderDetail ToPurchaseOrderDetailModel(TblPurchaseOrderDetail entity)
        {
            return entity == null ? new PurchaseOrderDetail() : new PurchaseOrderDetail
            {
                Id = entity.Id,
                PurchaseOrderNo = entity.PurchaseOrderNo,
                ProductCd = entity.ProductCd,
                ProductName = _context.TblProducts.Where(x=>x.ProductCd == entity.ProductCd).FirstOrDefault()?.Name,
                Quantity = entity.Quantity,
                CostPrice = entity.CostPrice,
                Cost = entity.Cost,
                Note = entity.Note
            };
        }

        private int CalcAveragePurchasePriceAfterAdd(int purchasePrice_1, int quantity_1, int purchasePrice_2, int quantity_2)
        {
            if (quantity_1 + quantity_2 == 0) return 0;

            return (int)Math.Round((double)(purchasePrice_1 * quantity_1 + purchasePrice_2 * quantity_2) / (quantity_1 + quantity_2), 0);
        }

        private int CalcAveragePurchasePriceAfterMinus(int purchasePrice_1, int quantity_1, int purchasePrice_2, int quantity_2)
        {
            if (quantity_1 - quantity_2 == 0) return 0;

            return (int)Math.Round((double)(purchasePrice_1 * quantity_1 - purchasePrice_2 * quantity_2) / (quantity_1 - quantity_2), 0);
        }
    }
}
