using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Components.Pages.Contracts
{
    public partial class New
    {
        private CreateContractFormModel model = new();
        private bool isSaving = false;
        private bool isLoadingOperators = true;
        private bool hasPermission = false;
        private string? errorMessage;
        private List<OperatorDropDownModel>? operators;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            await RequirePermission(PermissionSection.Contract, PermissionFunction.Create);
            await LoadOperators();
        }


        private async Task LoadOperators()
        {
            try
            {
                isLoadingOperators = true;
                var estateId = await GetEstateId();
                var result = await Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(CorrelationIdHelper.New(), estateId));
                if (result.IsSuccess)
                {
                    operators = ModelFactory.ConvertFrom(result.Data);
                }
            }
            finally
            {
                isLoadingOperators = false;
                StateHasChanged();
            }
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            errorMessage = null;

            try {
                var estateId = await this.GetEstateId();

                // Create contract
                var createCommand = new ContractCommands.CreateContractCommand(
                    CorrelationIdHelper.New(),
                    estateId,
                    model.Description!,
                    Guid.Parse(model.OperatorId!)
                );

                var createResult = await Mediator.Send(createCommand);

                if (!createResult.IsSuccess)
                {
                    errorMessage = createResult.Message ?? "Failed to create contract";
                    return;
                }

                // Navigate to contracts list with success
                NavigationManager.NavigateTo("/contracts");
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

        private void Cancel()
        {
            NavigationManager.NavigateTo("/contracts");
        }

        public class CreateContractFormModel
        {
            [Required(ErrorMessage = "Description is required")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Operator is required")]
            public string? OperatorId { get; set; }
        }
    }
}
