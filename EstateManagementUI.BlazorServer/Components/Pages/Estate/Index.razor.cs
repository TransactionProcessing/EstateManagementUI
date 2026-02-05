using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Estate;

public partial class Index {
    private bool isLoading = true;
    private EstateModel? estate;
    //private string activeTab = "overview";
    private bool showAddOperator = false;
    private string? selectedOperatorId;

    public Index() {
        this.SetActiveTab("overview");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender) {
            return;
        }

        Result authResult = await RequirePermission(PermissionSection.Estate, PermissionFunction.View);
        if (authResult.IsFailed)
            return;

        Result result = await this.LoadEstateData();
        if (result.IsFailed) {
            this.NavigationManager.NavigateToErrorPage();
            return;
        }

        isLoading = false;
        this.StateHasChanged();
    }

    private async Task<Result> LoadEstateData() {
        CorrelationId correlationId = new(Guid.NewGuid());
        Guid estateId = await this.GetEstateId();
        Result<EstateModel> result = await this.EstateUIService.LoadEstate(correlationId, estateId);
        if (result.IsFailed) {
            return ResultHelpers.CreateFailure(result);
        }

        estate = result.Data;
        return Result.Success();
    }
        
    private async Task AddOperatorToEstate() {
        if (string.IsNullOrEmpty(selectedOperatorId)) return;

        ClearMessages();

        try {
            CorrelationId correlationId = new CorrelationId(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();
            
            var result = await this.EstateUIService.AddOperatorToEstate(correlationId, estateId, this.selectedOperatorId);

            if (result.IsSuccess) {
                successMessage = "Operator added successfully";
                selectedOperatorId = null;
                showAddOperator = false;

                await this.LoadEstateData();
            }
            else {
                errorMessage = "Failed to add operator";
            }
        }
        finally {
            // Small delay so user sees confirmation (adjust duration as needed)
            await this.WaitOnUIRefresh();
            this.StateHasChanged();
        }
    }

    private async Task RemoveOperatorFromEstate(Guid operatorId) {
        ClearMessages();

        try {
            CorrelationId correlationId = new CorrelationId(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();
            var result = await this.EstateUIService.RemoveOperatorFromEstate(correlationId, estateId, operatorId);

            if (result.IsSuccess) {
                successMessage = "Operator removed successfully";
                await this.LoadEstateData();
            }
            else {
                errorMessage = "Failed to remove operator";
            }
        }
        finally {
            // Small delay so user sees confirmation (adjust duration as needed)
            await this.WaitOnUIRefresh();
            this.StateHasChanged();
        }
    }

}