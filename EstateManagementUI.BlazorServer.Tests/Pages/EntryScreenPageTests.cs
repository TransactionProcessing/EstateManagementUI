using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class EntryScreenPageTests : TestContext
{
    [Fact]
    public void EntryScreen_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        cut.Markup.ShouldContain("Estate Management");
        cut.Markup.ShouldContain("Merchant Management");
        cut.Markup.ShouldContain("File Processing");
    }
    
    [Fact]
    public void EntryScreen_HasEstateManagementCard()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        cut.Markup.ShouldContain("Manage estate details");
        cut.Markup.ShouldContain("Manage estate users");
        cut.Markup.ShouldContain("Operator Management");
    }
    
    [Fact]
    public void EntryScreen_HasMerchantManagementCard()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        cut.Markup.ShouldContain("Manage Merchant Details");
        cut.Markup.ShouldContain("Balance Management");
    }
    
    [Fact]
    public void EntryScreen_HasFileProcessingCard()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        cut.Markup.ShouldContain("Transaction Files");
        cut.Markup.ShouldContain("Settlement Processing");
    }
    
    [Fact]
    public void EntryScreen_HasViewMoreLinks()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        cut.FindAll("a[href='/estate-info']").Count.ShouldBeGreaterThan(0);
        cut.FindAll("a[href='/merchant-info']").Count.ShouldBeGreaterThan(0);
        cut.FindAll("a[href='/file-info']").Count.ShouldBeGreaterThan(0);
    }
    
    [Fact]
    public void EntryScreen_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<EntryScreen>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
