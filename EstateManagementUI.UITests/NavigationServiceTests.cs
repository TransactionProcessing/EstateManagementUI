using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Moq;
using Shared.Logger;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.UITests;

public class NavigationServiceTests
{
    private readonly Mock<IPermissionsService> PermissionsServiceMock;
    private readonly NavigationService NavigationService;

    public NavigationServiceTests() {
        Logger.Initialise(new NullLogger());
        this.PermissionsServiceMock = new Mock<IPermissionsService>();
        this.NavigationService = new NavigationService(this.PermissionsServiceMock.Object);
    }

    [Fact]
    public void GetCurrentPageClass_PageStartsWithPrefix_ReturnsActiveClass()
    {
        // Arrange
        var routeData = new RouteData();
        routeData.Values["page"] = "/Dashboard";
        var viewContext = new ViewContext { RouteData = routeData };

        // Act
        var result = this.NavigationService.GetCurrentPageClass(viewContext, "Dashboard");

        // Assert
        result.ShouldBe("btn btn-sm btn-neutral btn-active");
    }

    [Fact]
    public void GetCurrentPageClass_PageDoesNotStartWithPrefix_ReturnsGhostClass()
    {
        // Arrange
        var routeData = new RouteData();
        routeData.Values["page"] = "/Home";
        var viewContext = new ViewContext { RouteData = routeData };

        // Act
        var result = this.NavigationService.GetCurrentPageClass(viewContext, "Dashboard");

        // Assert
        result.ShouldBe("btn btn-sm btn-ghost");
    }

    [Fact]
    public void GetCurrentPageClass_PageIsNull_ReturnsGhostClass()
    {
        // Arrange
        var routeData = new RouteData();
        routeData.Values["page"] = null;
        var viewContext = new ViewContext { RouteData = routeData };

        // Act
        var result = this.NavigationService.GetCurrentPageClass(viewContext, "Dashboard");

        // Assert
        result.ShouldBe("btn btn-sm btn-ghost");
    }

    [Fact]
    public async Task RenderItem_PermissionGranted_ReturnsHtmlString()
    {
        // Arrange
        string userName = "testUser";
        string title = "Dashboard";
        string name = "Dashboard";
        string id = "dashboardLink";
        string pageName = "Index";
        this.PermissionsServiceMock.Setup(p => p.DoIHavePermissions(userName, name))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await this.NavigationService.RenderItem(userName, title, name, id, pageName);

        // Assert
        result.ShouldContain("nav-link");
        result.ShouldContain("fa-solid fa-gauge-high");
        result.ShouldContain("Dashboard");
    }

    [Fact]
    public async Task RenderItem_PermissionDenied_ReturnsEmptyString()
    {
        // Arrange
        string userName = "testUser";
        string title = "Dashboard";
        string name = "Dashboard";
        string id = "dashboardLink";
        string pageName = "Index";
        this.PermissionsServiceMock.Setup(p => p.DoIHavePermissions(userName, name))
            .ReturnsAsync(Result.Failure("Permission denied"));

        // Act
        var result = await this.NavigationService.RenderItem(userName, title, name, id, pageName);

        // Assert
        result.ShouldBeEmpty();
    }
    
    [Theory]
    [InlineData("Estate", "fa-solid fa-network-wired")]
    [InlineData("Merchant", "fa-solid fa-store")]
    [InlineData("Contract", "fa-solid fa-file-contract")]
    [InlineData("Operator", "fa-solid fa-building-columns")]
    [InlineData("Reporting", "fa-solid fa-chart-simple")]
    [InlineData("File Processing", "fa-solid fa-file-csv")]
    public async Task RenderItem_CorrectIconReturned(String name, String expectedIcon)
    {
        // Arrange
        string userName = "testUser";
        string title = "Dashboard";
        string id = "dashboardLink";
        string pageName = "Index";
        this.PermissionsServiceMock.Setup(p => p.DoIHavePermissions(userName, name))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await this.NavigationService.RenderItem(userName, title, name, id, pageName);

        // Assert
        result.ShouldContain(expectedIcon);
    }


}