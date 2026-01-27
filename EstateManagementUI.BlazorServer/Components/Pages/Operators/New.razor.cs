using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class New
    {
        private CreateOperatorModel model = new();
        private bool isSaving = false;
        private string? errorMessage;
        private string? successMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            await RequirePermission(PermissionSection.Operator, PermissionFunction.Create);
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            errorMessage = null;

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                // Create operator
                var createCommand = new OperatorCommands.CreateOperatorCommand(
                    correlationId,
                    estateId,
                    model.OperatorName!,
                    model.RequireCustomMerchantNumber,
                    model.RequireCustomTerminalNumber
                );

                var createResult = await Mediator.Send(createCommand);

                if (!createResult.IsSuccess)
                {
                    errorMessage = createResult.Message ?? "Failed to create operator";
                    return;
                }

                // Show success message briefly before navigating away
                successMessage = "Operator created successfully.";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await Task.Delay(2500);


                // Navigate to operators list with success
                NavigationManager.NavigateTo("/operators");
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

        private void Cancel()
        {
            NavigationManager.NavigateTo("/operators");
        }

        public class CreateOperatorModel
        {
            [Required(ErrorMessage = "Operator name is required")]
            public string? OperatorName { get; set; }

            public bool RequireCustomMerchantNumber { get; set; }

            public bool RequireCustomTerminalNumber { get; set; }
        }
    }
}
