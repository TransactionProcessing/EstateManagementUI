using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class PermissionsDebugPageTests : TestContext
{
    private readonly Mock<IPermissionService> _mockPermissionService;
    private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    private readonly Mock<IPermissionStore> _mockPermissionStore;

    public PermissionsDebugPageTests()
    {
        _mockPermissionService = new Mock<IPermissionService>();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        _mockPermissionStore = new Mock<IPermissionStore>();
        
        // Setup default mock behaviors
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        }, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(principal);
        
        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        
        _mockPermissionService.Setup(x => x.GetUserRoleAsync())
            .ReturnsAsync("TestRole");
        
        _mockPermissionService.Setup(x => x.GetUserPermissionsAsync())
            .ReturnsAsync(new List<Permission>());
        
        _mockPermissionService.Setup(x => x.HasPermissionAsync(It.IsAny<PermissionSection>(), It.IsAny<PermissionFunction>()))
            .ReturnsAsync(false);
        
        _mockPermissionStore.Setup(x => x.GetAllRolesAsync())
            .ReturnsAsync(new List<Role>());
        
        Services.AddSingleton(_mockPermissionService.Object);
        Services.AddSingleton(_mockAuthStateProvider.Object);
        Services.AddSingleton(_mockPermissionStore.Object);
    }

    [Fact]
    public void PermissionsDebug_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<PermissionsDebug>();
        
        // Assert
        cut.Markup.ShouldNotBeNullOrEmpty();
    }
    
    [Fact]
    public void PermissionsDebug_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<PermissionsDebug>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
