using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Requests;
using MediatR;

namespace EstateManagementUI.BlazorServer.Services;

/// <summary>
/// Test-enabled MediatR service that uses an in-memory data store
/// Allows CRUD operations on test data during test execution while maintaining the mediator pattern
/// </summary>
public class TestMediatorService : IMediator
{
    private readonly ITestDataStore _testDataStore;

    public TestMediatorService(ITestDataStore testDataStore)
    {
        _testDataStore = testDataStore;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return request switch
        {
            // Estate Queries
            Queries.GetEstateQuery query => Task.FromResult((TResponse)(object)Result<EstateModel>.Success(_testDataStore.GetEstate(query.EstateId))),
            
            // Merchant Queries
            Queries.GetMerchantsQuery query => Task.FromResult((TResponse)(object)Result<List<MerchantModel>>.Success(_testDataStore.GetMerchants(query.EstateId))),
            Queries.GetMerchantQuery query => Task.FromResult((TResponse)(object)GetMerchantResult(query.EstateId, query.MerchantId)),
            
            // Operator Queries
            Queries.GetOperatorsQuery query => Task.FromResult((TResponse)(object)Result<List<OperatorModel>>.Success(_testDataStore.GetOperators(query.EstateId))),
            Queries.GetOperatorQuery query => Task.FromResult((TResponse)(object)GetOperatorResult(query.EstateId, query.OperatorId)),
            
            // Contract Queries
            Queries.GetContractsQuery query => Task.FromResult((TResponse)(object)Result<List<ContractModel>>.Success(_testDataStore.GetContracts(query.EstateId))),
            Queries.GetContractQuery query => Task.FromResult((TResponse)(object)GetContractResult(query.EstateId, query.ContractId)),
            
            // File Processing Queries - return mock data
            Queries.GetFileImportLogsListQuery => Task.FromResult((TResponse)(object)Result<List<FileImportLogModel>>.Success(GetMockFileImportLogs())),
            Queries.GetFileImportLogQuery => Task.FromResult((TResponse)(object)Result<FileImportLogModel>.Success(GetMockFileImportLog())),
            Queries.GetFileDetailsQuery => Task.FromResult((TResponse)(object)Result<FileDetailsModel>.Success(GetMockFileDetails())),
            
            // Dashboard Queries - return mock data
            Queries.GetComparisonDatesQuery => Task.FromResult((TResponse)(object)Result<List<ComparisonDateModel>>.Success(GetMockComparisonDates())),
            Queries.GetTodaysSalesQuery => Task.FromResult((TResponse)(object)Result<TodaysSalesModel>.Success(GetMockTodaysSales())),
            Queries.GetTodaysSettlementQuery => Task.FromResult((TResponse)(object)Result<TodaysSettlementModel>.Success(GetMockTodaysSettlement())),
            Queries.GetTodaysSalesCountByHourQuery => Task.FromResult((TResponse)(object)Result<List<TodaysSalesCountByHourModel>>.Success(GetMockSalesCountByHour())),
            Queries.GetTodaysSalesValueByHourQuery => Task.FromResult((TResponse)(object)Result<List<TodaysSalesValueByHourModel>>.Success(GetMockSalesValueByHour())),
            Queries.GetMerchantKpiQuery => Task.FromResult((TResponse)(object)Result<MerchantKpiModel>.Success(GetMockMerchantKpi())),
            Queries.GetTodaysFailedSalesQuery => Task.FromResult((TResponse)(object)Result<TodaysSalesModel>.Success(GetMockTodaysSales())),
            Queries.GetTopProductDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomProductDataModel>>.Success(GetMockTopProducts())),
            Queries.GetBottomProductDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomProductDataModel>>.Success(GetMockBottomProducts())),
            Queries.GetTopMerchantDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomMerchantDataModel>>.Success(GetMockTopMerchants())),
            Queries.GetBottomMerchantDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomMerchantDataModel>>.Success(GetMockBottomMerchants())),
            Queries.GetTopOperatorDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomOperatorDataModel>>.Success(GetMockTopOperators())),
            Queries.GetBottomOperatorDataQuery => Task.FromResult((TResponse)(object)Result<List<TopBottomOperatorDataModel>>.Success(GetMockBottomOperators())),
            Queries.GetLastSettlementQuery => Task.FromResult((TResponse)(object)Result<LastSettlementModel>.Success(GetMockLastSettlement())),
            
            // Commands - execute against test data store
            Commands.CreateMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteCreateMerchant(cmd)),
            Commands.UpdateMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteUpdateMerchant(cmd)),
            Commands.UpdateMerchantAddressCommand cmd => Task.FromResult((TResponse)(object)ExecuteUpdateMerchantAddress(cmd)),
            Commands.UpdateMerchantContactCommand cmd => Task.FromResult((TResponse)(object)ExecuteUpdateMerchantContact(cmd)),
            Commands.CreateOperatorCommand cmd => Task.FromResult((TResponse)(object)ExecuteCreateOperator(cmd)),
            Commands.UpdateOperatorCommand cmd => Task.FromResult((TResponse)(object)ExecuteUpdateOperator(cmd)),
            Commands.CreateContractCommand cmd => Task.FromResult((TResponse)(object)ExecuteCreateContract(cmd)),
            Commands.AddProductToContractCommand cmd => Task.FromResult((TResponse)(object)ExecuteAddProductToContract(cmd)),
            Commands.AddTransactionFeeForProductToContractCommand cmd => Task.FromResult((TResponse)(object)ExecuteAddTransactionFee(cmd)),
            Commands.AssignContractToMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteAssignContractToMerchant(cmd)),
            Commands.RemoveContractFromMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteRemoveContractFromMerchant(cmd)),
            Commands.AddOperatorToMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteAddOperatorToMerchant(cmd)),
            Commands.RemoveOperatorFromMerchantCommand cmd => Task.FromResult((TResponse)(object)ExecuteRemoveOperatorFromMerchant(cmd)),
            Commands.AddOperatorToEstateCommand cmd => Task.FromResult((TResponse)(object)ExecuteAddOperatorToEstate(cmd)),
            Commands.RemoveOperatorFromEstateCommand cmd => Task.FromResult((TResponse)(object)ExecuteRemoveOperatorFromEstate(cmd)),
            Commands.AddMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.SwapMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateMerchantUserCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.MakeMerchantDepositCommand cmd => Task.FromResult((TResponse)(object)ExecuteMakeMerchantDeposit(cmd)),
            Commands.SetMerchantSettlementScheduleCommand => Task.FromResult((TResponse)(object)Result.Success()),
            
            _ => throw new NotImplementedException($"Request type {request.GetType().Name} is not implemented in test mediator")
        };
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        return Task.CompletedTask;
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<object?>(null);
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        return Task.CompletedTask;
    }

    // Helper methods for retrieving data
    private Result<MerchantModel> GetMerchantResult(Guid estateId, Guid merchantId)
    {
        var merchant = _testDataStore.GetMerchant(estateId, merchantId);
        return merchant != null 
            ? Result<MerchantModel>.Success(merchant) 
            : Result<MerchantModel>.Failure($"Merchant {merchantId} not found");
    }

    private Result<OperatorModel> GetOperatorResult(Guid estateId, Guid operatorId)
    {
        var operatorModel = _testDataStore.GetOperator(estateId, operatorId);
        return operatorModel != null 
            ? Result<OperatorModel>.Success(operatorModel) 
            : Result<OperatorModel>.Failure($"Operator {operatorId} not found");
    }

    private Result<ContractModel> GetContractResult(Guid estateId, Guid contractId)
    {
        var contract = _testDataStore.GetContract(estateId, contractId);
        return contract != null 
            ? Result<ContractModel>.Success(contract) 
            : Result<ContractModel>.Failure($"Contract {contractId} not found");
    }

    // Command execution methods
    private Result ExecuteCreateMerchant(Commands.CreateMerchantCommand cmd)
    {
        var merchant = new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = cmd.Name,
            ContactName = cmd.ContactName,
            ContactEmailAddress = cmd.ContactEmail,
            SettlementSchedule = "Immediate"
        };
        _testDataStore.AddMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    private Result ExecuteUpdateMerchant(Commands.UpdateMerchantCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        merchant.MerchantName = cmd.Name;
        _testDataStore.UpdateMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    private Result ExecuteUpdateMerchantAddress(Commands.UpdateMerchantAddressCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        merchant.AddressLine1 = cmd.AddressLine1;
        merchant.Town = cmd.Town;
        merchant.Region = cmd.Region;
        merchant.PostalCode = cmd.PostalCode;
        merchant.Country = cmd.Country;
        _testDataStore.UpdateMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    private Result ExecuteUpdateMerchantContact(Commands.UpdateMerchantContactCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        merchant.ContactName = cmd.ContactName;
        merchant.ContactEmailAddress = cmd.ContactEmail;
        merchant.ContactPhoneNumber = cmd.ContactPhone;
        _testDataStore.UpdateMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    private Result ExecuteCreateOperator(Commands.CreateOperatorCommand cmd)
    {
        var operatorModel = new OperatorModel
        {
            OperatorId = Guid.NewGuid(),
            Name = cmd.Name,
            RequireCustomMerchantNumber = cmd.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = cmd.RequireCustomTerminalNumber
        };
        _testDataStore.AddOperator(cmd.EstateId, operatorModel);
        return Result.Success();
    }

    private Result ExecuteUpdateOperator(Commands.UpdateOperatorCommand cmd)
    {
        var operatorModel = _testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operatorModel == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        operatorModel.Name = cmd.Name;
        operatorModel.RequireCustomMerchantNumber = cmd.RequireCustomMerchantNumber;
        operatorModel.RequireCustomTerminalNumber = cmd.RequireCustomTerminalNumber;
        _testDataStore.UpdateOperator(cmd.EstateId, operatorModel);
        return Result.Success();
    }

    private Result ExecuteCreateContract(Commands.CreateContractCommand cmd)
    {
        var contract = new ContractModel
        {
            ContractId = Guid.NewGuid(),
            Description = cmd.Description,
            OperatorId = cmd.OperatorId,
            Products = new List<ContractProductModel>()
        };
        
        var operatorModel = _testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operatorModel != null)
        {
            contract.OperatorName = operatorModel.Name;
        }
        
        _testDataStore.AddContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAddProductToContract(Commands.AddProductToContractCommand cmd)
    {
        var contract = _testDataStore.GetContract(cmd.EstateId, cmd.ContractId);
        if (contract == null)
            return Result.Failure($"Contract {cmd.ContractId} not found");
        
        if (contract.Products == null)
            contract.Products = new List<ContractProductModel>();
        
        contract.Products.Add(new ContractProductModel
        {
            ContractProductId = Guid.NewGuid(),
            ProductName = cmd.ProductName,
            DisplayText = cmd.DisplayText,
            Value = cmd.Value?.ToString() ?? "Variable"
        });
        
        _testDataStore.UpdateContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAddTransactionFee(Commands.AddTransactionFeeForProductToContractCommand cmd)
    {
        var contract = _testDataStore.GetContract(cmd.EstateId, cmd.ContractId);
        if (contract == null)
            return Result.Failure($"Contract {cmd.ContractId} not found");
        
        var product = contract.Products?.FirstOrDefault(p => p.ContractProductId == cmd.ProductId);
        if (product == null)
            return Result.Failure($"Product {cmd.ProductId} not found in contract");
        
        if (product.TransactionFees == null)
            product.TransactionFees = new List<ContractProductTransactionFeeModel>();
        
        product.TransactionFees.Add(new ContractProductTransactionFeeModel
        {
            TransactionFeeId = Guid.NewGuid(),
            Description = cmd.Description,
            Value = cmd.Value
        });
        
        _testDataStore.UpdateContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAssignContractToMerchant(Commands.AssignContractToMerchantCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        // This is a simplified implementation
        // In real scenario, you'd track contract assignments
        return Result.Success();
    }

    private Result ExecuteRemoveContractFromMerchant(Commands.RemoveContractFromMerchantCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteAddOperatorToMerchant(Commands.AddOperatorToMerchantCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteRemoveOperatorFromMerchant(Commands.RemoveOperatorFromMerchantCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteMakeMerchantDeposit(Commands.MakeMerchantDepositCommand cmd)
    {
        var merchant = _testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        // Update the merchant's balance by adding the deposit amount
        merchant.Balance = (merchant.Balance ?? 0) + cmd.Amount;
        merchant.AvailableBalance = (merchant.AvailableBalance ?? 0) + cmd.Amount;
        
        _testDataStore.UpdateMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    // Mock data methods for dashboard/file processing (not part of core CRUD)
    private static List<FileImportLogModel> GetMockFileImportLogs() => new()
    {
        new FileImportLogModel
        {
            FileImportLogId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            ImportLogDateTime = DateTime.Now.AddHours(-2),
            FileCount = 5,
            FileUploadedDateTime = DateTime.Now.AddHours(-3)
        }
    };

    private static FileImportLogModel GetMockFileImportLog() => new()
    {
        FileImportLogId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        ImportLogDateTime = DateTime.Now.AddHours(-2),
        FileCount = 5,
        FileUploadedDateTime = DateTime.Now.AddHours(-3)
    };

    private static FileDetailsModel GetMockFileDetails() => new()
    {
        FileId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
        FileLocation = "/files/transactions.csv",
        FileProfileName = "SafaricomTopup",
        MerchantName = "Test Merchant 1",
        UserId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
        FileUploadedDateTime = DateTime.Now.AddHours(-3),
        ProcessingCompletedDateTime = DateTime.Now.AddHours(-2),
        TotalLines = 100,
        SuccessfullyProcessedLines = 95,
        FailedLines = 5,
        IgnoredLines = 0
    };

    private static List<ComparisonDateModel> GetMockComparisonDates() => new()
    {
        new ComparisonDateModel { Date = DateTime.Today, Description = "Today" },
        new ComparisonDateModel { Date = DateTime.Today.AddDays(-1), Description = "Yesterday" }
    };

    private static TodaysSalesModel GetMockTodaysSales() => new()
    {
        ComparisonSalesCount = 450,
        ComparisonSalesValue = 125000.00m,
        ComparisonAverageValue = 277.78m,
        TodaysSalesCount = 523,
        TodaysSalesValue = 145000.00m,
        TodaysAverageValue = 277.24m
    };

    private static TodaysSettlementModel GetMockTodaysSettlement() => new()
    {
        ComparisonSettlementCount = 125,
        ComparisonSettlementValue = 112500.00m,
        TodaysSettlementCount = 150,
        TodaysSettlementValue = 130500.00m
    };

    private static List<TodaysSalesCountByHourModel> GetMockSalesCountByHour() => new()
    {
        new TodaysSalesCountByHourModel { Hour = 9, TodaysSalesCount = 45, ComparisonSalesCount = 38 },
        new TodaysSalesCountByHourModel { Hour = 10, TodaysSalesCount = 67, ComparisonSalesCount = 54 }
    };

    private static List<TodaysSalesValueByHourModel> GetMockSalesValueByHour() => new()
    {
        new TodaysSalesValueByHourModel { Hour = 9, TodaysSalesValue = 12500, ComparisonSalesValue = 10500 },
        new TodaysSalesValueByHourModel { Hour = 10, TodaysSalesValue = 18500, ComparisonSalesValue = 15000 }
    };

    private static MerchantKpiModel GetMockMerchantKpi() => new()
    {
        MerchantsWithNoSaleInLast7Days = 5,
        MerchantsWithNoSaleToday = 12,
        MerchantsWithSaleInLastHour = 45
    };

    private static List<TopBottomProductDataModel> GetMockTopProducts() => new()
    {
        new TopBottomProductDataModel { ProductName = "Mobile Topup", SalesValue = 125000.00m },
        new TopBottomProductDataModel { ProductName = "Bill Payment", SalesValue = 87000.00m }
    };

    private static List<TopBottomProductDataModel> GetMockBottomProducts() => new()
    {
        new TopBottomProductDataModel { ProductName = "Data Bundle", SalesValue = 5000.00m },
        new TopBottomProductDataModel { ProductName = "SMS Bundle", SalesValue = 3500.00m }
    };

    private static List<TopBottomMerchantDataModel> GetMockTopMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 1", SalesValue = 85000.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 2", SalesValue = 67000.00m }
    };

    private static List<TopBottomMerchantDataModel> GetMockBottomMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 10", SalesValue = 1200.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 11", SalesValue = 850.00m }
    };

    private static List<TopBottomOperatorDataModel> GetMockTopOperators() => new()
    {
        new TopBottomOperatorDataModel { OperatorName = "Safaricom", SalesValue = 195000.00m },
        new TopBottomOperatorDataModel { OperatorName = "Voucher", SalesValue = 67000.00m }
    };

    private static List<TopBottomOperatorDataModel> GetMockBottomOperators() => new()
    {
        new TopBottomOperatorDataModel { OperatorName = "PataPawa PostPay", SalesValue = 12000.00m }
    };

    private static LastSettlementModel GetMockLastSettlement() => new()
    {
        SettlementDate = DateTime.Today.AddDays(-1),
        FeesValue = 1250.00m,
        SalesCount = 450,
        SalesValue = 125000.00m,
        SettlementValue = 123750.00m
    };

    private Result ExecuteAddOperatorToEstate(Commands.AddOperatorToEstateCommand cmd)
    {
        var operator1 = _testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operator1 == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        // In a real implementation, we would add the operator to the estate
        // For now, we just return success as the operator already exists in the data store
        return Result.Success();
    }

    private Result ExecuteRemoveOperatorFromEstate(Commands.RemoveOperatorFromEstateCommand cmd)
    {
        var operator1 = _testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operator1 == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        // In a real implementation, we would remove the operator from the estate
        // For now, we just return success
        return Result.Success();
    }
}
