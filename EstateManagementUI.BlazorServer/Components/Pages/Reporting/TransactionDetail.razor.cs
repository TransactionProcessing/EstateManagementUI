using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.JSInterop;
using System.Text;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Reporting
{
    public partial class TransactionDetail
    {
        private bool isLoading = true;
        
        // Filter states
        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Now);
        private List<string> _selectedMerchantIds = new List<string>();
        private List<string> _selectedOperatorIds = new List<string>();
        private List<string> _selectedProductIds = new List<string>();

        // Data
        private TransactionModels.TransactionDetailReportResponse? detailData;
        private List<MerchantModels.MerchantDropDownModel>? merchants;
        private List<OperatorModels.OperatorDropDownModel>? operators;
        private List<ContractModels.ContractProductModel>? products;

        // KPIs
        private int totalTransactions = 0;
        private decimal totalGrossAmount = 0;
        private decimal totalFees = 0;
        private decimal totalNetAmount = 0;

        // Sorting
        private string? _currentSortColumn;
        private bool _sortAscending = true;

        // Paging
        private int _currentPage = 1;
        private int _pageSize = 25;
        private int _totalPages = 0;

        private List<TransactionModels.TransactionDetail> GetPagedData()
        {
            if (detailData.Transactions == null || detailData.Transactions.Any() == false)
                return new List<TransactionModels.TransactionDetail>();

            _totalPages = (int)Math.Ceiling(detailData.Transactions.Count / (double)_pageSize);

            return detailData.Transactions
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > _totalPages)
                return;

            _currentPage = page;
        }

        private void NextPage()
        {
            if (_currentPage < _totalPages)
                _currentPage++;
        }

        private void PreviousPage()
        {
            if (_currentPage > 1)
                _currentPage--;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            // Check for query parameters (from drill-down)
            var uri = new Uri(Navigation.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

            if (!string.IsNullOrEmpty(query["merchantId"]))
            {
                _selectedMerchantIds = new List<string> { query["merchantId"] };
            }

            if (!string.IsNullOrEmpty(query["startDate"]))
            {
                if (DateOnly.TryParse(query["startDate"], out var startDate))
                {
                    _startDate = startDate;
                }
            }

            if (!string.IsNullOrEmpty(query["endDate"]))
            {
                if (DateOnly.TryParse(query["endDate"], out var endDate))
                {
                    _endDate = endDate;
                }
            }

            Result result = await OnAfterRender(PermissionSection.Reporting, PermissionFunction.TransactionDetailReport, this.LoadData);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }


        private async Task<Result> LoadData() {
            isLoading = true;
            errorMessage = null;
            StateHasChanged();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            // Load filter options
            var merchantsTask = Mediator.Send(new MerchantQueries.GetMerchantsForDropDownQuery(correlationId, estateId));
            var operatorsTask = Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(correlationId, estateId));
            var contractsTask = Mediator.Send(new ContractQueries.GetContractsQuery(correlationId, estateId));

            await Task.WhenAll(merchantsTask, operatorsTask, contractsTask);

            if (merchantsTask.Result.IsSuccess)
                merchants = ModelFactory.ConvertFrom(merchantsTask.Result.Data);

            if (operatorsTask.Result.IsSuccess)
                operators = ModelFactory.ConvertFrom(operatorsTask.Result.Data);

            if (contractsTask.Result.IsSuccess) {
                // Extract all products from all contracts
                products = contractsTask.Result.Data?.SelectMany(c => ModelFactory.ConvertFrom(c.Products) ?? new List<ContractModels.ContractProductModel>()).Where(p => !string.IsNullOrEmpty(p.ProductName)).DistinctBy(p => p.ProductName).ToList();
            }

            // Load detail data
            var result = await LoadDetailData();

            if (result.IsFailed) {
                errorMessage = "Failed to load transaction detail data";
            }

            isLoading = false;
            StateHasChanged();

            return Result.Success();
        }

        private async Task<Result> LoadDetailData() {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            // Parse selected IDs with error handling
            var merchantIds = this.ParseInt32List(_selectedMerchantIds);
            var operatorIds = this.ParseInt32List(_selectedOperatorIds);
            var productIds = this.ParseInt32List(_selectedProductIds);

            var result = await Mediator.Send(new TransactionQueries.GetTransactionDetailQuery(correlationId, estateId,
                _startDate.ToDateTime(TimeOnly.MinValue), _endDate.ToDateTime(TimeOnly.MinValue), merchantIds, operatorIds, productIds));

            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);

            detailData = ModelFactory.ConvertFrom(result.Data);
            CalculateKPIs();
            return Result.Success();
        }

        private List<Guid>? ParseGuidList(List<string> stringIds)
        {
            if (!stringIds.Any()) return null;

            var guids = new List<Guid>();
            foreach (var id in stringIds)
            {
                if (Guid.TryParse(id, out var guid))
                {
                    guids.Add(guid);
                }
            }
            return guids.Any() ? guids : null;
        }

        private List<Int32>? ParseInt32List(List<string> stringIds)
        {
            if (!stringIds.Any()) return null;

            var ints = new List<Int32>();
            foreach (var id in stringIds)
            {
                if (Int32.TryParse(id, out var intValue))
                {
                    ints.Add(intValue);
                }
            }
            return ints.Any() ? ints : null;
        }

        private void CalculateKPIs()
        {
            if (detailData.Summary == null) {
                totalTransactions = 0;
                totalGrossAmount = 0;
                totalFees = 0;
                totalNetAmount = 0;
                return;
            }

            totalTransactions = detailData.Summary.TransactionCount;
            totalGrossAmount = detailData.Summary.TotalValue;
            totalFees = detailData.Summary.TotalFees;
            totalNetAmount = detailData.Summary.TotalValue - this.detailData.Summary.TotalFees;
        }

        private async Task ApplyFilters()
        {
            _currentPage = 1; // Reset to first page when filters are applied
            await LoadDetailData();
        }

        private async Task ClearFilters()
        {
            _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
            _endDate = DateOnly.FromDateTime(DateTime.Now);
            _selectedMerchantIds = new List<string>();
            _selectedOperatorIds = new List<string>();
            _selectedProductIds = new List<string>();
            _currentPage = 1; // Reset to first page when filters are cleared
            await LoadDetailData();
        }

        private void SortBy(string columnName)
        {
            if (detailData.Transactions == null || !detailData.Transactions.Any())
                return;

            if (_currentSortColumn == columnName)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _currentSortColumn = columnName;
                _sortAscending = true;
            }

            _currentPage = 1; // Reset to first page when sorting changes

            detailData.Transactions = columnName switch
            {
                nameof(TransactionDetailModel.TransactionId) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Id).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Id).ToList(),
                nameof(TransactionDetailModel.TransactionDateTime) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.DateTime).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.DateTime).ToList(),
                nameof(TransactionDetailModel.MerchantName) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Merchant).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Merchant).ToList(),
                nameof(TransactionDetailModel.OperatorName) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Operator).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Operator).ToList(),
                nameof(TransactionDetailModel.ProductName) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Product).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Product).ToList(),
                nameof(TransactionDetailModel.TransactionType) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Type).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Type).ToList(),
                nameof(TransactionDetailModel.TransactionStatus) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Status).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Status).ToList(),
                nameof(TransactionDetailModel.GrossAmount) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.Value).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.Value).ToList(),
                nameof(TransactionDetailModel.FeesCommission) => _sortAscending
                    ? detailData.Transactions.OrderBy(t => t.TotalFees).ToList()
                    : detailData.Transactions.OrderByDescending(t => t.TotalFees).ToList(),
                //nameof(TransactionDetailModel.NetAmount) => _sortAscending
                //    ? detailData.Transactions.OrderBy(t => t.NetAmount).ToList()
                //    : detailData.Transactions.OrderByDescending(t => t.NetAmount).ToList(),
                _ => detailData.Transactions
            };

            CalculateKPIs();
        }

        private string GetSortIcon(string columnName)
        {
            if (_currentSortColumn != columnName)
                return "↕";

            return _sortAscending ? "↑" : "↓";
        }

        private string GetShortId(Guid transactionId)
        {
            var idString = transactionId.ToString();
            return idString.Length >= 8 ? $"{idString.Substring(0, 8)}..." : idString;
        }

        private string GetTypeBadgeClass(string? type)
        {
            return type switch
            {
                "Sale" => "badge-primary",
                "Refund" => "badge-warning",
                "Reversal" => "badge-secondary",
                _ => "badge-default"
            };
        }

        private string GetStatusBadgeClass(string? status)
        {
            return status switch
            {
                "Successful" => "badge-success",
                "Failed" => "badge-danger",
                "Reversed" => "badge-warning",
                _ => "badge-default"
            };
        }

        private async Task ExportToCSV()
        {
            if (detailData.Transactions == null || !detailData.Transactions.Any())
                return;

            try
            {
                var csv = new StringBuilder();

                // Header
                csv.AppendLine("Transaction ID,Transaction Date & Time,Merchant,Operator,Product,Transaction Type,Transaction Status,Gross Amount,Fees/Commission,Settlement Reference");

                // Data rows
                foreach (var item in detailData.Transactions)
                {
                    csv.AppendLine($"\"{item.Id}\",\"{item.DateTime:yyyy-MM-dd HH:mm:ss}\",\"{item.Merchant}\",\"{item.Operator}\",\"{item.Product}\",\"{item.Type}\",\"{item.Status}\",{item.Value},{item.TotalFees},\"{item.SettlementReference ?? ""}\"");
                }

                var fileName = $"transaction-detail-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var base64 = Convert.ToBase64String(bytes);

                await JSRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error exporting to CSV");
                errorMessage = "Failed to export data to CSV";
            }
        }

        private string GetBackUrl()
        {
            var uri = Navigation.Uri;
            if (uri.Contains("returnUrl="))
            {
                var startIndex = uri.IndexOf("returnUrl=") + "returnUrl=".Length;
                var endIndex = uri.IndexOf("&", startIndex);
                var returnUrl = endIndex > 0
                    ? uri.Substring(startIndex, endIndex - startIndex)
                    : uri.Substring(startIndex);
                return Uri.UnescapeDataString(returnUrl);
            }

            return "/reporting";
        }

        private string GetBackButtonText()
        {
            var backUrl = GetBackUrl();
            if (backUrl.Contains("transaction-summary-merchant"))
            {
                return "Back to Merchant Summary";
            }

            return "Back to Reporting";
        }

        private void OnMerchantSelectionChanged(List<string> selectedIds)
        {
            _selectedMerchantIds = selectedIds;
        }

        private void OnOperatorSelectionChanged(List<string> selectedIds)
        {
            _selectedOperatorIds = selectedIds;
        }

        private void OnProductSelectionChanged(List<string> selectedIds)
        {
            _selectedProductIds = selectedIds;
        }
    }
}
