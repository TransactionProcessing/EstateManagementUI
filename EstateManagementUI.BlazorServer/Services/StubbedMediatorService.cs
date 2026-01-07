using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Requests;
using MediatR;

namespace EstateManagementUI.BlazorServer.Services;

/// <summary>
/// Stubbed MediatR service that returns mock data without making remote calls
/// This allows quick development and testing without backend dependencies
/// </summary>
public class StubbedMediatorService : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return request switch
        {
            // Estate Queries
            Queries.GetEstateQuery => Task.FromResult((TResponse)(object)Result<EstateModel>.Success(GetMockEstate())),
            Queries.GetMerchantsQuery => Task.FromResult((TResponse)(object)Result<List<MerchantModel>>.Success(GetMockMerchants())),
            Queries.GetOperatorsQuery => Task.FromResult((TResponse)(object)Result<List<OperatorModel>>.Success(GetMockOperators())),
            Queries.GetContractsQuery => Task.FromResult((TResponse)(object)Result<List<ContractModel>>.Success(GetMockContracts())),
            Queries.GetOperatorQuery => Task.FromResult((TResponse)(object)Result<OperatorModel>.Success(GetMockOperator())),
            Queries.GetContractQuery => Task.FromResult((TResponse)(object)Result<ContractModel>.Success(GetMockContract())),
            Queries.GetMerchantQuery => Task.FromResult((TResponse)(object)Result<MerchantModel>.Success(GetMockMerchant())),
            
            // File Processing Queries
            Queries.GetFileImportLogsListQuery => Task.FromResult((TResponse)(object)Result<List<FileImportLogModel>>.Success(GetMockFileImportLogs())),
            Queries.GetFileImportLogQuery => Task.FromResult((TResponse)(object)Result<FileImportLogModel>.Success(GetMockFileImportLog())),
            Queries.GetFileDetailsQuery => Task.FromResult((TResponse)(object)Result<FileDetailsModel>.Success(GetMockFileDetails())),
            
            // Dashboard Queries
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
            
            // Commands - just return success
            Commands.AddMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.AddOperatorToMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.AddOperatorToEstateCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.AssignContractToMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateMerchantUserCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.CreateOperatorCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.MakeMerchantDepositCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.RemoveContractFromMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.RemoveOperatorFromMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.RemoveOperatorFromEstateCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.SetMerchantSettlementScheduleCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.SwapMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.UpdateMerchantAddressCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.UpdateMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.UpdateMerchantContactCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.UpdateOperatorCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.AddProductToContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            Commands.AddTransactionFeeForProductToContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            
            _ => throw new NotImplementedException($"Request type {request.GetType().Name} is not implemented in stubbed mediator")
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

    // Mock data methods
    private static EstateModel GetMockEstate() => new()
    {
        EstateId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        EstateName = "Test Estate",
        Reference = "Test Estate"
    };

    private static List<MerchantModel> GetMockMerchants() => new()
    {
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MerchantName = "Test Merchant 1",
            MerchantReference = "MERCH001",
            Balance = 10000.00m,
            AvailableBalance = 8500.00m,
            SettlementSchedule = "Immediate"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
            MerchantName = "Test Merchant 2",
            MerchantReference = "MERCH002",
            Balance = 5000.00m,
            AvailableBalance = 4200.00m,
            SettlementSchedule = "Weekly"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
            MerchantName = "Test Merchant 3",
            MerchantReference = "MERCH003",
            Balance = 15000.00m,
            AvailableBalance = 12000.00m,
            SettlementSchedule = "Monthly"
        }
    };

    private static MerchantModel GetMockMerchant() => new()
    {
        MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        MerchantName = "Test Merchant 1",
        MerchantReference = "MERCH001",
        Balance = 10000.00m,
        AvailableBalance = 8500.00m,
        SettlementSchedule = "Immediate",
        AddressLine1 = "123 Main Street",
        Town = "Test Town",
        Region = "Test Region",
        PostalCode = "12345",
        Country = "Test Country",
        ContactName = "John Smith",
        ContactEmailAddress = "john@testmerchant.com",
        ContactPhoneNumber = "555-1234"
    };

    private static List<OperatorModel> GetMockOperators() => new()
    {
        new OperatorModel
        {
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Safaricom",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        },
        new OperatorModel
        {
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
            Name = "Voucher",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        }
    };

    private static OperatorModel GetMockOperator() => new()
    {
        OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Name = "Safaricom",
        RequireCustomMerchantNumber = true,
        RequireCustomTerminalNumber = false
    };

    private static List<ContractModel> GetMockContracts() => new()
    {
        new ContractModel
        {
            ContractId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Description = "Standard Transaction Contract",
            OperatorName = "Safaricom",
            OperatorId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ProductName = "Mobile Topup",
                    DisplayText = "Mobile Airtime"
                }
            }
        },
        new ContractModel
        {
            ContractId = Guid.Parse("44444444-4444-4444-4444-444444444445"),
            Description = "Voucher Sales Contract",
            OperatorName = "Voucher",
            OperatorId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
            Products = new List<ContractProductModel>
            {
                new ContractProductModel
                {
                    ContractProductId = Guid.Parse("55555555-5555-5555-5555-555555555556"),
                    ProductName = "Voucher",
                    DisplayText = "Voucher Purchase"
                }
            }
        }
    };

    private static ContractModel GetMockContract() => new()
    {
        ContractId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        Description = "Standard Transaction Contract",
        OperatorName = "Safaricom",
        OperatorId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Products = new List<ContractProductModel>
        {
            new ContractProductModel
            {
                ContractProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ProductName = "Mobile Topup",
                DisplayText = "Mobile Airtime",
                ProductType = "Mobile Topup",
                Value = "Variable",
                NumberOfFees = 2,
                TransactionFees = new List<ContractProductTransactionFeeModel>
                {
                    new ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                        Description = "Merchant Commission",
                        CalculationType = "Fixed",
                        FeeType = "Merchant",
                        Value = 0.50m
                    },
                    new ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                        Description = "Service Provider Fee",
                        CalculationType = "Percentage",
                        FeeType = "Service Provider",
                        Value = 2.5m
                    }
                }
            },
            new ContractProductModel
            {
                ContractProductId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                ProductName = "Bill Payment",
                DisplayText = "Utility Bill Payment",
                ProductType = "Bill Payment",
                Value = "Variable",
                NumberOfFees = 1,
                TransactionFees = new List<ContractProductTransactionFeeModel>
                {
                    new ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        Description = "Transaction Fee",
                        CalculationType = "Fixed",
                        FeeType = "Merchant",
                        Value = 1.00m
                    }
                }
            }
        }
    };

    private static List<FileImportLogModel> GetMockFileImportLogs() => new()
    {
        new FileImportLogModel
        {
            FileImportLogId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            ImportLogDateTime = DateTime.Now.AddHours(-2),
            FileCount = 5,
            FileUploadedDateTime = DateTime.Now.AddHours(-3)
        },
        new FileImportLogModel
        {
            FileImportLogId = Guid.Parse("66666666-6666-6666-6666-666666666667"),
            ImportLogDateTime = DateTime.Now.AddDays(-1),
            FileCount = 3,
            FileUploadedDateTime = DateTime.Now.AddDays(-1).AddHours(-1)
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
        new ComparisonDateModel
        {
            Date = DateTime.Today,
            Description = "Today"
        },
        new ComparisonDateModel
        {
            Date = DateTime.Today.AddDays(-1),
            Description = "Yesterday"
        }
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
        TodaysSettlementValue = 130500.00m,
        ComparisonPendingSettlementCount = 12,
        ComparisonPendingSettlementValue = 12500.00m,
        TodaysPendingSettlementCount = 10,
        TodaysPendingSettlementValue = 14500.00m
    };

    private static List<TodaysSalesCountByHourModel> GetMockSalesCountByHour() => new()
    {
        new TodaysSalesCountByHourModel { Hour = 9, TodaysSalesCount = 45, ComparisonSalesCount = 38 },
        new TodaysSalesCountByHourModel { Hour = 10, TodaysSalesCount = 67, ComparisonSalesCount = 54 },
        new TodaysSalesCountByHourModel { Hour = 11, TodaysSalesCount = 89, ComparisonSalesCount = 76 },
        new TodaysSalesCountByHourModel { Hour = 12, TodaysSalesCount = 102, ComparisonSalesCount = 95 },
        new TodaysSalesCountByHourModel { Hour = 13, TodaysSalesCount = 78, ComparisonSalesCount = 82 },
        new TodaysSalesCountByHourModel { Hour = 14, TodaysSalesCount = 65, ComparisonSalesCount = 71 },
        new TodaysSalesCountByHourModel { Hour = 15, TodaysSalesCount = 77, ComparisonSalesCount = 34 }
    };

    private static List<TodaysSalesValueByHourModel> GetMockSalesValueByHour() => new()
    {
        new TodaysSalesValueByHourModel { Hour = 9, TodaysSalesValue = 12500, ComparisonSalesValue = 10500 },
        new TodaysSalesValueByHourModel { Hour = 10, TodaysSalesValue = 18500, ComparisonSalesValue = 15000 },
        new TodaysSalesValueByHourModel { Hour = 11, TodaysSalesValue = 24500, ComparisonSalesValue = 21000 },
        new TodaysSalesValueByHourModel { Hour = 12, TodaysSalesValue = 28000, ComparisonSalesValue = 26000 },
        new TodaysSalesValueByHourModel { Hour = 13, TodaysSalesValue = 21500, ComparisonSalesValue = 22500 },
        new TodaysSalesValueByHourModel { Hour = 14, TodaysSalesValue = 18000, ComparisonSalesValue = 19500 },
        new TodaysSalesValueByHourModel { Hour = 15, TodaysSalesValue = 21500, ComparisonSalesValue = 9500 }
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
        new TopBottomProductDataModel { ProductName = "Bill Payment", SalesValue = 87000.00m },
        new TopBottomProductDataModel { ProductName = "Voucher", SalesValue = 45000.00m }
    };

    private static List<TopBottomProductDataModel> GetMockBottomProducts() => new()
    {
        new TopBottomProductDataModel { ProductName = "Data Bundle", SalesValue = 5000.00m },
        new TopBottomProductDataModel { ProductName = "SMS Bundle", SalesValue = 3500.00m },
        new TopBottomProductDataModel { ProductName = "International Topup", SalesValue = 1200.00m }
    };

    private static List<TopBottomMerchantDataModel> GetMockTopMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 1", SalesValue = 85000.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 2", SalesValue = 67000.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 3", SalesValue = 45000.00m }
    };

    private static List<TopBottomMerchantDataModel> GetMockBottomMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 10", SalesValue = 1200.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 11", SalesValue = 850.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 12", SalesValue = 450.00m }
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
}
