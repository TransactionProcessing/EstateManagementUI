using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

namespace EstateManagementUI.BusinessLogic.Services;

/// <summary>
/// Test-enabled MediatR service that uses an in-memory data store
/// Allows CRUD operations on test data during test execution while maintaining the mediator pattern
/// </summary>
public class TestMediatorService : IMediator
{
    private readonly ITestDataStore _testDataStore;

    public TestMediatorService(ITestDataStore testDataStore)
    {
        this._testDataStore = testDataStore;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return request switch
        {
            // Estate Queries
            EstateQueries.GetEstateQuery query => Task.FromResult((TResponse)(object)Result.Success(this._testDataStore.GetEstate(query.EstateId))),

            // Merchant Queries
            MerchantQueries.GetMerchantsQuery query => Task.FromResult((TResponse)(object)Result.Success(this._testDataStore.GetMerchants(query.EstateId))),
            MerchantQueries.GetMerchantQuery query => Task.FromResult((TResponse)(object)this.GetMerchantResult(query.EstateId, query.MerchantId)),
            MerchantQueries.GetRecentMerchantsQuery query => Task.FromResult((TResponse)(object)Result.Success(this._testDataStore.GetRecentMerchants(query.EstateId))),

            // Operator Queries
            OperatorQueries.GetOperatorsQuery query => Task.FromResult((TResponse)(object)Result.Success(this._testDataStore.GetOperators(query.EstateId))),
            OperatorQueries.GetOperatorQuery query => Task.FromResult((TResponse)(object)this.GetOperatorResult(query.EstateId, query.OperatorId)),

            // Contract Queries
            ContractQueries.GetContractsQuery query => Task.FromResult((TResponse)(object)Result.Success(this._testDataStore.GetContracts(query.EstateId))),
            ContractQueries.GetContractQuery query => Task.FromResult((TResponse)(object)this.GetContractResult(query.EstateId, query.ContractId)),
            
            // File Processing Queries - return mock data
            Queries.GetFileImportLogsListQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockFileImportLogs())),
            Queries.GetFileImportLogQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockFileImportLog())),
            Queries.GetFileDetailsQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockFileDetails())),
            
            // Dashboard Queries - return mock data
            Queries.GetComparisonDatesQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockComparisonDates())),
            Queries.GetTodaysSalesQuery query => Task.FromResult((TResponse)(object)Result.Success(GetMockTodaysSales(query.ComparisonDate))),
            Queries.GetTodaysSettlementQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockTodaysSettlement())),
            Queries.GetTodaysSalesCountByHourQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockSalesCountByHour())),
            Queries.GetTodaysSalesValueByHourQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockSalesValueByHour())),
            MerchantQueries.GetMerchantKpiQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockMerchantKpi())),
            Queries.GetTodaysFailedSalesQuery query => Task.FromResult((TResponse)(object)Result.Success(GetMockTodaysFailedSales(query.ComparisonDate))),
            Queries.GetTopProductDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockTopProducts())),
            Queries.GetBottomProductDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockBottomProducts())),
            Queries.GetTopMerchantDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockTopMerchants())),
            Queries.GetBottomMerchantDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockBottomMerchants())),
            Queries.GetTopOperatorDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockTopOperators())),
            Queries.GetBottomOperatorDataQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockBottomOperators())),
            Queries.GetLastSettlementQuery => Task.FromResult((TResponse)(object)Result.Success(GetMockLastSettlement())),
            Queries.GetMerchantTransactionSummaryQuery query => Task.FromResult((TResponse)(object)Result.Success(this.GetMockMerchantTransactionSummary(query))),
            Queries.GetProductPerformanceQuery query => Task.FromResult((TResponse)(object)Result.Success(this.GetMockProductPerformance(query))),
            Queries.GetOperatorTransactionSummaryQuery query => Task.FromResult((TResponse)(object)Result.Success(this.GetMockOperatorTransactionSummary(query))),
            Queries.GetMerchantSettlementHistoryQuery query => Task.FromResult((TResponse)(object)Result.Success(this.GetMockMerchantSettlementHistory(query))),
            Queries.GetSettlementSummaryQuery query => Task.FromResult((TResponse)(object)Result.Success(this.GetMockSettlementSummary(query))),
            Queries.GetTransactionDetailQuery q => Task.FromResult((TResponse)(object)Result.Success(this.GetMockTransactionDetails(q))),

            // Commands - execute against test data store
            MerchantCommands.CreateMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteCreateMerchant(cmd)),
            MerchantCommands.UpdateMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteUpdateMerchant(cmd)),
            Commands.CreateOperatorCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteCreateOperator(cmd)),
            Commands.UpdateOperatorCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteUpdateOperator(cmd)),
            Commands.CreateContractCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteCreateContract(cmd)),
            Commands.AddProductToContractCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteAddProductToContract(cmd)),
            Commands.AddTransactionFeeForProductToContractCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteAddTransactionFee(cmd)),
            MerchantCommands.AssignContractToMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteAssignContractToMerchant(cmd)),
            MerchantCommands.RemoveContractFromMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteRemoveContractFromMerchant(cmd)),
            MerchantCommands.AddOperatorToMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteAddOperatorToMerchant(cmd)),
            MerchantCommands.RemoveOperatorFromMerchantCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteRemoveOperatorFromMerchant(cmd)),
            EstateCommands.AddOperatorToEstateCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteAddOperatorToEstate(cmd)),
            EstateCommands.RemoveOperatorFromEstateCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteRemoveOperatorFromEstate(cmd)),
            MerchantCommands.AddMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            MerchantCommands.SwapMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateMerchantUserCommand => Task.FromResult((TResponse)(object)Result.Success()),
            MerchantCommands.MakeMerchantDepositCommand cmd => Task.FromResult((TResponse)(object)this.ExecuteMakeMerchantDeposit(cmd)),
            
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
        var merchant = this._testDataStore.GetMerchant(estateId, merchantId);
        return merchant != null 
            ? Result.Success(merchant) 
            : Result.Failure($"Merchant {merchantId} not found");
    }

    private Result<OperatorModel> GetOperatorResult(Guid estateId, Guid operatorId)
    {
        var operatorModel = this._testDataStore.GetOperator(estateId, operatorId);
        return operatorModel != null 
            ? Result.Success(operatorModel) 
            : Result.Failure($"Operator {operatorId} not found");
    }

    private Result<ContractModel> GetContractResult(Guid estateId, Guid contractId)
    {
        var contract = this._testDataStore.GetContract(estateId, contractId);
        return contract != null 
            ? Result.Success(contract) 
            : Result.Failure($"Contract {contractId} not found");
    }

    // Command execution methods
    private Result ExecuteCreateMerchant(MerchantCommands.CreateMerchantCommand cmd)
    {
        var merchant = new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = cmd.Name,
            ContactName = cmd.MerchantContact.ContactName,
            ContactEmailAddress = cmd.MerchantContact.ContactEmail,
            SettlementSchedule = "Immediate"
        };
        this._testDataStore.AddMerchant(cmd.EstateId, merchant);
        return Result.Success();
    }

    private Result ExecuteUpdateMerchant(MerchantCommands.UpdateMerchantCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        merchant.MerchantName = cmd.Name;
        this._testDataStore.UpdateMerchant(cmd.EstateId, merchant);
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
        this._testDataStore.AddOperator(cmd.EstateId, operatorModel);
        return Result.Success();
    }

    private Result ExecuteUpdateOperator(Commands.UpdateOperatorCommand cmd)
    {
        var operatorModel = this._testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operatorModel == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        operatorModel.Name = cmd.Name;
        operatorModel.RequireCustomMerchantNumber = cmd.RequireCustomMerchantNumber;
        operatorModel.RequireCustomTerminalNumber = cmd.RequireCustomTerminalNumber;
        this._testDataStore.UpdateOperator(cmd.EstateId, operatorModel);
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
        
        var operatorModel = this._testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operatorModel != null)
        {
            contract.OperatorName = operatorModel.Name;
        }
        
        this._testDataStore.AddContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAddProductToContract(Commands.AddProductToContractCommand cmd)
    {
        var contract = this._testDataStore.GetContract(cmd.EstateId, cmd.ContractId);
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
        
        this._testDataStore.UpdateContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAddTransactionFee(Commands.AddTransactionFeeForProductToContractCommand cmd)
    {
        var contract = this._testDataStore.GetContract(cmd.EstateId, cmd.ContractId);
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
        
        this._testDataStore.UpdateContract(cmd.EstateId, contract);
        return Result.Success();
    }

    private Result ExecuteAssignContractToMerchant(MerchantCommands.AssignContractToMerchantCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        // This is a simplified implementation
        // In real scenario, you'd track contract assignments
        return Result.Success();
    }

    private Result ExecuteRemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteAddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteRemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        return Result.Success();
    }

    private Result ExecuteMakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand cmd)
    {
        var merchant = this._testDataStore.GetMerchant(cmd.EstateId, cmd.MerchantId);
        if (merchant == null)
            return Result.Failure($"Merchant {cmd.MerchantId} not found");
        
        // Update the merchant's balance by adding the deposit amount
        merchant.Balance = (merchant.Balance ?? 0) + cmd.Amount;
        merchant.AvailableBalance = (merchant.AvailableBalance ?? 0) + cmd.Amount;
        
        this._testDataStore.UpdateMerchant(cmd.EstateId, merchant);
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

    private static List<ComparisonDateModel> GetMockComparisonDates()
    {
        var dates = new List<ComparisonDateModel>();
        var today = DateTime.Today;
        
        // Add primary options
        dates.Add(new ComparisonDateModel { Date = today.AddDays(-1), Description = "Yesterday" });
        dates.Add(new ComparisonDateModel { Date = today.AddDays(-7), Description = $"Last Week ({today.AddDays(-7).DayOfWeek})" });
        dates.Add(new ComparisonDateModel { Date = today.AddMonths(-1), Description = $"Last Month ({today.AddMonths(-1).Day}{GetDaySuffix(today.AddMonths(-1).Day)})" });
        
        // Add other dates, excluding those already covered (yesterday, last week's exact day, last month's exact day)
        var excludeDates = new HashSet<DateTime> 
        { 
            today.AddDays(-1).Date,  // Yesterday
            today.AddDays(-7).Date,  // Last week
            today.AddMonths(-1).Date // Last month
        };
        
        // Add dates from 2-30 days ago (excluding already covered dates)
        for (int i = 2; i <= 30; i++)
        {
            var date = today.AddDays(-i);
            if (!excludeDates.Contains(date))
            {
                dates.Add(new ComparisonDateModel 
                { 
                    Date = date, 
                    Description = $"{i} days ago ({date:MMM d})" 
                });
            }
        }
        
        return dates;
    }
    
    private static string GetDaySuffix(int day)
    {
        return day switch
        {
            1 or 21 or 31 => "st",
            2 or 22 => "nd",
            3 or 23 => "rd",
            _ => "th"
        };
    }

    private static TodaysSalesModel GetMockTodaysSales(DateTime comparisonDate)
    {
        // Generate different data based on how many days ago the comparison date is
        var daysAgo = (DateTime.Today - comparisonDate.Date).Days;
        
        // Use days ago to create variance in the data
        // The further back, the more the comparison differs
        var baseComparisonValue = 100000.00m;
        var baseTodayValue = 145000.00m;
        
        // Add some variance based on the comparison date
        var comparisonMultiplier = 1.0m + (daysAgo * 0.02m); // 2% increase per day back
        var comparisonValue = baseComparisonValue * comparisonMultiplier;
        var comparisonCount = (int)(400 + (daysAgo * 5)); // 5 more transactions per day back
        
        return new TodaysSalesModel
        {
            ComparisonSalesCount = comparisonCount,
            ComparisonSalesValue = comparisonValue,
            ComparisonAverageValue = comparisonValue / comparisonCount,
            TodaysSalesCount = 523,
            TodaysSalesValue = baseTodayValue,
            TodaysAverageValue = baseTodayValue / 523
        };
    }
    
    private static TodaysSalesModel GetMockTodaysFailedSales(DateTime comparisonDate)
    {
        // Generate different failed sales data based on comparison date
        var daysAgo = (DateTime.Today - comparisonDate.Date).Days;
        
        // Failed sales should ideally decrease over time (improving)
        var baseComparisonValue = 5000.00m;
        var baseTodayValue = 850.00m;
        
        // More failures in the past, fewer now (showing improvement)
        var comparisonMultiplier = 1.0m + (daysAgo * 0.05m); // 5% more failures per day back
        var comparisonValue = baseComparisonValue * comparisonMultiplier;
        var comparisonCount = (int)(25 + (daysAgo * 2)); // 2 more failed transactions per day back
        
        return new TodaysSalesModel
        {
            ComparisonSalesCount = comparisonCount,
            ComparisonSalesValue = comparisonValue,
            ComparisonAverageValue = comparisonValue / comparisonCount,
            TodaysSalesCount = 15,
            TodaysSalesValue = baseTodayValue,
            TodaysAverageValue = baseTodayValue / 15
        };
    }

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

    private List<MerchantTransactionSummaryModel> GetMockMerchantTransactionSummary(Queries.GetMerchantTransactionSummaryQuery query)
    {
        const double MinSuccessRate = 0.80; // 80% minimum success rate for test data
        const double SuccessRateRange = 0.15; // Range up to 95% success rate
        
        var merchants = this._testDataStore.GetMerchants(query.EstateId);
        var random = new Random(42); // Use seed for consistent data
        
        var summary = merchants.Select(merchant =>
        {
            var totalCount = random.Next(100, 1000);
            var successRate = MinSuccessRate + random.NextDouble() * SuccessRateRange;
            var successfulCount = (int)(totalCount * successRate);
            var failedCount = totalCount - successfulCount;
            var totalValue = (decimal)(random.NextDouble() * 50000 + 10000);
            
            return new MerchantTransactionSummaryModel
            {
                MerchantId = merchant.MerchantId,
                MerchantName = merchant.MerchantName ?? "Unknown Merchant",
                TotalTransactionCount = totalCount,
                TotalTransactionValue = Math.Round(totalValue, 2),
                AverageTransactionValue = Math.Round(totalValue / totalCount, 2),
                SuccessfulTransactionCount = successfulCount,
                FailedTransactionCount = failedCount
            };
        }).ToList();
        
        // Apply filters if specified
        if (query.MerchantId.HasValue)
        {
            summary = summary.Where(s => s.MerchantId == query.MerchantId.Value).ToList();
        }
        
        return summary;
    }

    private List<MerchantSettlementHistoryModel> GetMockMerchantSettlementHistory(Queries.GetMerchantSettlementHistoryQuery query)
    {
        var merchants = this._testDataStore.GetMerchants(query.EstateId);
        
        // Filter by merchant if specified
        if (query.MerchantId.HasValue)
        {
            merchants = merchants.Where(m => m.MerchantId == query.MerchantId.Value).ToList();
        }
        
        // Generate settlement history data
        var settlements = new List<MerchantSettlementHistoryModel>();
        var random = new Random(42); // Use seed for consistent data
        
        // Calculate the number of weeks in the date range
        var startDate = query.StartDate.Date;
        var endDate = query.EndDate.Date;
        var currentDate = startDate;
        
        // Generate weekly settlements (every Monday)
        int settlementCounter = 1;
        while (currentDate <= endDate)
        {
            // Find the next Monday or use current date if it's already Monday
            var daysUntilMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;
            if (daysUntilMonday > 0)
            {
                currentDate = currentDate.AddDays(daysUntilMonday);
            }
            
            if (currentDate > endDate)
                break;
            
            foreach (var merchant in merchants)
            {
                // Generate unique settlement reference
                var settlementRef = $"STL-{currentDate:yyyyMMdd}-{merchant.MerchantReference ?? merchant.MerchantId.ToString().Substring(0, 8)}-{settlementCounter:D4}";
                
                // Generate realistic transaction counts and amounts
                var transactionCount = random.Next(50, 500);
                var averageTransactionValue = (decimal)(random.NextDouble() * 50 + 10); // $10-$60 average
                var grossAmount = transactionCount * averageTransactionValue;
                var feePercentage = 0.015m + (decimal)(random.NextDouble() * 0.01); // 1.5%-2.5% fee
                var fees = grossAmount * feePercentage;
                var netAmount = grossAmount - fees;
                
                settlements.Add(new MerchantSettlementHistoryModel
                {
                    SettlementDate = currentDate,
                    SettlementReference = settlementRef,
                    TransactionCount = transactionCount,
                    NetAmountPaid = Math.Round(netAmount, 2)
                });
                
                settlementCounter++;
            }
            
            // Move to next week
            currentDate = currentDate.AddDays(7);
        }
        
        // Sort by date descending (most recent first)
        return settlements.OrderByDescending(s => s.SettlementDate).ToList();
    }

    private List<ProductPerformanceModel> GetMockProductPerformance(Queries.GetProductPerformanceQuery query)
    {
        var contracts = this._testDataStore.GetContracts(query.EstateId);
        
        // Calculate days in the date range to vary data based on period
        var daysInRange = (query.EndDate - query.StartDate).Days + 1;
        
        // Use date range as seed for consistent but varying data
        var seed = query.StartDate.GetHashCode() ^ query.EndDate.GetHashCode();
        var random = new Random(seed);
        
        // Collect all unique products from all contracts
        var productNames = contracts
            .SelectMany(c => c.Products ?? new List<ContractProductModel>())
            .Select(p => p.ProductName)
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct()
            .ToList();
        
        var products = new List<ProductPerformanceModel>();
        decimal totalValue = 0;
        
        // Generate mock transaction data for each product
        // Scale transaction counts based on the date range length
        var countMultiplier = Math.Max(1, daysInRange / 30.0); // Scale based on 30-day baseline
        
        foreach (var productName in productNames)
        {
            var baseTransactionCount = random.Next(50, 500);
            var transactionCount = (int)(baseTransactionCount * countMultiplier);
            var transactionValue = Math.Round((decimal)(random.NextDouble() * 30000 + 5000) * (decimal)countMultiplier, 2);
            totalValue += transactionValue;
            
            products.Add(new ProductPerformanceModel
            {
                ProductName = productName,
                TransactionCount = transactionCount,
                TransactionValue = transactionValue,
                PercentageContribution = 0 // Will be calculated after total is known
            });
        }
        
        // Calculate percentage contributions (ensure they sum to 100%)
        if (totalValue > 0)
        {
            decimal percentageSum = 0;
            for (int i = 0; i < products.Count; i++)
            {
                if (i == products.Count - 1)
                {
                    // Last item gets the remainder to ensure exact 100% (protected against negative values)
                    products[i].PercentageContribution = Math.Max(0, Math.Round(100 - percentageSum, 2));
                }
                else
                {
                    var percentage = Math.Round((products[i].TransactionValue / totalValue) * 100, 2);
                    products[i].PercentageContribution = percentage;
                    percentageSum += percentage;
                }
            }
        }
        
        // Sort by transaction value descending
        products = products.OrderByDescending(p => p.TransactionValue).ToList();
        
        return products;
    }

    private Result ExecuteAddOperatorToEstate(EstateCommands.AddOperatorToEstateCommand cmd)
    {
        var operator1 = this._testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operator1 == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        // In a real implementation, we would add the operator to the estate
        // For now, we just return success as the operator already exists in the data store
        return Result.Success();
    }

    private Result ExecuteRemoveOperatorFromEstate(EstateCommands.RemoveOperatorFromEstateCommand cmd)
    {
        var operator1 = this._testDataStore.GetOperator(cmd.EstateId, cmd.OperatorId);
        if (operator1 == null)
            return Result.Failure($"Operator {cmd.OperatorId} not found");
        
        // In a real implementation, we would remove the operator from the estate
        // For now, we just return success
        return Result.Success();
    }

    private List<SettlementSummaryModel> GetMockSettlementSummary(Queries.GetSettlementSummaryQuery query)
    {
        var merchants = this._testDataStore.GetMerchants(query.EstateId);
        
        // Use date range as seed for consistent but varying data
        var seed = query.StartDate.GetHashCode() ^ query.EndDate.GetHashCode();
        var random = new Random(seed);
        
        // Settlement statuses to distribute (mock data for testing)
        var statuses = new[] { "settled", "settled", "settled", "pending", "failed" };
        // Mock fee percentage - in production this would come from the settlement engine API
        const decimal DefaultFeePercentage = 0.025m; // 2.5% fee rate for mock data
        
        var summary = merchants.Select(merchant =>
        {
            var grossValue = (decimal)(random.NextDouble() * 100000 + 10000);
            var totalFees = Math.Round(grossValue * DefaultFeePercentage, 2);
            var netAmount = Math.Round(grossValue - totalFees, 2);
            var status = statuses[random.Next(statuses.Length)];
            
            return new SettlementSummaryModel
            {
                SettlementPeriodStart = query.StartDate,
                SettlementPeriodEnd = query.EndDate,
                MerchantId = merchant.MerchantId,
                MerchantName = merchant.MerchantName ?? "Unknown Merchant",
                GrossTransactionValue = Math.Round(grossValue, 2),
                TotalFees = totalFees,
                NetSettlementAmount = netAmount,
                SettlementStatus = status
            };
        }).ToList();
        
        // Apply filters if specified
        if (query.MerchantId.HasValue)
        {
            summary = summary.Where(s => s.MerchantId == query.MerchantId.Value).ToList();
        }
        
        if (!string.IsNullOrEmpty(query.Status))
        {
            summary = summary.Where(s => s.SettlementStatus != null && 
                                         s.SettlementStatus.Equals(query.Status, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        return summary;
    }

    private List<OperatorTransactionSummaryModel> GetMockOperatorTransactionSummary(Queries.GetOperatorTransactionSummaryQuery query)
    {
        var operators = this._testDataStore.GetOperators(query.EstateId);
        var summary = new List<OperatorTransactionSummaryModel>();
        var random = new Random(42); // Use seed for consistent test data
        
        const decimal DefaultSuccessRate = 0.92m; // 92% success rate for mock data
        const decimal DefaultFeePercentage = 0.015m; // 1.5% fee rate
        
        foreach (var op in operators)
        {
            // Apply operator filter if specified
            if (query.OperatorId.HasValue && op.OperatorId != query.OperatorId.Value)
                continue;
                
            var totalCount = random.Next(500, 5000);
            var successfulCount = (int)(totalCount * DefaultSuccessRate);
            var failedCount = totalCount - successfulCount;
            var totalValue = (decimal)(random.NextDouble() * 500000 + 50000);
            var totalFees = Math.Round(totalValue * DefaultFeePercentage, 2);
            
            summary.Add(new OperatorTransactionSummaryModel
            {
                OperatorId = op.OperatorId,
                OperatorName = op.Name,
                TotalTransactionCount = totalCount,
                TotalTransactionValue = Math.Round(totalValue, 2),
                AverageTransactionValue = Math.Round(totalValue / totalCount, 2),
                SuccessfulTransactionCount = successfulCount,
                FailedTransactionCount = failedCount,
                TotalFeesEarned = totalFees
            });
        }
        
        return summary;
    }

    private List<TransactionDetailModel> GetMockTransactionDetails(Queries.GetTransactionDetailQuery query)
    {
        var merchants = this._testDataStore.GetMerchants(query.EstateId);
        var operators = this._testDataStore.GetOperators(query.EstateId);
        var contracts = this._testDataStore.GetContracts(query.EstateId);
        
        // Get all products with their IDs from contracts
        var productList = contracts
            .SelectMany(c => c.Products ?? new List<ContractProductModel>())
            .Where(p => !string.IsNullOrEmpty(p.ProductName))
            .ToList();
        
        var random = new Random(42); // Use seed for consistent data
        var transactions = new List<TransactionDetailModel>();
        
        // Calculate days in date range
        var daysInRange = (query.EndDate - query.StartDate).Days + 1;
        var transactionsPerDay = 50;
        var totalTransactions = daysInRange * transactionsPerDay;
        
        for (int i = 0; i < totalTransactions; i++)
        {
            // Random date within range
            var randomDays = random.Next(0, daysInRange);
            var randomHours = random.Next(0, 24);
            var randomMinutes = random.Next(0, 60);
            var transactionDate = query.StartDate.AddDays(randomDays)
                .AddHours(randomHours)
                .AddMinutes(randomMinutes);
            
            // Random merchant, operator, and product
            var merchant = merchants[random.Next(merchants.Count)];
            var op = operators[random.Next(operators.Count)];
            var product = productList.Count > 0 ? productList[random.Next(productList.Count)] : null;
            
            // Random type and status (90% successful sales)
            var typeRoll = random.NextDouble();
            var statusRoll = random.NextDouble();
            
            string transactionType;
            string transactionStatus;
            
            if (typeRoll < 0.85)
            {
                transactionType = "Sale";
                transactionStatus = statusRoll < 0.95 ? "Successful" : "Failed";
            }
            else if (typeRoll < 0.95)
            {
                transactionType = "Refund";
                transactionStatus = "Successful";
            }
            else
            {
                transactionType = "Reversal";
                transactionStatus = "Reversed";
            }
            
            // Random amounts
            var grossAmount = Math.Round((decimal)(random.NextDouble() * 200 + 10), 2);
            var feePercentage = 0.015m; // 1.5%
            var feesCommission = Math.Round(grossAmount * feePercentage, 2);
            var netAmount = grossAmount - feesCommission;
            
            // Settlement reference (70% have one for successful transactions)
            string? settlementReference = null;
            if (transactionStatus == "Successful" && random.NextDouble() < 0.7)
            {
                settlementReference = $"STL-{transactionDate:yyyyMMdd}-{random.Next(1000, 9999)}";
            }
            
            transactions.Add(new TransactionDetailModel
            {
                TransactionId = Guid.NewGuid(),
                TransactionDateTime = transactionDate,
                MerchantName = merchant.MerchantName,
                MerchantId = merchant.MerchantId,
                OperatorName = op.Name,
                OperatorId = op.OperatorId,
                ProductName = product?.ProductName ?? "Default Product",
                ProductId = product?.ContractProductId ?? Guid.Empty,
                TransactionType = transactionType,
                TransactionStatus = transactionStatus,
                GrossAmount = grossAmount,
                FeesCommission = feesCommission,
                NetAmount = netAmount,
                SettlementReference = settlementReference
            });
        }
        
        // Apply filters
        IEnumerable<TransactionDetailModel> filteredTransactions = transactions;
        
        if (query.MerchantIds != null && query.MerchantIds.Any())
        {
            filteredTransactions = filteredTransactions.Where(t => query.MerchantIds.Contains(t.MerchantId));
        }
        
        if (query.OperatorIds != null && query.OperatorIds.Any())
        {
            filteredTransactions = filteredTransactions.Where(t => query.OperatorIds.Contains(t.OperatorId));
        }
        
        if (query.ProductIds != null && query.ProductIds.Any())
        {
            filteredTransactions = filteredTransactions.Where(t => query.ProductIds.Contains(t.ProductId));
        }
        
        // Sort by transaction date descending (most recent first) and materialize
        return filteredTransactions.OrderByDescending(t => t.TransactionDateTime).ToList();
    }
}
