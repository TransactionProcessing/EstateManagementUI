using Bunit;
using Bunit.TestDoubles;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;
using FileImportLogModel = EstateManagementUI.BusinessLogic.Models.FileImportLogModel;
using FileProcessingIndex = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public abstract class BaseTest :TestContext {
    protected BaseTest() {
        _mockMediator = new Mock<IMediator>();
        _mockNavigationManager = new Mock<NavigationManager>();
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        _mockPermissionService = new Mock<IPermissionService>();
        _mockPermissionStore = new Mock<IPermissionStore>();

        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        _mockPermissionService.Setup(x => x.HasPermissionAsync(It.IsAny<PermissionSection>(), It.IsAny<PermissionFunction>())).ReturnsAsync(true);

        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        Services.AddSingleton(_mockPermissionService.Object);
        Services.AddSingleton(_mockAuthStateProvider.Object);
        Services.AddSingleton(_mockPermissionStore.Object);

        // Add required permission components
        ComponentFactories.AddStub<RequirePermission>();
        ComponentFactories.AddStub<RequireSectionAccess>();

        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"), new Claim("estateId", Guid.NewGuid().ToString()), new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser") };
        this.AddTestAuthorization().SetClaims(claims);
    }

    protected readonly Mock<IMediator> _mockMediator;
    protected readonly Mock<NavigationManager> _mockNavigationManager;
    protected readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;
    protected readonly Mock<IPermissionService> _mockPermissionService;
    protected readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    protected readonly Mock<IPermissionStore> _mockPermissionStore;
}

public class FileProcessingIndexPageTests : BaseTest
{
    [Fact]
    public void FileProcessingIndex_RendersCorrectly()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), default))
            .ReturnsAsync(Result.Success(new List<FileImportLogModel>()));
        
        // Act
        var cut = RenderComponent<FileProcessingIndex>();
        
        // Assert
        cut.Markup.ShouldContain("File Processing");
    }

    [Fact]
    public void FileProcessingIndex_WithNoFiles_ShowsEmptyState()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), default))
            .ReturnsAsync(Result.Success(new List<FileImportLogModel>()));
        
        // Act
        var cut = RenderComponent<FileProcessingIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No file import logs found");
    }

    [Fact]
    public void FileProcessingIndex_HasCorrectPageTitle()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), default))
            .ReturnsAsync(Result.Success(new List<FileImportLogModel>()));
        
        // Act
        var cut = RenderComponent<FileProcessingIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
