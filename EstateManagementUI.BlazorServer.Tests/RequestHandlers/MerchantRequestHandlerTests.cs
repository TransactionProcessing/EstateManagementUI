using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Tests.RequestHandlers;

public class MerchantRequestHandlerTests
{
    private readonly IApiClientImposter _mockApiClient;
    private readonly MerchantRequestHandler _handler;

    public MerchantRequestHandlerTests()
    {
        _mockApiClient = new IApiClientImposter();
        _handler = new MerchantRequestHandler(_mockApiClient.Instance());
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

        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(merchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), null, null, null, null, null);

        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchant(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(merchant));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.MerchantId.ShouldBe(merchantId);
        _mockApiClient.GetMerchant(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.GetMerchant(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchant(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetRecentMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(recentMerchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetRecentMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetRecentMerchantsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetRecentMerchantsQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.GetRecentMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetRecentMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchantKpi(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(kpi));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.MerchantsWithSaleInLastHour.ShouldBe(10);
        _mockApiClient.GetMerchantKpi(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantKpiQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantKpiQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.GetMerchantKpi(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchantKpi(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(merchants));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantsForDropDownQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantsForDropDownQuery(CorrelationIdHelper.New(), Guid.NewGuid());

        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchants(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchantContracts(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(contracts));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetMerchantContracts(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantContractsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantContractsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.GetMerchantContracts(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchantContracts(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchantOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(operators));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetMerchantOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantOperatorsQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantOperatorsQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.GetMerchantOperators(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchantOperators(query, Arg<CancellationToken>.Any()).Called(Count.Once());
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

        _mockApiClient.GetMerchantDevices(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(devices));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data!.Count.ShouldBe(1);
        _mockApiClient.GetMerchantDevices(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_GetMerchantDevicesQuery_ReturnsFailure_WhenApiClientFails()
    {
        var query = new MerchantQueries.GetMerchantDevicesQuery(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.GetMerchantDevices(query, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.GetMerchantDevices(query, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region AddMerchantDeviceCommand

    [Fact]
    public async Task Handle_AddMerchantDeviceCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AddMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "DEVICE001");

        _mockApiClient.AddDeviceToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.AddDeviceToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_AddMerchantDeviceCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AddMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "DEVICE001");

        _mockApiClient.AddDeviceToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.AddDeviceToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region AddOperatorToMerchantCommand

    [Fact]
    public async Task Handle_AddOperatorToMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AddOperatorToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "M001", "T001");

        _mockApiClient.AddOperatorToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.AddOperatorToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_AddOperatorToMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AddOperatorToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "M001", "T001");

        _mockApiClient.AddOperatorToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.AddOperatorToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region CreateMerchantCommand

    [Fact]
    public async Task Handle_CreateMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.CreateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.CreateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.CreateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_CreateMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.CreateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.CreateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.CreateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region MakeMerchantDepositCommand

    [Fact]
    public async Task Handle_MakeMerchantDepositCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.MakeMerchantDepositCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), 100.00m, DateTime.UtcNow, "REF001");

        _mockApiClient.MakeMerchantDeposit(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.MakeMerchantDeposit(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_MakeMerchantDepositCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.MakeMerchantDepositCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), 100.00m, DateTime.UtcNow, "REF001");

        _mockApiClient.MakeMerchantDeposit(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.MakeMerchantDeposit(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region UpdateMerchantOpeningHoursCommand

    [Fact]
    public async Task Handle_UpdateMerchantOpeningHoursCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.UpdateMerchantOpeningHoursCommand(
            CorrelationIdHelper.New(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(new MerchantCommands.OpeningHours("0800","1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700")));

        _mockApiClient.UpdateMerchantOpeningHours(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.UpdateMerchantOpeningHours(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_UpdateMerchantOpeningHoursCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.UpdateMerchantOpeningHoursCommand(
            CorrelationIdHelper.New(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(new MerchantCommands.OpeningHours("0800", "1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700"), new MerchantCommands.OpeningHours("0800", "1700"),
                new MerchantCommands.OpeningHours("0800", "1700")));

        _mockApiClient.UpdateMerchantOpeningHours(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.UpdateMerchantOpeningHours(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region RemoveContractFromMerchantCommand

    [Fact]
    public async Task Handle_RemoveContractFromMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.RemoveContractFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.RemoveContractFromMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.RemoveContractFromMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_RemoveContractFromMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.RemoveContractFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.RemoveContractFromMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.RemoveContractFromMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region RemoveOperatorFromMerchantCommand

    [Fact]
    public async Task Handle_RemoveOperatorFromMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.RemoveOperatorFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.RemoveOperatorFromMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.RemoveOperatorFromMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_RemoveOperatorFromMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.RemoveOperatorFromMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.RemoveOperatorFromMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.RemoveOperatorFromMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region SwapMerchantDeviceCommand

    [Fact]
    public async Task Handle_SwapMerchantDeviceCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.SwapMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "OLD001", "NEW001");

        _mockApiClient.SwapMerchantDevice(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.SwapMerchantDevice(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_SwapMerchantDeviceCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.SwapMerchantDeviceCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "OLD001", "NEW001");

        _mockApiClient.SwapMerchantDevice(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.SwapMerchantDevice(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region AssignContractToMerchantCommand

    [Fact]
    public async Task Handle_AssignContractToMerchantCommand_ReturnsSuccess_WhenApiClientSucceeds()
    {
        var command = new MerchantCommands.AssignContractToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.AddContractToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.AddContractToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_AssignContractToMerchantCommand_ReturnsFailure_WhenApiClientFails()
    {
        var command = new MerchantCommands.AssignContractToMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _mockApiClient.AddContractToMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("api error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.AddContractToMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion

    #region UpdateMerchantCommand

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsSuccess_WhenAllApiCallsSucceed()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("update merchant failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any()).Called(Count.Never());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantAddressFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("update address failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task Handle_UpdateMerchantCommand_ReturnsFailure_WhenUpdateMerchantContactFails()
    {
        var address = new MerchantCommands.MerchantAddress(Guid.NewGuid(), "1 High St", "Town", "Region", "AB1 2CD", "Country");
        var contact = new MerchantCommands.MerchantContact(Guid.NewGuid(), "John Doe", "john@example.com", "01234567890");
        var command = new MerchantCommands.UpdateMerchantCommand(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), "Merchant1", "Immediate", address, contact);

        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("update contact failed"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        _mockApiClient.UpdateMerchant(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantAddress(command, Arg<CancellationToken>.Any()).Called(Count.Once());
        _mockApiClient.UpdateMerchantContact(command, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    #endregion
}
