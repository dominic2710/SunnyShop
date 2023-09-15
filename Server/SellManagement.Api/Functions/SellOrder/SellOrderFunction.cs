using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;
using SellManagement.Api.Helpers;
using SellManagement.Api.Const;

namespace SellManagement.Api.Functions
{
    public class SellOrderFunction : ISellOrderFunction
    {
        SellManagementContext _context;
        IVoucherNoManagementFunction _voucherNoManagementFunction;
        UserOperator _userOperator;
        public SellOrderFunction(SellManagementContext context, IVoucherNoManagementFunction voucherNoManagementFunction, UserOperator userOperator)
        {
            _context = context;
            _voucherNoManagementFunction = voucherNoManagementFunction;
            _userOperator = userOperator;
        }
        public async Task<SellOrder> GetSellOrderByNo(string SellOrderNo)
        {
            var entity = await _context.TblSellOrderHeads.Where(x => x.SellOrderNo == SellOrderNo).ToListAsync();
            return entity.Select(ToSellOrderModel).FirstOrDefault();
        }
        public async Task<IEnumerable<SellOrder>> GetListSellOrder()
        {
            var entity = await _context.TblSellOrderHeads.ToListAsync();
            return entity.Select(ToSellOrderModel).ToList();
        }
        public async Task<SellOrder> AddSellOrder(SellOrder SellOrder)
        {
            SellOrder.SellOrderNo = await _voucherNoManagementFunction.GetVoucherNo(VoucherCategoryCd.SELL_ORDER, true);

            TblSellOrderHead orderHead = new TblSellOrderHead
            {
                SellOrderNo = SellOrder.SellOrderNo,
                SellOrderDate = SellOrder.SellOrderDate,
                PlannedExportDate = SellOrder.PlannedExportDate,
                CustomerCd = SellOrder.CustomerCd,
                ShippingCompanyCd = SellOrder.ShippingCompanyCd,
                Status = SellOrder.Status,
                ForControlStatus = SellOrder.ForControlStatus,
                SummaryCost = SellOrder.SummaryCost,
                SaleOffCost = SellOrder.SaleOffCost,
                ShippingCost = SellOrder.ShippingCost,
                PaidCost = SellOrder.PaidCost,
                SellCost = SellOrder.SellCost,
                Note = SellOrder.Note,
                WaybillVoucherNo = SellOrder.WaybillVoucherNo,
                PlannedPickingDate = SellOrder.PlannedPickingDate,
                PlannedDeliveryDate = SellOrder.PlannedDeliveryDate,
                DeliveryDate = SellOrder.DeliveryDate,
                CollectOnDeliveryCash = SellOrder.CollectOnDeliveryCash,
                WaybillCost = SellOrder.WaybillCost,
                WaybillStatus = SellOrder.WaybillStatus,
                CancelDate = null,
                CreateDate = DateTime.Now,
                CreateUserId = _userOperator.GetRequestUserId(),
                UpdateDate = DateTime.Now,
                UpdateUserId = _userOperator.GetRequestUserId()
            };
            await _context.TblSellOrderHeads.AddAsync(orderHead);
            await _context.SaveChangesAsync();

            foreach (var detail in SellOrder.SellOrderDetails)
            {
                TblSellOrderDetail orderDetail = new TblSellOrderDetail
                {
                    SellOrderNo = SellOrder.SellOrderNo,
                    ProductCd = detail.ProductCd,
                    Quantity = detail.Quantity,
                    CostPrice = detail.CostPrice,
                    Cost = detail.Cost,
                    Note = detail.Note
                };
                await _context.TblSellOrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();

                int minusInStock = orderHead.Status == 2 ? detail.Quantity : 0;
                int minusPlannedOutStock = orderHead.Status == 0 ? detail.Quantity : 0;

                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == orderDetail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null)
                {
                    productInventory = new TblProductInventory
                    {
                        ProductCd = orderDetail.ProductCd,
                        AvailabilityInStock = -minusInStock,
                        InStock = -minusInStock,
                        PlannedInpStock =   0,
                        PlannedOutStock = minusPlannedOutStock,
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
                    productInventory.InStock -= minusInStock;
                    productInventory.PlannedOutStock += minusPlannedOutStock;
                    productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                    productInventory.UpdateDate = DateTime.Now;
                    productInventory.UpdateUserId = "Admin";
                    _context.Update(productInventory);
                    await _context.SaveChangesAsync();
                }
            }

            SellOrder.Id = orderHead.Id;
            return SellOrder;
        }
        public async Task<int> UpdateSellOrder(SellOrder SellOrder)
        {
            var entity = await _context.TblSellOrderHeads.Where(x => x.SellOrderNo == SellOrder.SellOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0;

            int prevStatus = entity.Status;

            entity.SellOrderNo = SellOrder.SellOrderNo;
            entity.SellOrderDate = SellOrder.SellOrderDate;
            entity.PlannedExportDate = SellOrder.PlannedExportDate;
            entity.CustomerCd = SellOrder.CustomerCd;
            entity.ShippingCompanyCd = SellOrder.ShippingCompanyCd;
            entity.Status = SellOrder.Status;
            entity.ForControlStatus = SellOrder.ForControlStatus;
            entity.SummaryCost = SellOrder.SummaryCost;
            entity.SaleOffCost = SellOrder.SaleOffCost;
            entity.ShippingCost = SellOrder.ShippingCost;
            entity.PaidCost = SellOrder.PaidCost;
            entity.SellCost = SellOrder.SellCost;
            entity.Note = SellOrder.Note;
            entity.WaybillVoucherNo = SellOrder.WaybillVoucherNo;
            entity.PlannedPickingDate = SellOrder.PlannedPickingDate;
            entity.PlannedDeliveryDate = SellOrder.PlannedDeliveryDate;
            entity.DeliveryDate = SellOrder.DeliveryDate;
            entity.CollectOnDeliveryCash = SellOrder.CollectOnDeliveryCash;
            entity.WaybillCost = SellOrder.WaybillCost;
            entity.WaybillStatus = SellOrder.WaybillStatus;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateUserId = "Admin";

            if (entity.Status == 9)
            {
                if (entity.CancelDate == null)
                    entity.CancelDate = DateTime.Now.Date;
            }
            else
                entity.CancelDate = null;

            _context.Update(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblSellOrderDetails.Where(x => x.SellOrderNo == SellOrder.SellOrderNo).ToListAsync();

            foreach (var detail in entities)
            {
                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == detail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null) continue;

                int addInStock = prevStatus == 2 ? detail.Quantity : 0;
                int minusPlannedOutStock = prevStatus == 0 ? detail.Quantity : 0;

                productInventory.InStock += addInStock;
                productInventory.PlannedOutStock -= minusPlannedOutStock;
                _context.Update(detail);
                await _context.SaveChangesAsync();
            }

            _context.TblSellOrderDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();

            foreach (var detail in SellOrder.SellOrderDetails)
            {
                TblSellOrderDetail orderDetail = new TblSellOrderDetail
                {
                    SellOrderNo = SellOrder.SellOrderNo,
                    ProductCd = detail.ProductCd,
                    Quantity = detail.Quantity,
                    CostPrice = detail.CostPrice,
                    Cost = detail.Cost,
                    Note = detail.Note,
                };
                await _context.TblSellOrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();

                int minusInStock = SellOrder.Status == 2 ? detail.Quantity : 0;
                int minusPlannedOutStock = SellOrder.Status == 0 ? detail.Quantity : 0;

                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == orderDetail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null)
                {
                    productInventory = new TblProductInventory
                    {
                        ProductCd = orderDetail.ProductCd,
                        InStock = -minusInStock,
                        AvailabilityInStock = -minusInStock,
                        PlannedInpStock = 0,
                        PlannedOutStock = minusPlannedOutStock,
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
                    productInventory.InStock -= minusInStock;
                    productInventory.PlannedOutStock += minusPlannedOutStock;
                    productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                    productInventory.UpdateDate = DateTime.Now;
                    productInventory.UpdateUserId = "Admin";
                    _context.Update(productInventory);
                    await _context.SaveChangesAsync();
                }
            }

            return count;
        }
        public async Task<int> UpdateSellOrderStatus(string SellOrderNo, int status)
        {
            var entity = await _context.TblSellOrderHeads.Where(x => x.SellOrderNo == SellOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0;
            if (entity.Status == 2) return 0;

            entity.Status = status;
            entity.UpdateDate = DateTime.Now;
            entity.UpdateUserId = "Admin";

            _context.Update(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblSellOrderDetails.Where(x => x.SellOrderNo == SellOrderNo).ToListAsync();

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
        public async Task<int> DeleteSellOrder(string SellOrderNo)
        {
            var entity = await _context.TblSellOrderHeads.Where(x => x.SellOrderNo == SellOrderNo).FirstOrDefaultAsync();
            if (entity == null) return 0;

            _context.TblSellOrderHeads.Remove(entity);
            var count = await _context.SaveChangesAsync();

            var entities = await _context.TblSellOrderDetails.Where(x => x.SellOrderNo == SellOrderNo).ToListAsync();

            foreach (var detail in entities)
            {
                int addInStock = entity.Status == 2 ? detail.Quantity : 0;
                int minusPlannedOutStock = entity.Status == 0 ? detail.Quantity : 0;

                var productInventory = await _context.TblProductInventories.Where(x => x.ProductCd == detail.ProductCd).FirstOrDefaultAsync();
                if (productInventory == null) continue;

                productInventory.InStock += addInStock;
                productInventory.PlannedOutStock -= minusPlannedOutStock;
                productInventory.AvailabilityInStock = productInventory.InStock - productInventory.PlannedOutStock;
                productInventory.UpdateDate = DateTime.Now;
                productInventory.UpdateUserId = "Admin";
                _context.Update(productInventory);
                await _context.SaveChangesAsync();
            }

            _context.TblSellOrderDetails.RemoveRange(entities);
            await _context.SaveChangesAsync();


            return count;
        }
        private SellOrder ToSellOrderModel(TblSellOrderHead entity)
        {
            return entity == null ? new SellOrder() : new SellOrder
            {
                Id = entity.Id,
                SellOrderNo = entity.SellOrderNo,
                SellOrderDate = entity.SellOrderDate,
                PlannedExportDate = entity.PlannedExportDate,
                CustomerCd = entity.CustomerCd,
                CustomerName = _context.TblCustomers.Where(x => x.CustomerCd == entity.CustomerCd).FirstOrDefault()?.Name,
                ShippingCompanyCd = entity.ShippingCompanyCd,
                ShippingCompanyName = _context.TblShippingCompanys.Where(x => x.ShippingCompanyCd == entity.ShippingCompanyCd).FirstOrDefault()?.Name,
                Status = entity.Status,
                ForControlStatus = entity.ForControlStatus,
                SummaryCost = entity.SummaryCost,
                ShippingCost = entity.ShippingCost,
                SaleOffCost = entity.SaleOffCost,
                PaidCost = entity.PaidCost,
                SellCost = entity.SellCost,
                Note = entity.Note,
                WaybillVoucherNo = entity.WaybillVoucherNo,
                PlannedPickingDate = entity.PlannedPickingDate,
                PlannedDeliveryDate = entity.PlannedDeliveryDate,
                DeliveryDate = entity.DeliveryDate,
                CollectOnDeliveryCash = entity.CollectOnDeliveryCash,
                WaybillCost = entity.WaybillCost,
                WaybillStatus = entity.WaybillStatus,
                SellOrderDetails = _context.TblSellOrderDetails.ToList().Where(x => x.SellOrderNo == entity.SellOrderNo)
                                                                        .Select(ToSellOrderDetailModel).ToList(),
                CreateDate = entity.CreateDate,
                CreateUserId = entity.CreateUserId,
                UpdateDate = entity.UpdateDate,
                UpdateUserId = "Admin"
            };
        }
        private SellOrderDetail ToSellOrderDetailModel(TblSellOrderDetail entity)
        {
            return entity == null ? new SellOrderDetail() : new SellOrderDetail
            {
                Id = entity.Id,
                SellOrderNo = entity.SellOrderNo,
                ProductCd = entity.ProductCd,
                ProductName = _context.TblProducts.Where(x => x.ProductCd == entity.ProductCd).FirstOrDefault()?.Name,
                Quantity = entity.Quantity,
                CostPrice = entity.CostPrice,
                Cost = entity.Cost,
                Note = entity.Note
            };
        }
    }
}