using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class New
    {
        private readonly New.CreateMerchantModel model = new();
        private bool isSaving = false;
        private string? successMessage;
        private string? errorMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender) {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }

            await RequirePermission(PermissionSection.Merchant, PermissionFunction.Create);
        }

        private async Task HandleSubmit()
        {
            isSaving = true;
            errorMessage = null;

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var address = new MerchantCommands.MerchantAddress(Guid.Empty, model.AddressLine1, model.Town, model.Region, model.PostCode, model.Country);
                var contact = new MerchantCommands.MerchantContact(Guid.Empty, model.ContactName, model.EmailAddress, model.PhoneNumber);
                
                // Get the newly created merchant ID
                var merchantId = Guid.NewGuid();

                // Create merchant
                var createCommand = new MerchantCommands.CreateMerchantCommand(
                    correlationId,
                    estateId, merchantId,
                    this.model.MerchantName,
                    this.model.SettlementSchedule,address, contact);

                var createResult = await Mediator.Send(createCommand);

                if (!createResult.IsSuccess)
                {
                    errorMessage = createResult.Message ?? "Failed to create merchant";
                    return;
                }

                // Show success message briefly before navigating away
                successMessage = "Merchant created successfully.";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to merchant list
                NavigationManager.NavigateTo($"/merchants");
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

        public class CreateMerchantModel
        {
            [Required(ErrorMessage = "Merchant name is required")]
            public string? MerchantName { get; set; }

            [Required(ErrorMessage = "Settlement schedule is required")]
            public string? SettlementSchedule { get; set; }

            [Required(ErrorMessage = "Address line 1 is required")]
            public string? AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            [Required(ErrorMessage = "Town is required")]
            public string? Town { get; set; }

            [Required(ErrorMessage = "Region is required")]
            public string? Region { get; set; }

            [Required(ErrorMessage = "PostCode is required")]
            public string? PostCode { get; set; }

            [Required(ErrorMessage = "Country is required")]
            public string? Country { get; set; }

            [Required(ErrorMessage = "Contact name is required")]
            public string? ContactName { get; set; }

            [Required(ErrorMessage = "Email address is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string? EmailAddress { get; set; }

            [Required(ErrorMessage = "Phone number is required")]
            public string? PhoneNumber { get; set; }
        }
    }
}
