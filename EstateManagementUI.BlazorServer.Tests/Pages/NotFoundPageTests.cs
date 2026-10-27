using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

using TestContext = Bunit.BunitContext;

public class NotFoundPageTests : TestContext
{
    [Fact]
    public void NotFound_RendersCorrectly()
    {
        // Act
        var cut = Render<NotFound>();

        // Assert
        cut.Find("h3").TextContent.ShouldBe("Not Found");
        cut.Find("p").TextContent.ShouldBe("Sorry, the content you are looking for does not exist.");
    }

    [Fact]
    public void NotFound_HasCorrectLayout()
    {
        // Act
        var cut = Render<NotFound>();

        // Assert
        cut.Markup.ShouldNotBeNullOrEmpty();
    }
}
