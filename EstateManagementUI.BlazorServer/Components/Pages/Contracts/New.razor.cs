using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using Result = SimpleResults.Result;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class New
    {
        private ContractModels.CreateContractFormModel model = new();
        private bool isSaving = false;
        private bool isLoadingOperators = true;
        private List<OperatorModels.OperatorDropDownModel>? operators;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        { if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Contract, PermissionFunction.Create, this.LoadOperators);
            if (result.IsFailed)
            {
                return;
            }

            this.StateHasChanged();
        }
        
        private async Task<Result> LoadOperators()
        {
            isLoadingOperators = true;
            var estateId = await GetEstateId();
            var result = await this.OperatorUiService.GetOperatorsForDropDown(CorrelationIdHelper.New(), estateId);

            if (result.IsFailed) {
                return ResultHelpers.CreateFailure(result);
            }

            operators = result.Data;
            this.isLoadingOperators = false;
            return Result.Success();

        }

        private async Task HandleSubmit() {
            isSaving = true;
            errorMessage = null;

            var estateId = await this.GetEstateId();

            var result = await this.ContractUiService.CreateContract(CorrelationIdHelper.New(), estateId, this.model);

            if (result.IsSuccess) {
                successMessage = "Contract created successfully";

                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to contracts list with success
                NavigationManager.NavigateTo("/contracts");
            }
            else {
                this.errorMessage = "Failed to create contract";
            }

            isSaving = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/contracts");
        }
    }
}
