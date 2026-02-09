using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class Deposit
    {
        [Parameter]
        public Guid MerchantId { get; set; }

        private MerchantModels.DepositModel model = new();
        private bool isSaving = false;
        private string? merchantName;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Merchant, PermissionFunction.MakeDeposit, this.LoadMerchant);
            if (result.IsFailed)
            {
                return;
            }
            this.StateHasChanged();
        }

        private async Task<Result> LoadMerchant() {
            CorrelationId correlationId = new(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();

            Result<MerchantModels.MerchantModel> getMerchantResult = await this.MerchantUiService.GetMerchant(correlationId, estateId, MerchantId);

            if (getMerchantResult.IsFailed)
                return ResultHelpers.CreateFailure(getMerchantResult);

            this.merchantName = getMerchantResult.Data.MerchantName;

            return Result.Success();
        }

        private async Task HandleSubmit() {
            isSaving = true;
            this.ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.MerchantUiService.MakeMerchantDeposit(correlationId, estateId, MerchantId, model);

            if (result.IsSuccess) {
                successMessage = "Deposit recorded successfully";

                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to contracts list with success
                NavigationManager.NavigateTo("/merchants");
            }
            else {
                this.errorMessage = "Failed to make deposit";
            }

            isSaving = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/merchants");
        }
    }
}
