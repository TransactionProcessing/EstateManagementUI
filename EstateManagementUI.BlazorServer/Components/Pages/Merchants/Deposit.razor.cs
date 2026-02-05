using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class Deposit
    {
        [Parameter]
        public Guid MerchantId { get; set; }

        private DepositModel model = new();
        private bool isSaving = false;
        private string? errorMessage;
        private string? successMessage;
        private string? merchantName;
        private bool hasPermission = false;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender) {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            await RequirePermission(PermissionSection.Merchant, PermissionFunction.MakeDeposit);

            // Set default date to today
            model.Date = DateTime.Today;

            // Load merchant name
            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var result = await Mediator.Send(new MerchantQueries.GetMerchantQuery(correlationId, estateId, MerchantId));
                if (result.IsSuccess && result.Data != null)
                {
                    merchantName = result.Data.MerchantName;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load merchant details: {ex.Message}";
            }
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            errorMessage = null;
            successMessage = null;

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var command = new MerchantCommands.MakeMerchantDepositCommand(
                    correlationId,
                    estateId,
                    MerchantId,
                    model.Amount,
                    model.Date,
                    model.Reference!
                );

                var result = await Mediator.Send(command);

                if (!result.IsSuccess)
                {
                    errorMessage = result.Message ?? "Failed to make deposit";
                    return;
                }

                // Show success message briefly before navigating away
                successMessage = "Deposit recorded successfully.";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate back to merchant list on success
                NavigationManager.NavigateTo("/merchants");
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
            NavigationManager.NavigateTo("/merchants");
        }

        public class DepositModel
        {
            [Required(ErrorMessage = "Deposit amount is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Deposit amount must be greater than 0")]
            public int Amount { get; set; }

            [Required(ErrorMessage = "Date of deposit is required")]
            [DateNotInFuture(ErrorMessage = "Date cannot be in the future")]
            public DateTime Date { get; set; } = DateTime.Today;

            [Required(ErrorMessage = "Reference is required")]
            public string? Reference { get; set; }
        }

        // Custom validation attribute for date not in future
        private class DateNotInFutureAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is DateTime date)
                {
                    if (date.Date > DateTime.Today)
                    {
                        return new ValidationResult(ErrorMessage ?? "Date cannot be in the future");
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}
