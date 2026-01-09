using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class CounterPageTests : TestContext
{
    [Fact]
    public void Counter_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Counter>();
        
        // Assert
        cut.Find("h1").TextContent.ShouldBe("Counter");
        cut.Find("p[role='status']").TextContent.ShouldBe("Current count: 0");
        cut.Find("button").TextContent.Trim().ShouldBe("Click me");
    }
    
    [Fact]
    public void Counter_IncrementButton_IncrementsCount()
    {
        // Arrange
        var cut = RenderComponent<Counter>();
        var button = cut.Find("button");
        
        // Act
        button.Click();
        
        // Assert
        cut.Find("p[role='status']").TextContent.ShouldBe("Current count: 1");
    }
    
    [Fact]
    public void Counter_MultipleClicks_IncrementsCorrectly()
    {
        // Arrange
        var cut = RenderComponent<Counter>();
        var button = cut.Find("button");
        
        // Act
        button.Click();
        button.Click();
        button.Click();
        
        // Assert
        cut.Find("p[role='status']").TextContent.ShouldBe("Current count: 3");
    }
    
    [Fact]
    public void Counter_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<Counter>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
