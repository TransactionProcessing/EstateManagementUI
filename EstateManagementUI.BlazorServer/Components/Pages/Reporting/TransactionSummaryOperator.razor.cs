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

        // Sorting
        private string _sortColumn = "OperatorName";
        private bool _sortAscending = true;

        // Paging
        private int _pageSize = 10;
        private int _currentPage = 1;

        private int TotalPages => summaryData?.Operators != null
            ? (int)Math.Ceiling(summaryData.Operators.Count / (double)_pageSize)
            : 0;

        private IEnumerable<TransactionModels.OperatorDetail> PagedOperators
        {
            get
            {
                if (summaryData?.Operators == null)
                    return Enumerable.Empty<TransactionModels.OperatorDetail>();

                IEnumerable<TransactionModels.OperatorDetail> sorted = _sortColumn switch
                {
                    "TotalCount" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.TotalCount)
                        : summaryData.Operators.OrderByDescending(o => o.TotalCount),
                    "TotalValue" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.TotalValue)
                        : summaryData.Operators.OrderByDescending(o => o.TotalValue),
                    "AverageValue" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.AverageValue)
                        : summaryData.Operators.OrderByDescending(o => o.AverageValue),
                    "AuthorisedCount" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.AuthorisedCount)
                        : summaryData.Operators.OrderByDescending(o => o.AuthorisedCount),
                    "DeclinedCount" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.DeclinedCount)
                        : summaryData.Operators.OrderByDescending(o => o.DeclinedCount),
                    "AuthorisedPercentage" => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.AuthorisedPercentage)
                        : summaryData.Operators.OrderByDescending(o => o.AuthorisedPercentage),
                    _ => _sortAscending
                        ? summaryData.Operators.OrderBy(o => o.OperatorName)
                        : summaryData.Operators.OrderByDescending(o => o.OperatorName),
                };

                return sorted.Skip((_currentPage - 1) * _pageSize).Take(_pageSize);
            }
        }

        private void SortBy(string column)
        {
            if (_sortColumn == column)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortColumn = column;
                _sortAscending = true;
            }
            _currentPage = 1;
        }

        private void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                _currentPage = page;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Reporting, PermissionFunction.TransactionOperatorSummaryReport, this.LoadData);
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
                var merchantsTask = this.MerchantUiService.GetMerchantsForDropDown(correlationId, estateId);
                var operatorsTask = this.OperatorUiService.GetOperatorsForDropDown(correlationId, estateId);

                await Task.WhenAll(merchantsTask, operatorsTask);

                if (merchantsTask.Result.IsSuccess)
                    merchants = merchantsTask.Result.Data;

                if (operatorsTask.Result.IsSuccess)
                    operators = operatorsTask.Result.Data;

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

                var result = await this.TransactionUiService.GetOperatorTransactionSummary(correlationId, estateId, startDate, endDate, merchant, @operator);
                
                if (result.IsSuccess && result.Data != null)
                {
                    summaryData = result.Data;
                    _currentPage = 1;
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
            _selectedMerchant = "-1";
            _selectedOperator = "-1";
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
