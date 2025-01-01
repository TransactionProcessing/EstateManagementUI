using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.FileProcessing.FileDetails;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class FileDetailsTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly FileDetails _fileDetails;

    public FileDetailsTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._fileDetails = new FileDetails(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_LoadsFileDetails_WhenFileIdIsNotEmpty()
    {
        // Arrange
        this._fileDetails.FileId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileDetailsResult);

        // Act
        await this._fileDetails.MountAsync();

        // Assert
        this._fileDetails.FileName.ShouldBe("path/to/file");
        this._fileDetails.MerchantName.ShouldBe("Test Merchant");
        this._fileDetails.FileProfile.ShouldBe("Test Profile");
        this._fileDetails.UploadedBy.ShouldBe("user@example.com");
        this._fileDetails.FileLines.ShouldNotBeNull();
        this._fileDetails.FileLines.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task Sort_SortsFileLinesByLineNumber()
    {
        // Arrange
        this._fileDetails.FileId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileDetailsResult);

        // Act
        await this._fileDetails.Sort(FileDetailsSorting.LineNumber);

        // Assert
        this._fileDetails.FileLines.ShouldNotBeNull();
        this._fileDetails.FileLines.Count.ShouldBe(2);
        this._fileDetails.FileLines[0].LineNumber.ShouldBe(2);
        this._fileDetails.FileLines[1].LineNumber.ShouldBe(1);

        // Act
        await this._fileDetails.Sort(FileDetailsSorting.LineNumber);

        this._fileDetails.FileLines[0].LineNumber.ShouldBe(1);
        this._fileDetails.FileLines[1].LineNumber.ShouldBe(2);
    }

    [Fact]
    public async Task Sort_SortsFileLinesByResult()
    {
        // Arrange
        this._fileDetails.FileId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileDetailsResult);

        // Act
        await this._fileDetails.Sort(FileDetailsSorting.Result);

        // Assert
        this._fileDetails.FileLines.ShouldNotBeNull();
        this._fileDetails.FileLines.Count.ShouldBe(2);
        this._fileDetails.FileLines[0].ProcessingResult.ShouldBe(ViewModels.FileLineProcessingResult.Successful);
        this._fileDetails.FileLines[1].ProcessingResult.ShouldBe(ViewModels.FileLineProcessingResult.Failed);

        // Act
        await this._fileDetails.Sort(FileDetailsSorting.Result);

        this._fileDetails.FileLines[0].ProcessingResult.ShouldBe(ViewModels.FileLineProcessingResult.Failed);
        this._fileDetails.FileLines[1].ProcessingResult.ShouldBe(ViewModels.FileLineProcessingResult.Successful);
    }
}