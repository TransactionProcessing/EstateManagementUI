using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class Edit
    {
        [Parameter]
        public Guid OperatorId { get; set; }

        private OperatorModels.OperatorModel? operatorModel;
        private OperatorModels.EditOperatorModel model = new();
        private bool isLoading = true;
        private bool isSaving = false;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Operator, PermissionFunction.Edit, this.LoadOperator);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadOperator()
        {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.OperatorUiService.GetOperator(correlationId, estateId, this.OperatorId);

            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            operatorModel = result.Data;

            // Initialize model with current values
            model = new OperatorModels.EditOperatorModel
            {
                OperatorName = operatorModel.Name,
                RequireCustomMerchantNumber = operatorModel.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = operatorModel.RequireCustomTerminalNumber
            };

            return Result.Success();
        }


        private async Task HandleSubmit()
        {
            isSaving = true;
            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.OperatorUiService.UpdateOperator(correlationId, estateId, this.OperatorId, this.model);
            if (result.IsSuccess)
            {
                successMessage = "Operator updated successfully";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                NavigationManager.NavigateTo("/operators");
            }
            else
            {
                errorMessage = "Failed to update operator";
            }
        
            isSaving = false;
        }
        
        private void BackToList()
        {
            NavigationManager.NavigateTo("/operators");
        }
    }
}
