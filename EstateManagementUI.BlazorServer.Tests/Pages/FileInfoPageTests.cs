using Bunit;
using Shouldly;
using FileInfoPage = EstateManagementUI.BlazorServer.Components.Pages.FileInfo;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class FileInfoPageTests : TestContext
{
    [Fact]
    public void FileInfo_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<FileInfoPage>();
        
        // Assert
        cut.Markup.ShouldContain("File Processing");
        cut.Markup.ShouldContain("Automated file processing and settlement management");
    }
    
    [Fact]
    public void FileInfo_HasFeatureSections()
    {
        // Act
        var cut = RenderComponent<FileInfoPage>();
        
        // Assert
        cut.Markup.ShouldContain("Transaction Files");
        cut.Markup.ShouldContain("Settlement Processing");
        cut.Markup.ShouldContain("Bulk Processing");
    }
    
    [Fact]
    public void FileInfo_HasSignInButton()
    {
        // Act
        var cut = RenderComponent<FileInfoPage>();
        
        // Assert
        var loginButton = cut.Find("a#loginButton");
        loginButton.GetAttribute("href").ShouldBe("/login");
        loginButton.TextContent.ShouldContain("Sign In");
    }
    
    [Fact]
    public void FileInfo_HasBackButton()
    {
        // Act
        var cut = RenderComponent<FileInfoPage>();
        
        // Assert
        cut.FindAll("a[href='/entry']").Count.ShouldBeGreaterThan(0);
    }
    
    [Fact]
    public void FileInfo_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<FileInfoPage>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
