using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using ReportingIndex = EstateManagementUI.BlazorServer.Components.Pages.Reporting.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Reporting;

public class ReportingIndexPageTests : TestContext
{
    private readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;

    public ReportingIndexPageTests()
    {
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        
        ComponentFactories.AddStub<RequireSectionAccess>();
    }

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
