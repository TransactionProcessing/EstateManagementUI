using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class Index
    {
        private bool isLoading = true;
        private List<ContractModels.ContractModel>? contracts;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Contract, PermissionFunction.List, this.LoadContracts);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadContracts()
        {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            Result<List<ContractModels.ContractModel>> result = await this.ContractUiService.GetContracts(correlationId, estateId);

            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            contracts = result.Data;

            return Result.Success();
        }
    }
}
