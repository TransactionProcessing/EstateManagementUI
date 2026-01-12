using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class MerchantInfoPageTests : TestContext
{
    [Fact]
    public void MerchantInfo_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<MerchantInfo>();
        
        // Assert
        cut.Markup.ShouldContain("Merchant Management");
    }
    
    [Fact]
    public void MerchantInfo_HasFeatureSections()
    {
        // Act
        var cut = RenderComponent<MerchantInfo>();
        
        // Assert
        cut.Markup.ShouldContain("Manage Merchant Details");
        cut.Markup.ShouldContain("Balance Management");
    }
    
    [Fact]
    public void MerchantInfo_HasSignInButton()
    {
        // Act
        var cut = RenderComponent<MerchantInfo>();
        
        // Assert
        var loginButtons = cut.FindAll("a#loginButton");
        loginButtons.Count.ShouldBeGreaterThan(0);
        loginButtons[0].GetAttribute("href").ShouldBe("/login");
    }
    
    [Fact]
    public void MerchantInfo_HasBackButton()
    {
        // Act
        var cut = RenderComponent<MerchantInfo>();
        
        // Assert
        cut.FindAll("a[href='/entry']").Count.ShouldBeGreaterThan(0);
    }
    
    [Fact]
    public void MerchantInfo_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<MerchantInfo>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
