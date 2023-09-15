using Microsoft.EntityFrameworkCore;
using SellManagement.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class OverviewFunction : IOverviewFunction
    {
        SellManagementContext _context;
        public OverviewFunction(SellManagementContext context)
        {
            _context = context;
        }
        public async Task<Overview> GetOverviewInfo()
        {
            var result = new Overview();

            //Today sale
            var entities = await _context.TblSellOrderHeads.Where(x => x.SellOrderDate == DateTime.Now.Date)
                                                            .Where(x => x.Status == 2).ToListAsync();
            result.TodaySaleCount = entities.Count();
            result.TodaySaleCash = entities.Sum(x => x.SummaryCost + x.ShippingCost - x.SaleOffCost);

            //Today sale order
            var entities_2 = await _context.TblSellOrderHeads.Where(x => x.SellOrderDate == DateTime.Now.Date)
                                                            .Where(x => x.Status == 0 || x.Status == 1).ToListAsync();
            result.TodaySaleOrderCount = entities_2.Count();
            result.TodaySaleOrderCash = entities_2.Sum(x => x.SummaryCost + x.ShippingCost - x.SaleOffCost);

            //Today cancel
            var entities_3 = await _context.TblSellOrderHeads.Where(x => x.CancelDate == DateTime.Now.Date)
                                                            .Where(x => x.Status == 9).ToListAsync();
            result.TodaySaleOrderCancelCount = entities_3.Count();
            result.TodaySaleOrderCancelCash = entities_3.Sum(x => x.SummaryCost + x.ShippingCost - x.SaleOffCost);

            //This month cash
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1);
            var entities_4 = await _context.TblSellOrderHeads.Where(x => x.SellOrderDate >= startDate)
                                                            .Where(x=> x.SellOrderDate <= endDate)
                                                            .Where(x => x.Status == 2).ToListAsync();

            result.ThisMonthSaleCash = entities_4.Sum(x => x.SummaryCost + x.ShippingCost - x.SaleOffCost - x.WaybillCost);

            var entities_5 = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseOrderDate >= startDate)
                                                                .Where(x => x.PurchaseOrderDate <= endDate)
                                                                .Where(x => x.Status == 2).ToListAsync();

            result.ThisMonthPurchaseCash = entities_5.Sum(x => x.SummaryCost - x.SaleOffCost);
            result.ThisMonthSaleProfit = result.ThisMonthSaleCash - result.ThisMonthPurchaseCash;

            result.Instock = await _context.TblProductInventories.SumAsync(x => x.InStock);
            result.InstockValue = await _context.TblProductInventories.SumAsync(x => x.InStock * x.AveragePurchasePrice);


            var entities_6 = await _context.TblSellOrderHeads.Where(x => x.SellCost > 0)
                                                            .Where(x => x.Status == 2)
                                                            .Where(x => x.ForControlStatus == 9).ToListAsync();
            result.PaymentCount  = entities_6.Count();
            result.PaymentCash = entities_6.Sum(x => x.SellCost);

            var entities_7 = await _context.TblSellOrderHeads.Where(x => x.Status == 2)
                                                            .Where(x=>x.ForControlStatus == 0).ToListAsync();
            result.ForControlCount = entities_7.Count();
            result.ForControlCash = entities_7.Sum(x => x.CollectOnDeliveryCash - x.WaybillCost);

            var entities_8 = await _context.TblPurchaseOrderHeads.Where(x => x.PurchaseCost >0)
                                                            .Where(x => x.Status == 2).ToListAsync();
            result.ExpenseCount = entities_8.Count();
            result.ExpenseCash = entities_8.Sum(x => x.PurchaseCost);

            return result;
        }
    }
}
