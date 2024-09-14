using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EstateManagementUI.ViewModels;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using SimpleResults;
using static EstateManagementUI.Common.ChartBuilder;

namespace EstateManagementUI.Pages.Reporting.TransactionAnalysis
{
    public class TransactionAnalysis : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        [Display(Name = "Comparison Date")]
        public ComparisonDateListModel ComparisonDate { get; set; }

        public TodaysSales TodaysSales { get; set; }

        public TodaysSales TodaysFailedSales { get; set; }

        public MerchantKpi MerchantKpi { get; set; }

        public TransactionAnalysis(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.FileProcessing, ContractFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.ComparisonDate = await DataHelperFunctions.GetComparisonDates(this.AccessToken, this.EstateId, this.Mediator);

            await this.Query();
        }

        public async Task Query()
        {
            if (this.ComparisonDate.SelectedDate.HasValue)
            {
                await this.PopulateTokenAndEstateId();

                await this.GetTodaysSales(this.ComparisonDate.SelectedDate.Value);
                await this.GetTodaysFailedSales(this.ComparisonDate.SelectedDate.Value, "1009");
                await this.GetMerchantKpis();
                await this.GetBottomMerchantsBySales();
                await this.GetBottomOperatorsBySales();
                await this.GetBottomProductsBySales();

                this.Client.ExecuteJs(ChartHelpers.GetScriptForCharts("bottommerchantsbysaleschart", ChartHelpers.ConvertChartOptionsToHtml(this.GetSalesValueByMerchantChart())));
                this.Client.ExecuteJs(ChartHelpers.GetScriptForCharts("bottomoperatorssbysaleschart", ChartHelpers.ConvertChartOptionsToHtml(this.GetSalesValueByOperatorChart())));
                this.Client.ExecuteJs(ChartHelpers.GetScriptForCharts("bottomproductsbysaleschart", ChartHelpers.ConvertChartOptionsToHtml(this.GetSalesValueByProductChart())));
            }
        }

        private async Task GetMerchantKpis() {
            Queries.GetMerchantKpiQuery getMerchantKpiQuery = new(this.AccessToken, this.EstateId);

            Result<MerchantKpiModel> getMerchantKpiQueryResult = await this.Mediator.Send(getMerchantKpiQuery, CancellationToken.None);
            if (getMerchantKpiQueryResult.IsSuccess && getMerchantKpiQueryResult.Data != null)
            {
                this.MerchantKpi = new MerchantKpi
                {
                    MerchantsWithNoSaleInLast7Days = getMerchantKpiQueryResult.Data.MerchantsWithNoSaleInLast7Days,
                    MerchantsWithNoSaleToday = getMerchantKpiQueryResult.Data.MerchantsWithNoSaleToday,
                    MerchantsWithSaleInLastHour = getMerchantKpiQueryResult.Data.MerchantsWithSaleInLastHour
                };
            }
            else
            {
                this.MerchantKpi = new MerchantKpi();
            }
        }

        private async Task GetTodaysSales(DateTime selectedDate)
        {
            Queries.GetTodaysSalesQuery getTodaysSalesQuery = new(this.AccessToken,
                this.EstateId, selectedDate);

            Result<TodaysSalesModel> getTodaysSalesQueryResult = await this.Mediator.Send(getTodaysSalesQuery, CancellationToken.None);
            if (getTodaysSalesQueryResult.IsSuccess && getTodaysSalesQueryResult.Data != null)
            {
                this.TodaysSales = new TodaysSales
                {
                    ComparisonSalesValue = getTodaysSalesQueryResult.Data.ComparisonSalesValue,
                    Variance = (getTodaysSalesQueryResult.Data.TodaysSalesValue - getTodaysSalesQueryResult.Data.ComparisonSalesValue).SafeDivision(getTodaysSalesQueryResult.Data.TodaysSalesValue),
                    TodaysSalesValue = getTodaysSalesQueryResult.Data.TodaysSalesValue,
                    ComparisonLabel = $"{selectedDate:yyyy-MM-dd} Sales",
                };
            }
            else
            {
                this.TodaysSales = new TodaysSales();
            }
        }

        private async Task GetBottomMerchantsBySales()
        {
            Queries.GetBottomMerchantDataQuery getBottomMerchantDataQuery = new(this.AccessToken,
                this.EstateId,3);

            var getBottomMerchantDataQueryResult = await this.Mediator.Send(getBottomMerchantDataQuery, CancellationToken.None);
            if (getBottomMerchantDataQueryResult.IsSuccess && getBottomMerchantDataQueryResult.Data != null) {
                this.TopBottomMerchants = new List<TopBottomMerchant>();
                foreach (TopBottomMerchantDataModel topBottomMerchantDataModel in getBottomMerchantDataQueryResult.Data) {
                    this.TopBottomMerchants.Add(new TopBottomMerchant {
                        MerchantName = topBottomMerchantDataModel.MerchantName,
                        SalesValue = topBottomMerchantDataModel.SalesValue
                    });
                }
            }
            else
            {
                this.TopBottomMerchants = new();
            }
        }

