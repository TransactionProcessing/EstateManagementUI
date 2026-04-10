using System.Globalization;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class Schedule
    {
        [Parameter]
        public Guid MerchantId { get; set; }

        [Parameter]
        //[SupplyParameterFromQuery(Name = "readOnly")]
        public Boolean ReadOnly { get; set; }

        private readonly DateTime today = DateTime.Today;
        private readonly IReadOnlyList<Int32> availableYears = Enumerable.Range(DateTime.Today.Year, 10).ToList();
        private MerchantModels.MerchantModel? merchant;
        private List<ScheduleMonthEditor> monthEditors = [];
        private bool isLoading = true;
        private bool isSaving;
        private int selectedYear = DateTime.Today.Year;
        private int yearInput = DateTime.Today.Year;
        private string? errorMessage;
        private string? successMessage;

        private bool CanSave => this.ReadOnly == false && this.isSaving == false && this.monthEditors.Any(month => month.IsReadOnly == false);
        private bool CanClonePreviousYear => this.ReadOnly == false && this.isSaving == false && this.selectedYear > 1900 && this.monthEditors.Any(month => month.IsReadOnly == false);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender == false)
            {
                return;
            }

            PermissionFunction permissionFunction = this.ReadOnly ? PermissionFunction.View : PermissionFunction.Edit;
            Result result = await OnAfterRender(PermissionSection.Merchant, permissionFunction, this.LoadPage);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadPage()
        {
            try
            {
                isLoading = true;
                Guid estateId = await this.GetEstateId();
                CorrelationId correlationId = new(Guid.NewGuid());

                Result<MerchantModels.MerchantModel> merchantResult = await this.MerchantUIService.GetMerchant(correlationId, estateId, this.MerchantId);
                if (merchantResult.IsFailed)
                {
                    return ResultHelpers.CreateFailure(merchantResult);
                }

                this.merchant = merchantResult.Data;
                this.yearInput = this.selectedYear;

                return await this.LoadSchedule(estateId, this.selectedYear);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task LoadSelectedYearAsync()
        {
            this.successMessage = null;
            this.errorMessage = null;

            Int32 previousYear = this.selectedYear;
            Int32 requestedYear = this.yearInput;
            this.selectedYear = requestedYear;
            Guid estateId = await this.GetEstateId();
            Result result = await this.LoadSchedule(estateId, this.selectedYear);
            if (result.IsFailed)
            {
                this.selectedYear = previousYear;
                this.errorMessage = result.Errors.SingleOrDefault() ?? $"Failed to load schedule for {requestedYear}.";
            }
            this.StateHasChanged();
        }

        private async Task ClonePreviousYearAsync()
        {
            if (this.CanClonePreviousYear == false)
            {
                return;
            }

            this.successMessage = null;
            this.errorMessage = null;

            Guid estateId = await this.GetEstateId();
            CorrelationId correlationId = new(Guid.NewGuid());
            Int32 sourceYear = this.selectedYear - 1;

            Result<MerchantModels.MerchantScheduleModel> result = await this.MerchantUIService.GetMerchantSchedule(correlationId, estateId, this.MerchantId, sourceYear);
            if (result.Status == ResultStatus.NotFound)
            {
                this.errorMessage = $"No schedule exists for {sourceYear} to clone.";
                return;
            }

            if (result.IsFailed)
            {
                this.errorMessage = result.Errors.SingleOrDefault() ?? $"Failed to load schedule for {sourceYear}.";
                return;
            }

            Dictionary<Int32, MerchantModels.MerchantScheduleMonthModel> sourceMonths = result.Data.Months.ToDictionary(month => month.Month);
            foreach (ScheduleMonthEditor month in this.monthEditors.Where(month => month.IsReadOnly == false))
            {
                List<Int32> closedDays = sourceMonths.TryGetValue(month.Month, out MerchantModels.MerchantScheduleMonthModel? sourceMonth)
                    ? sourceMonth.ClosedDays.OrderBy(day => day).ToList()
                    : [];

                month.ClosedDaysInput = this.FormatClosedDays(closedDays);
            }

            this.successMessage = $"Editable months were cloned from {sourceYear}.";
        }

        private async Task SaveScheduleAsync()
        {
            if (this.CanSave == false)
            {
                return;
            }

            this.successMessage = null;
            this.errorMessage = null;
            this.isSaving = true;

            try
            {
                Result<MerchantModels.MerchantScheduleModel> scheduleResult = this.BuildScheduleToSave();
                if (scheduleResult.IsFailed)
                {
                    this.errorMessage = scheduleResult switch
                    {
                        _ when scheduleResult.Errors.Any() => scheduleResult.Errors.First(),
                        _ when String.IsNullOrEmpty(scheduleResult.Message) == false => scheduleResult.Message,
                        _ => "Invalid schedule."
                    };
                    return;
                }

                Guid estateId = await this.GetEstateId();
                CorrelationId correlationId = new(Guid.NewGuid());
                Result result = await this.MerchantUIService.SaveMerchantSchedule(correlationId, estateId, this.MerchantId, scheduleResult.Data);
                if (result.IsFailed)
                {
                    this.errorMessage = result switch {
                        _ when result.Errors.Any() => result.Errors.First(),
                        _ when String.IsNullOrEmpty(result.Message) == false => result.Message,
                        _ => "Failed to save merchant schedule."
                    };
                    return;
                }

                this.successMessage = $"Schedule saved for {this.selectedYear}.";
            }
            finally
            {
                this.isSaving = false;
                this.StateHasChanged();
            }
        }

        private async Task<Result> LoadSchedule(Guid estateId,
                                                Int32 year)
        {
            CorrelationId correlationId = new(Guid.NewGuid());
            Result<MerchantModels.MerchantScheduleModel> result = await this.MerchantUIService.GetMerchantSchedule(correlationId, estateId, this.MerchantId, year);

            if (result.Status == ResultStatus.NotFound)
            {
                this.BuildMonthEditors(null);
                return Result.Success();
            }

            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            this.BuildMonthEditors(result.Data);
            return Result.Success();
        }

        private Result<MerchantModels.MerchantScheduleModel> BuildScheduleToSave()
        {
            List<MerchantModels.MerchantScheduleMonthModel> editableMonths = [];

            foreach (ScheduleMonthEditor month in this.monthEditors.Where(month => month.IsReadOnly == false))
            {
                Result<List<Int32>> parseResult = this.ParseClosedDays(month.ClosedDaysInput, month.Month);
                if (parseResult.IsFailed)
                {
                    this.errorMessage = parseResult.Errors.SingleOrDefault() ?? $"Invalid closed days for {month.MonthName}.";
                    return Result.Failure();
                }

                month.ClosedDaysInput = this.FormatClosedDays(parseResult.Data);
                editableMonths.Add(new MerchantModels.MerchantScheduleMonthModel
                {
                    Month = month.Month,
                    ClosedDays = parseResult.Data
                });
            }

            return Result.Success(new MerchantModels.MerchantScheduleModel
            {
                Year = this.selectedYear,
                Months = editableMonths
            });
        }

        private Result<List<Int32>> ParseClosedDays(String? closedDaysInput,
                                                    Int32 month)
        {
            List<Int32> days = [];
            if (String.IsNullOrWhiteSpace(closedDaysInput))
            {
                return Result.Success(days);
            }

            foreach (String token in closedDaysInput.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                if (Int32.TryParse(token, out Int32 day) == false)
                {
                    return Result.Failure($"'{token}' is not a valid day number for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}.");
                }

                days.Add(day);
            }

            Int32 maxDay = DateTime.DaysInMonth(this.selectedYear, month);
            List<Int32> normalisedDays = days.Distinct().OrderBy(day => day).ToList();
            if (normalisedDays.Any(day => day < 1 || day > maxDay))
            {
                return Result.Failure($"Only days between 1 and {maxDay} can be supplied for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {this.selectedYear}.");
            }

            return Result.Success(normalisedDays);
        }

        private void BuildMonthEditors(MerchantModels.MerchantScheduleModel? schedule)
        {
            Dictionary<Int32, MerchantModels.MerchantScheduleMonthModel> monthLookup = schedule?.Months.ToDictionary(month => month.Month) ?? [];

            this.monthEditors = Enumerable.Range(1, 12).Select(month =>
            {
                List<Int32> closedDays = monthLookup.TryGetValue(month, out MerchantModels.MerchantScheduleMonthModel? scheduleMonth)
                    ? scheduleMonth.ClosedDays.OrderBy(day => day).ToList()
                    : [];
                Boolean isReadOnly = this.ReadOnly || this.IsMonthReadOnly(this.selectedYear, month);

                return new ScheduleMonthEditor
                {
                    Month = month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    ClosedDaysInput = this.FormatClosedDays(closedDays),
                    IsReadOnly = isReadOnly,
                    Description = isReadOnly
                        ? this.ReadOnly
                            ? "This schedule is read only from the merchant view screen."
                            : "This month has passed and cannot be changed."
                        : "Closed days can still be updated."
                };
            }).ToList();
        }

        private bool IsMonthReadOnly(Int32 year,
                                     Int32 month)
        {
            if (year < this.today.Year)
            {
                return true;
            }

            return year == this.today.Year && month < this.today.Month;
        }

        private String FormatClosedDays(IEnumerable<Int32> closedDays) => String.Join(", ", closedDays.OrderBy(day => day));

        private void BackToMerchant()
        {
            if (this.ReadOnly)
            {
                this.NavigationManager.NavigateToMerchant(this.MerchantId);
                return;
            }

            this.NavigationManager.NavigateToEditMerchant(this.MerchantId);
        }

        private sealed class ScheduleMonthEditor
        {
            public Int32 Month { get; init; }
            public String MonthName { get; init; } = String.Empty;
            public String ClosedDaysInput { get; set; } = String.Empty;
            public Boolean IsReadOnly { get; init; }
            public String Description { get; init; } = String.Empty;
            public String InputId => $"month-{this.Month}-closed-days";
        }
    }
}
