using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class Index
    {
        private bool isLoading = true;
        private List<OperatorModels.OperatorModel> operators;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Operator, PermissionFunction.List, this.LoadOperators);
            if (result.IsFailed) {
                return;
            }
            
            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadOperators()
        {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            Result<List<OperatorModels.OperatorModel>> result = await this.OperatorUiService.GetOperators(correlationId, estateId);

            if (result.IsFailed) {
                return ResultHelpers.CreateFailure(result);
            }

            operators = result.Data;

            return Result.Success();
        }
    }
}
