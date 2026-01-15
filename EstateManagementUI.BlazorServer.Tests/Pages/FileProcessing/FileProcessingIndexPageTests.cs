using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using FileImportLogModel = EstateManagementUI.BusinessLogic.Models.FileImportLogModel;
using FileProcessingIndex = EstateManagementUI.BlazorServer.Components.Pages.FileProcessing.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;

public class FileProcessingIndexPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<NavigationManager> _mockNavigationManager;
    private readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;

    public FileProcessingIndexPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockNavigationManager = new Mock<NavigationManager>();
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        
        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        
        ComponentFactories.AddStub<RequirePermission>();
    }

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
