using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using EstateManagementUI.BusinessLogic.Services;
using MediatR;
using SimpleResults;
using FileImportLogDetailsModel = EstateManagementUI.BusinessLogic.Models.FileProcessingModels.FileImportLogDetailsModel;

namespace EstateManagementUI.BlazorServer.Testing;

public sealed class TestMediator : IMediator
{
    private const string ReportingMerchantA = "Reporting Merchant A";
    private const string ReportingMerchantB = "Reporting Merchant B";
    private const string Safaricom = "Safaricom";
    private const string ReportingProductA = "Reporting Product A";
    private const string ReportingProductB = "Reporting Product B";
    private const string Successful = "Successful";
    private const string Voucher = "Voucher";
    private readonly ITestDataStore _dataStore;
    private readonly TestSupportState _supportState;
    private readonly object _gate = new();
    private readonly Dictionary<Guid, HashSet<Guid>> _assignedOperatorsByEstate = new();

    public TestMediator(ITestDataStore dataStore, TestSupportState supportState)
    {
        _dataStore = dataStore;
        _supportState = supportState;
        Reset();
    }

    public void Reset()
    {
        lock (_gate)
        {
            _dataStore.Reset();
            _assignedOperatorsByEstate.Clear();

            var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            _assignedOperatorsByEstate[estateId] = _dataStore.GetOperators(estateId)
                .Where(op => string.Equals(op.Name, "Test Operator", StringComparison.OrdinalIgnoreCase))
                .Select(op => op.OperatorId)
                .ToHashSet();
        }
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;
    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => AsyncEnumerable.Empty<TResponse>();
    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) => AsyncEnumerable.Empty<object?>();
    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest => Task.CompletedTask;
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => Task.FromResult((TResponse)Dispatch(request)!);
    public Task<object?> Send(object request, CancellationToken cancellationToken = default) => Task.FromResult(Dispatch(request));

