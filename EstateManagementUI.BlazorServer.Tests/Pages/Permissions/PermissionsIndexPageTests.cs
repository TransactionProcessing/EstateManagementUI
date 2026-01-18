using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using PermissionsIndex = EstateManagementUI.BlazorServer.Components.Pages.Permissions.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Permissions;

public class PermissionsIndexPageTests : BaseTest
{
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
