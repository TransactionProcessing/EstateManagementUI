using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Estate
{
    public partial class Index
    {
        private bool isLoading = true;
        private EstateModel? estate;
        private List<RecentMerchantsModel> merchants;
        private List<OperatorDropDownModel>? availableOperators;
        private List<OperatorModel> assignedOperators = new();
        private List<RecentContractModel> contracts;
        private string activeTab = "overview";
        private bool showAddOperator = false;
        private string? selectedOperatorId;
        private string? successMessage;
        private string? errorMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            try {
                await base.OnInitializedAsync();

                await RequirePermission(PermissionSection.Estate, PermissionFunction.View);
                CorrelationId correlationId = new(Guid.NewGuid());
                Guid estateId = await this.GetEstateId();
                var result = await this.LoadEstateData(correlationId, estateId);
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

        private async Task<Result> LoadEstateData(CorrelationId correlationId, Guid estateId)
        {
            Task<Result<BusinessLogic.Models.EstateModel>> estateTask = Mediator.Send(new EstateQueries.GetEstateQuery(correlationId, estateId));
            Task<Result<List<BusinessLogic.Models.RecentMerchantsModel>>> merchantTask = Mediator.Send(new MerchantQueries.GetRecentMerchantsQuery(correlationId, estateId));
            Task<Result<List<BusinessLogic.Models.RecentContractModel>>> contractsTask = Mediator.Send(new ContractQueries.GetRecentContractsQuery(correlationId, estateId));
            Task<Result<List<BusinessLogic.Models.OperatorModel>>> assignedOperatorsTask = Mediator.Send(new EstateQueries.GetAssignedOperatorsQuery(correlationId, estateId));
            Task<Result<List<BusinessLogic.Models.OperatorDropDownModel>>> allOperatorsTask= Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(correlationId, estateId));

            await Task.WhenAll(estateTask, merchantTask, contractsTask, assignedOperatorsTask, allOperatorsTask);
            
            if (estateTask.Result.IsFailed)
                return ResultHelpers.CreateFailure(estateTask.Result);
            estate = ModelFactory.ConvertFrom(estateTask.Result.Data);

            if (merchantTask.Result.IsFailed)
                return ResultHelpers.CreateFailure(merchantTask.Result);
            this.merchants = ModelFactory.ConvertFrom(merchantTask.Result.Data);

            if (contractsTask.Result.IsFailed)
                return ResultHelpers.CreateFailure(contractsTask.Result);
            this.contracts = ModelFactory.ConvertFrom(contractsTask.Result.Data);

            if (assignedOperatorsTask.Result.IsFailed)
                return ResultHelpers.CreateFailure(assignedOperatorsTask.Result);
            this.assignedOperators = ModelFactory.ConvertFrom(assignedOperatorsTask.Result.Data);

            if (allOperatorsTask.Result.IsFailed)
                return ResultHelpers.CreateFailure(allOperatorsTask.Result);

            List<OperatorDropDownModel> unfiltered = ModelFactory.ConvertFrom(allOperatorsTask.Result.Data);
            this.availableOperators = unfiltered
                .Where(u => !this.assignedOperators.Any(a => a.OperatorId == u.OperatorId))
                .Select(u => new OperatorDropDownModel
                {
                    OperatorId = u.OperatorId,
                    OperatorName = u.OperatorName
                })
                .ToList();


            return Result.Success();
        }


        private void SetActiveTab(string tab)
        {
            activeTab = tab;
            ClearMessages();
        }

        private void ClearMessages()
        {
            successMessage = null;
            errorMessage = null;
        }

        private async Task AddOperatorToEstate()
        {
            if (string.IsNullOrEmpty(selectedOperatorId)) return;

            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                Guid estateId = await this.GetEstateId();
                var operatorId = Guid.Parse(selectedOperatorId);

                var command = new EstateCommands.AddOperatorToEstateCommand(
                    correlationId,
                    estateId,
                    operatorId
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Operator added successfully";
                    selectedOperatorId = null;
                    showAddOperator = false;

                    // Add to assigned list
                    var op = availableOperators?.FirstOrDefault(o => o.OperatorId == operatorId);
                    if (op != null && !assignedOperators.Any(a => a.OperatorId == operatorId)) {
                        OperatorQueries.GetOperatorQuery query = new OperatorQueries.GetOperatorQuery(CorrelationIdHelper.New(), await this.GetEstateId(), operatorId);
                        var operatorResult = await Mediator.Send(query);
                        if (operatorResult.IsFailed) 
                            this.NavigationManager.NavigateToErrorPage();

                        assignedOperators.Add(new OperatorModel() {
                            OperatorId = operatorResult.Data.OperatorId,
                            Name = operatorResult.Data.Name,
                            RequireCustomMerchantNumber = operatorResult.Data.RequireCustomMerchantNumber,
                            RequireCustomTerminalNumber = operatorResult.Data.RequireCustomTerminalNumber
                        });
                    }
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to add operator";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task RemoveOperatorFromEstate(Guid operatorId)
        {
            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                Guid estateId = await this.GetEstateId();
                var command = new EstateCommands.RemoveOperatorFromEstateCommand(
                    correlationId,
                    estateId,
                    operatorId
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Operator removed successfully";
                    assignedOperators.RemoveAll(o => o.OperatorId == operatorId);
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to remove operator";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }
    }
}
