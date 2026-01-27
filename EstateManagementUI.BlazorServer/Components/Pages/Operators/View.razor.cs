using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class View
    {
        [Parameter]
        public Guid OperatorId { get; set; }

        private OperatorModel? operatorModel;
        private bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            await RequirePermission(PermissionSection.Operator, PermissionFunction.View);

            await LoadOperator();
        }

        private async Task LoadOperator()
        {
            try
            {
                isLoading = true;
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var result = await Mediator.Send(new OperatorQueries.GetOperatorQuery(correlationId, estateId, OperatorId));

                if (result.IsSuccess && result.Data != null)
                {
                    operatorModel = ModelFactory.ConvertFrom(result.Data);
                }
            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/operators");
        }
    }
}
