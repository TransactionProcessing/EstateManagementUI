using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EstateManagementUI.ViewModels;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using SimpleResults;
using static EstateManagementUI.Common.ChartBuilder;

namespace EstateManagementUI.Pages.Reporting.SettlementAnalysis
{
    public class SettlementAnalysis : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        [Display(Name = "Comparison Date")]
        public ComparisonDateListModel ComparisonDate { get; set; }
        
        public SettlementAnalysis(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Reporting, ReportingFunctions.SettlementAnalysis, permissionsService)
        {
            this.Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.ComparisonDate = await DataHelperFunctions.GetComparisonDates(this.AccessToken, this.EstateId, this.Mediator);

            await this.Query();
        }

        public TodaysSettlement TodaysSettlement { get; set; }
        public LastSettlement LastSettlement { get; set; }

        public async Task Query()
        {
            if (this.ComparisonDate.SelectedDate.HasValue)
            {
                await this.PopulateTokenAndEstateId();

                await this.GetTodaysSettlement(this.ComparisonDate.SelectedDate.Value);
                await this.GetLastSettlement();
            }
        }

        private async Task GetTodaysSettlement(DateTime selectedDate)
        {
            Queries.GetTodaysSettlementQuery getTodaysSettlementQuery = new(this.AccessToken,
                this.EstateId, selectedDate);

            Result<TodaysSettlementModel> getTodaysSettlementQueryResult = await this.Mediator.Send(getTodaysSettlementQuery, CancellationToken.None);
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

        private async Task GetLastSettlement()
        {
            Queries.GetLastSettlementQuery getLastSettlementQuery = new(this.AccessToken,
                this.EstateId);

            Result<LastSettlementModel> getLastSettlementQueryResult = await this.Mediator.Send(getLastSettlementQuery, CancellationToken.None);
            if (getLastSettlementQueryResult.IsSuccess)
            {
                this.LastSettlement = new LastSettlement
                {
                    SettlementDate = getLastSettlementQueryResult.Data.SettlementDate,
                    SettlementFeesValue = getLastSettlementQueryResult.Data.FeesValue,
                    SettlementSalesValue = getLastSettlementQueryResult.Data.SalesValue
                };
            }
            else
            {
                this.LastSettlement = new LastSettlement();
            }
        }


    }
}
