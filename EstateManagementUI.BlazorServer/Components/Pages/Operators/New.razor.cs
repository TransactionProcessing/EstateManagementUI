using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BlazorServer.Models;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class New
    {
        private OperatorModels.CreateOperatorModel model = new();
        private bool isSaving = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Operator, PermissionFunction.Create);
            if (result.IsFailed)
            {
                return;
            }

            this.StateHasChanged();
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.OperatorUiService.CreateOperator(correlationId, estateId, this.model);
            if (result.IsSuccess)
            {
                successMessage = "Operator created successfully";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                NavigationManager.NavigateTo("/operators");
            }
            else
            {
                errorMessage = "Failed to create operator";
            }

            isSaving = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/operators");
        }

        
    }
}
