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
        private List<OperatorModel>? availableOperators;
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
            Result<BusinessLogic.Models.EstateModel> estateResult = await Mediator.Send(new Queries.GetEstateQuery(correlationId, estateId));
            if (estateResult.IsFailed)
                return ResultHelpers.CreateFailure(estateResult);
            estate = ModelFactory.ConvertFrom(estateResult.Data);
            
            Result<List<BusinessLogic.Models.RecentMerchantsModel>> merchantResult = await Mediator.Send(new Queries.GetRecentMerchantsQuery(correlationId, estateId));

            if (merchantResult.IsFailed)
                return ResultHelpers.CreateFailure(merchantResult);

            this.merchants = ModelFactory.ConvertFrom(merchantResult.Data);

            Result<List<BusinessLogic.Models.RecentContractModel>> contractResult = await Mediator.Send(new Queries.GetRecentContractsQuery(correlationId, estateId));

            if (contractResult.IsFailed)
                return ResultHelpers.CreateFailure(contractResult);

            this.contracts = ModelFactory.ConvertFrom(contractResult.Data);

            Result<List<BusinessLogic.Models.OperatorModel>> assignedOperatorsResult = await Mediator.Send(new Queries.GetAssignedOperatorsQuery(correlationId, estateId));

            if (assignedOperatorsResult.IsFailed)
                return ResultHelpers.CreateFailure(assignedOperatorsResult);

            this.assignedOperators = ModelFactory.ConvertFrom(assignedOperatorsResult.Data);

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
                var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var accessToken = "stubbed-token";
                var operatorId = Guid.Parse(selectedOperatorId);

                var command = new Commands.AddOperatorToEstateCommand(
                    correlationId,
                    accessToken,
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
                    if (op != null && !assignedOperators.Any(a => a.OperatorId == operatorId))
                    {
                        assignedOperators.Add(op);
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
                var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var accessToken = "stubbed-token";

                var command = new Commands.RemoveOperatorFromEstateCommand(
                    correlationId,
                    accessToken,
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
