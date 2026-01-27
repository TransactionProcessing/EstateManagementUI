using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class View
    {
        [Parameter]
        public Guid ContractId { get; set; }

        private ContractModel? contractModel;
        private bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            try
            {
                await RequirePermission(PermissionSection.Contract, PermissionFunction.View);
                await LoadContract();
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private async Task LoadContract()
        {
            try
            {
                isLoading = true;
                var estateId = await this.GetEstateId();
                var result = await Mediator.Send(new ContractQueries.GetContractQuery(CorrelationIdHelper.New(), estateId, ContractId));

                if (result.IsSuccess && result.Data != null)
                {
                    contractModel = ModelFactory.ConvertFrom(result.Data);
                }
            }
            finally
            {
                isLoading = false;
            }
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/contracts");
        }
    }
}
