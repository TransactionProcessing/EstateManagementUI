using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TestContext = Bunit.TestContext;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public class EntryScreenPageTests : TestContext
{
    public EntryScreenPageTests()
    {
        Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppSettings:TestMode"] = "Disabled"
            })
            .Build());
    }

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

    [Fact]
    public void EntryScreen_live_mode_sign_in_link_goes_directly_to_authentication()
    {
        var cut = RenderComponent<EntryScreen>();

        cut.Find("#loginButton").GetAttribute("href").ShouldBe("/authentication/login");
    }

    [Fact]
    public void EntryScreen_test_mode_sign_in_link_goes_to_login_page()
    {
        Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppSettings:TestMode"] = "BackedByTestDataStore"
            })
            .Build());

        var cut = RenderComponent<EntryScreen>();

        cut.Find("#loginButton").GetAttribute("href").ShouldBe("/login");
    }
}