        private async Task GetBottomOperatorsBySales()
        {
            Queries.GetBottomOperatorDataQuery getBottomOperatorDataQuery = new(this.AccessToken,
                this.EstateId, 3);

            var getBottomOperatorDataQueryResult = await this.Mediator.Send(getBottomOperatorDataQuery, CancellationToken.None);
            if (getBottomOperatorDataQueryResult.IsSuccess && getBottomOperatorDataQueryResult.Data != null)
            {
                this.TopBottomOperators = new();
                foreach (var topBottomOperatorDataModel in getBottomOperatorDataQueryResult.Data)
                {
                    this.TopBottomOperators.Add(new TopBottomOperator
                    {
                        OperatorName = topBottomOperatorDataModel.OperatorName,
                        SalesValue = topBottomOperatorDataModel.SalesValue
                    });
                }
            }
            else
            {
                this.TopBottomOperators = new();
            }
        }

        private async Task GetBottomProductsBySales()
        {
            Queries.GetBottomProductDataQuery getBottomProductDataQuery = new(this.AccessToken,
                this.EstateId, 3);

            var getBottomProductDataQueryResult = await this.Mediator.Send(getBottomProductDataQuery, CancellationToken.None);
            if (getBottomProductDataQueryResult.IsSuccess && getBottomProductDataQueryResult.Data != null)
            {
                this.TopBottomProducts = new List<TopBottomProduct>();
                foreach (var topBottomProductDataModel in getBottomProductDataQueryResult.Data)
                {
                    this.TopBottomProducts.Add(new TopBottomProduct
                    {
                        ProductName = topBottomProductDataModel.ProductName,
                        SalesValue = topBottomProductDataModel.SalesValue
                    });
                }
            }
            else
            {
                this.TopBottomProducts = new();
            }
        }

        private async Task GetTodaysFailedSales(DateTime selectedDate, String responseCode)
        {
            Queries.GetTodaysFailedSalesQuery getTodaysFailedSalesQuery = new(this.AccessToken,
                this.EstateId, responseCode,  selectedDate);

            Result<TodaysSalesModel> getTodaysFailedSalesQueryResult = await this.Mediator.Send(getTodaysFailedSalesQuery, CancellationToken.None);
            if (getTodaysFailedSalesQueryResult.IsSuccess && getTodaysFailedSalesQueryResult.Data != null)
            {
                this.TodaysFailedSales = new TodaysSales
                {
                    ComparisonSalesValue = getTodaysFailedSalesQueryResult.Data.ComparisonSalesValue,
                    Variance = (getTodaysFailedSalesQueryResult.Data.TodaysSalesValue - getTodaysFailedSalesQueryResult.Data.ComparisonSalesValue).SafeDivision(getTodaysFailedSalesQueryResult.Data.TodaysSalesValue),
                    TodaysSalesValue = getTodaysFailedSalesQueryResult.Data.TodaysSalesValue,
                    ComparisonLabel = $"{selectedDate:yyyy-MM-dd} Sales",
                };
            }
            else
            {
                this.TodaysFailedSales = new TodaysSales();
            }
        }

        public List<TopBottomMerchant> TopBottomMerchants { get; set; }
        public List<TopBottomOperator> TopBottomOperators { get; set; }
        public List<TopBottomProduct> TopBottomProducts { get; set; }

        private String GetSalesValueByMerchantChart()
        {
            List<ChartObjects.Series<Decimal>> seriesList = new List<ChartObjects.Series<Decimal>>();
            List<String> categoryList = new List<String>();
            IOrderedEnumerable<TopBottomMerchant> orderedList = this.TopBottomMerchants.OrderBy(o => o.SalesValue);
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Sales"
            });
            foreach (TopBottomMerchant topBottomMerchant in orderedList)
            {
                categoryList.Add(topBottomMerchant.MerchantName);
                seriesList[0].Data.Add(topBottomMerchant.SalesValue);
            }

            return ChartBuilder.BuildBarChartOptions(categoryList, seriesList, "bar", $"Today's Bottom 3 Merchants",
                yAxisFormatFunction: StandardJavascriptFunctions.CurrencyFormatter);
        }

        private String GetSalesValueByOperatorChart()
        {
            List<ChartObjects.Series<Decimal>> seriesList = new List<ChartObjects.Series<Decimal>>();
            List<String> categoryList = new List<String>();
            IOrderedEnumerable<TopBottomOperator> orderedList = this.TopBottomOperators.OrderBy(o => o.SalesValue);
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Sales"
            });
            foreach (TopBottomOperator topBottomOperator in orderedList)
            {
                categoryList.Add(topBottomOperator.OperatorName);
                seriesList[0].Data.Add(topBottomOperator.SalesValue);
            }

            return ChartBuilder.BuildBarChartOptions(categoryList, seriesList, "bar", $"Today's Bottom 3 Operators",
                yAxisFormatFunction: StandardJavascriptFunctions.CurrencyFormatter);
        }

        private String GetSalesValueByProductChart()
        {
            List<ChartObjects.Series<Decimal>> seriesList = new List<ChartObjects.Series<Decimal>>();
            List<String> categoryList = new List<String>();
            IOrderedEnumerable<TopBottomProduct> orderedList = this.TopBottomProducts.OrderBy(o => o.SalesValue);
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Sales"
            });
            foreach (TopBottomProduct topBottomProduct in orderedList)
            {
                categoryList.Add(topBottomProduct.ProductName);
                seriesList[0].Data.Add(topBottomProduct.SalesValue);
            }

            return ChartBuilder.BuildBarChartOptions(categoryList, seriesList, "bar", $"Today's Bottom 3 Products",
                yAxisFormatFunction: StandardJavascriptFunctions.CurrencyFormatter);
        }
    }
}
