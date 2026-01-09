using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class PermissionsDebugPageTests : TestContext
{
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
