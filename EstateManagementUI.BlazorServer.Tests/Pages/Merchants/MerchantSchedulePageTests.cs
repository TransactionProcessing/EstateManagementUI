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
}
