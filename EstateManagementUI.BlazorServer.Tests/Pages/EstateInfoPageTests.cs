using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class EstateInfoPageTests : TestContext
{
    [Fact]
    public void EstateInfo_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<EstateInfo>();
        
        // Assert
        cut.Markup.ShouldContain("Estate Management");
        cut.Markup.ShouldContain("Comprehensive estate management and configuration");
    }
    
    [Fact]
    public void EstateInfo_HasFeatureSections()
    {
        // Act
        var cut = RenderComponent<EstateInfo>();
        
        // Assert
        cut.Markup.ShouldContain("Manage Estate Details");
        cut.Markup.ShouldContain("Manage Estate Users");
        cut.Markup.ShouldContain("Operator Management");
    }
    
    [Fact]
    public void EstateInfo_HasSignInButton()
    {
        // Act
        var cut = RenderComponent<EstateInfo>();
        
        // Assert
        var loginButton = cut.Find("a#loginButton");
        loginButton.GetAttribute("href").ShouldBe("/login");
        loginButton.TextContent.ShouldContain("Sign In");
    }
    
    [Fact]
    public void EstateInfo_HasBackButton()
    {
        // Act
        var cut = RenderComponent<EstateInfo>();
        
        // Assert
        cut.FindAll("a[href='/entry']").Count.ShouldBeGreaterThan(0);
    }
    
    [Fact]
    public void EstateInfo_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<EstateInfo>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
