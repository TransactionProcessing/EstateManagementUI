using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class HomePageTests : BaseTest
{
    private readonly Mock<ICalendarUIService> _mockCalenderUiService;
    private readonly Mock<IMerchantUIService> _mockMerchantUiService;
    private readonly Mock<ITransactionUIService> _mockTransactionUiService;
    private readonly Mock<IJSRuntime> _mockJSRuntime;
    
    public HomePageTests()
    {
        _mockCalenderUiService = new Mock<ICalendarUIService>();
        _mockMerchantUiService = new Mock<IMerchantUIService>();
        _mockTransactionUiService = new Mock<ITransactionUIService>();

        _mockJSRuntime = new Mock<IJSRuntime>();
        
        Services.AddSingleton(_mockCalenderUiService.Object);
        Services.AddSingleton(_mockMerchantUiService.Object);
        Services.AddSingleton(_mockTransactionUiService.Object);
        Services.AddSingleton(_mockJSRuntime.Object);
        
        // Add required permission components
        ComponentFactories.AddStub<RequirePermission>();
        ComponentFactories.AddStub<RequireSectionAccess>();
    }

    [Fact]
    public void Home_RendersCorrectly()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"),
            new Claim("estateId", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));

        List<ComparisonDateModel> comparisonDates = new()
        {
            new ComparisonDateModel
            {
                Date = DateTime.Today,
                Description = "Today"
            },
            new ComparisonDateModel
            {
                Date = DateTime.Today.AddDays(-1),
                Description = "Yesterday"
            }
        };

        TransactionModels.MerchantKpiModel merchantKpi = new() { MerchantsWithNoSaleInLast7Days = 5, MerchantsWithNoSaleToday = 12, MerchantsWithSaleInLastHour = 45 };

        TransactionModels.TodaysSalesModel todaysSales = new()
        {
            ComparisonSalesCount = 450,
            ComparisonSalesValue = 125000.00m,
            ComparisonAverageValue = 277.78m,
            TodaysSalesCount = 523,
            TodaysSalesValue = 145000.00m,
            TodaysAverageValue = 277.24m
        };

        List<MerchantModels.RecentMerchantsModel> recentMerchants = new()
        {
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Test Merchant 1",
                Reference = "MERCH001",
                CreatedDateTime = DateTime.Now
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                Name = "Test Merchant 2",
                Reference = "MERCH002",
                CreatedDateTime = DateTime.Now.AddDays(-1)
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
                Name = "Test Merchant 3",
                Reference = "MERCH003",
                CreatedDateTime = DateTime.Now.AddDays(-5)
            }
        };

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);
        this._mockCalenderUiService.Setup(m => m.GetComparisonDates(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(comparisonDates));
        this._mockMerchantUiService.Setup(m => m.GetMerchantKpis(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(merchantKpi));
        this._mockMerchantUiService.Setup(m => m.GetRecentMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(recentMerchants));
        this._mockTransactionUiService.Setup(m => m.GetTodaysSales(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<DateTime>())).ReturnsAsync(Result.Success(todaysSales));
        this._mockTransactionUiService.Setup(m => m.GetTodaysFailedSales(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<DateTime>())).ReturnsAsync(Result.Success(todaysSales));

        // Act
        var cut = RenderComponent<Home>();
        
        // Assert
        cut.Markup.ShouldContain("Dashboard");
        cut.Markup.ShouldContain("Welcome to Estate Management System");
    }

    [Fact]
    public void Home_HasCorrectPageTitle()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"), 
            new Claim("estateId", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));

        List<ComparisonDateModel> comparisonDates = new()
        {
            new ComparisonDateModel
            {
                Date = DateTime.Today,
                Description = "Today"
            },
            new ComparisonDateModel
            {
                Date = DateTime.Today.AddDays(-1),
                Description = "Yesterday"
            }
        };

        TransactionModels.MerchantKpiModel merchantKpi = new() { MerchantsWithNoSaleInLast7Days = 5, MerchantsWithNoSaleToday = 12, MerchantsWithSaleInLastHour = 45 };

        TransactionModels.TodaysSalesModel todaysSales = new()
        {
            ComparisonSalesCount = 450,
            ComparisonSalesValue = 125000.00m,
            ComparisonAverageValue = 277.78m,
            TodaysSalesCount = 523,
            TodaysSalesValue = 145000.00m,
            TodaysAverageValue = 277.24m
        };

        List<MerchantModels.RecentMerchantsModel> recentMerchants = new()
        {
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Test Merchant 1",
                Reference = "MERCH001",
                CreatedDateTime = DateTime.Now
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                Name = "Test Merchant 2",
                Reference = "MERCH002",
                CreatedDateTime = DateTime.Now.AddDays(-1)
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
                Name = "Test Merchant 3",
                Reference = "MERCH003",
                CreatedDateTime = DateTime.Now.AddDays(-5)
            }
        };

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);

        this._mockCalenderUiService.Setup(m => m.GetComparisonDates(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(comparisonDates));
        this._mockMerchantUiService.Setup(m => m.GetMerchantKpis(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(merchantKpi));
        this._mockMerchantUiService.Setup(m => m.GetRecentMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(recentMerchants));
        this._mockTransactionUiService.Setup(m => m.GetTodaysSales(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<DateTime>())).ReturnsAsync(Result.Success(todaysSales));
        this._mockTransactionUiService.Setup(m => m.GetTodaysFailedSales(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<DateTime>())).ReturnsAsync(Result.Success(todaysSales));


        // Act
        var cut = RenderComponent<Home>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
