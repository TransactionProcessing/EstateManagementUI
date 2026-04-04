using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using SimpleResults;
using System.Globalization;
using System.Linq;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class New
    {
        private static readonly string[] OpeningHoursFormats = ["HHmm", "Hmm", "HH:mm", "H:mm"];
        private readonly MerchantModels.CreateMerchantModel model = new();
        private bool isSaving = false;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender)
            {
                return;
            }
            Result result = await OnAfterRender(PermissionSection.Merchant, PermissionFunction.Create);
            if (result.IsFailed)
            {
                return;
            }

            this.StateHasChanged();
        }

        private async Task HandleSubmit() {
            isSaving = true;
            errorMessage = null;
            successMessage = null;

            bool hasOpeningHoursValues = HasOpeningHoursValues();
            if (hasOpeningHoursValues && TryNormaliseAndValidateOpeningHours(out string validationError) == false) {
                errorMessage = validationError;
                isSaving = false;
                StateHasChanged();
                return;
            }

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            // Get the newly created merchant ID
            var merchantId = Guid.NewGuid();

            var result = await this.MerchantUiService.CreateMerchant(correlationId, estateId, merchantId, this.model);

            if (result.IsSuccess) {
                if (hasOpeningHoursValues) {
                    Result openingHoursResult = await this.MerchantUiService.UpdateMerchantOpeningHours(correlationId, estateId, merchantId, this.model.OpeningHours);
                    if (openingHoursResult.IsFailed) {
                        this.errorMessage = "Merchant created but opening hours could not be saved";
                        isSaving = false;
                        StateHasChanged();
                        return;
                    }
                }

                successMessage = "Merchant created successfully";

                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to contracts list with success
                NavigationManager.NavigateToMerchantList();
            }
            else {
                this.errorMessage = "Failed to create merchant";
            }

            isSaving = false;
        }

        private void Cancel() => NavigationManager.NavigateToMerchantList();

        private IReadOnlyList<OpeningHoursRow> GetOpeningHoursRows() =>
        [
            new("Monday", model.OpeningHours.Monday),
            new("Tuesday", model.OpeningHours.Tuesday),
            new("Wednesday", model.OpeningHours.Wednesday),
            new("Thursday", model.OpeningHours.Thursday),
            new("Friday", model.OpeningHours.Friday),
            new("Saturday", model.OpeningHours.Saturday),
            new("Sunday", model.OpeningHours.Sunday)
        ];

        private bool HasOpeningHoursValues() =>
            this.GetOpeningHoursRows().Any(row => string.IsNullOrWhiteSpace(row.Hours.Opening) == false ||
                                                 string.IsNullOrWhiteSpace(row.Hours.Closing) == false);

        private bool TryNormaliseAndValidateOpeningHours(out string validationError) {
            foreach (OpeningHoursRow row in this.GetOpeningHoursRows()) {
                if (TryValidateOpeningHoursRow(row, out validationError) == false) {
                    return false;
                }
            }

            validationError = string.Empty;
            return true;
        }

        private static string? NormaliseOpeningHoursValue(string? value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return value;
            }

            string trimmedValue = value.Trim();

            if (DateTime.TryParseExact(trimmedValue,
                                       OpeningHoursFormats,
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out DateTime parsed) == false) {
                return trimmedValue;
            }

            return parsed.ToString("HHmm", CultureInfo.InvariantCulture);
        }

        private static bool TryParseOpeningHoursValue(string? value, out TimeSpan time) {
            time = default;

            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            if (DateTime.TryParseExact(value, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed) == false) {
                return false;
            }

            time = parsed.TimeOfDay;
            return true;
        }

        private static bool TryValidateOpeningHoursRow(OpeningHoursRow row, out string validationError) {
            row.Hours.Opening = NormaliseOpeningHoursValue(row.Hours.Opening);
            row.Hours.Closing = NormaliseOpeningHoursValue(row.Hours.Closing);

            if (TryParseOpeningHoursValue(row.Hours.Opening, out TimeSpan openingTime) == false) {
                validationError = $"{row.DayName} opening time must be entered in HHmm format.";
                return false;
            }

            if (TryParseOpeningHoursValue(row.Hours.Closing, out TimeSpan closingTime) == false) {
                validationError = $"{row.DayName} closing time must be entered in HHmm format.";
                return false;
            }

            if (closingTime <= openingTime) {
                validationError = $"{row.DayName} closing time must be later than opening time.";
                return false;
            }

            validationError = string.Empty;
            return true;
        }

        private sealed record OpeningHoursRow(string DayName, MerchantModels.DayOpeningHoursModel Hours);
    }
}
