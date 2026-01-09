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
        var claims = new[] { new Claim(ClaimTypes.Role, "Estate") };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        
        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);
        
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
        var claims = new[] { new Claim(ClaimTypes.Role, "Estate") };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        
        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);
        
        // Act
        var cut = RenderComponent<Home>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
