using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class Edit
    {
        [Parameter]
        public Guid OperatorId { get; set; }

        private OperatorModel? operatorModel;
        private EditOperatorModel model = new();
        private bool isLoading = true;
        private bool isSaving = false;
        private bool hasPermission = false;
        private string? errorMessage;
        private string? successMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            await RequirePermission(PermissionSection.Operator, PermissionFunction.Edit);
            await LoadOperator();
        }

        private async Task LoadOperator()
        {
            try
            {
                isLoading = true;
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await GetEstateId();

                var result = await Mediator.Send(new OperatorQueries.GetOperatorQuery(correlationId, estateId, OperatorId));

                if (result.IsSuccess && result.Data != null)
                {
                    operatorModel = ModelFactory.ConvertFrom(result.Data);

                    // Initialize model with current values
                    model = new EditOperatorModel
                    {
                        OperatorName = operatorModel.Name,
                        RequireCustomMerchantNumber = operatorModel.RequireCustomMerchantNumber,
                        RequireCustomTerminalNumber = operatorModel.RequireCustomTerminalNumber
                    };
                }
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var command = new OperatorCommands.UpdateOperatorCommand(
                    correlationId,
                    estateId,
                    OperatorId,
                    model.OperatorName!,
                    model.RequireCustomMerchantNumber,
                    model.RequireCustomTerminalNumber
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Operator updated successfully";
                    StateHasChanged();

                    // Small delay so user sees confirmation (adjust duration as needed)
                    await Task.Delay(2500);

                    NavigationManager.NavigateTo("/operators");
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to update operator";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                isSaving = false;
            }
        }

        private void ClearMessages()
        {
            errorMessage = null;
            successMessage = null;
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/operators");
        }

        public class EditOperatorModel
        {
            [Required(ErrorMessage = "Operator name is required")]
            public string? OperatorName { get; set; }

            public bool RequireCustomMerchantNumber { get; set; }

            public bool RequireCustomTerminalNumber { get; set; }
        }
    }
}
