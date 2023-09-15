using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Functions
{
    public class Overview
    {
        public int TodaySaleCount { get; set; }
        public int TodaySaleCash { get; set; }
        public int TodaySaleOrderCount { get; set; }
        public int TodaySaleOrderCash { get; set; }
        public int TodaySaleOrderCancelCount { get; set; }
        public int TodaySaleOrderCancelCash { get; set; }
        public int ThisMonthPurchaseCash { get; set; }
        public int ThisMonthSaleCash { get; set; }
        public int ThisMonthSaleProfit { get; set; }
        public int Instock { get; set; }
        public int InstockValue { get; set; }
        public int ExpenseCount { get; set; }
        public int ExpenseCash { get; set; }
        public int ForControlCount { get; set; }
        public int ForControlCash { get; set; }
        public int PaymentCash { get; set; }
        public int PaymentCount { get; set; }

    }
}
