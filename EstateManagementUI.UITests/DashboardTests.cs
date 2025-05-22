using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Dashboard.Dashboard;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.UITests;

public class DashboardTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Dashboard _dashboard;
    
    public DashboardTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._dashboard = new Dashboard(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._dashboard.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_ShouldPopulateDropdownsAndQueryData()
    {
        // Arrange
        var merchants = TestData.GetMerchants();
        var operators = TestData.GetOperators();
        var comparisonDates = TestData.GetComparisonDates();

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(merchants);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(operators);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comparisonDates);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await this._dashboard.MountAsync();

        // Assert
        this._dashboard.Merchant.ShouldNotBeNull();
        this._dashboard.Operator.ShouldNotBeNull();
        this._dashboard.ComparisonDate.ShouldNotBeNull();
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        this._dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await this._dashboard.Query();

        // Assert
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData_GetTodaysSalesFailed()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        this._dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await this._dashboard.Query();

        // Assert
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData_GetTodaysSettlementFailed()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        this._dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await this._dashboard.Query();

        // Assert
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData_GetTodaysSalesCountByHourFailed()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        this._dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesValueByHourModels()));

        // Act
        await this._dashboard.Query();

        // Assert
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Query_ShouldGetTodaysData_GetTodaysSalesValueByHourFailed()
    {
        // Arrange
        var selectedDate = DateTime.Now;
        this._dashboard.ComparisonDate = new ComparisonDateListModel { SelectedDate = selectedDate };

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSettlementModel()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.GetTodaysSalesCountByHourModels()));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());

        // Act
        await this._dashboard.Query();

        // Assert
        this._dashboard.TodaysSales.ShouldNotBeNull();
        this._dashboard.TodaysSettlement.ShouldNotBeNull();
        this._dashboard.TodaysSalesCountByHour.ShouldNotBeNull();
        this._dashboard.TodaysSalesValueByHour.ShouldNotBeNull();
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesCountByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Queries.GetTodaysSalesValueByHourQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}