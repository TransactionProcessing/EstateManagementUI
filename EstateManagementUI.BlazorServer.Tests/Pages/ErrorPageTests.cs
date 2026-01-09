using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class ErrorPageTests : TestContext
{
    [Fact]
    public void Error_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Find("h1").TextContent.ShouldBe("Error.");
        cut.Find("h2").TextContent.ShouldBe("An error occurred while processing your request.");
    }
    
    [Fact]
    public void Error_WithoutRequestId_DoesNotShowRequestId()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.FindAll("strong").ShouldNotContain(e => e.TextContent.Contains("Request ID:"));
    }
    
    [Fact]
    public void Error_ShowsDevelopmentModeInformation()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Markup.ShouldContain("Development Mode");
        cut.Markup.ShouldContain("ASPNETCORE_ENVIRONMENT");
    }
    
    [Fact]
    public void Error_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
