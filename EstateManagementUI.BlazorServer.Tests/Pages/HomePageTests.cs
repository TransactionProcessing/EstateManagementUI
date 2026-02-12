using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Shouldly;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class HomePageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    private readonly Mock<NavigationManager> _mockNavigationManager;
    private readonly Mock<IJSRuntime> _mockJSRuntime;
    private readonly Mock<IPermissionService> _mockPermissionService;

    public HomePageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        _mockNavigationManager = new Mock<NavigationManager>();
        _mockJSRuntime = new Mock<IJSRuntime>();
        _mockPermissionService = new Mock<IPermissionService>();
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockAuthStateProvider.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        Services.AddSingleton(_mockJSRuntime.Object);
        Services.AddSingleton(_mockPermissionService.Object);
        
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

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);
        this._mockMediator.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockComparisonDates()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantKpiQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockMerchantKpi()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockTodaysSales()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysFailedSalesQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockTodaysSales()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockRecentMerchants()));

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
        
        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);
        this._mockMediator.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>())).ReturnsAsync(Result.Success( StubTestData.GetMockComparisonDates()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantKpiQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockMerchantKpi()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysSalesQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockTodaysSales()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<TransactionQueries.GetTodaysFailedSalesQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockTodaysSales()));
        this._mockMediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>())).ReturnsAsync(Result.Success(StubTestData.GetMockRecentMerchants()));


        // Act
        var cut = RenderComponent<Home>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
