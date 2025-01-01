using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.FileProcessing.FileImportLogs;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using FileImportLogList = EstateManagementUI.Pages.FileProcessing.FileImportLogs.FileImportLogList;

namespace EstateManagementUI.UITests;

public class FileImportLogListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly FileImportLogList _fileImportLogList;

    public FileImportLogListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._fileImportLogList = new FileImportLogList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_PopulatesMerchantsAndDates()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._fileImportLogList.MountAsync();

        // Assert
        this._fileImportLogList.Merchant.ShouldNotBeNull();
        this._fileImportLogList.Merchant.Merchants.ShouldNotBeEmpty();
        this._fileImportLogList.StartDate.ShouldNotBeNull();
        this._fileImportLogList.EndDate.ShouldNotBeNull();
    }

    [Fact]
    public async Task Query_PopulatesFileImportLogs_WhenMerchantIdIsNotEmpty()
    {
        // Arrange
        this._fileImportLogList.Merchant = new MerchantListModel { MerchantId = Guid.NewGuid().ToString() };
        this._fileImportLogList.StartDate = new DateModel { SelectedDate = DateTime.Now };
        this._fileImportLogList.EndDate = new DateModel { SelectedDate = DateTime.Now };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogsResult);

        // Act
        await this._fileImportLogList.Query();

        // Assert
        this._fileImportLogList.FileImportLogs.ShouldNotBeNull();
        this._fileImportLogList.FileImportLogs.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task Sort_SortsFileImportLogsByImportLogDate()
    {
        // Arrange
        this._fileImportLogList.Merchant = new MerchantListModel { MerchantId = Guid.NewGuid().ToString() };
        this._fileImportLogList.StartDate = new DateModel { SelectedDate = DateTime.Now };
        this._fileImportLogList.EndDate = new DateModel { SelectedDate = DateTime.Now };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogsResult);

        // Act
        await this._fileImportLogList.Sort(FileImportLogListSorting.ImportLogDate);

        // Assert
        this._fileImportLogList.FileImportLogs.ShouldNotBeNull();
        this._fileImportLogList.FileImportLogs.Count.ShouldBe(2);
        this._fileImportLogList.FileImportLogs[0].ImportLogDate.ShouldBe(TestData.FileImportLogs[0].ImportLogDate);
        this._fileImportLogList.FileImportLogs[1].ImportLogDate.ShouldBe(TestData.FileImportLogs[1].ImportLogDate);

        // Act
        await this._fileImportLogList.Sort(FileImportLogListSorting.ImportLogDate);

        this._fileImportLogList.FileImportLogs[0].ImportLogDate.ShouldBe(TestData.FileImportLogs[1].ImportLogDate);
        this._fileImportLogList.FileImportLogs[1].ImportLogDate.ShouldBe(TestData.FileImportLogs[0].ImportLogDate);
    }

    [Fact]
    public async Task Sort_SortsFileImportLogsByNumberOfFiles()
    {
        // Arrange
        this._fileImportLogList.Merchant = new MerchantListModel { MerchantId = Guid.NewGuid().ToString() };
        this._fileImportLogList.StartDate = new DateModel { SelectedDate = DateTime.Now };
        this._fileImportLogList.EndDate = new DateModel { SelectedDate = DateTime.Now };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetFileImportLogsListQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.FileImportLogsResult);

        // Act
        await this._fileImportLogList.Sort(FileImportLogListSorting.NumberOfFiles);

        // Assert
        this._fileImportLogList.FileImportLogs.ShouldNotBeNull();
        this._fileImportLogList.FileImportLogs.Count.ShouldBe(2);
        this._fileImportLogList.FileImportLogs[0].FileCount.ShouldBe(3);
        this._fileImportLogList.FileImportLogs[1].FileCount.ShouldBe(5);

        // Act
        await this._fileImportLogList.Sort(FileImportLogListSorting.NumberOfFiles);

        this._fileImportLogList.FileImportLogs[0].FileCount.ShouldBe(5);
        this._fileImportLogList.FileImportLogs[1].FileCount.ShouldBe(3);
    }

    [Fact]
    public async Task ViewFiles_NavigatesToFileImportLogPage()
    {
        // Arrange
        var fileImportLogId = Guid.NewGuid();
        this._fileImportLogList.Merchant = new MerchantListModel { MerchantId = Guid.NewGuid().ToString() };

        // Act
        await this._fileImportLogList.ViewFiles(fileImportLogId);

        // Assert
        this._fileImportLogList.LocationUrl.ShouldNotBeNull();
        this._fileImportLogList.LocationUrl.ShouldBe("/FileProcessing/FileImportLog");
        Guid payloadFileImportLogId = TestHelpers.GetPropertyValue<Guid>(this._fileImportLogList.Payload, "FileImportLogId");
        payloadFileImportLogId.ShouldBe(fileImportLogId);
    }
}