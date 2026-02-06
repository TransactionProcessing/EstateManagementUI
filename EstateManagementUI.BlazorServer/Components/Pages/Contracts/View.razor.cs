using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class View
    {
        [Parameter]
        public Guid ContractId { get; set; }

        private ContractModels.ContractModel? contractModel;
        private bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Contract, PermissionFunction.View, this.LoadContract);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadContract()
        {
            var estateId = await this.GetEstateId();
            var result = await this.ContractUiService.GetContract(CorrelationIdHelper.New(), estateId, this.ContractId);

            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }
            contractModel = result.Data;
            return Result.Success();
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/contracts");
        }
    }
}
