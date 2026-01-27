using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class Index
    {
        private bool isLoading = true;
        private List<ContractModel>? contracts;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            try
            {
                await RequirePermission(PermissionSection.Contract, PermissionFunction.List);
                var estateId = await this.GetEstateId();
                var result = await Mediator.Send(new ContractQueries.GetContractsQuery(CorrelationIdHelper.New(), estateId));
                if (result.IsSuccess)
                {
                    contracts = ModelFactory.ConvertFrom(result.Data);
                }
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }
    }
}
