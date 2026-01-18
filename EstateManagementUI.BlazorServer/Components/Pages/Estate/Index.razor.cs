using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components.Authorization;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Estate
{
    public partial class Index
    {
        private bool isLoading = true;
        private EstateModel? estate;
        private List<MerchantModel>? merchants;
        private List<OperatorModel>? availableOperators;
        private List<OperatorModel> assignedOperators = new();
        private List<ContractModel>? contracts;
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

                Result<BusinessLogic.Models.EstateModel> estateResult = await Mediator.Send(new Queries.GetEstateQuery(correlationId, estateId));
                if (estateResult.IsSuccess)
                {
                    estate = ModelFactory.ConvertFrom(estateResult.Data);
                    // Load assigned operators from estate model
                    if (estate.Operators != null)
                    {
                        assignedOperators = estate.Operators.Select(o => new OperatorModel
                        {
                            OperatorId = o.OperatorId,
                            Name = o.Name,
                            RequireCustomMerchantNumber = o.RequireCustomMerchantNumber,
                            RequireCustomTerminalNumber = o.RequireCustomTerminalNumber
                        }).ToList();
                    }
                }

                // TODO: Make these calls concurrent...
                var merchantsResult = await Mediator.Send(new Queries.GetMerchantsQuery(correlationId, estateId));
                if (merchantsResult.IsSuccess)
                {
                    merchants = ModelFactory.ConvertFrom(merchantsResult.Data);
                }

                var operatorsResult = await Mediator.Send(new Queries.GetOperatorsQuery(correlationId, String.Empty, estateId));
                if (operatorsResult.IsSuccess)
                {
                    availableOperators = ModelFactory.ConvertFrom(operatorsResult.Data);
                }

                var contractsResult = await Mediator.Send(new Queries.GetContractsQuery(correlationId, String.Empty, estateId));
                if (contractsResult.IsSuccess)
                {
                    contracts = ModelFactory.ConvertFrom(contractsResult.Data);
                }
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
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
