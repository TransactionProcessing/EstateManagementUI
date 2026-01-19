using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;

namespace EstateManagementUI.BusinessLogic.Client;

public static class StubTestData {

    public static EstateModel GetMockEstate() => new()
    {
        EstateId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        EstateName = "Test Estate",
        Reference = "Test Estate"
    };

    public static List<MerchantModel> GetMockMerchants() => new()
    {
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MerchantName = "Test Merchant 1",
            MerchantReference = "MERCH001",
            Balance = 10000.00m,
            AvailableBalance = 8500.00m,
            SettlementSchedule = "Immediate",
            Region = "North Region",
            PostalCode = "12345"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
            MerchantName = "Test Merchant 2",
            MerchantReference = "MERCH002",
            Balance = 5000.00m,
            AvailableBalance = 4200.00m,
            SettlementSchedule = "Weekly",
            Region = "South Region",
            PostalCode = "67890"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
            MerchantName = "Test Merchant 3",
            MerchantReference = "MERCH003",
            Balance = 15000.00m,
            AvailableBalance = 12000.00m,
            SettlementSchedule = "Monthly",
            Region = "East Region",
            PostalCode = "54321"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222225"),
            MerchantName = "Test Merchant 4",
            MerchantReference = "MERCH004",
            Balance = 7500.00m,
            AvailableBalance = 6000.00m,
            SettlementSchedule = "Weekly",
            Region = "West Region",
            PostalCode = "11111"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222226"),
            MerchantName = "Test Merchant 5",
            MerchantReference = "MERCH005",
            Balance = 9000.00m,
            AvailableBalance = 7500.00m,
            SettlementSchedule = "Immediate",
            Region = "North Region",
            PostalCode = "22222"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222227"),
            MerchantName = "Test Merchant 6",
            MerchantReference = "MERCH006",
            Balance = 12000.00m,
            AvailableBalance = 10000.00m,
            SettlementSchedule = "Monthly",
            Region = "South Region",
            PostalCode = "33333"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222228"),
            MerchantName = "Test Merchant 7",
            MerchantReference = "MERCH007",
            Balance = 8500.00m,
            AvailableBalance = 7000.00m,
            SettlementSchedule = "Weekly",
            Region = "East Region",
            PostalCode = "44444"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222229"),
            MerchantName = "Test Merchant 8",
            MerchantReference = "MERCH008",
            Balance = 11000.00m,
            AvailableBalance = 9000.00m,
            SettlementSchedule = "Immediate",
            Region = "West Region",
            PostalCode = "55555"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222230"),
            MerchantName = "Test Merchant 9",
            MerchantReference = "MERCH009",
            Balance = 6500.00m,
            AvailableBalance = 5500.00m,
            SettlementSchedule = "Monthly",
            Region = "North Region",
            PostalCode = "66666"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222231"),
            MerchantName = "Test Merchant 10",
            MerchantReference = "MERCH010",
            Balance = 13000.00m,
            AvailableBalance = 11000.00m,
            SettlementSchedule = "Weekly",
            Region = "South Region",
            PostalCode = "77777"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222232"),
            MerchantName = "Test Merchant 11",
            MerchantReference = "MERCH011",
            Balance = 9500.00m,
            AvailableBalance = 8000.00m,
            SettlementSchedule = "Immediate",
            Region = "East Region",
            PostalCode = "88888"
        },
        new MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222233"),
            MerchantName = "Test Merchant 12",
            MerchantReference = "MERCH012",
            Balance = 14000.00m,
            AvailableBalance = 12500.00m,
            SettlementSchedule = "Monthly",
            Region = "West Region",
            PostalCode = "99999"
        }
    };

    public static List<RecentMerchantsModel> GetMockRecentMerchants() => new()
    {
        new RecentMerchantsModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Test Merchant 1",
            Reference = "MERCH001",
            CreatedDateTime = DateTime.Now
        },
        new RecentMerchantsModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
            Name = "Test Merchant 2",
            Reference = "MERCH002",
            CreatedDateTime = DateTime.Now.AddDays(-1)
        },
        new RecentMerchantsModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
            Name = "Test Merchant 3",
            Reference = "MERCH003",
            CreatedDateTime = DateTime.Now.AddDays(-5)
        }
    };

    public static MerchantModel GetMockMerchant() => new()
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

    public static List<OperatorModel> GetMockOperators() => new()
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

    public static OperatorModel GetMockOperator() => new()
    {
        OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Name = "Safaricom",
        RequireCustomMerchantNumber = true,
        RequireCustomTerminalNumber = false
    };

    public static List<ContractModel> GetMockContracts() => new()
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

    public static ContractModel GetMockContract() => new()
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

    public static List<FileImportLogModel> GetMockFileImportLogs() => new()
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

    public static FileImportLogModel GetMockFileImportLog() => new()
    {
        FileImportLogId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        ImportLogDateTime = DateTime.Now.AddHours(-2),
        FileCount = 5,
        FileUploadedDateTime = DateTime.Now.AddHours(-3)
    };

    public static FileDetailsModel GetMockFileDetails() => new()
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

    public static List<TodaysSalesCountByHourModel> GetMockSalesCountByHour() => new()
    {
        new TodaysSalesCountByHourModel { Hour = 9, TodaysSalesCount = 45, ComparisonSalesCount = 38 },
        new TodaysSalesCountByHourModel { Hour = 10, TodaysSalesCount = 67, ComparisonSalesCount = 54 },
        new TodaysSalesCountByHourModel { Hour = 11, TodaysSalesCount = 89, ComparisonSalesCount = 76 },
        new TodaysSalesCountByHourModel { Hour = 12, TodaysSalesCount = 102, ComparisonSalesCount = 95 },
        new TodaysSalesCountByHourModel { Hour = 13, TodaysSalesCount = 78, ComparisonSalesCount = 82 },
        new TodaysSalesCountByHourModel { Hour = 14, TodaysSalesCount = 65, ComparisonSalesCount = 71 },
        new TodaysSalesCountByHourModel { Hour = 15, TodaysSalesCount = 77, ComparisonSalesCount = 34 }
    };

    public static List<TodaysSalesValueByHourModel> GetMockSalesValueByHour() => new()
    {
        new TodaysSalesValueByHourModel { Hour = 9, TodaysSalesValue = 12500, ComparisonSalesValue = 10500 },
        new TodaysSalesValueByHourModel { Hour = 10, TodaysSalesValue = 18500, ComparisonSalesValue = 15000 },
        new TodaysSalesValueByHourModel { Hour = 11, TodaysSalesValue = 24500, ComparisonSalesValue = 21000 },
        new TodaysSalesValueByHourModel { Hour = 12, TodaysSalesValue = 28000, ComparisonSalesValue = 26000 },
        new TodaysSalesValueByHourModel { Hour = 13, TodaysSalesValue = 21500, ComparisonSalesValue = 22500 },
        new TodaysSalesValueByHourModel { Hour = 14, TodaysSalesValue = 18000, ComparisonSalesValue = 19500 },
        new TodaysSalesValueByHourModel { Hour = 15, TodaysSalesValue = 21500, ComparisonSalesValue = 9500 }
    };

    public static MerchantKpiModel GetMockMerchantKpi() => new()
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

    public static List<TransactionDetailModel> GetMockTransactionDetails(Queries.GetTransactionDetailQuery query)
    {
        var merchants = GetMockMerchants();
        var operators = GetMockOperators();
        var contracts = GetMockContracts();

        // Get all products with their IDs from contracts
        var productList = contracts
            .SelectMany(c => c.Products ?? new List<ContractProductModel>())
            .Where(p => !string.IsNullOrEmpty(p.ProductName))
            .ToList();

        var transactionTypes = new[] { "Sale", "Refund", "Reversal" };
        var transactionStatuses = new[] { "Successful", "Failed", "Reversed" };

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
            var product = productList[random.Next(productList.Count)];

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

            // Response code
            string? responseCode = transactionStatus == "Successful" ? "0000" : $"{random.Next(1001, 9999)}";

            // Settlement reference and datetime (70% have one for successful transactions)
            string? settlementReference = null;
            DateTime? settlementDateTime = null;
            if (transactionStatus == "Successful" && random.NextDouble() < 0.7)
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