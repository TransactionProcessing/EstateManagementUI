using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using NLog.Config;

namespace EstateManagementUI.BusinessLogic.Client;

public static class StubTestData {

    public static Guid TestEstateId = Guid.Parse("F2BB633C-C922-4F01-B516-19545FE80616");
    public static Guid TestMerchant1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static Guid TestMerchant2Id = Guid.Parse("22222222-2222-2222-2222-222222222223");
    public static Guid TestMerchant3Id = Guid.Parse("22222222-2222-2222-2222-222222222224");
    public static Guid TestMerchant4Id = Guid.Parse("22222222-2222-2222-2222-222222222225");
    public static Guid TestMerchant5Id = Guid.Parse("22222222-2222-2222-2222-222222222226");
    public static Guid TestMerchant6Id = Guid.Parse("22222222-2222-2222-2222-222222222227");
    public static Guid TestMerchant7Id = Guid.Parse("22222222-2222-2222-2222-222222222228");
    public static Guid TestMerchant8Id = Guid.Parse("22222222-2222-2222-2222-222222222229");
    public static Guid TestMerchant9Id = Guid.Parse("22222222-2222-2222-2222-222222222230");
    public static Guid TestMerchant10Id = Guid.Parse("22222222-2222-2222-2222-222222222231");
    public static Guid TestMerchant11Id = Guid.Parse("22222222-2222-2222-2222-222222222232");
    public static Guid TestMerchant12Id = Guid.Parse("22222222-2222-2222-2222-222222222233");

    public static EstateModels.EstateModel GetMockEstate() => new()
    {
        EstateId = TestEstateId,
        EstateName = "Test Estate",
        Reference = "Test Estate"
    };

    public static String SettlementScheduleImmediate = "Immediate";
    public static String SettlementScheduleWeekly = "Weekly";
    public static String SettlementScheduleMonthly = "Monthly";

