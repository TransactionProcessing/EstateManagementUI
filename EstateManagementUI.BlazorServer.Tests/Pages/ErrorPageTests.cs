using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class ErrorPageTests : BaseTest
{
    public ErrorPageTests() :base() {
        Mock<IWebHostEnvironment> _mockWebHostEnvironment = new();
        Mock<IConfiguration> _mockConfiguration = new();
        _mockWebHostEnvironment.Setup(x => x.EnvironmentName).Returns(Environments.Development);
        
        Services.AddSingleton(_mockWebHostEnvironment.Object);

        // Use a real IConfiguration with an in-memory value so GetSection/GetValue work as expected
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "AppSettings:SupportEmail", "support@example.com" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        Services.AddSingleton<IConfiguration>(configuration);

    }

    [Fact]
    public void Error_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Find("h1").TextContent.ShouldContain("Oops! Something went wrong");
        cut.Markup.ShouldContain("We encountered an unexpected error");
    }
    
    [Fact]
    public void Error_WithoutRequestId_DoesNotShowRequestId()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Markup.ShouldNotContain("Reference ID for support:");
    }
    
    [Fact]
    public void Error_ShowsDevelopmentModeInformation()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Markup.ShouldContain("Development Mode Information");
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
    
    [Fact]
    public void Error_HasHomePageLink()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        var homeLink = cut.Find("a[href='/']");
        homeLink.TextContent.ShouldContain("Go to Home");
    }
    
    [Fact]
    public void Error_HasBackButton()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        var backButton = cut.Find("button[onclick='window.history.back()']");
        backButton.TextContent.ShouldContain("Go Back");
    }
    
    [Fact]
    public void Error_HasSupportEmail()
    {
        // Act
        var cut = RenderComponent<Error>();
        
        // Assert
        cut.Markup.ShouldContain("support@example.com");
    }
}
