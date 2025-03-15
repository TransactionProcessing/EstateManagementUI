using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Reporting.TransactionAnalysis;
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

public class TransactionAnalysisTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly TransactionAnalysis _transactionAnalysis;

    public TransactionAnalysisTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._transactionAnalysis = new TransactionAnalysis(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._transactionAnalysis.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_PopulatesComparisonDate()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.ComparisonDates1));
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.MountAsync();

        // Assert
        this._transactionAnalysis.ComparisonDate.ShouldNotBeNull();
        this._transactionAnalysis.ComparisonDate.Dates.ShouldNotBeEmpty();
        this._transactionAnalysis.ComparisonDate.Dates[0].Text.ShouldBe("2023-01-01");
    }

    [Fact]
    public async Task Query_AllQueriesSuccessful()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetTodaysSalesQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.ComparisonLabel.ShouldBeNull();
        this._transactionAnalysis.TodaysSales.Variance.ShouldBe(0);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(0);
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(0);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetMerchantKpiQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(0);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(0);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(0);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetBottomMerchantDataQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldBe(new List<TopBottomMerchant>());

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetBottomOperatorDataQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldBe(new List<TopBottomOperator>());

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetBottomProductDataQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysFailedSales1));
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldBe(new List<TopBottomProduct>());

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(70);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(50);
    }

    [Fact]
    public async Task Query_GetTodaysFailedSalesQueryFailed()
    {
        // Arrange
        this._transactionAnalysis.ComparisonDate = new ComparisonDateListModel
        {
            SelectedDate = DateTime.Parse("2023-01-01")
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.TodaysSales1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.MerchantKpi1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomMerchantDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomMerchants1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomOperatorDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomOperators1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetBottomProductDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.BottomProducts1));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetTodaysFailedSalesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure());
        // Act
        await this._transactionAnalysis.Query();

        // Assert
        this._transactionAnalysis.TodaysSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysSales.TodaysSalesValue.ShouldBe(100);
        this._transactionAnalysis.TodaysSales.ComparisonSalesValue.ShouldBe(80);

        // Assert
        this._transactionAnalysis.MerchantKpi.ShouldNotBeNull();
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
        this._transactionAnalysis.MerchantKpi.MerchantsWithNoSaleToday.ShouldBe(3);
        this._transactionAnalysis.MerchantKpi.MerchantsWithSaleInLastHour.ShouldBe(2);

        this._transactionAnalysis.TopBottomMerchants.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomMerchants.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomMerchants[0].MerchantName.ShouldBe("Merchant1");
        this._transactionAnalysis.TopBottomMerchants[0].SalesValue.ShouldBe(50);

        this._transactionAnalysis.TopBottomOperators.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomOperators.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomOperators[0].OperatorName.ShouldBe("Operator1");
        this._transactionAnalysis.TopBottomOperators[0].SalesValue.ShouldBe(40);

        this._transactionAnalysis.TopBottomProducts.ShouldNotBeNull();
        this._transactionAnalysis.TopBottomProducts.Count.ShouldBe(2);
        this._transactionAnalysis.TopBottomProducts[0].ProductName.ShouldBe("Product1");
        this._transactionAnalysis.TopBottomProducts[0].SalesValue.ShouldBe(60);

        this._transactionAnalysis.TodaysFailedSales.ShouldNotBeNull();
        this._transactionAnalysis.TodaysFailedSales.ComparisonLabel.ShouldBeNull();
        this._transactionAnalysis.TodaysFailedSales.TodaysSalesValue.ShouldBe(0);
        this._transactionAnalysis.TodaysFailedSales.ComparisonSalesValue.ShouldBe(0);
        this._transactionAnalysis.TodaysFailedSales.Variance.ShouldBe(0);
    }
}