    public static List<MerchantModels.MerchantModel> GetMockMerchants() => new()
    {
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant1Id,
            MerchantName = "Test Merchant 1",
            MerchantReference = "MERCH001",
            Balance = 10000.00m,
            AvailableBalance = 8500.00m,
            SettlementSchedule = SettlementScheduleImmediate,
            Region = "North Region",
            PostalCode = "12345"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant2Id,
            MerchantName = "Test Merchant 2",
            MerchantReference = "MERCH002",
            Balance = 5000.00m,
            AvailableBalance = 4200.00m,
            SettlementSchedule = SettlementScheduleWeekly,
            Region = "South Region",
            PostalCode = "67890"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant3Id,
            MerchantName = "Test Merchant 3",
            MerchantReference = "MERCH003",
            Balance = 15000.00m,
            AvailableBalance = 12000.00m,
            SettlementSchedule = SettlementScheduleMonthly,
            Region = "East Region",
            PostalCode = "54321"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant4Id,
            MerchantName = "Test Merchant 4",
            MerchantReference = "MERCH004",
            Balance = 7500.00m,
            AvailableBalance = 6000.00m,
            SettlementSchedule = SettlementScheduleWeekly,
            Region = "West Region",
            PostalCode = "11111"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant5Id,
            MerchantName = "Test Merchant 5",
            MerchantReference = "MERCH005",
            Balance = 9000.00m,
            AvailableBalance = 7500.00m,
            SettlementSchedule = SettlementScheduleImmediate,
            Region = "North Region",
            PostalCode = "22222"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant6Id,
            MerchantName = "Test Merchant 6",
            MerchantReference = "MERCH006",
            Balance = 12000.00m,
            AvailableBalance = 10000.00m,
            SettlementSchedule = SettlementScheduleMonthly,
            Region = "South Region",
            PostalCode = "33333"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant7Id,
            MerchantName = "Test Merchant 7",
            MerchantReference = "MERCH007",
            Balance = 8500.00m,
            AvailableBalance = 7000.00m,
            SettlementSchedule = SettlementScheduleWeekly,
            Region = "East Region",
            PostalCode = "44444"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant8Id,
            MerchantName = "Test Merchant 8",
            MerchantReference = "MERCH008",
            Balance = 11000.00m,
            AvailableBalance = 9000.00m,
            SettlementSchedule = SettlementScheduleImmediate,
            Region = "West Region",
            PostalCode = "55555"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant9Id,
            MerchantName = "Test Merchant 9",
            MerchantReference = "MERCH009",
            Balance = 6500.00m,
            AvailableBalance = 5500.00m,
            SettlementSchedule = SettlementScheduleMonthly,
            Region = "North Region",
            PostalCode = "66666"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant10Id,
            MerchantName = "Test Merchant 10",
            MerchantReference = "MERCH010",
            Balance = 13000.00m,
            AvailableBalance = 11000.00m,
            SettlementSchedule = SettlementScheduleWeekly,
            Region = "South Region",
            PostalCode = "77777"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant11Id,
            MerchantName = "Test Merchant 11",
            MerchantReference = "MERCH011",
            Balance = 9500.00m,
            AvailableBalance = 8000.00m,
            SettlementSchedule = SettlementScheduleImmediate,
            Region = "East Region",
            PostalCode = "88888"
        },
        new MerchantModels.MerchantModel
        {
            MerchantId = TestMerchant12Id,
            MerchantName = "Test Merchant 12",
            MerchantReference = "MERCH012",
            Balance = 14000.00m,
            AvailableBalance = 12500.00m,
            SettlementSchedule = SettlementScheduleMonthly,
            Region = "West Region",
            PostalCode = "99999"
        }
    };

    public static List<MerchantModels.RecentMerchantsModel> GetMockRecentMerchants() => new()
    {
        new MerchantModels.RecentMerchantsModel
        {
            MerchantId = TestMerchant1Id,
            Name = "Test Merchant 1",
            Reference = "MERCH001",
            CreatedDateTime = DateTime.Now
        },
        new MerchantModels.RecentMerchantsModel
        {
            MerchantId = TestMerchant2Id,
            Name = "Test Merchant 2",
            Reference = "MERCH002",
            CreatedDateTime = DateTime.Now.AddDays(-1)
        },
        new MerchantModels.RecentMerchantsModel
        {
            MerchantId = TestMerchant3Id,
            Name = "Test Merchant 3",
            Reference = "MERCH003",
            CreatedDateTime = DateTime.Now.AddDays(-5)
        }
    };

    public static MerchantModels.MerchantModel GetMockMerchant() => new()
    {
        MerchantId = TestMerchant1Id,
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

    public static Guid SafaricomOperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static Guid VoucherOperatorId = Guid.Parse("33333333-3333-3333-3333-333333333334");

    public static List<OperatorModels.OperatorModel> GetMockOperators() => new()
    {
        new OperatorModels.OperatorModel
        {
            OperatorId = SafaricomOperatorId,
            Name = "Safaricom",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        },
        new OperatorModels.OperatorModel
        {
            OperatorId = VoucherOperatorId,
            Name = "Voucher",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        }
    };

    public static OperatorModels.OperatorModel GetMockOperator() => new()
    {
        OperatorId = SafaricomOperatorId,
        Name = "Safaricom",
        RequireCustomMerchantNumber = true,
        RequireCustomTerminalNumber = false
    };

    public static Guid SafaricomContractId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static Guid MobileTopupProductId = Guid.Parse("55555555-5555-5555-5555-555555555555");

    public static Guid VoucherContractId = Guid.Parse("44444444-4444-4444-4444-444444444445");
    public static Guid VoucherProductId = Guid.Parse("55555555-5555-5555-5555-555555555556");
    public static Guid BillPaymentProductId = Guid.Parse("88888888-8888-8888-8888-888888888888");
    
    public static List<ContractModels.ContractModel> GetMockContracts() => new()
    {
        new ContractModels.ContractModel
        {
            ContractId = SafaricomContractId,
            Description = "Standard Transaction Contract",
            OperatorName = "Safaricom",
            OperatorId = SafaricomOperatorId,
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
                {
                    ContractProductId = MobileTopupProductId,
                    ProductName = "Mobile Topup",
                    DisplayText = "Mobile Airtime"
                }
            }
        },
        new ContractModels.ContractModel
        {
            ContractId = VoucherContractId,
            Description = "Voucher Sales Contract",
            OperatorName = "Voucher",
            OperatorId = VoucherOperatorId,
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
                {
                    ContractProductId = VoucherProductId,
                    ProductName = "Voucher",
                    DisplayText = "Voucher Purchase"
                }
            }
        }
    };

    public static Guid MerchantCommissionFeeId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static Guid ServiceProviderFeeId = Guid.Parse("77777777-7777-7777-7777-777777777777");

    public static Guid TransactionFeeId = Guid.Parse("99999999-9999-9999-9999-999999999999");

    public static ContractModels.ContractModel GetMockContract() => new()
    {
        ContractId = SafaricomContractId,
        Description = "Standard Transaction Contract",
        OperatorName = "Safaricom",
        OperatorId = SafaricomOperatorId,
        Products = new List<ContractModels.ContractProductModel>
        {
            new ContractModels.ContractProductModel
            {
                ContractProductId = MobileTopupProductId,
                ProductName = "Mobile Topup",
                DisplayText = "Mobile Airtime",
                ProductType = "Mobile Topup",
                Value = "Variable",
                NumberOfFees = 2,
                TransactionFees = new List<ContractModels.ContractProductTransactionFeeModel>
                {
                    new ContractModels.ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = MerchantCommissionFeeId,
                        Description = "Merchant Commission",
                        CalculationType = 0,
                        FeeType = 0,
                        Value = 0.50m
                    },
                    new ContractModels.ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = ServiceProviderFeeId,
                        Description = "Service Provider Fee",
                        CalculationType = 1,
                        FeeType = 1,
                        Value = 2.5m
                    }
                }
            },
            new ContractModels.ContractProductModel
            {
                ContractProductId = BillPaymentProductId,
                ProductName = "Bill Payment",
                DisplayText = "Utility Bill Payment",
                ProductType = "Bill Payment",
                Value = "Variable",
                NumberOfFees = 1,
                TransactionFees = new List<ContractModels.ContractProductTransactionFeeModel>
                {
                    new ContractModels.ContractProductTransactionFeeModel
                    {
                        TransactionFeeId = TransactionFeeId,
                        Description = "Transaction Fee",
                        CalculationType = 0,
                        FeeType = 0,
                        Value = 1.00m
                    }
                }
            }
        }
    };

    public static Guid FileImportLogId1 = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static Guid FileImportLogId2 = Guid.Parse("66666666-6666-6666-6666-666666666667");
    public static Guid FileId = Guid.Parse("77777777-7777-7777-7777-777777777777");
    public static Guid UserId = Guid.Parse("88888888-8888-8888-8888-888888888888");

    public static List<FileImportLogModel> GetMockFileImportLogs() => new()
    {
        new FileImportLogModel
        {
            FileImportLogId = FileImportLogId1,
            ImportLogDateTime = DateTime.Now.AddHours(-2),
            FileCount = 5,
            FileUploadedDateTime = DateTime.Now.AddHours(-3)
        },
        new FileImportLogModel
        {
            FileImportLogId = FileImportLogId2,
            ImportLogDateTime = DateTime.Now.AddDays(-1),
            FileCount = 3,
            FileUploadedDateTime = DateTime.Now.AddDays(-1).AddHours(-1)
        }
    };

    public static FileImportLogModel GetMockFileImportLog() => new()
    {
        FileImportLogId = FileImportLogId1,
        ImportLogDateTime = DateTime.Now.AddHours(-2),
        FileCount = 5,
        FileUploadedDateTime = DateTime.Now.AddHours(-3)
    };

    public static FileDetailsModel GetMockFileDetails() => new()
    {
        FileId = FileId,
        FileLocation = "/files/transactions.csv",
        FileProfileName = "SafaricomTopup",
        MerchantName = "Test Merchant 1",
        UserId = UserId,
        FileUploadedDateTime = DateTime.Now.AddHours(-3),
        ProcessingCompletedDateTime = DateTime.Now.AddHours(-2),
        TotalLines = 100,
        SuccessfullyProcessedLines = 95,
        FailedLines = 5,
        IgnoredLines = 0
    };

    public static List<ComparisonDateModel> GetMockComparisonDates() => new()
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

    public static TodaysSalesModel GetMockTodaysSales() => new()
    {
        ComparisonSalesCount = 450,
        ComparisonSalesValue = 125000.00m,
        ComparisonAverageValue = 277.78m,
        TodaysSalesCount = 523,
        TodaysSalesValue = 145000.00m,
        TodaysAverageValue = 277.24m
    };

    public static TodaysSettlementModel GetMockTodaysSettlement() => new()
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

    public static List<TransactionModels.TodaysSalesByHourModel> GetMockSalesCountByHour() => new()
    {
        new TransactionModels.TodaysSalesByHourModel { Hour = 9, TodaysSalesCount = 45, ComparisonSalesCount = 38, TodaysSalesValue = 12500, ComparisonSalesValue = 10500 },
        new TransactionModels.TodaysSalesByHourModel { Hour = 10, TodaysSalesCount = 67, ComparisonSalesCount = 54, TodaysSalesValue = 18500, ComparisonSalesValue = 15000 },
        new TransactionModels.TodaysSalesByHourModel { Hour = 11, TodaysSalesCount = 89, ComparisonSalesCount = 76, TodaysSalesValue = 24500, ComparisonSalesValue = 21000 },
        new TransactionModels.TodaysSalesByHourModel { Hour = 12, TodaysSalesCount = 102, ComparisonSalesCount = 95, TodaysSalesValue = 28000, ComparisonSalesValue = 26000 },
        new TransactionModels.TodaysSalesByHourModel { Hour = 13, TodaysSalesCount = 78, ComparisonSalesCount = 82, TodaysSalesValue = 21500, ComparisonSalesValue = 22500 },
        new TransactionModels.TodaysSalesByHourModel { Hour = 14, TodaysSalesCount = 65, ComparisonSalesCount = 71 , TodaysSalesValue = 18000, ComparisonSalesValue = 19500},
        new TransactionModels.TodaysSalesByHourModel { Hour = 15, TodaysSalesCount = 77, ComparisonSalesCount = 34 , TodaysSalesValue = 21500, ComparisonSalesValue = 9500}
    };


    public static MerchantModels.MerchantKpiModel GetMockMerchantKpi() => new()
    {
        MerchantsWithNoSaleInLast7Days = 5,
        MerchantsWithNoSaleToday = 12,
        MerchantsWithSaleInLastHour = 45
    };

    public static List<TopBottomProductDataModel> GetMockTopProducts() => new()
    {
        new TopBottomProductDataModel { ProductName = "Mobile Topup", SalesValue = 125000.00m },
        new TopBottomProductDataModel { ProductName = "Bill Payment", SalesValue = 87000.00m },
        new TopBottomProductDataModel { ProductName = "Voucher", SalesValue = 45000.00m }
    };

    public static List<TopBottomProductDataModel> GetMockBottomProducts() => new()
    {
        new TopBottomProductDataModel { ProductName = "Data Bundle", SalesValue = 5000.00m },
        new TopBottomProductDataModel { ProductName = "SMS Bundle", SalesValue = 3500.00m },
        new TopBottomProductDataModel { ProductName = "International Topup", SalesValue = 1200.00m }
    };

    public static List<TopBottomMerchantDataModel> GetMockTopMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 1", SalesValue = 85000.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 2", SalesValue = 67000.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 3", SalesValue = 45000.00m }
    };

    public static List<TopBottomMerchantDataModel> GetMockBottomMerchants() => new()
    {
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 10", SalesValue = 1200.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 11", SalesValue = 850.00m },
        new TopBottomMerchantDataModel { MerchantName = "Test Merchant 12", SalesValue = 450.00m }
    };

    public static List<TopBottomOperatorDataModel> GetMockTopOperators() => new()
    {
        new TopBottomOperatorDataModel { OperatorName = "Safaricom", SalesValue = 195000.00m },
        new TopBottomOperatorDataModel { OperatorName = "Voucher", SalesValue = 67000.00m }
    };

    public static List<TopBottomOperatorDataModel> GetMockBottomOperators() => new()
    {
        new TopBottomOperatorDataModel { OperatorName = "PataPawa PostPay", SalesValue = 12000.00m }
    };

    public static LastSettlementModel GetMockLastSettlement() => new()
    {
        SettlementDate = DateTime.Today.AddDays(-1),
        FeesValue = 1250.00m,
        SalesCount = 450,
        SalesValue = 125000.00m,
        SettlementValue = 123750.00m
    };

    // Generates demo settlement history data specifically for Test Merchant 1
    private static List<TransactionDetailModel> GetDemoMerchantTransactions()
    {
        var operators = GetMockOperators();
        var contracts = GetMockContracts();
        
        var productList = contracts
            .SelectMany(c => c.Products ?? new List<ContractModels.ContractProductModel>())
            .Where(p => !string.IsNullOrEmpty(p.ProductName))
            .ToList();

        var demoTransactions = new List<TransactionDetailModel>();
        var baseDate = DateTime.Today.AddDays(-7); // Last 7 days

        // Day 1: Multiple successful transactions
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddHours(9).AddMinutes(15),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 50.00m,
            FeesCommission = 0.75m,
            NetAmount = 49.25m,
            SettlementReference = $"STL-{baseDate:yyyyMMdd}-1001",
            ResponseCode = "0000",
            SettlementDateTime = baseDate.AddDays(2)
        });

        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddHours(11).AddMinutes(32),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 100.00m,
            FeesCommission = 1.50m,
            NetAmount = 98.50m,
            SettlementReference = $"STL-{baseDate:yyyyMMdd}-1002",
            ResponseCode = "0000",
            SettlementDateTime = baseDate.AddDays(2)
        });

        // Day 2: Mix of successful and failed
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(1).AddHours(10).AddMinutes(20),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[1].Name,
            OperatorId = operators[1].OperatorId,
            ProductName = "Bill Payment",
            ProductId = productList[1].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = FailedTransactionStatus,
            GrossAmount = 75.00m,
            FeesCommission = 1.13m,
            NetAmount = 73.87m,
            SettlementReference = null,
            ResponseCode = "1051",
            SettlementDateTime = null
        });

        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(1).AddHours(14).AddMinutes(45),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 25.00m,
            FeesCommission = 0.38m,
            NetAmount = 24.62m,
            SettlementReference = null, // Not yet settled
            ResponseCode = "0000",
            SettlementDateTime = null
        });

        // Day 3: Successful with settlement
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(2).AddHours(8).AddMinutes(10),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[2].Name,
            OperatorId = operators[2].OperatorId,
            ProductName = "Voucher Purchase",
            ProductId = productList[2].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 200.00m,
            FeesCommission = 3.00m,
            NetAmount = 197.00m,
            SettlementReference = $"STL-{baseDate.AddDays(2):yyyyMMdd}-2001",
            ResponseCode = "0000",
            SettlementDateTime = baseDate.AddDays(4)
        });

        // Day 4: Reversal
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(3).AddHours(13).AddMinutes(25),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = ReversalTransactionType,
            TransactionStatus = ReversedTransactionStatus,
            GrossAmount = 50.00m,
            FeesCommission = 0.75m,
            NetAmount = 49.25m,
            SettlementReference = $"STL-{baseDate.AddDays(3):yyyyMMdd}-3001",
            ResponseCode = "9999",
            SettlementDateTime = baseDate.AddDays(5)
        });

        // Day 5: Multiple successful transactions
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(4).AddHours(9).AddMinutes(30),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[1].Name,
            OperatorId = operators[1].OperatorId,
            ProductName = "Bill Payment",
            ProductId = productList[1].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 150.00m,
            FeesCommission = 2.25m,
            NetAmount = 147.75m,
            SettlementReference = $"STL-{baseDate.AddDays(4):yyyyMMdd}-4001",
            ResponseCode = "0000",
            SettlementDateTime = baseDate.AddDays(6)
        });

        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(4).AddHours(15).AddMinutes(50),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 30.00m,
            FeesCommission = 0.45m,
            NetAmount = 29.55m,
            SettlementReference = $"STL-{baseDate.AddDays(4):yyyyMMdd}-4002",
            ResponseCode = "0000",
            SettlementDateTime = baseDate.AddDays(6)
        });

        // Day 6: Recent transactions (not yet settled)
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(5).AddHours(10).AddMinutes(15),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[2].Name,
            OperatorId = operators[2].OperatorId,
            ProductName = "Voucher Purchase",
            ProductId = productList[2].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 120.00m,
            FeesCommission = 1.80m,
            NetAmount = 118.20m,
            SettlementReference = null,
            ResponseCode = "0000",
            SettlementDateTime = null
        });

        // Day 7: Today's transactions (pending)
        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(6).AddHours(8).AddMinutes(45),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[0].Name,
            OperatorId = operators[0].OperatorId,
            ProductName = "Mobile Topup",
            ProductId = productList[0].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = SuccessfulTransactionStatus,
            GrossAmount = 40.00m,
            FeesCommission = 0.60m,
            NetAmount = 39.40m,
            SettlementReference = null,
            ResponseCode = "0000",
            SettlementDateTime = null
        });

        demoTransactions.Add(new TransactionDetailModel
        {
            TransactionId = Guid.NewGuid(),
            TransactionDateTime = baseDate.AddDays(6).AddHours(12).AddMinutes(20),
            MerchantName = "Test Merchant 1",
            MerchantId = TestMerchant1Id,
            OperatorName = operators[1].Name,
            OperatorId = operators[1].OperatorId,
            ProductName = "Bill Payment",
            ProductId = productList[1].ContractProductId,
            TransactionType = SaleTransactionType,
            TransactionStatus = FailedTransactionStatus,
            GrossAmount = 85.00m,
            FeesCommission = 1.28m,
            NetAmount = 83.72m,
            SettlementReference = null,
            ResponseCode = "2043",
            SettlementDateTime = null
        });

        return demoTransactions;
    }

    public const String ReversedTransactionStatus = "Reversed";
    public const String FailedTransactionStatus = "Failed";
    public const String SuccessfulTransactionStatus = "Successful";

    public const String ReversalTransactionType = "Reversal";
    public const String RefundTransactionType = "Refund";
    public const String SaleTransactionType = "Sale";

    public static List<TransactionDetailModel> GetMockTransactionDetails(TransactionQueries.GetTransactionDetailQuery query)
    {
        var merchants = GetMockMerchants();
        var operators = GetMockOperators();
        var contracts = GetMockContracts();

        // Get all products with their IDs from contracts
        var productList = contracts
            .SelectMany(c => c.Products ?? new List<ContractModels.ContractProductModel>())
            .Where(p => !string.IsNullOrEmpty(p.ProductName))
            .ToList();

        var transactionTypes = new[] { SaleTransactionType, RefundTransactionType, ReversalTransactionType };
        var transactionStatuses = new[] { SuccessfulTransactionStatus, FailedTransactionStatus, ReversedTransactionStatus };

        var random = new Random(42); // Use seed for consistent data
        var transactions = new List<TransactionDetailModel>();

        // Add demo transactions for Test Merchant 1 if they're in the date range
        var demoTransactions = GetDemoMerchantTransactions();
        var demoInRange = demoTransactions.Where(t => 
            t.TransactionDateTime >= query.StartDate && 
            t.TransactionDateTime <= query.EndDate).ToList();
        transactions.AddRange(demoInRange);

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
            var product = productList[random.Next(productList.Count)];

            // Random type and status (90% successful sales)
            var typeRoll = random.NextDouble();
            var statusRoll = random.NextDouble();

            string transactionType;
            string transactionStatus;

            if (typeRoll < 0.85)
            {
                transactionType = SaleTransactionType;
                transactionStatus = statusRoll < 0.95 ? SuccessfulTransactionStatus : FailedTransactionStatus;
            }
            else if (typeRoll < 0.95)
            {
                transactionType = RefundTransactionType;
                transactionStatus = SuccessfulTransactionStatus;
            }
            else
            {
                transactionType = ReversalTransactionType;
                transactionStatus = ReversedTransactionStatus;
            }

            // Random amounts
            var grossAmount = Math.Round((decimal)(random.NextDouble() * 200 + 10), 2);
            var feePercentage = 0.015m; // 1.5%
            var feesCommission = Math.Round(grossAmount * feePercentage, 2);
            var netAmount = grossAmount - feesCommission;

            // Response code based on transaction status
            string? responseCode = transactionStatus switch
            {
                SuccessfulTransactionStatus => "0000",
                FailedTransactionStatus => $"{random.Next(1001, 9999)}",
                ReversedTransactionStatus => "9999", // Specific code for reversals
                _ => $"{random.Next(1001, 9999)}"
            };

            // Settlement reference and datetime (70% have one for successful transactions)
            string? settlementReference = null;
            DateTime? settlementDateTime = null;
            if (transactionStatus == $"{SuccessfulTransactionStatus} " && random.NextDouble() < 0.7)
            {
                settlementReference = $"STL-{transactionDate:yyyyMMdd}-{random.Next(1000, 9999)}";
                // Settlement usually occurs 1-3 days after transaction
                settlementDateTime = transactionDate.AddDays(random.Next(1, 4));
            }

            transactions.Add(new TransactionDetailModel
            {
                TransactionId = Guid.NewGuid(),
                TransactionDateTime = transactionDate,
                MerchantName = merchant.MerchantName,
                MerchantId = merchant.MerchantId,
                OperatorName = op.Name,
                OperatorId = op.OperatorId,
                ProductName = product.ProductName,
                ProductId = product.ContractProductId,
                TransactionType = transactionType,
                TransactionStatus = transactionStatus,
                GrossAmount = grossAmount,
                FeesCommission = feesCommission,
                NetAmount = netAmount,
                SettlementReference = settlementReference,
                ResponseCode = responseCode,
                SettlementDateTime = settlementDateTime
            });
        }

        // Apply filters
        IEnumerable<TransactionDetailModel> filteredTransactions = transactions;

        // Sort by transaction date descending (most recent first) and materialize
        return filteredTransactions.OrderByDescending(t => t.TransactionDateTime).ToList();
    }
}