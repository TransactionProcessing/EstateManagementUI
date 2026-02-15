using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Reporting
{
    public partial class TransactionSummaryOperator
    {
        private bool isLoading = true;
        
        // Filter states
        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));
        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Now);
        private string _selectedMerchant = "-1";
        private string _selectedOperator = "-1";

        // Data
        private TransactionModels.TransactionSummaryByOperatorResponse summaryData;
        private List<MerchantModels.MerchantDropDownModel>? merchants;
        private List<OperatorModels.OperatorDropDownModel>? operators;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Reporting, PermissionFunction.TransactionMerchantSummaryReport, this.LoadData);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadData()
        {
            try
            {
                isLoading = true;
                errorMessage = null;
                StateHasChanged();

                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                // Load filter options
                var merchantsTask = Mediator.Send(new MerchantQueries.GetMerchantsForDropDownQuery(correlationId, estateId));
                var operatorsTask = Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(correlationId, estateId));

                await Task.WhenAll(merchantsTask, operatorsTask);

                if (merchantsTask.Result.IsSuccess)
                    merchants = ModelFactory.ConvertFrom(merchantsTask.Result.Data);

                if (operatorsTask.Result.IsSuccess)
                    operators = ModelFactory.ConvertFrom(operatorsTask.Result.Data);

                // Load summary data
                return await LoadSummaryData();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load data: {ex.Message}";
                return Result.Failure();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task<Result> LoadSummaryData()
        {
            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var startDate = _startDate.ToDateTime(TimeOnly.MinValue);
                var endDate = _endDate.ToDateTime(TimeOnly.MaxValue);

                Int32? merchant = null;
                Int32? @operator = null;
                if (this._selectedMerchant != "-1")
                {
                    merchant = Int32.Parse(this._selectedMerchant);
                }
                if (this._selectedOperator != "-1")
                {
                    @operator = Int32.Parse(this._selectedOperator);
                }

                var result = await Mediator.Send(new TransactionQueries.GetOperatorTransactionSummaryQuery(
                    correlationId,
                    estateId,
                    startDate,
                    endDate,
                    merchant,
                    @operator));

                if (result.IsSuccess && result.Data != null)
                {
                    summaryData = ModelFactory.ConvertFrom(result.Data);
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to load summary data";
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load summary data: {ex.Message}";
                return Result.Failure(errorMessage);
            }
        }

        private double CalculateFailureRate(int failed, int total)
        {
            return total > 0 ? (failed * 100.0 / total) : 0;
        }

        private string GetFailureRateClass(double failureRate)
        {
            return failureRate <= 5 ? "text-green-600" : failureRate <= 10 ? "text-yellow-600" : "text-red-600";
        }

        private async Task ApplyFilters()
        {
            await LoadSummaryData();
        }

        private async Task ClearFilters()
        {
            _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
            _endDate = DateOnly.FromDateTime(DateTime.Now);
            _selectedMerchant = "";
            _selectedOperator = "";
            await LoadSummaryData();
        }

        private void DrillDownToDetail(Int32 operatorReportingId)
        {
            // Navigate to transaction detail page with merchant filter and return URL
            var startDate = _startDate.ToString("yyyy-MM-dd");
            var endDate = _endDate.ToString("yyyy-MM-dd");
            Navigation.NavigateTo($"/reporting/transaction-detail?operatorId={operatorReportingId}&startDate={startDate}&endDate={endDate}&returnUrl=/reporting/transaction-summary-operator");
        }
    }
}
