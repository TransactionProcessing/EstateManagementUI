using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using TransactionProcessing.UnionResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Operators
{
    public partial class View
    {
        [Parameter]
        public Guid OperatorId { get; set; }

        private OperatorModels.OperatorModel? operatorModel;
        private bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Operator, PermissionFunction.View, this.LoadOperator);
            if (result.IsFailed) {
                return;
            }
            isLoading = false;
            this.StateHasChanged();
        }
        
        private async Task<Result> LoadOperator() {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.OperatorUiService.GetOperator(correlationId, estateId, this.OperatorId);

            if (result.IsFailed && result.Errors.Any(error => error.ErrorType != ErrorType.NotFound)) {
                return ResultHelpers.CreateFailure(result);
            }

            if (result.IsFailed) {
                return Result.Success();
            }

            operatorModel = result.Data;

            return Result.Success();
        }

        private void BackToList() => NavigationManager.NavigateToOperatorList();
    }
}
