using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Merchants;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using Xunit;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantSchedulePageTests : BaseTest
{
    [Fact]
    public void MerchantSchedule_LoadsMerchantAndSchedule()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Markup.ShouldContain("Merchant Schedule: Test Merchant");
        cut.Markup.ShouldContain("Selected Year Maintenance");
        cut.Find("#month-1-closed-days");
    }

    [Fact]
    public void MerchantSchedule_YearSelector_DisplaysNextTenYears()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        var options = cut.FindAll("#selectedYear option");

        options.Count.ShouldBe(10);
        options.First().GetAttribute("value").ShouldBe(currentYear.ToString());
        options.Last().GetAttribute("value").ShouldBe((currentYear + 9).ToString());
    }

    [Fact]
    public void MerchantSchedule_ClonePreviousYear_CopiesEditableMonths()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var futureYear = currentYear + 1;

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(futureYear, new MerchantModels.MerchantScheduleModel { Year = futureYear, Months = [] });
        SetupSchedule(currentYear, new MerchantModels.MerchantScheduleModel
        {
            Year = currentYear,
            Months = new List<MerchantModels.MerchantScheduleMonthModel>
            {
                new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } }
            }
        });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(futureYear.ToString());
        cut.Find("#loadYearButton").Click();
        cut.WaitForAssertion(() => cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeFalse(), timeout: TimeSpan.FromSeconds(5));
        cut.Find("#clonePreviousYearButton").Click();

        cut.Find("#month-1-closed-days").GetAttribute("value").ShouldBe("1, 2, 15");
    }

    [Fact]
    public void MerchantSchedule_SaveSelectedYear_SendsYearPayload()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var futureYear = currentYear + 1;

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(futureYear, new MerchantModels.MerchantScheduleModel { Year = futureYear, Months = [] });
        this.MerchantUIService.Setup(m => m.SaveMerchantSchedule(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId,
                It.IsAny<MerchantModels.MerchantScheduleModel>()))
            .ReturnsAsync(Result.Success());

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(futureYear.ToString());
        cut.Find("#loadYearButton").Click();
        cut.WaitForAssertion(() => cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeFalse(), timeout: TimeSpan.FromSeconds(5));
        cut.Find("#month-1-closed-days").Change("1, 2, 15");
        cut.Find("#saveScheduleButton").Click();

        this.MerchantUIService.Verify(m => m.SaveMerchantSchedule(
            It.IsAny<CorrelationId>(),
            It.IsAny<Guid>(),
            merchantId,
            It.Is<MerchantModels.MerchantScheduleModel>(schedule =>
                schedule.Year == futureYear &&
                schedule.Months.Any(month => month.Month == 1 && month.ClosedDays.SequenceEqual(new[] { 1, 2, 15 }))
             )), Times.Once);
    }

    [Fact]
    public async Task MerchantSchedule_SaveSelectedYear_InvalidNovemberDate_ShowsErrorAndDoesNotSave()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var futureYear = currentYear + 1;

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(futureYear, new MerchantModels.MerchantScheduleModel { Year = futureYear, Months = [] });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(futureYear.ToString());
        cut.Find("#loadYearButton").Click();
        cut.WaitForAssertion(() => cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeFalse(), timeout: TimeSpan.FromSeconds(5));
        cut.Find("#month-11-closed-days").Change("31");
        
        var saveScheduleMethod = GetSaveScheduleMethod();
        var errorMessageField = GetErrorMessageField(cut.Instance);

        saveScheduleMethod.ShouldNotBeNull();
        errorMessageField.ShouldNotBeNull();

        await cut.InvokeAsync(async () =>
        {
            var task = (Task)saveScheduleMethod.Invoke(cut.Instance, null);
            await task;
        });

        errorMessageField.GetValue(cut.Instance).ShouldBe($"Only days between 1 and 30 can be supplied for November {futureYear}.");
        this.MerchantUIService.Verify(m => m.SaveMerchantSchedule(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId,
            It.IsAny<MerchantModels.MerchantScheduleModel>()), Times.Never);
    }

    [Fact]
    public void MerchantSchedule_SaveSelectedYear_LeapYearFebruary_Allows29th()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var leapYear = GetNextLeapYear(currentYear);

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(leapYear, new MerchantModels.MerchantScheduleModel { Year = leapYear, Months = [] });
        this.MerchantUIService.Setup(m => m.SaveMerchantSchedule(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId,
                It.IsAny<MerchantModels.MerchantScheduleModel>()))
            .ReturnsAsync(Result.Success());

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(leapYear.ToString());
        cut.Find("#loadYearButton").Click();
        cut.WaitForAssertion(() => cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeFalse(), timeout: TimeSpan.FromSeconds(5));
        cut.Find("#month-2-closed-days").Change("29");
        cut.Find("#saveScheduleButton").Click();

        this.MerchantUIService.Verify(m => m.SaveMerchantSchedule(
            It.IsAny<CorrelationId>(),
            It.IsAny<Guid>(),
            merchantId,
            It.Is<MerchantModels.MerchantScheduleModel>(schedule =>
                schedule.Year == leapYear &&
                schedule.Months.Any(month => month.Month == 2 && month.ClosedDays.SequenceEqual(new[] { 29 }))
            )), Times.Once);
    }

    [Fact]
    public async Task MerchantSchedule_SaveSelectedYear_NonLeapYearFebruary_Rejects29th()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var nonLeapYear = GetNextNonLeapYear(currentYear);

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(nonLeapYear, new MerchantModels.MerchantScheduleModel { Year = nonLeapYear, Months = [] });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(nonLeapYear.ToString());
        cut.Find("#loadYearButton").Click();
        cut.WaitForAssertion(() => cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeFalse(), timeout: TimeSpan.FromSeconds(5));
        cut.Find("#month-2-closed-days").Change("29");
        
        var saveScheduleMethod = GetSaveScheduleMethod();
        var errorMessageField = GetErrorMessageField(cut.Instance);

        saveScheduleMethod.ShouldNotBeNull();
        errorMessageField.ShouldNotBeNull();

        await cut.InvokeAsync(async () =>
        {
            var task = (Task)saveScheduleMethod.Invoke(cut.Instance, null);
            await task;
        });

        errorMessageField.GetValue(cut.Instance).ShouldBe($"Only days between 1 and 28 can be supplied for February {nonLeapYear}.");
        this.MerchantUIService.Verify(m => m.SaveMerchantSchedule(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId,
            It.IsAny<MerchantModels.MerchantScheduleModel>()), Times.Never);
    }

    [Fact]
    public void MerchantSchedule_PreviousYear_IsReadOnly()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;
        var previousYear = currentYear - 1;

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel { Year = currentYear, Months = [] });
        SetupSchedule(previousYear, new MerchantModels.MerchantScheduleModel { Year = previousYear, Months = [] });

        var cut = RenderComponent<Schedule>(parameters => parameters.Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#selectedYear").Change(previousYear.ToString());
        cut.Find("#loadYearButton").Click();

        cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeTrue();
        cut.Find("#saveScheduleButton").HasAttribute("disabled").ShouldBeTrue();
    }

    [Fact]
    public void MerchantSchedule_ReadOnlyMode_DisablesEditingControls()
    {
        var merchantId = Guid.NewGuid();
        var currentYear = DateTime.Today.Year;

        SetupPageData(merchantId, currentYear, new MerchantModels.MerchantScheduleModel
        {
            Year = currentYear,
            Months = new List<MerchantModels.MerchantScheduleMonthModel>
            {
                new() { Month = currentYear == DateTime.Today.Year && DateTime.Today.Month == 12 ? 12 : DateTime.Today.Month + 1, ClosedDays = new List<Int32> { 1, 2 } }
            }
        });

        _fakeNavigationManager.NavigateTo($"/merchants/{merchantId}/schedule?readOnly=true");

        var cut = RenderComponent<Schedule>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Markup.ShouldContain("Selected Year Schedule");
        cut.Find("#month-1-closed-days").HasAttribute("disabled").ShouldBeTrue();
        cut.FindAll("#clonePreviousYearButton").Count.ShouldBe(0);
        cut.FindAll("#saveScheduleButton").Count.ShouldBe(0);
    }

    private void SetupPageData(Guid merchantId,
                               Int32 scheduleYear,
                               MerchantModels.MerchantScheduleModel schedule)
    {
        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(new MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }));

        SetupSchedule(scheduleYear, schedule);
    }

    private void SetupSchedule(Int32 year,
                               MerchantModels.MerchantScheduleModel schedule)
    {
        this.MerchantUIService.Setup(m => m.GetMerchantSchedule(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), year))
            .ReturnsAsync(Result.Success(schedule));
    }

    private static Int32 GetNextLeapYear(Int32 startYear)
    {
        var year = startYear + 1;
        while (DateTime.IsLeapYear(year) == false)
        {
            year++;
        }

        return year;
    }

    private static Int32 GetNextNonLeapYear(Int32 startYear)
    {
        var year = startYear + 1;
        while (DateTime.IsLeapYear(year))
        {
            year++;
        }

        return year;
    }

    private static System.Reflection.MethodInfo GetSaveScheduleMethod()
    {
        return typeof(Schedule).GetMethod("SaveScheduleAsync",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    }

    private static System.Reflection.FieldInfo GetErrorMessageField(object instance)
    {
        Type currentType = instance.GetType();
        System.Reflection.FieldInfo field = null;

        while (currentType != null && field == null)
        {
            field = currentType.GetField("errorMessage",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            currentType = currentType.BaseType;
        }

        return field;
    }
}
