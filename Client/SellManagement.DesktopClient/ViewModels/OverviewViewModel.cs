using SellManagement.DesktopClient.Services;
using SellManagement.DesktopClient.Services.Overview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SellManagement.DesktopClient.ViewModels
{
    public class OverviewViewModel:BaseViewModel
    {

        public OverviewViewModel()
        {
        }

        async Task GetOverviewInfoAsync()
        {
            var response = await ServiceProvider.GetInstance().CallWebApi<object, GetOverviewInfoResponse>("Overview/GetOverviewInfo", HttpMethod.Post, null);

            if (response.StatusCode == 200)
            {
                TodaySaleCount = response.OverviewData.TodaySaleCount;
                TodaySaleCash = response.OverviewData.TodaySaleCash;
                TodaySaleOrderCount = response.OverviewData.TodaySaleOrderCount;
                TodaySaleOrderCash = response.OverviewData.TodaySaleOrderCash;
                TodaySaleOrderCancelCount = response.OverviewData.TodaySaleOrderCancelCount;
                TodaySaleOrderCancelCash = response.OverviewData.TodaySaleOrderCancelCash;
                ThisMonthPurchaseCash = response.OverviewData.ThisMonthPurchaseCash;
                ThisMonthSaleCash = response.OverviewData.ThisMonthSaleCash;
                ThisMonthSaleProfit = response.OverviewData.ThisMonthSaleProfit;
                Instock = response.OverviewData.Instock;
                InstockValue = response.OverviewData.InstockValue;
                ExpenseCount = response.OverviewData.ExpenseCount;
                ExpenseCash = response.OverviewData.ExpenseCash;
                ForControlCount = response.OverviewData.ForControlCount;
                ForControlCash = response.OverviewData.ForControlCash;
                PaymentCash = response.OverviewData.PaymentCash;
                PaymentCount = response.OverviewData.PaymentCount;
            }

        }

        #region "Properties"
        public int TodaySaleCount
        {
            get { return todaySaleCount; }
            set
            {
                todaySaleCount = value;
                OnPropertyChanged();
            }
        }
        public int TodaySaleCash
        {
            get { return todaySaleCash; }
            set
            {
                todaySaleCash = value;
                OnPropertyChanged();
            }
        }
        public int TodaySaleOrderCount
        {
            get { return todaySaleOrderCount; }
            set
            {
                todaySaleOrderCount = value;
                OnPropertyChanged();
            }
        }
        public int TodaySaleOrderCash
        {
            get { return todaySaleOrderCash; }
            set
            {
                todaySaleOrderCash = value;
                OnPropertyChanged();
            }
        }
        public int TodaySaleOrderCancelCount
        {
            get { return todaySaleOrderCancelCount; }
            set
            {
                todaySaleOrderCancelCount = value;
                OnPropertyChanged();
            }
        }
        public int TodaySaleOrderCancelCash
        {
            get { return todaySaleOrderCancelCash; }
            set
            {
                todaySaleOrderCancelCash = value;
                OnPropertyChanged();
            }
        }
        public int ThisMonthPurchaseCash
        {
            get { return thisMonthPurchaseCash; }
            set
            {
                thisMonthPurchaseCash = value;
                OnPropertyChanged();
            }
        }
        public int ThisMonthSaleCash
        {
            get { return thisMonthSaleCash; }
            set
            {
                thisMonthSaleCash = value;
                OnPropertyChanged();
            }
        }
        public int ThisMonthSaleProfit
        {
            get { return thisMonthSaleProfit; }
            set
            {
                thisMonthSaleProfit = value;
                OnPropertyChanged();
            }
        }
        public int Instock
        {
            get { return instock; }
            set
            {
                instock = value;
                OnPropertyChanged();
            }
        }
        public int InstockValue
        {
            get { return instockValue; }
            set
            {
                instockValue = value;
                OnPropertyChanged();
            }
        }
        public int ExpenseCount
        {
            get { return expenseCount; }
            set
            {
                expenseCount = value;
                OnPropertyChanged();
            }
        }
        public int ExpenseCash
        {
            get { return expenseCash; }
            set
            {
                expenseCash = value;
                OnPropertyChanged();
            }
        }
        public int ForControlCount
        {
            get { return forControlCount; }
            set
            {
                forControlCount = value;
                OnPropertyChanged();
            }
        }
        public int ForControlCash
        {
            get { return forControlCash; }
            set
            {
                forControlCash = value;
                OnPropertyChanged();
            }
        }
        public int PaymentCash
        {
            get { return paymentCash; }
            set
            {
                paymentCash = value;
                OnPropertyChanged();
            }
        }
        public int PaymentCount
        {
            get { return paymentCount; }
            set
            {
                paymentCount = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisble
        {
            set
            {
                if (value == true)
                {
                    new Task(async () =>
                    {
                        await GetOverviewInfoAsync();
                    }).Start();
                }
            }
        }

        #endregion

        #region "Variable"
        int todaySaleCount;
        int todaySaleCash;
        int todaySaleOrderCount;
        int todaySaleOrderCash;
        int todaySaleOrderCancelCount;
        int todaySaleOrderCancelCash;
        int thisMonthPurchaseCash;
        int thisMonthSaleCash;
        int thisMonthSaleProfit;
        int instock;
        int instockValue;
        int expenseCount;
        int expenseCash;
        int forControlCount;
        int forControlCash;
        int paymentCash;
        int paymentCount;
        #endregion
    }
}
