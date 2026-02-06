using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts;

public partial class Edit
{
    [Parameter]
    public Guid ContractId { get; set; }

    private ContractModels.ContractModel? contractModel;
    private ContractModels.EditContractModel model = new();
    private bool isLoading = true;
    private bool isSaving = false;

    // Product modal state
    private bool showAddProductModal = false;
    private bool isAddingProduct = false;
    private ContractModels.AddProductModel productModel = new();
    private string? productErrorMessage;

    // Transaction fee modal state
    private bool showAddFeeModal = false;
    private bool isAddingFee = false;
    private ContractModels.AddTransactionFeeModel feeModel = new();
    private string? feeErrorMessage;
    private Guid currentProductId;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) {
            return;
        }

        Result result = await OnAfterRender(PermissionSection.Contract, PermissionFunction.Edit, this.LoadContract);
        if (result.IsFailed) {
            return;
        }

        isLoading = false;
        this.StateHasChanged();
    }

    private async Task<Result> LoadContract() {
        isLoading = true;
        Guid estateId = await GetEstateId();
        var result = await this.ContractUiService.GetContract(CorrelationIdHelper.New(), estateId, ContractId);

        if (result.IsFailed) {
            return ResultHelpers.CreateFailure(result);
        }

        contractModel = result.Data;

        // Initialize model with current values
        model = new ContractModels.EditContractModel { Description = contractModel.Description };
        return Result.Success();
    }

    private async Task HandleSubmit()
    {
        isSaving = true;
        
        // Note: Update contract description endpoint is not currently implemented in the backend
        // This form is present for future implementation
        errorMessage = "Contract description update is not yet supported by the backend API";
        isSaving = false;
    }

    private void ShowAddProductModal()
    {
        productModel = new ContractModels.AddProductModel();
        productErrorMessage = null;
        showAddProductModal = true;
    }

    private void CloseAddProductModal()
    {
        showAddProductModal = false;
        productModel = new ContractModels.AddProductModel();
        productErrorMessage = null;
    }

    private async Task HandleAddProduct()
    {
        isAddingProduct = true;
        productErrorMessage = null;

        var estateId = await this.GetEstateId();

        var result = await this.ContractUiService.AddProductToContract(CorrelationIdHelper.New(), estateId, this.ContractId, this.productModel);

        if (result.IsSuccess) {
            successMessage = "Product added successfully";
            CloseAddProductModal();
        }
        else {
            productErrorMessage = "Failed to add product";
        }

        await this.WaitOnUIRefresh();

        await LoadContract();

        StateHasChanged();
        isAddingProduct = false;
    }

    private void ShowAddFeeModal(Guid productId)
    {
        currentProductId = productId;
        feeModel = new ContractModels.AddTransactionFeeModel();
        feeErrorMessage = null;
        showAddFeeModal = true;
    }

    private void CloseAddFeeModal()
    {
        showAddFeeModal = false;
        feeModel = new ContractModels.AddTransactionFeeModel();
        feeErrorMessage = null;
        currentProductId = Guid.Empty;
    }

    private async Task HandleAddFee() {
        isAddingFee = true;
        feeErrorMessage = null;

        var estateId = await this.GetEstateId();

        var result = await this.ContractUiService.AddTransactionFeeToProduct(CorrelationIdHelper.New(), estateId, this.ContractId, this.currentProductId, this.feeModel);

        if (result.IsSuccess) {
            successMessage = "Transaction fee added successfully";
            CloseAddProductModal();
        }
        else {
            productErrorMessage = "Failed to transaction fee";
        }

        await this.WaitOnUIRefresh();

        await LoadContract();

        StateHasChanged();
    }

    private async Task RemoveProduct(Guid productId)
    {
        //if (contractModel == null || contractModel.Products == null) return;

        //var product = contractModel.Products.FirstOrDefault(p => p.ContractProductId == productId);
        //if (product != null)
        //{
        // Note: Backend API for product removal not yet implemented
        // For now, show a message to the user
        errorMessage = "Product removal is not yet supported by the backend API. This feature will be available once the backend endpoint is implemented.";
        //}
    }

    private async Task RemoveFee(Guid productId, Guid feeId)
    {
        var estateId = await this.GetEstateId();

        var result = await this.ContractUiService.RemoveTransactionFeeFromProduct(CorrelationIdHelper.New(), estateId, this.ContractId, productId, feeId);

        if (result.IsSuccess)
        {
            successMessage = "Transaction fee removed successfully";
            CloseAddProductModal();
        }
        else
        {
            productErrorMessage = "Failed to remove transaction fee";
        }

        await this.WaitOnUIRefresh();

        await LoadContract();

        StateHasChanged();
    }

    private void BackToView()
    {
        NavigationManager.NavigateTo($"/contracts/{ContractId}");
    }

    private void BackToList()
    {
        NavigationManager.NavigateTo("/contracts");
    }
}