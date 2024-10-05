using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using EstateManagementUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using SimpleResults;
using EstateReportingAPI.DataTransferObjects;
using System.Reflection.Metadata;
using System;
using System.Linq.Expressions;
using EstateManagementUI.Pages.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using static EstateManagementUI.Common.ChartBuilder;
using TodaysSales = EstateManagementUI.ViewModels.TodaysSales;
using TodaysSalesCountByHour = EstateManagementUI.ViewModels.TodaysSalesCountByHour;
using TodaysSalesCountByHourModel = EstateManagementUI.ViewModels.TodaysSalesCountByHourModel;
using TodaysSalesValueByHour = EstateManagementUI.ViewModels.TodaysSalesValueByHour;
using TodaysSalesValueByHourModel = EstateManagementUI.ViewModels.TodaysSalesValueByHourModel;
using TodaysSettlement = EstateManagementUI.ViewModels.TodaysSettlement;

namespace EstateManagementUI.Pages.Dashboard.Dashboard
{
    public class Dashboard : SecureHydroComponent
    {
        [Display(Name = "Merchant")]
        public MerchantListModel Merchant { get; set; }

        [Display(Name = "Operator")]
        public OperatorListModel Operator { get; set; }

        [Display(Name = "Comparison Date")]
        public ComparisonDateListModel ComparisonDate { get; set; }

        private readonly IMediator Mediator;

        public Dashboard(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Dashboard, DashboardFunctions.Dashboard, permissionsService)
        {
            this.Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.Merchant = await DataHelperFunctions.GetMerchants(this.AccessToken, this.EstateId, this.Mediator);
            this.Operator =  await DataHelperFunctions.GetOperators(this.AccessToken, this.EstateId, this.Mediator);
            this.ComparisonDate = await DataHelperFunctions.GetComparisonDates(this.AccessToken, this.EstateId, this.Mediator );

            await this.Query();
        }