    private object? Dispatch(object request)
    {
        return request switch
        {
            EstateQueries.GetEstateQuery query => HandleGetEstate(query),
            EstateQueries.GetAssignedOperatorsQuery query => HandleGetAssignedOperators(query),
            EstateCommands.AddOperatorToEstateCommand command => HandleAddOperatorToEstate(command),
            EstateCommands.RemoveOperatorFromEstateCommand command => HandleRemoveOperatorFromEstate(command),

            MerchantQueries.GetMerchantsQuery query => HandleGetMerchants(query),
            MerchantQueries.GetMerchantsForDropDownQuery query => HandleGetMerchantsForDropDown(query),
            MerchantQueries.GetRecentMerchantsQuery query => HandleGetRecentMerchants(query),
            MerchantQueries.GetMerchantKpiQuery query => HandleGetMerchantKpi(query),
            MerchantQueries.GetMerchantQuery query => HandleGetMerchant(query),
            MerchantQueries.GetMerchantScheduleQuery query => HandleGetMerchantSchedule(query),
            MerchantQueries.GetMerchantOperatorsQuery query => HandleGetMerchantOperators(query),
            MerchantQueries.GetMerchantContractsQuery query => HandleGetMerchantContracts(query),
            MerchantQueries.GetMerchantDevicesQuery query => HandleGetMerchantDevices(query),
            MerchantQueries.GetMerchantOpeningHoursQuery query => HandleGetMerchantOpeningHours(query),
            MerchantCommands.CreateMerchantCommand command => HandleCreateMerchant(command),
            MerchantCommands.UpdateMerchantCommand command => HandleUpdateMerchant(command),
            MerchantCommands.UpdateMerchantOpeningHoursCommand command => HandleUpdateMerchantOpeningHours(command),
            MerchantCommands.AddOperatorToMerchantCommand command => HandleAddOperatorToMerchant(command),
            MerchantCommands.RemoveOperatorFromMerchantCommand command => HandleRemoveOperatorFromMerchant(command),
            MerchantCommands.AssignContractToMerchantCommand command => HandleAssignContractToMerchant(command),
            MerchantCommands.RemoveContractFromMerchantCommand command => HandleRemoveContractFromMerchant(command),
            MerchantCommands.AddMerchantDeviceCommand command => HandleAddMerchantDevice(command),
            MerchantCommands.SwapMerchantDeviceCommand command => HandleSwapMerchantDevice(command),
            MerchantCommands.MakeMerchantDepositCommand command => HandleMakeMerchantDeposit(command),
            MerchantCommands.CreateMerchantScheduleCommand command => HandleCreateMerchantSchedule(command),
            MerchantCommands.UpdateMerchantScheduleCommand command => HandleUpdateMerchantSchedule(command),

            OperatorQueries.GetOperatorsQuery query => HandleGetOperators(query),
            OperatorQueries.GetOperatorQuery query => HandleGetOperator(query),
            OperatorQueries.GetOperatorsForDropDownQuery query => HandleGetOperatorsForDropDown(query),
            OperatorCommands.CreateOperatorCommand command => HandleCreateOperator(command),
            OperatorCommands.UpdateOperatorCommand command => HandleUpdateOperator(command),

            ContractQueries.GetRecentContractsQuery query => HandleGetRecentContracts(query),
            ContractQueries.GetContractsForDropDownQuery query => HandleGetContractsForDropDown(query),
            ContractQueries.GetContractsQuery query => HandleGetContracts(query),
            ContractQueries.GetContractQuery query => HandleGetContract(query),
            ContractCommands.CreateContractCommand command => HandleCreateContract(command),
            ContractCommands.AddProductToContractCommand command => HandleAddProductToContract(command),
            ContractCommands.AddTransactionFeeToProductCommand command => HandleAddTransactionFeeToProduct(command),
            ContractCommands.RemoveTransactionFeeFromProductCommand command => HandleRemoveTransactionFeeFromProduct(command),

            TransactionQueries.GetTodaysSalesQuery _ => Result.Success(GetTodaysSales()),
            TransactionQueries.GetTodaysFailedSalesQuery _ => Result.Success(GetTodaysFailedSales()),
            TransactionQueries.GetTodaysSalesByHourQuery _ => Result.Success(GetTodaysSalesByHour()),
            TransactionQueries.GetTransactionDetailQuery _ => Result.Success(GetTransactionDetail()),
            TransactionQueries.GetMerchantTransactionSummaryQuery _ => Result.Success(GetMerchantSummary()),
            TransactionQueries.GetOperatorTransactionSummaryQuery _ => Result.Success(GetOperatorSummary()),
            TransactionQueries.GetProductPerformanceQuery _ => Result.Success(GetProductPerformance()),
            SettlementQueries.GetTodaysSettlementQuery _ => Result.Success(GetTodaysSettlement()),
            DateQueries.GetComparisonDatesQuery query => GetComparisonDates(query.EstateId),

            FileProcessingQueries.GetFileImportLogsListQuery _ => Result.Success(GetFileImportLogs()),
            FileProcessingQueries.GetFileImportLogQuery query => HandleGetFileImportLog(query),

            _ => throw new NotSupportedException($"Test mediator does not support {request.GetType().FullName}.")
        };
    }

    private Result<EstateModels.EstateModel> HandleGetEstate(EstateQueries.GetEstateQuery query)
    {
        var estate = _dataStore.GetEstate(query.EstateId);
        estate.Operators = GetAssignedOperatorModels(query.EstateId).Select(op => new EstateModels.EstateOperatorModel
        {
            OperatorId = op.OperatorId,
            Name = op.Name,
            RequireCustomMerchantNumber = op.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = op.RequireCustomTerminalNumber
        }).ToList();
        return Result.Success(estate);
    }

    private Result<List<OperatorModels.OperatorModel>> HandleGetAssignedOperators(EstateQueries.GetAssignedOperatorsQuery query)
        => Result.Success(GetAssignedOperatorModels(query.EstateId));

