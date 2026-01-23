using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;
using ContractModel = EstateManagementUI.BlazorServer.Models.ContractModel;
using ContractProductModel = EstateManagementUI.BlazorServer.Models.ContractProductModel;
using ContractProductTransactionFeeModel = EstateManagementUI.BlazorServer.Models.ContractProductTransactionFeeModel;
using MerchantModel = EstateManagementUI.BlazorServer.Models.MerchantModel;
using OperatorModel = EstateManagementUI.BlazorServer.Models.OperatorModel;
using TransactionDetailModel = EstateManagementUI.BlazorServer.Models.TransactionDetailModel;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class View
    {
        [Parameter]
        public Guid MerchantId { get; set; }

        private MerchantModel? merchant;
        private bool isLoading = true;
        private string activeTab = "details";

        // Mock assigned data
        private List<MerchantOperatorModel> assignedOperators = new();
        private List<MerchantContractModel> assignedContracts = new();
        private List<string> assignedDevices = new();

        // Settlement transaction history data
        private List<TransactionDetailModel>? settlementTransactions;
        private bool loadingSettlementTransactions = false;

        // Pagination for settlement transactions
        private int settlementCurrentPage = 1;
        private int settlementPageSize = 25;
        private int settlementTotalPages = 0;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            try
            {
                await base.OnInitializedAsync();

                await RequirePermission(PermissionSection.Merchant, PermissionFunction.View);

                Result result = await LoadMerchant();
                if (result.IsFailed) {
                    this.NavigationManager.NavigateToErrorPage();
                }
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private async Task<Result> LoadMerchant()
        {
            try
            {
                isLoading = true;
                CorrelationId correlationId = new(Guid.NewGuid());
                Guid estateId = await this.GetEstateId();

                Result<BusinessLogic.Models.MerchantModel> getMerchantResult = await Mediator.Send(new MerchantQueries.GetMerchantQuery(correlationId, estateId, MerchantId));

                if (getMerchantResult.IsFailed)
                    return ResultHelpers.CreateFailure(getMerchantResult);

                merchant = ModelFactory.ConvertFrom(getMerchantResult.Data);

                var operatorsResultTask = Mediator.Send(new MerchantQueries.GetMerchantOperatorsQuery(correlationId, estateId, MerchantId));
                var contractsResultTask = Mediator.Send(new MerchantQueries.GetMerchantContractsQuery(correlationId, estateId, MerchantId));
                var devicesResultTask = Mediator.Send(new MerchantQueries.GetMerchantDevicesQuery(correlationId, estateId, MerchantId));

                await Task.WhenAll(operatorsResultTask, contractsResultTask, devicesResultTask);

                if (operatorsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(operatorsResultTask.Result);
                
                if (contractsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(contractsResultTask.Result);

                if (devicesResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(devicesResultTask.Result);

                assignedOperators = new List<MerchantOperatorModel>();
                foreach (MerchantOperatorModel merchantOperatorModel in operatorsResultTask.Result.Data)
                {
                    this.assignedOperators.Add(new MerchantOperatorModel
                    {
                        OperatorName = merchantOperatorModel.OperatorName,
                        OperatorId = merchantOperatorModel.OperatorId,
                        MerchantNumber = merchantOperatorModel.MerchantNumber,
                        TerminalNumber = merchantOperatorModel.TerminalNumber
                    });
                }

                assignedContracts = new List<MerchantContractModel>();
                foreach (MerchantContractModel merchantContractModel in contractsResultTask.Result.Data) {
                    var cm = new MerchantContractModel {
                        ContractId = merchantContractModel.ContractId,
                        OperatorName = merchantContractModel.OperatorName,
                        ContractName = merchantContractModel.ContractName,
                        ContractProducts = new List<MerchantContractProductModel>()
                    };
                    foreach (MerchantContractProductModel merchantContractProductModel in merchantContractModel.ContractProducts) {
                        cm.ContractProducts.Add(new MerchantContractProductModel
                        {
                            ProductName = merchantContractProductModel.ProductName,
                            DisplayText = merchantContractProductModel.DisplayText,
                            ProductId = merchantContractProductModel.ProductId,
                            ProductType = merchantContractProductModel.ProductType,
                            ContractId = merchantContractProductModel.ContractId,
                            Value = merchantContractProductModel.Value,
                            MerchantId = merchantContractProductModel.MerchantId
                        });
                    }
                    this.assignedContracts.Add(cm);
                }

                foreach (var merchantDevice in devicesResultTask.Result.Data) {
                    this.assignedDevices.Add(merchantDevice.DeviceIdentifier);
                }

                return Result.Success();
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task LoadSettlementTransactions()
        {
            loadingSettlementTransactions = true;
            try
            {
                var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var accessToken = "stubbed-token";
                var correlationId = CorrelationIdHelper.New();

                // Load last 6 months of transaction data for this merchant
                var query = new Queries.GetTransactionDetailQuery(
                    correlationId,
                    accessToken,
                    estateId,
                    DateTime.Today.AddMonths(-6),
                    DateTime.Today,
                    new List<Guid> { MerchantId }, // Filter by this merchant only
                    null, // All operators
                    null  // All products
                );

                var result = await Mediator.Send(query);
                if (result.IsSuccess && result.Data != null)
                {
                    settlementTransactions = ModelFactory.ConvertFrom(result.Data);
                }
                else
                {
                    settlementTransactions = new List<TransactionDetailModel>();
                }
            }
            catch (Exception)
            {
                // Handle error silently for now
                settlementTransactions = new List<TransactionDetailModel>();
            }
            finally
            {
                loadingSettlementTransactions = false;
            }
        }

        private string GetResultBadgeClass(string? status)
        {
            return status switch
            {
                "Successful" => "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800",
                "Failed" => "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-red-100 text-red-800",
                "Reversed" => "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-yellow-100 text-yellow-800",
                _ => "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-gray-100 text-gray-800"
            };
        }

        private string GetResultText(string? status)
        {
            return status switch
            {
                "Successful" => "Authorised",
                "Failed" => "Not Authorised",
                "Reversed" => "Reversed",
                _ => status ?? "Unknown"
            };
        }

        private string GetSettlementStatusBadgeClass(string? settlementReference)
        {
            return string.IsNullOrEmpty(settlementReference)
                ? "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-orange-100 text-orange-800"
                : "inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-blue-100 text-blue-800";
        }

        private string GetSettlementStatusText(string? settlementReference)
        {
            return string.IsNullOrEmpty(settlementReference) ? "Pending" : "Settled";
        }

        private async Task OnSettlementsTabClick()
        {
            activeTab = "settlements";
            if (settlementTransactions == null)
            {
                await LoadSettlementTransactions();
            }
        }

        private List<TransactionDetailModel> GetPagedSettlementTransactions()
        {
            if (settlementTransactions == null || !settlementTransactions.Any())
                return new List<TransactionDetailModel>();

            settlementTotalPages = (int)Math.Ceiling(settlementTransactions.Count / (double)settlementPageSize);

            return settlementTransactions
                .Skip((settlementCurrentPage - 1) * settlementPageSize)
                .Take(settlementPageSize)
                .ToList();
        }

        private void SettlementNextPage()
        {
            if (settlementCurrentPage < settlementTotalPages)
                settlementCurrentPage++;
        }

        private void SettlementPreviousPage()
        {
            if (settlementCurrentPage > 1)
                settlementCurrentPage--;
        }

        private string GetTabClass(string tab)
        {
            var baseClass = "px-6 py-3 text-sm font-medium border-b-2 transition-colors ";
            return tab == activeTab
                ? baseClass + "border-blue-600 text-blue-600"
                : baseClass + "border-transparent text-gray-600 hover:text-gray-800 hover:border-gray-300";
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/merchants");
        }
    }
}