        private async Task GetTodaysSales(DateTime selectedDate) {
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

        private async Task GetTodaysSettlement(DateTime selectedDate) {
            Queries.GetTodaysSettlementQuery getTodaysSettlemntQuery = new(this.AccessToken,
                this.EstateId, selectedDate);

            Result<TodaysSettlementModel> getTodaysSettlementQueryResult = await this.Mediator.Send(getTodaysSettlemntQuery, CancellationToken.None);
            if (getTodaysSettlementQueryResult.IsSuccess)
            {
                this.TodaysSettlement = new TodaysSettlement
                {
                    ComparisonLabel = $"{selectedDate:yyyy-MM-dd} Settlement",
                    TodaysSettlementValue = getTodaysSettlementQueryResult.Data.TodaysSettlementValue,
                    ComparisonSettlementValue = getTodaysSettlementQueryResult.Data.ComparisonSettlementValue,
                    Variance = (getTodaysSettlementQueryResult.Data.TodaysSettlementValue -
                                getTodaysSettlementQueryResult.Data.ComparisonSettlementValue)
                        .SafeDivision(getTodaysSettlementQueryResult.Data.TodaysSettlementValue),
                };
            }
            else
            {
                this.TodaysSettlement = new TodaysSettlement();
            }
        }

        private async Task GetTodaysSalesCountByHour(DateTime selectedDate) {
            Queries.GetTodaysSalesCountByHourQuery getTodaysSalesCountByHourQuery =
                new(this.AccessToken, this.EstateId, selectedDate);

            Result<List<BusinessLogic.Models.TodaysSalesCountByHourModel>> getTodaysSalesCountByHourQueryResult = await this.Mediator.Send(getTodaysSalesCountByHourQuery, CancellationToken.None);
            if (getTodaysSalesCountByHourQueryResult.IsSuccess && getTodaysSalesCountByHourQueryResult.Data != null)
            {
                this.TodaysSalesCountByHour = new TodaysSalesCountByHour
                {
                    Hours = new List<TodaysSalesCountByHourModel>()
                };

                foreach (BusinessLogic.Models.TodaysSalesCountByHourModel todaysSalesCountByHourModel in getTodaysSalesCountByHourQueryResult.Data)
                {
                    this.TodaysSalesCountByHour.Hours.Add(new TodaysSalesCountByHourModel
                    {
                        ComparisonSalesCount = todaysSalesCountByHourModel.ComparisonSalesCount,
                        Hour = todaysSalesCountByHourModel.Hour,
                        TodaysSalesCount = todaysSalesCountByHourModel.TodaysSalesCount
                    });
                }
            }
            else
            {
                this.TodaysSalesCountByHour = new TodaysSalesCountByHour
                {
                    Hours = new List<TodaysSalesCountByHourModel>()
                };
            }
        }

        private async Task GetTodaysSalesValueByHour(DateTime selectedDate) {
            Queries.GetTodaysSalesValueByHourQuery getTodaysSalesValueByHourQuery =
                new(this.AccessToken, this.EstateId, selectedDate);


            Result<List<BusinessLogic.Models.TodaysSalesValueByHourModel>> getTodaysSalesValueByHourQueryResult = await this.Mediator.Send(getTodaysSalesValueByHourQuery, CancellationToken.None);
            if (getTodaysSalesValueByHourQueryResult.IsSuccess && getTodaysSalesValueByHourQueryResult.Data != null)
            {
                this.TodaysSalesValueByHour = new TodaysSalesValueByHour
                {
                    Hours = new List<TodaysSalesValueByHourModel>()
                };

                foreach (BusinessLogic.Models.TodaysSalesValueByHourModel todaysSalesValueByHourModel in getTodaysSalesValueByHourQueryResult.Data)
                {
                    this.TodaysSalesValueByHour.Hours.Add(new TodaysSalesValueByHourModel
                    {
                        ComparisonSalesValue = todaysSalesValueByHourModel.ComparisonSalesValue,
                        Hour = todaysSalesValueByHourModel.Hour,
                        TodaysSalesValue = todaysSalesValueByHourModel.TodaysSalesValue
                    });
                }
            }
            else
            {
                this.TodaysSalesValueByHour = new TodaysSalesValueByHour
                {
                    Hours = new List<TodaysSalesValueByHourModel>()
                };
            }
        }


        public async Task Query() {

            if (this.ComparisonDate.SelectedDate.HasValue) {

                await this.PopulateTokenAndEstateId();

                await this.GetTodaysSales(this.ComparisonDate.SelectedDate.Value);
                await this.GetTodaysSettlement(this.ComparisonDate.SelectedDate.Value);
                await this.GetTodaysSalesCountByHour(this.ComparisonDate.SelectedDate.Value);
                await this.GetTodaysSalesValueByHour(this.ComparisonDate.SelectedDate.Value);

                this.Client.ExecuteJs(ChartHelpers.GetScriptForCharts("salesvaluebyhourchart", ChartHelpers.ConvertChartOptionsToHtml(this.GetSalesValueByHourChart())));
                this.Client.ExecuteJs(ChartHelpers.GetScriptForCharts("salescountbyhourchart", ChartHelpers.ConvertChartOptionsToHtml(this.GetSalesCountByHourChart())));
            }
        }
        
        private String GetSalesValueByHourChart() {
            List<ChartObjects.Series<Decimal>> seriesList = new List<ChartObjects.Series<Decimal>>();
            List<String> categoryList = new List<String>();
            IOrderedEnumerable<TodaysSalesValueByHourModel> orderedList = this.TodaysSalesValueByHour.Hours.OrderBy(o => o.Hour);
            seriesList.Add(new ChartObjects.Series<Decimal> {
                Name = "Todays Sales"
            });
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Comparison Sales"
            });
            foreach (TodaysSalesValueByHourModel todaysSalesValueByHourModel in orderedList) {
                categoryList.Add(todaysSalesValueByHourModel.Hour.ToString());
                seriesList[0].Data.Add(todaysSalesValueByHourModel.TodaysSalesValue);
                seriesList[1].Data.Add(todaysSalesValueByHourModel.ComparisonSalesValue);
            }

            return ChartBuilder.BuildLineChartOptions(categoryList, seriesList, "line", $"Sales Value Comparison - Today vs {this.ComparisonDate.SelectedDate.Value:yyyy-MM-dd}",
                yAxisFormatFunction: StandardJavascriptFunctions.CurrencyFormatter);
        }

        private String GetSalesCountByHourChart()
        {
            List<ChartObjects.Series<Decimal>> seriesList = new List<ChartObjects.Series<Decimal>>();
            List<String> categoryList = new List<String>();
            IOrderedEnumerable<TodaysSalesCountByHourModel> orderedList = this.TodaysSalesCountByHour.Hours.OrderBy(o => o.Hour);
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Todays Sales"
            });
            seriesList.Add(new ChartObjects.Series<Decimal>
            {
                Name = "Comparison Sales"
            });
            foreach (TodaysSalesCountByHourModel todaysSalesCountByHourModel in orderedList)
            {
                categoryList.Add(todaysSalesCountByHourModel.Hour.ToString());
                seriesList[0].Data.Add(todaysSalesCountByHourModel.TodaysSalesCount);
                seriesList[1].Data.Add(todaysSalesCountByHourModel.ComparisonSalesCount);
            }

            return ChartBuilder.BuildLineChartOptions(categoryList, seriesList, "line", $"Sales Count Comparison - Today vs {this.ComparisonDate.SelectedDate.Value:yyyy-MM-dd}",
                yAxisFormatFunction: StandardJavascriptFunctions.CurrencyFormatter);
        }

        public TodaysSales TodaysSales { get; set; }
        public TodaysSettlement TodaysSettlement { get; set; }
        public TodaysSalesValueByHour TodaysSalesValueByHour { get; set; }
        public TodaysSalesCountByHour TodaysSalesCountByHour { get; set; }
    }

}