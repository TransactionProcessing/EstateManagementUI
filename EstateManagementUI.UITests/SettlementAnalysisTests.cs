using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Reporting.SettlementAnalysis;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;

namespace EstateManagementUI.UITests;

public class SettlementAnalysisTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly SettlementAnalysis _settlementAnalysis;

    public SettlementAnalysisTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._settlementAnalysis = new SettlementAnalysis(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._settlementAnalysis.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_PopulatesComparisonDate()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ComparisonDates1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSettlement1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetLastSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.LastSettlement1));
        // Act
        await this._settlementAnalysis.MountAsync();

        // Assert
        this._settlementAnalysis.ComparisonDate.ShouldNotBeNull();
        this._settlementAnalysis.ComparisonDate.Dates.ShouldNotBeEmpty();
        this._settlementAnalysis.ComparisonDate.Dates[0].Text.ShouldBe("2023-01-01");
    }

    [Fact]
    public async Task Query_PopulatesTodaysSettlement()
    {
        // Arrange
        this._settlementAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSettlement1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetLastSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.LastSettlement1));


        // Act
        await this._settlementAnalysis.Query();

        // Assert
        this._settlementAnalysis.TodaysSettlement.ShouldNotBeNull();
        this._settlementAnalysis.TodaysSettlement.TodaysSettlementValue.ShouldBe(1000);
        this._settlementAnalysis.TodaysSettlement.ComparisonSettlementValue.ShouldBe(800);

        this._settlementAnalysis.LastSettlement.ShouldNotBeNull();
        this._settlementAnalysis.LastSettlement.SettlementDate.ShouldBe(DateTime.Parse("2023-01-01"));
        this._settlementAnalysis.LastSettlement.SettlementSalesValue.ShouldBe(1500);
        this._settlementAnalysis.LastSettlement.SettlementFeesValue.ShouldBe(100);
    }

    [Fact]
    public async Task Query_GetTodaysSettlementQueryFailed()
    {
        // Arrange
        this._settlementAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetLastSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.LastSettlement1));


        // Act
        await this._settlementAnalysis.Query();

        // Assert
        this._settlementAnalysis.TodaysSettlement.ShouldNotBeNull();
        this._settlementAnalysis.TodaysSettlement.TodaysSettlementValue.ShouldBe(0);
        this._settlementAnalysis.TodaysSettlement.ComparisonSettlementValue.ShouldBe(0);

        this._settlementAnalysis.LastSettlement.ShouldNotBeNull();
        this._settlementAnalysis.LastSettlement.SettlementDate.ShouldBe(DateTime.Parse("2023-01-01"));
        this._settlementAnalysis.LastSettlement.SettlementSalesValue.ShouldBe(1500);
        this._settlementAnalysis.LastSettlement.SettlementFeesValue.ShouldBe(100);
    }

    [Fact]
    public async Task Query_GetLastSettlementQueryFailed()
    {
        // Arrange
        this._settlementAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSettlement1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetLastSettlementQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());


        // Act
        await this._settlementAnalysis.Query();

        // Assert
        this._settlementAnalysis.TodaysSettlement.ShouldNotBeNull();
        this._settlementAnalysis.TodaysSettlement.TodaysSettlementValue.ShouldBe(1000);
        this._settlementAnalysis.TodaysSettlement.ComparisonSettlementValue.ShouldBe(800);

        this._settlementAnalysis.LastSettlement.ShouldNotBeNull();
        this._settlementAnalysis.LastSettlement.SettlementDate.ShouldBe(DateTime.MinValue);
        this._settlementAnalysis.LastSettlement.SettlementSalesValue.ShouldBe(0);
        this._settlementAnalysis.LastSettlement.SettlementFeesValue.ShouldBe(0);
    }
}