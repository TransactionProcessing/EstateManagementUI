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
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using System.Security.Claims;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class HomePageTests : BaseTest
{
    private readonly ICalendarUIServiceImposter _mockCalenderUiService;
    private readonly IMerchantUIServiceImposter _mockMerchantUiService;
    private readonly ITransactionUIServiceImposter _mockTransactionUiService;
    private readonly IJSRuntimeImposter _mockJSRuntime;

    public HomePageTests()
    {
        _mockCalenderUiService = new ICalendarUIServiceImposter();
        _mockMerchantUiService = new IMerchantUIServiceImposter();
        _mockTransactionUiService = new ITransactionUIServiceImposter();

        _mockJSRuntime = new IJSRuntimeImposter();

        Services.AddSingleton(_mockCalenderUiService.Instance());
        Services.AddSingleton(_mockMerchantUiService.Instance());
        Services.AddSingleton(_mockTransactionUiService.Instance());
        Services.AddSingleton(_mockJSRuntime.Instance());

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
                MerchantId = Guid.Parse("974774C4-2928-49D2-B54D-E71DDFE77099"),
                Name = "Test Merchant 1",
                Reference = "MERCH001",
                CreatedDateTime = DateTime.Now
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("6ECC0B88-E028-4840-AE6A-86F979349575"),
                Name = "Test Merchant 2",
                Reference = "MERCH002",
                CreatedDateTime = DateTime.Now.AddDays(-1)
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("2C407ABE-8B1D-40A8-BA7C-943AE189D6BC"),
                Name = "Test Merchant 3",
                Reference = "MERCH003",
                CreatedDateTime = DateTime.Now.AddDays(-5)
            }
        };

        _mockAuthStateProvider.GetAuthenticationStateAsync().Returns(authState);
        this._mockCalenderUiService.GetComparisonDates(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(comparisonDates));
        this._mockMerchantUiService.GetMerchantKpis(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(merchantKpi));
        this._mockMerchantUiService.GetRecentMerchants(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(recentMerchants));
        this._mockTransactionUiService.GetTodaysSales(Arg<CorrelationId>.Any(), Arg<Guid>.Any(), Arg<DateTime>.Any()).ReturnsAsync(Result.Success(todaysSales));
        this._mockTransactionUiService.GetTodaysFailedSales(Arg<CorrelationId>.Any(), Arg<Guid>.Any(), Arg<String>.Any(), Arg<DateTime>.Any()).ReturnsAsync(Result.Success(todaysSales));

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
                MerchantId = Guid.Parse("974774C4-2928-49D2-B54D-E71DDFE77099"),
                Name = "Test Merchant 1",
                Reference = "MERCH001",
                CreatedDateTime = DateTime.Now
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("6ECC0B88-E028-4840-AE6A-86F979349575"),
                Name = "Test Merchant 2",
                Reference = "MERCH002",
                CreatedDateTime = DateTime.Now.AddDays(-1)
            },
            new MerchantModels.RecentMerchantsModel
            {
                MerchantId = Guid.Parse("2C407ABE-8B1D-40A8-BA7C-943AE189D6BC"),
                Name = "Test Merchant 3",
                Reference = "MERCH003",
                CreatedDateTime = DateTime.Now.AddDays(-5)
            }
        };

        _mockAuthStateProvider.GetAuthenticationStateAsync().Returns(authState);

        this._mockCalenderUiService.GetComparisonDates(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(comparisonDates));
        this._mockMerchantUiService.GetMerchantKpis(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(merchantKpi));
        this._mockMerchantUiService.GetRecentMerchants(Arg<CorrelationId>.Any(), Arg<Guid>.Any()).ReturnsAsync(Result.Success(recentMerchants));
        this._mockTransactionUiService.GetTodaysSales(Arg<CorrelationId>.Any(), Arg<Guid>.Any(), Arg<DateTime>.Any()).ReturnsAsync(Result.Success(todaysSales));
        this._mockTransactionUiService.GetTodaysFailedSales(Arg<CorrelationId>.Any(), Arg<Guid>.Any(), Arg<String>.Any(), Arg<DateTime>.Any()).ReturnsAsync(Result.Success(todaysSales));


        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
