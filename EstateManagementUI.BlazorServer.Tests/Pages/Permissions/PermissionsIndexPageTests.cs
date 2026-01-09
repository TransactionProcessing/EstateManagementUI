using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using PermissionsIndex = EstateManagementUI.BlazorServer.Components.Pages.Permissions.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Permissions;

public class PermissionsIndexPageTests : TestContext
{
    private readonly Mock<IPermissionStore> _mockPermissionStore;
    private readonly Mock<NavigationManager> _mockNavigationManager;
    private readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;

    public PermissionsIndexPageTests()
    {
        _mockPermissionStore = new Mock<IPermissionStore>();
        _mockNavigationManager = new Mock<NavigationManager>();
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        
        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        
        Services.AddSingleton(_mockPermissionStore.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        
        ComponentFactories.AddStub<RequirePermission>();
        ComponentFactories.AddStub<RequireSectionAccess>();
    }

    [Fact]
    public void PermissionsIndex_RendersCorrectly()
    {
        // Arrange
        _mockPermissionStore.Setup(x => x.GetAllRolesAsync())
            .ReturnsAsync(new List<Role>());
        
        // Act
        var cut = RenderComponent<PermissionsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Permission Management");
    }

    [Fact]
    public void PermissionsIndex_HasCorrectPageTitle()
    {
        // Arrange
        _mockPermissionStore.Setup(x => x.GetAllRolesAsync())
            .ReturnsAsync(new List<Role>());
        
        // Act
        var cut = RenderComponent<PermissionsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
