using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class Index
    {
        private bool isLoading = true;
        private List<OperatorModel> operators;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            try {
                await RequirePermission(PermissionSection.Operator, PermissionFunction.List);

                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var result = await Mediator.Send(new OperatorQueries.GetOperatorsQuery(correlationId, estateId));
                if (result.IsSuccess)
                {
                    operators = ModelFactory.ConvertFrom(result.Data);
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
