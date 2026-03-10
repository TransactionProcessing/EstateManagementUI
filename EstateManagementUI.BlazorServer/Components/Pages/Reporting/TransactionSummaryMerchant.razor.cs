using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Reporting
{
    public partial class TransactionSummaryMerchant
    {
        private bool isLoading = true;

        // Filter states
        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Now);
        private String _selectedMerchant = "-1";
        private String _selectedOperator = "-1";

        // Data
        private TransactionModels.TransactionSummaryByMerchantResponse summaryData;
        private List<MerchantModels.MerchantDropDownModel>? merchants;
        private List<OperatorModels.OperatorDropDownModel>? operators;

        // KPIs
        private int totalMerchants = 0;
        private int totalTransactions = 0;
        private decimal totalValue = 0;
        private decimal averageValue = 0;

        // Sorting
        private string _sortColumn = "MerchantName";
        private bool _sortAscending = true;

        // Paging
        private int _pageSize = 10;
        private int _currentPage = 1;

        private int TotalPages => summaryData?.Merchants != null
            ? (int)Math.Ceiling(summaryData.Merchants.Count / (double)_pageSize)
            : 0;

        private IEnumerable<TransactionModels.MerchantDetail> PagedMerchants
        {
            get
            {
                if (summaryData?.Merchants == null)
                    return Enumerable.Empty<TransactionModels.MerchantDetail>();

                IEnumerable<TransactionModels.MerchantDetail> sorted = _sortColumn switch
                {
                    "TotalCount" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.TotalCount)
                        : summaryData.Merchants.OrderByDescending(m => m.TotalCount),
                    "TotalValue" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.TotalValue)
                        : summaryData.Merchants.OrderByDescending(m => m.TotalValue),
                    "AverageValue" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.AverageValue)
                        : summaryData.Merchants.OrderByDescending(m => m.AverageValue),
                    "AuthorisedCount" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.AuthorisedCount)
                        : summaryData.Merchants.OrderByDescending(m => m.AuthorisedCount),
                    "DeclinedCount" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.DeclinedCount)
                        : summaryData.Merchants.OrderByDescending(m => m.DeclinedCount),
                    "AuthorisedPercentage" => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.AuthorisedPercentage)
                        : summaryData.Merchants.OrderByDescending(m => m.AuthorisedPercentage),
                    _ => _sortAscending
                        ? summaryData.Merchants.OrderBy(m => m.MerchantName)
                        : summaryData.Merchants.OrderByDescending(m => m.MerchantName),
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
                if (this._selectedMerchant != "-1") {
                    merchant = Int32.Parse(this._selectedMerchant);
                }

                if (this._selectedOperator != "-1") {
                    @operator = Int32.Parse(this._selectedOperator);
                }

                var result = await this.TransactionUiService.GetMerchantTransactionSummary(correlationId, estateId, startDate, endDate, merchant, @operator);

                if (result.IsSuccess && result.Data != null) {
                    summaryData = result.Data;
                    _currentPage = 1;
                }
                else {
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

        private void DrillDownToDetail(Int32 merchantReportingId)
        {
            // Navigate to transaction detail page with merchant filter and return URL
            var startDate = _startDate.ToString("yyyy-MM-dd");
            var endDate = _endDate.ToString("yyyy-MM-dd");
            Navigation.NavigateTo($"/reporting/transaction-detail?merchantId={merchantReportingId}&startDate={startDate}&endDate={endDate}&returnUrl=/reporting/transaction-summary-merchant");
        }
    }
}

