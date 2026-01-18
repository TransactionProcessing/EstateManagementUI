using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using ReportingIndex = EstateManagementUI.BlazorServer.Components.Pages.Reporting.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Reporting;

public class ReportingIndexPageTests : BaseTest
{
    [Fact]
    public void ReportingIndex_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<ReportingIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Reporting");
    }

    [Fact]
    public void ReportingIndex_HasCorrectPageTitle()
    {
        // Act
        var cut = RenderComponent<ReportingIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
