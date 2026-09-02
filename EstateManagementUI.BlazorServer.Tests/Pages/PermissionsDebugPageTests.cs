using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Imposter.Abstractions;
using Shouldly;
using System.Security.Claims;
using TestContext = Bunit.TestContext;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class PermissionsDebugPageTests : TestContext
{
    private readonly IPermissionServiceImposter _mockPermissionService;
    private readonly AuthenticationStateProviderImposter _mockAuthStateProvider;
    private readonly IPermissionStoreImposter _mockPermissionStore;

    public PermissionsDebugPageTests()
    {
        _mockPermissionService = new IPermissionServiceImposter();
        _mockAuthStateProvider = new AuthenticationStateProviderImposter();
        _mockPermissionStore = new IPermissionStoreImposter();

        // Setup default mock behaviors
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        }, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(principal);

        _mockAuthStateProvider.GetAuthenticationStateAsync()
            .ReturnsAsync(authState);

        _mockPermissionService.GetUserRoleAsync()
            .ReturnsAsync("TestRole");

        _mockPermissionService.GetUserPermissionsAsync()
            .ReturnsAsync(new List<Permission>());

        _mockPermissionService.HasPermissionAsync(Arg<PermissionSection>.Any(), Arg<PermissionFunction>.Any())
            .ReturnsAsync(false);

        _mockPermissionStore.GetAllRolesAsync()
            .ReturnsAsync(new List<Role>());

        Services.AddSingleton(_mockPermissionService.Instance());
        Services.AddSingleton(_mockAuthStateProvider.Instance());
        Services.AddSingleton(_mockPermissionStore.Instance());
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
