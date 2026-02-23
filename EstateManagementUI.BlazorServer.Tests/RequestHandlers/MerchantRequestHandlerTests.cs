using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class MerchantRequestHandlerTests
{
    private readonly Mock<IApiClient> _mockApiClient;
    private readonly MerchantRequestHandler _handler;

    public MerchantRequestHandlerTests()
    {
        _mockApiClient = new Mock<IApiClient>();
        _handler = new MerchantRequestHandler(_mockApiClient.Object);
    }

    #region GetMerchantsQuery

    [Fact]
    public async Task Handle_GetMerchantsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantsQuery(CorrelationIdHelper.New(), estateId, null, null, null, null, null);
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new() { MerchantId = Guid.NewGuid(), MerchantName = "Merchant1" }
        };

        _mockApiClient.Setup(c => c.GetMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(merchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), null, null, null, null, null);

        _mockApiClient.Setup(c => c.GetMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.MerchantListModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantQuery

    [Fact]
    public async Task Handle_GetMerchantQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantQuery(CorrelationIdHelper.New(), estateId, merchantId);
        var merchant = new MerchantModels.MerchantModel { MerchantId = merchantId, MerchantName = "Merchant1" };

        _mockApiClient.Setup(c => c.GetMerchant(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(merchant));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.MerchantId.ShouldBe(merchantId);
        _mockApiClient.Verify(c => c.GetMerchant(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchant(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<MerchantModels.MerchantModel>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchant(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetRecentMerchantsQuery

    [Fact]
    public async Task Handle_GetRecentMerchantsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var query = new MerchantQueries.GetRecentMerchantsQuery(CorrelationIdHelper.New(), estateId);
        var recentMerchants = new List<MerchantModels.RecentMerchantsModel>
        {
            new() { MerchantId = Guid.NewGuid(), Name = "Merchant1", Reference = "REF1" }
        };

        _mockApiClient.Setup(c => c.GetRecentMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(recentMerchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetRecentMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetRecentMerchantsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetRecentMerchantsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetRecentMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.RecentMerchantsModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetRecentMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantKpiQuery

    [Fact]
    public async Task Handle_GetMerchantKpiQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantKpiQuery(CorrelationIdHelper.New(), estateId);
        var kpi = new MerchantModels.MerchantKpiModel
        {
            MerchantsWithNoSaleInLast7Days = 5,
            MerchantsWithNoSaleToday = 3,
            MerchantsWithSaleInLastHour = 10
        };

        _mockApiClient.Setup(c => c.GetMerchantKpi(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(kpi));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.MerchantsWithSaleInLastHour.ShouldBe(10);
        _mockApiClient.Verify(c => c.GetMerchantKpi(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantKpiQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantKpiQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchantKpi(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<MerchantModels.MerchantKpiModel>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchantKpi(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantsForDropDownQuery

    [Fact]
    public async Task Handle_GetMerchantsForDropDownQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantsForDropDownQuery(CorrelationIdHelper.New(), estateId);
        var merchants = new List<MerchantModels.MerchantDropDownModel>
        {
            new() { MerchantId = Guid.NewGuid(), MerchantName = "Merchant1" }
        };

        _mockApiClient.Setup(c => c.GetMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(merchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantsForDropDownQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantsForDropDownQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchants(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.MerchantDropDownModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchants(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantContractsQuery

    [Fact]
    public async Task Handle_GetMerchantContractsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantContractsQuery(CorrelationIdHelper.New(), estateId, merchantId);
        var contracts = new List<MerchantModels.MerchantContractModel>
        {
            new() { MerchantId = merchantId, ContractId = Guid.NewGuid(), ContractName = "Contract1" }
        };

        _mockApiClient.Setup(c => c.GetMerchantContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(contracts));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetMerchantContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantContractsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantContractsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchantContracts(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.MerchantContractModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchantContracts(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantOperatorsQuery

    [Fact]
    public async Task Handle_GetMerchantOperatorsQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantOperatorsQuery(CorrelationIdHelper.New(), estateId, merchantId);
        var operators = new List<MerchantModels.MerchantOperatorModel>
        {
            new() { MerchantId = merchantId, OperatorId = Guid.NewGuid(), OperatorName = "Operator1" }
        };

        _mockApiClient.Setup(c => c.GetMerchantOperators(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(operators));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetMerchantOperators(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantOperatorsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantOperatorsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchantOperators(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.MerchantOperatorModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchantOperators(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetMerchantDevicesQuery

    [Fact]
    public async Task Handle_GetMerchantDevicesQuery_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var query = new MerchantQueries.GetMerchantDevicesQuery(CorrelationIdHelper.New(), estateId, merchantId);
        var devices = new List<MerchantModels.MerchantDeviceModel>
        {
            new() { MerchantId = merchantId, DeviceId = Guid.NewGuid(), DeviceIdentifier = "DEVICE001" }
        };

        _mockApiClient.Setup(c => c.GetMerchantDevices(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(devices));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.Verify(c => c.GetMerchantDevices(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetMerchantDevicesQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantDevicesQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.GetMerchantDevices(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<MerchantModels.MerchantDeviceModel>>("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.GetMerchantDevices(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region AddMerchantDeviceCommand

    [Fact]
    public async Task Handle_AddMerchantDeviceCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AddMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "DEVICE001");

        _mockApiClient.Setup(c => c.AddDeviceToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddDeviceToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddMerchantDeviceCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AddMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "DEVICE001");

        _mockApiClient.Setup(c => c.AddDeviceToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddDeviceToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region AddOperatorToMerchantCommand

    [Fact]
    public async Task Handle_AddOperatorToMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AddOperatorToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "M001", "T001");

        _mockApiClient.Setup(c => c.AddOperatorToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddOperatorToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AddOperatorToMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AddOperatorToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "M001", "T001");

        _mockApiClient.Setup(c => c.AddOperatorToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddOperatorToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region CreateMerchantCommand

    [Fact]
    public async Task Handle_CreateMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.CreateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.CreateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.CreateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.CreateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.CreateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.CreateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region MakeMerchantDepositCommand

    [Fact]
    public async Task Handle_MakeMerchantDepositCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.MakeMerchantDepositCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), 100.00m, DateTime.UtcNow, "REF001");

        _mockApiClient.Setup(c => c.MakeMerchantDeposit(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.MakeMerchantDeposit(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_MakeMerchantDepositCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.MakeMerchantDepositCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), 100.00m, DateTime.UtcNow, "REF001");

        _mockApiClient.Setup(c => c.MakeMerchantDeposit(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.MakeMerchantDeposit(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region RemoveContractFromMerchantCommand

    [Fact]
    public async Task Handle_RemoveContractFromMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.RemoveContractFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.RemoveContractFromMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.RemoveContractFromMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RemoveContractFromMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.RemoveContractFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.RemoveContractFromMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.RemoveContractFromMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region RemoveOperatorFromMerchantCommand

    [Fact]
    public async Task Handle_RemoveOperatorFromMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.RemoveOperatorFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.RemoveOperatorFromMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.RemoveOperatorFromMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RemoveOperatorFromMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.RemoveOperatorFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.RemoveOperatorFromMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.RemoveOperatorFromMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region SwapMerchantDeviceCommand

    [Fact]
    public async Task Handle_SwapMerchantDeviceCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.SwapMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "OLD001", "NEW001");

        _mockApiClient.Setup(c => c.SwapMerchantDevice(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.SwapMerchantDevice(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_SwapMerchantDeviceCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.SwapMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "OLD001", "NEW001");

        _mockApiClient.Setup(c => c.SwapMerchantDevice(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.SwapMerchantDevice(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region AssignContractToMerchantCommand

    [Fact]
    public async Task Handle_AssignContractToMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AssignContractToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.AddContractToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddContractToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AssignContractToMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AssignContractToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.Setup(c => c.AddContractToMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.AddContractToMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateMerchantCommand

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsSuccess_WhenAllApiCallsSucceed()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _mockApiClient.Setup(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _mockApiClient.Setup(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.Verify(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("update merchant failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()), Times.Never);
        _mockApiClient.Verify(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantAddressFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _mockApiClient.Setup(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("update address failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantContactFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.Setup(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _mockApiClient.Setup(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        _mockApiClient.Setup(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("update contact failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.Verify(c => c.UpdateMerchant(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantAddress(command, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(c => c.UpdateMerchantContact(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
