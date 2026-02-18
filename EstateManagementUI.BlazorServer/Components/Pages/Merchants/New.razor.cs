using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BlazorServer.Models;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    [ExcludeFromCodeCoverage(Justification = "BuildRenderTree is compiler-generated and not testable")]
    public partial class New
    {
        private readonly MerchantModels.CreateMerchantModel model = new();
        private bool isSaving = false;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Merchant, PermissionFunction.Create);
            if (result.IsFailed)
            {
                return;
            }

            this.StateHasChanged();
        }

        private async Task HandleSubmit() {
            isSaving = true;
            errorMessage = null;

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            // Get the newly created merchant ID
            var merchantId = Guid.NewGuid();

            var result = await this.MerchantUiService.CreateMerchant(correlationId, estateId, merchantId, this.model);

            if (result.IsSuccess) {
                successMessage = "Merchant created successfully";

                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to contracts list with success
                NavigationManager.NavigateTo("/merchants");
            }
            else {
                this.errorMessage = "Failed to create merchant";
            }

            isSaving = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/merchants");
        }
    }
}