    private Result HandleAddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand command)
    {
        lock (_gate)
        {
            if (_assignedOperatorsByEstate.TryGetValue(command.EstateId, out var assigned) == false)
            {
                assigned = new HashSet<Guid>();
                _assignedOperatorsByEstate[command.EstateId] = assigned;
            }

            assigned.Add(command.OperatorId);
            return Result.Success();
        }
    }

    private Result HandleRemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand command)
    {
        lock (_gate)
        {
            if (_assignedOperatorsByEstate.TryGetValue(command.EstateId, out var assigned))
            {
                assigned.Remove(command.OperatorId);
            }
            return Result.Success();
        }
    }

    private Result<List<MerchantModels.MerchantListModel>> HandleGetMerchants(MerchantQueries.GetMerchantsQuery query)
    {
        var merchants = _dataStore.GetMerchants(query.EstateId)
            .Select(merchant => new MerchantModels.MerchantListModel
            {
                MerchantId = merchant.MerchantId,
                MerchantName = merchant.MerchantName,
                MerchantReference = merchant.MerchantReference,
                Balance = merchant.Balance,
                SettlementSchedule = merchant.SettlementSchedule,
                Region = merchant.Region,
                PostalCode = merchant.PostalCode,
                CreatedDateTime = merchant.CreatedDateTime
            })
            .Where(m =>
                Matches(m.MerchantName, query.Name) &&
                Matches(m.MerchantReference, query.Reference) &&
                Matches(m.Region, query.Region) &&
                Matches(m.PostalCode, query.PostCode) &&
                (query.SettlementSchedule is null || string.Equals(m.SettlementSchedule, query.SettlementSchedule.ToString(), StringComparison.OrdinalIgnoreCase))
            )
            .ToList();
        return Result.Success(merchants);
    }

    private Result<List<MerchantModels.MerchantDropDownModel>> HandleGetMerchantsForDropDown(MerchantQueries.GetMerchantsForDropDownQuery query)
    {
        var merchants = _dataStore.GetMerchants(query.EstateId)
            .Select((merchant, index) => new MerchantModels.MerchantDropDownModel
            {
                MerchantId = merchant.MerchantId,
                MerchantReportingId = index + 1,
                MerchantName = merchant.MerchantName
            }).ToList();
        return Result.Success(merchants);
    }

    private Result<List<MerchantModels.RecentMerchantsModel>> HandleGetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery _)
        => Result.Success(new List<MerchantModels.RecentMerchantsModel>());

    private Result<MerchantModels.MerchantKpiModel> HandleGetMerchantKpi(MerchantQueries.GetMerchantKpiQuery _)
        => Result.Success(new MerchantModels.MerchantKpiModel { MerchantsWithNoSaleInLast7Days = 5, MerchantsWithNoSaleToday = 12, MerchantsWithSaleInLastHour = 45 });

    private Result<MerchantModels.MerchantModel> HandleGetMerchant(MerchantQueries.GetMerchantQuery query)
    {
        var merchant = _dataStore.GetMerchant(query.EstateId, query.MerchantId);
        return merchant is null ? Result.NotFound() : Result.Success(merchant);
    }

    private Result<MerchantModels.MerchantScheduleModel> HandleGetMerchantSchedule(MerchantQueries.GetMerchantScheduleQuery query)
        => _supportState.TryGetMerchantSchedule(query.EstateId, query.MerchantId, query.Year, out var schedule) && schedule is not null
            ? Result.Success(schedule)
            : Result.NotFound();

    private Result<List<MerchantModels.MerchantOperatorModel>> HandleGetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery query)
        => Result.Success(_supportState.GetMerchantOperators(query.EstateId, query.MerchantId));

    private Result<List<MerchantModels.MerchantContractModel>> HandleGetMerchantContracts(MerchantQueries.GetMerchantContractsQuery query)
        => Result.Success(_supportState.GetMerchantContracts(query.EstateId, query.MerchantId));

    private Result<List<MerchantModels.MerchantDeviceModel>> HandleGetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery query)
        => Result.Success(_supportState.GetMerchantDevices(query.EstateId, query.MerchantId));

    private Result<MerchantModels.MerchantOpeningHoursModel> HandleGetMerchantOpeningHours(MerchantQueries.GetMerchantOpeningHoursQuery query)
        => _supportState.TryGetMerchantOpeningHours(query.EstateId, query.MerchantId, out var openingHours) && openingHours is not null
            ? Result.Success(openingHours)
            : Result.NotFound();

    private Result HandleCreateMerchant(MerchantCommands.CreateMerchantCommand command)
    {
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = command.MerchantId,
            MerchantName = command.Name,
            SettlementSchedule = command.SettlementSchedule,
            AddressId = command.MerchantAddress.AddressId,
            AddressLine1 = command.MerchantAddress.AddressLine1,
            Town = command.MerchantAddress.Town,
            Region = command.MerchantAddress.Region,
            PostalCode = command.MerchantAddress.PostalCode,
            Country = command.MerchantAddress.Country,
            ContactId = command.MerchantContact.ContactId,
            ContactName = command.MerchantContact.ContactName,
            ContactEmailAddress = command.MerchantContact.ContactEmail,
            ContactPhoneNumber = command.MerchantContact.ContactPhone,
            Balance = 0,
            AvailableBalance = 0,
            CreatedDateTime = DateTime.UtcNow
        };

        _dataStore.AddMerchant(command.EstateId, merchant);
        return Result.Success();
    }

    private Result HandleUpdateMerchant(MerchantCommands.UpdateMerchantCommand command)
    {
        var merchant = _dataStore.GetMerchant(command.EstateId, command.MerchantId) ?? new MerchantModels.MerchantModel
        {
            MerchantId = command.MerchantId,
            CreatedDateTime = DateTime.UtcNow
        };

        merchant.MerchantName = command.Name;
        merchant.SettlementSchedule = command.SettlementSchedule;
        merchant.AddressId = command.MerchantAddress.AddressId;
        merchant.AddressLine1 = command.MerchantAddress.AddressLine1;
        merchant.Town = command.MerchantAddress.Town;
        merchant.Region = command.MerchantAddress.Region;
        merchant.PostalCode = command.MerchantAddress.PostalCode;
        merchant.Country = command.MerchantAddress.Country;
        merchant.ContactId = command.MerchantContact.ContactId;
        merchant.ContactName = command.MerchantContact.ContactName;
        merchant.ContactEmailAddress = command.MerchantContact.ContactEmail;
        merchant.ContactPhoneNumber = command.MerchantContact.ContactPhone;

        _dataStore.UpdateMerchant(command.EstateId, merchant);
        return Result.Success();
    }

    private Result HandleUpdateMerchantOpeningHours(MerchantCommands.UpdateMerchantOpeningHoursCommand command)
    {
        _supportState.SetMerchantOpeningHours(command.EstateId, command.MerchantId, new MerchantModels.MerchantOpeningHoursModel
        {
            Sunday = ToDay(command.OpeningHours.Sunday),
            Monday = ToDay(command.OpeningHours.Monday),
            Tuesday = ToDay(command.OpeningHours.Tuesday),
            Wednesday = ToDay(command.OpeningHours.Wednesday),
            Thursday = ToDay(command.OpeningHours.Thursday),
            Friday = ToDay(command.OpeningHours.Friday),
            Saturday = ToDay(command.OpeningHours.Saturday)
        });
        return Result.Success();
    }

    private Result HandleAddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand command)
    {
        var operatorModel = GetOperators(command.EstateId).FirstOrDefault(op => op.OperatorId == command.OperatorId);
        if (operatorModel is null)
        {
            return Result.NotFound();
        }

        var list = _supportState.GetMerchantOperators(command.EstateId, command.MerchantId);
        if (list.Any(op => op.OperatorId == command.OperatorId) == false)
        {
            list.Add(new MerchantModels.MerchantOperatorModel
            {
                MerchantId = command.MerchantId,
                OperatorId = command.OperatorId,
                OperatorName = operatorModel.Name ?? string.Empty,
                MerchantNumber = command.MerchantNumber ?? string.Empty,
                TerminalNumber = command.TerminalNumber ?? string.Empty,
                IsDeleted = false
            });
        }

        return Result.Success();
    }

    private Result HandleRemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command)
    {
        _supportState.GetMerchantOperators(command.EstateId, command.MerchantId).RemoveAll(op => op.OperatorId == command.OperatorId);
        return Result.Success();
    }

    private Result HandleAssignContractToMerchant(MerchantCommands.AssignContractToMerchantCommand command)
    {
        var contract = _dataStore.GetContract(command.EstateId, command.ContractId);
        if (contract is null)
        {
            return Result.NotFound();
        }

        var list = _supportState.GetMerchantContracts(command.EstateId, command.MerchantId);
        if (list.Any(item => item.ContractId == command.ContractId) == false)
        {
            list.Add(new MerchantModels.MerchantContractModel
            {
                MerchantId = command.MerchantId,
                ContractId = command.ContractId,
                OperatorName = contract.OperatorName ?? string.Empty,
                ContractName = contract.Description ?? string.Empty,
                IsDeleted = false,
                ContractProducts = contract.Products?.Select(product => new MerchantModels.MerchantContractProductModel
                {
                    MerchantId = command.MerchantId,
                    ContractId = command.ContractId,
                    ProductId = product.ContractProductId,
                    ProductName = product.ProductName ?? string.Empty,
                    DisplayText = product.DisplayText ?? string.Empty,
                    ProductType = product.ProductType ?? string.Empty,
                    Value = decimal.TryParse(product.Value, out var value) ? value : null
                }).ToList() ?? []
            });
        }

        return Result.Success();
    }

    private Result HandleRemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand command)
    {
        _supportState.GetMerchantContracts(command.EstateId, command.MerchantId).RemoveAll(item => item.ContractId == command.ContractId);
        return Result.Success();
    }

    private Result HandleAddMerchantDevice(MerchantCommands.AddMerchantDeviceCommand command)
    {
        var list = _supportState.GetMerchantDevices(command.EstateId, command.MerchantId);
        if (list.Any(item => item.DeviceIdentifier == command.DeviceIdentifier) == false)
        {
            list.Add(new MerchantModels.MerchantDeviceModel
            {
                MerchantId = command.MerchantId,
                DeviceId = Guid.NewGuid(),
                DeviceIdentifier = command.DeviceIdentifier,
                IsDeleted = false
            });
        }

        return Result.Success();
    }

    private Result HandleSwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command)
    {
        var list = _supportState.GetMerchantDevices(command.EstateId, command.MerchantId);
        var device = list.FirstOrDefault(item => item.DeviceIdentifier == command.OldDevice);
        if (device is not null)
        {
            device.DeviceIdentifier = command.NewDevice;
        }
        return Result.Success();
    }

    private Result HandleMakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command)
    {
        var merchant = _dataStore.GetMerchant(command.EstateId, command.MerchantId);
        if (merchant is null)
        {
            return Result.NotFound();
        }

        merchant.Balance = (merchant.Balance ?? 0) + command.Amount;
        merchant.AvailableBalance = (merchant.AvailableBalance ?? 0) + command.Amount;
        _dataStore.UpdateMerchant(command.EstateId, merchant);
        return Result.Success();
    }

    private Result HandleCreateMerchantSchedule(MerchantCommands.CreateMerchantScheduleCommand command)
    {
        _supportState.SetMerchantSchedule(command.EstateId, command.MerchantId, command.Schedule);
        return Result.Success();
    }

    private Result HandleUpdateMerchantSchedule(MerchantCommands.UpdateMerchantScheduleCommand command)
    {
        _supportState.SetMerchantSchedule(command.EstateId, command.MerchantId, command.Schedule);
        return Result.Success();
    }

    private Result<List<OperatorModels.OperatorModel>> HandleGetOperators(OperatorQueries.GetOperatorsQuery query)
        => Result.Success(GetOperators(query.EstateId));

    private Result<OperatorModels.OperatorModel> HandleGetOperator(OperatorQueries.GetOperatorQuery query)
    {
        var @operator = GetOperators(query.EstateId).FirstOrDefault(item => item.OperatorId == query.OperatorId);
        return @operator is null ? Result.NotFound() : Result.Success(@operator);
    }

    private Result<List<OperatorModels.OperatorDropDownModel>> HandleGetOperatorsForDropDown(OperatorQueries.GetOperatorsForDropDownQuery query)
        => Result.Success(GetOperators(query.EstateId).Select((op, index) => new OperatorModels.OperatorDropDownModel
        {
            OperatorId = op.OperatorId,
            OperatorReportingId = index + 1,
            OperatorName = op.Name
        }).ToList());

    private Result HandleCreateOperator(OperatorCommands.CreateOperatorCommand command)
    {
        _dataStore.AddOperator(command.EstateId, new OperatorModels.OperatorModel
        {
            OperatorId = Guid.NewGuid(),
            Name = command.Name,
            RequireCustomMerchantNumber = command.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = command.RequireCustomTerminalNumber
        });
        return Result.Success();
    }

    private Result HandleUpdateOperator(OperatorCommands.UpdateOperatorCommand command)
    {
        _dataStore.UpdateOperator(command.EstateId, new OperatorModels.OperatorModel
        {
            OperatorId = command.OperatorId,
            Name = command.Name,
            RequireCustomMerchantNumber = command.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = command.RequireCustomTerminalNumber
        });
        return Result.Success();
    }

    private Result<List<ContractModels.RecentContractModel>> HandleGetRecentContracts(ContractQueries.GetRecentContractsQuery query)
        => Result.Success(GetContracts(query.EstateId).Select(contract => new ContractModels.RecentContractModel
        {
            ContractId = contract.ContractId,
            Description = contract.Description,
            OperatorName = contract.OperatorName
        }).ToList());

    private Result<List<ContractModels.ContractDropDownModel>> HandleGetContractsForDropDown(ContractQueries.GetContractsForDropDownQuery query)
        => Result.Success(GetContracts(query.EstateId).Select(contract => new ContractModels.ContractDropDownModel
        {
            ContractId = contract.ContractId,
            Description = contract.Description,
            OperatorName = contract.OperatorName
        }).ToList());

    private Result<List<ContractModels.ContractModel>> HandleGetContracts(ContractQueries.GetContractsQuery query)
        => Result.Success(GetContracts(query.EstateId));

    private Result<ContractModels.ContractModel> HandleGetContract(ContractQueries.GetContractQuery query)
    {
        var contract = GetContracts(query.EstateId).FirstOrDefault(item => item.ContractId == query.ContractId);
        return contract is null ? Result.NotFound() : Result.Success(contract);
    }

    private Result HandleCreateContract(ContractCommands.CreateContractCommand command)
    {
        var contract = new ContractModels.ContractModel
        {
            ContractId = Guid.NewGuid(),
            Description = command.Description,
            OperatorId = command.OperatorId,
            OperatorName = GetOperators(command.EstateId).FirstOrDefault(op => op.OperatorId == command.OperatorId)?.Name,
            Products = []
        };
        _dataStore.AddContract(command.EstateId, contract);
        return Result.Success();
    }

    private Result HandleAddProductToContract(ContractCommands.AddProductToContractCommand command)
    {
        var contract = GetContracts(command.EstateId).FirstOrDefault(item => item.ContractId == command.ContractId);
        if (contract is null)
        {
            return Result.NotFound();
        }

        contract.Products ??= [];
        contract.Products.Add(new ContractModels.ContractProductModel
        {
            ContractProductId = Guid.NewGuid(),
            ProductName = command.ProductName,
            DisplayText = command.DisplayText,
            ProductType = command.Value.HasValue ? "Fixed" : "Variable",
            Value = command.Value?.ToString(),
            NumberOfFees = 0,
            TransactionFees = []
        });
        _dataStore.UpdateContract(command.EstateId, contract);
        return Result.Success();
    }

    private Result HandleAddTransactionFeeToProduct(ContractCommands.AddTransactionFeeToProductCommand command)
    {
        var contract = GetContracts(command.EstateId).FirstOrDefault(item => item.ContractId == command.ContractId);
        var product = contract?.Products?.FirstOrDefault(item => item.ContractProductId == command.ProductId);
        if (contract is null || product is null)
        {
            return Result.NotFound();
        }

        product.TransactionFees ??= [];
        product.TransactionFees.Add(new ContractModels.ContractProductTransactionFeeModel
        {
            TransactionFeeId = Guid.NewGuid(),
            Description = command.Description,
            CalculationType = command.CalculationType.Equals("Fixed", StringComparison.OrdinalIgnoreCase) ? 0 : 1,
            FeeType = command.FeeType.Equals("Merchant", StringComparison.OrdinalIgnoreCase) ? 0 : 1,
            Value = command.Value
        });
        product.NumberOfFees = product.TransactionFees.Count;
        _dataStore.UpdateContract(command.EstateId, contract);
        return Result.Success();
    }

    private Result HandleRemoveTransactionFeeFromProduct(ContractCommands.RemoveTransactionFeeFromProductCommand command)
    {
        var contract = GetContracts(command.EstateId).FirstOrDefault(item => item.ContractId == command.ContractId);
        var product = contract?.Products?.FirstOrDefault(item => item.ContractProductId == command.ProductId);
        if (contract is null || product is null)
        {
            return Result.NotFound();
        }

        product.TransactionFees?.RemoveAll(fee => fee.TransactionFeeId == command.FeeId);
        product.NumberOfFees = product.TransactionFees?.Count ?? 0;
        _dataStore.UpdateContract(command.EstateId, contract);
        return Result.Success();
    }

    private Result<List<ComparisonDateModel>> GetComparisonDates(Guid _)
        => Result.Success(new List<ComparisonDateModel>
        {
            new() { Date = DateTime.Today, Description = "Today", OrderValue = 1 },
            new() { Date = DateTime.Today.AddDays(-1), Description = "Yesterday", OrderValue = 2 }
        });

    private TodaysSalesModel GetTodaysSales()
        => new()
        {
            TodaysSalesCount = 523,
            TodaysSalesValue = 145000m,
            TodaysAverageValue = 277.62m,
            ComparisonSalesCount = 481,
            ComparisonSalesValue = 130000m,
            ComparisonAverageValue = 270.27m
        };

    private TodaysSalesModel GetTodaysFailedSales()
        => new()
        {
            TodaysSalesCount = 15,
            TodaysSalesValue = 850m,
            TodaysAverageValue = 56.67m,
            ComparisonSalesCount = 12,
            ComparisonSalesValue = 700m,
            ComparisonAverageValue = 58.33m
        };

    private List<TransactionModels.TodaysSalesByHourModel> GetTodaysSalesByHour()
        => new()
        {
            new() { Hour = 9, TodaysSalesCount = 1, ComparisonSalesCount = 0, TodaysSalesValue = 10m, ComparisonSalesValue = 0m },
            new() { Hour = 10, TodaysSalesCount = 0, ComparisonSalesCount = 1, TodaysSalesValue = 0m, ComparisonSalesValue = 10m },
            new() { Hour = 13, TodaysSalesCount = 1, ComparisonSalesCount = 0, TodaysSalesValue = 10m, ComparisonSalesValue = 0m },
            new() { Hour = 14, TodaysSalesCount = 0, ComparisonSalesCount = 1, TodaysSalesValue = 0m, ComparisonSalesValue = 10m },
            new() { Hour = 15, TodaysSalesCount = 1, ComparisonSalesCount = 0, TodaysSalesValue = 10m, ComparisonSalesValue = 0m }
        };

    private TransactionModels.TransactionDetailReportResponse GetTransactionDetail()
        => new()
        {
            Summary = new TransactionModels.TransactionDetailSummary
            {
                TransactionCount = 5,
                TotalValue = 50m,
                TotalFees = 6.25m
            },
            Transactions = new List<TransactionModels.TransactionDetail>
            {
                CreateTransaction(1001, ReportingMerchantA, ReportingProductA, Safaricom, 10m, 1.25m, Successful),
                CreateTransaction(1002, ReportingMerchantA, ReportingProductA, Safaricom, 10m, 1.25m, Successful),
                CreateTransaction(1003, ReportingMerchantB, ReportingProductB, Voucher, 10m, 1.25m, Successful),
                CreateTransaction(1004, ReportingMerchantA, ReportingProductA, Safaricom, 10m, 1.25m, Successful),
                CreateTransaction(1005, ReportingMerchantB, ReportingProductB, Voucher, 10m, 1.25m, Successful)
            }
        };

    private TransactionModels.TransactionSummaryByMerchantResponse GetMerchantSummary()
        => new()
        {
            Summary = new TransactionModels.MerchantDetailSummary
            {
                TotalMerchants = 2,
                TotalCount = 5,
                TotalValue = 50m,
                AverageValue = 10m
            },
            Merchants = new List<TransactionModels.MerchantDetail>
            {
                new() { MerchantName = ReportingMerchantA, TotalCount = 3, TotalValue = 30m, AverageValue = 30m, AuthorisedCount = 3, DeclinedCount = 0, AuthorisedPercentage = 100m },
                new() { MerchantName = ReportingMerchantB, TotalCount = 2, TotalValue = 20m, AverageValue = 20m, AuthorisedCount = 2, DeclinedCount = 0, AuthorisedPercentage = 100m }
            }
        };

    private TransactionModels.TransactionSummaryByOperatorResponse GetOperatorSummary()
        => new()
        {
            Summary = new TransactionModels.OperatorDetailSummary
            {
                TotalOperators = 2,
                TotalCount = 5,
                TotalValue = 50m,
                AverageValue = 10m
            },
            Operators = new List<TransactionModels.OperatorDetail>
            {
                new() { OperatorName = Safaricom, TotalCount = 3, TotalValue = 30m, AverageValue = 30m, AuthorisedCount = 3, DeclinedCount = 0, AuthorisedPercentage = 100m },
                new() { OperatorName = Voucher, TotalCount = 2, TotalValue = 20m, AverageValue = 20m, AuthorisedCount = 2, DeclinedCount = 0, AuthorisedPercentage = 100m }
            }
        };

    private TransactionModels.ProductPerformanceResponse GetProductPerformance()
        => new()
        {
            Summary = new TransactionModels.ProductPerformanceSummary
            {
                TotalProducts = 2,
                TotalCount = 5,
                TotalValue = 50m,
                AveragePerProduct = 25m
            },
            ProductDetails = new List<TransactionModels.ProductPerformanceDetail>
            {
                new() { ProductName = ReportingProductA, TransactionCount = 3, TransactionValue = 30m, PercentageOfTotal = 60m },
                new() { ProductName = ReportingProductB, TransactionCount = 2, TransactionValue = 20m, PercentageOfTotal = 40m }
            }
        };

    private TodaysSettlementModel GetTodaysSettlement()
        => new()
        {
            ComparisonSettlementCount = 2,
            ComparisonSettlementValue = 20m,
            ComparisonPendingSettlementCount = 1,
            ComparisonPendingSettlementValue = 10m,
            TodaysSettlementCount = 3,
            TodaysSettlementValue = 30m,
            TodaysPendingSettlementCount = 0,
            TodaysPendingSettlementValue = 0m
        };

    private List<FileImportLogDetailsModel> GetFileImportLogs()
        => _supportState.GetFileImportLogs();

    private Result<FileImportLogDetailsModel> HandleGetFileImportLog(FileProcessingQueries.GetFileImportLogQuery query)
    {
        var log = _supportState.GetFileImportLog(query.FileImportLogId);
        return log is null ? Result.NotFound() : Result.Success(log);
    }

    private static TransactionModels.TransactionDetail CreateTransaction(int number, string merchant, string product, string @operator, decimal grossAmount, decimal fee, string status)
        => new()
        {
            TransactionNumber = number,
            Merchant = merchant,
            Product = product,
            Operator = @operator,
            Type = "sale",
            Status = status,
            Value = grossAmount,
            TotalFees = fee,
            SettlementReference = string.Empty,
            DateTime = DateTime.Today
        };

    private static MerchantModels.DayOpeningHoursModel ToDay(MerchantCommands.OpeningHours hours)
        => new() { Opening = hours.Opening, Closing = hours.Closing };

    private List<OperatorModels.OperatorModel> GetOperators(Guid estateId) => _dataStore.GetOperators(estateId);

    private List<OperatorModels.OperatorModel> GetAssignedOperatorModels(Guid estateId)
    {
        lock (_gate)
        {
            if (_assignedOperatorsByEstate.TryGetValue(estateId, out var assigned) == false)
            {
                assigned = _dataStore.GetOperators(estateId).Select(op => op.OperatorId).ToHashSet();
                _assignedOperatorsByEstate[estateId] = assigned;
            }

            return _dataStore.GetOperators(estateId)
                .Where(op => assigned.Contains(op.OperatorId))
                .Select(op => new OperatorModels.OperatorModel
                {
                    OperatorId = op.OperatorId,
                    Name = op.Name,
                    RequireCustomMerchantNumber = op.RequireCustomMerchantNumber,
                    RequireCustomTerminalNumber = op.RequireCustomTerminalNumber
                })
                .ToList();
        }
    }

    private List<ContractModels.ContractModel> GetContracts(Guid estateId) => _dataStore.GetContracts(estateId);

    private static bool Matches(string? value, string? filter)
        => string.IsNullOrWhiteSpace(filter) || string.IsNullOrWhiteSpace(value) || value.Contains(filter, StringComparison.OrdinalIgnoreCase);
}
