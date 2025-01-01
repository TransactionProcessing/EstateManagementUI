using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.FileProcessing.FileImportLog;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class FileImportLogTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly FileImportLog _fileImportLog;

    public FileImportLogTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._fileImportLog = new FileImportLog(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Theory]
    [InlineData(FileImportLogSorting.FileName, true)]
    [InlineData(FileImportLogSorting.FileName, false)]
    [InlineData(FileImportLogSorting.DateTimeUploaded, true)]
    [InlineData(FileImportLogSorting.DateTimeUploaded, false)]
    [InlineData(FileImportLogSorting.FileProfile, true)]
    [InlineData(FileImportLogSorting.FileProfile, false)]
    [InlineData(FileImportLogSorting.OriginalFileName, true)]
    [InlineData(FileImportLogSorting.OriginalFileName, false)]
    [InlineData(FileImportLogSorting.UserName, true)]
    [InlineData(FileImportLogSorting.UserName, false)]
    public async Task MountAsync_LoadsFileImportLog_WhenFileImportLogIdIsNotEmpty(FileImportLogSorting sortField, Boolean ascending)
    {
        // Arrange
        this._fileImportLog.FileImportLogId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogResult);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);
        this._fileImportLog.Sorting = (Column: sortField, Ascending: ascending);
        // Act
        await this._fileImportLog.MountAsync();

        // Assert
        this._fileImportLog.ImportLogDate.ShouldBe(TestData.FileImportLog.ImportLogDate);
        this._fileImportLog.FileImportLogId.ShouldBe(TestData.FileImportLog.FileImportLogId);
        this._fileImportLog.Files.Count.ShouldBe(5);
    }

    [Fact]
    public async Task ViewFileDetails_UrlSet()
    {
        Guid fileId = Guid.Parse("D726ED3C-93DE-4FC3-81F9-30CF330079BB");
        // Act
        await this._fileImportLog.ViewFileDetails(fileId);

        // Assert
        this._fileImportLog.LocationUrl.ShouldBe("/FileProcessing/FileDetails");
        Guid payloadFileId = TestHelpers.GetPropertyValue<Guid>(this._fileImportLog.Payload, "FileId");
        payloadFileId.ShouldBe(fileId);

    }

    [Fact]
    public async Task Sort_ByDateTimeUploaded()
    {
        this._fileImportLog.FileImportLogId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogResult);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.DateTimeUploaded);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-25"));
        this._fileImportLog.Files[1].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-26"));
        this._fileImportLog.Files[2].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-27"));
        this._fileImportLog.Files[3].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-28"));
        this._fileImportLog.Files[4].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-29"));

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.DateTimeUploaded);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-29"));
        this._fileImportLog.Files[1].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-28"));
        this._fileImportLog.Files[2].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-27"));
        this._fileImportLog.Files[3].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-26"));
        this._fileImportLog.Files[4].UploadDateTime.ShouldBe(DateTime.Parse("2024-12-25"));
    }

    [Fact]
    public async Task Sort_ByOriginalFileName()
    {
        this._fileImportLog.FileImportLogId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogResult);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.OriginalFileName);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].OriginalFileName.ShouldBe("File1.txt");
        this._fileImportLog.Files[1].OriginalFileName.ShouldBe("File2.txt");
        this._fileImportLog.Files[2].OriginalFileName.ShouldBe("File3.txt");
        this._fileImportLog.Files[3].OriginalFileName.ShouldBe("File4.txt");
        this._fileImportLog.Files[4].OriginalFileName.ShouldBe("File5.txt");

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.OriginalFileName);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].OriginalFileName.ShouldBe("File5.txt");
        this._fileImportLog.Files[1].OriginalFileName.ShouldBe("File4.txt");
        this._fileImportLog.Files[2].OriginalFileName.ShouldBe("File3.txt");
        this._fileImportLog.Files[3].OriginalFileName.ShouldBe("File2.txt");
        this._fileImportLog.Files[4].OriginalFileName.ShouldBe("File1.txt");
    }

    [Fact]
    public async Task Sort_ByFileProfile()
    {
        this._fileImportLog.FileImportLogId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogResult);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.FileProfile);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[1].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[2].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[3].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[4].FileProfileName.ShouldBe("Safaricom Topup");

        // Act
        await this._fileImportLog.Sort(FileImportLogSorting.FileProfile);

        // Assert
        this._fileImportLog.Files.Count.ShouldBe(5);
        this._fileImportLog.Files[0].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[1].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[2].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[3].FileProfileName.ShouldBe("Safaricom Topup");
        this._fileImportLog.Files[4].FileProfileName.ShouldBe("Safaricom Topup");
    }
}