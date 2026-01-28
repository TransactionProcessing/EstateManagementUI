using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class Edit
    {
        [Parameter]
        public Guid ContractId { get; set; }

        private ContractModel? contractModel;
        private EditContractModel model = new();
        private bool isLoading = true;
        private bool isSaving = false;
        private bool hasPermission = false;
        private string? errorMessage;
        private string? successMessage;

        // Product modal state
        private bool showAddProductModal = false;
        private bool isAddingProduct = false;
        private AddProductModel productModel = new();
        private string? productErrorMessage;

        // Transaction fee modal state
        private bool showAddFeeModal = false;
        private bool isAddingFee = false;
        private AddTransactionFeeModel feeModel = new();
        private string? feeErrorMessage;
        private Guid currentProductId;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            await RequirePermission(PermissionSection.Contract, PermissionFunction.Edit);
            await LoadContract();
        }

        private async Task LoadContract()
        {
            try
            {
                isLoading = true;
                Guid estateId = await GetEstateId();
                var result = await Mediator.Send(new ContractQueries.GetContractQuery(CorrelationIdHelper.New(), estateId, ContractId));

                if (result.IsSuccess && result.Data != null)
                {
                    contractModel = ModelFactory.ConvertFrom(result.Data);

                    // Initialize model with current values
                    model = new EditContractModel
                    {
                        Description = contractModel.Description
                    };
                }
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            ClearMessages();

            try
            {
                // Note: Update contract description endpoint is not currently implemented in the backend
                // This form is present for future implementation
                errorMessage = "Contract description update is not yet supported by the backend API";
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                isSaving = false;
            }
        }

        private void ShowAddProductModal()
        {
            productModel = new AddProductModel();
            productErrorMessage = null;
            showAddProductModal = true;
        }

        private void CloseAddProductModal()
        {
            showAddProductModal = false;
            productModel = new AddProductModel();
            productErrorMessage = null;
        }

        private async Task HandleAddProduct()
        {
            isAddingProduct = true;
            productErrorMessage = null;

            try
            {
                var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var accessToken = "stubbed-token";

                var command = new Commands.AddProductToContractCommand(
                    CorrelationIdHelper.New(),
                    accessToken,
                    estateId,
                    ContractId,
                    productModel.ProductName!,
                    productModel.DisplayText!,
                    productModel.IsVariableValue ? null : productModel.Value
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Product added successfully";
                    CloseAddProductModal();
                    await LoadContract();
                }
                else
                {
                    productErrorMessage = result.Message ?? "Failed to add product";
                }
            }
            catch (Exception ex)
            {
                productErrorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                isAddingProduct = false;
            }
        }

        private void ShowAddFeeModal(Guid productId)
        {
            currentProductId = productId;
            feeModel = new AddTransactionFeeModel();
            feeErrorMessage = null;
            showAddFeeModal = true;
        }

        private void CloseAddFeeModal()
        {
            showAddFeeModal = false;
            feeModel = new AddTransactionFeeModel();
            feeErrorMessage = null;
            currentProductId = Guid.Empty;
        }

        private async Task HandleAddFee()
        {
            isAddingFee = true;
            feeErrorMessage = null;

            try
            {
                var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var accessToken = "stubbed-token";

                var command = new Commands.AddTransactionFeeForProductToContractCommand(
                    CorrelationIdHelper.New(),
                    accessToken,
                    estateId,
                    ContractId,
                    currentProductId,
                    feeModel.Description!,
                    feeModel.FeeValue!.Value
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Transaction fee added successfully";
                    CloseAddFeeModal();
                    await LoadContract();
                }
                else
                {
                    feeErrorMessage = result.Message ?? "Failed to add transaction fee";
                }
            }
            catch (Exception ex)
            {
                feeErrorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                isAddingFee = false;
            }
        }

        private void ClearMessages()
        {
            errorMessage = null;
            successMessage = null;
        }

        private async Task RemoveProduct(Guid productId)
        {
            if (contractModel == null || contractModel.Products == null) return;

            var product = contractModel.Products.FirstOrDefault(p => p.ContractProductId == productId);
            if (product != null)
            {
                // Note: Backend API for product removal not yet implemented
                // For now, show a message to the user
                errorMessage = "Product removal is not yet supported by the backend API. This feature will be available once the backend endpoint is implemented.";
            }
        }

        private async Task RemoveFee(Guid productId, Guid feeId)
        {
            if (contractModel == null || contractModel.Products == null) return;

            var product = contractModel.Products.FirstOrDefault(p => p.ContractProductId == productId);
            if (product?.TransactionFees != null)
            {
                var fee = product.TransactionFees.FirstOrDefault(f => f.TransactionFeeId == feeId);
                if (fee != null)
                {
                    // Note: Backend API for fee removal not yet implemented
                    // For now, show a message to the user
                    errorMessage = "Transaction fee removal is not yet supported by the backend API. This feature will be available once the backend endpoint is implemented.";
                }
            }
        }

        private void BackToView()
        {
            NavigationManager.NavigateTo($"/contracts/{ContractId}");
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/contracts");
        }

        public class EditContractModel
        {
            [Required(ErrorMessage = "Description is required")]
            public string? Description { get; set; }
        }

        public class AddProductModel
        {
            [Required(ErrorMessage = "Product name is required")]
            public string? ProductName { get; set; }

            [Required(ErrorMessage = "Display text is required")]
            public string? DisplayText { get; set; }

            public bool IsVariableValue { get; set; }

            public decimal? Value { get; set; }
        }

        public class AddTransactionFeeModel
        {
            [Required(ErrorMessage = "Description is required")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Calculation type is required")]
            public string? CalculationType { get; set; }

            [Required(ErrorMessage = "Fee type is required")]
            public string? FeeType { get; set; }

            [Required(ErrorMessage = "Fee value is required")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Fee value must be greater than 0")]
            public decimal? FeeValue { get; set; }
        }
    }
}
