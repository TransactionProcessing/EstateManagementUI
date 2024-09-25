using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.DataTransferObjects.Responses;
using FileLineProcessingResult = FileProcessor.DataTransferObjects.Responses.FileLineProcessingResult;
using SettlementSchedule = EstateManagement.DataTransferObjects.Responses.Merchant.SettlementSchedule;

namespace EstateManagementUI.Testing
{
    public static class TestData {
        
        public static DateTime ComparisonDate = new DateTime(2024,1,1);
        public static String AccessToken = "token1";
        public static Guid EstateId = Guid.Parse("BD6F1ED7-6290-4285-A200-E4F8D25F4CBE");
        public static Queries.GetEstateQuery GetEstateQuery => new(AccessToken, EstateId);
        public static Queries.GetMerchantsQuery GetMerchantsQuery => new(AccessToken, EstateId);
        public static Queries.GetOperatorsQuery GetOperatorsQuery => new(AccessToken, EstateId);
        public static Queries.GetOperatorQuery GetOperatorQuery => new(AccessToken, EstateId, Operator1Id);
        public static Queries.GetContractsQuery GetContractsQuery => new(AccessToken, EstateId);
        public static Queries.GetContractQuery GetContractQuery => new(AccessToken, EstateId, Contract1Id);

        public static Queries.GetFileImportLogsListQuery GetFileImportLogsListQuery =>
            new Queries.GetFileImportLogsListQuery(AccessToken, EstateId, Merchant1Id, FileImportLogQueryStartDate,
                FileImportLogQueryEndDate);

        public static Queries.GetFileImportLogQuery GetFileImportLogQuery =>
            new Queries.GetFileImportLogQuery(AccessToken, EstateId, Merchant1Id, FileImportLogId);

        public static Queries.GetFileDetailsQuery GetFileDetailsQuery =>
            new(AccessToken, EstateId, FileImportLogFile1.FileId);

        public static Queries.GetComparisonDatesQuery GetComparisonDatesQuery => new(AccessToken, EstateId);
        public static Queries.GetTodaysSalesQuery GetTodaysSalesQuery => new(AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSettlementQuery GetTodaysSettlementQuery => new(AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSalesCountByHourQuery GetTodaysSalesCountByHourQuery => new(AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSalesValueByHourQuery GetTodaysSalesValueByHourQuery => new(AccessToken, EstateId, DateTime.Now);

        public static Queries.GetMerchantKpiQuery GetMerchantKpiQuery => new (AccessToken, EstateId);

        public static Queries.GetTodaysFailedSalesQuery GetTodaysFailedSalesQuery =>
            new(AccessToken, EstateId, "1009", DateTime.Now);

        public static Queries.GetTopProductDataQuery GetTopProductDataQuery =>
            new Queries.GetTopProductDataQuery(AccessToken, EstateId, 1);

        public static Queries.GetBottomProductDataQuery GetBottomProductDataQuery => new(AccessToken, EstateId, 1);
        public static Queries.GetTopMerchantDataQuery GetTopMerchantDataQuery => new(AccessToken, EstateId, 1);
        public static Queries.GetBottomMerchantDataQuery GetBottomMerchantDataQuery => new(AccessToken, EstateId, 1);
        public static Queries.GetTopOperatorDataQuery GetTopOperatorDataQuery => new(AccessToken, EstateId, 1);
        public static Queries.GetBottomOperatorDataQuery GetBottomOperatorDataQuery => new(AccessToken, EstateId, 1);

        public static Queries.GetLastSettlementQuery GetLastSettlementQuery => new(AccessToken, EstateId);

        public static Commands.AddNewMerchantCommand AddNewMerchantCommand => new(AccessToken, EstateId, CreateMerchantModel(BusinessLogic.Models.SettlementSchedule.Immediate));

        public static CreateMerchantResponse CreateMerchantResponse =>
            new() { EstateId = EstateId, MerchantId = Merchant1Id };

        public static CreateMerchantModel CreateMerchantModel(BusinessLogic.Models.SettlementSchedule settlementSchedule) =>
            new CreateMerchantModel
            {
                MerchantName = Merchant1Name,
                SettlementSchedule = settlementSchedule,
                Contact = new ContactModel
                {
                    ContactName = TestData.Merchant1ContactName,
                    ContactPhoneNumber = TestData.Merchant1ContactPhoneNumber,
                    ContactEmailAddress = TestData.Merchant1ContactEmailAddress
                },
                Address = new AddressModel
                {
                    AddressLine4 = TestData.Merchant1AddressLine4,
                    AddressLine1 = TestData.Merchant1AddressLine1,
                    AddressLine2 = TestData.Merchant1AddressLine2,
                    AddressLine3 = TestData.Merchant1AddressLine3,
                    Country = TestData.Merchant1Country,
                    PostalCode = TestData.Merchant1PostalCode,
                    Region = TestData.Merchant1Region,
                    Town = TestData.Merchant1Town
                }
            };



        public static Commands.AddNewOperatorCommand AddNewOperatorCommand =>
            new(AccessToken, EstateId, Operator1Id, Operator1Name, RequireCustomMerchantNumber,
                RequireCustomTerminalNumber);

        public static Commands.UpdateOperatorCommand UpdateOperatorCommand =>
            new(AccessToken, EstateId, Operator1Id, Operator1Name, RequireCustomMerchantNumber,
                RequireCustomTerminalNumber);

        public static String EstateName = "Test Estate 1";

        public static String Operator1Name = "Operator 1";
        public static Guid Operator1Id = Guid.Parse("DECA8293-F045-41C5-A2F7-30F2792FD273");
        public static String Operator2Name = "Operator 2";
        public static Guid Operator2Id = Guid.Parse("4412915E-02C5-442B-981C-1710F8770FBC");

        public static Boolean RequireCustomMerchantNumber = true;

        public static Boolean RequireCustomTerminalNumber = true;

        public static String EmailAddress = "estateuser1@testestate1.co.uk";

        public static Guid SecurityUserId = Guid.Parse("6A3C1B74-F01E-4017-85D5-6E6082038AB8");

        public static String Merchant1Reference = "Reference1";
        public static String Merchant1Name = "Test Merchant 1";
        public static Guid Merchant1Id = Guid.Parse("2F8431D9-8D04-4AE5-B66C-DB40DFADE581");
        public static SettlementSchedule Merchant1SettlementSchedule = SettlementSchedule.Immediate;

        public static String Merchant1AddressLine1 = "Address Line 1";

        public static String Merchant1AddressLine2 = "Address Line 2";

        public static String Merchant1AddressLine3 = "Address Line 3";

        public static String Merchant1AddressLine4 = "Address Line 4";

        public static String Merchant1ContactEmailAddress = "testcontact@merchant1.co.uk";

        public static String Merchant1ContactName = "Mr Test Contact";

        public static String Merchant1ContactPhoneNumber = "1234567890";

        public static String Merchant1Country = "United Kingdom";

        public static String Merchant1PostalCode = "TE571NG";

        public static String Merchant1Region = "Test Region";

        public static String Merchant1Town = "Test Town";

        public static AddressResponse Merchant1Address = new() {
            AddressLine1 = Merchant1AddressLine1, Town = Merchant1Town
        };

        public static ContactResponse Merchant1Contact = new() { ContactName = "Contact 1" };

        public static String Merchant2Reference = "Reference2";
        public static String Merchant2Name = "Test Merchant 2";
        public static Guid Merchant2Id = Guid.Parse("8959608C-2448-48EA-AFB4-9D10FFFB6140");
        public static SettlementSchedule Merchant2SettlementSchedule = SettlementSchedule.Weekly;

        public static AddressResponse
            Merchant2Address = new() { AddressLine1 = "Address Line 2", Town = "Test Town 2" };

        public static ContactResponse Merchant2Contact = new() { ContactName = "Contact 2" };

        public static String Merchant3Reference = "Reference3";
        public static String Merchant3Name = "Test Merchant 3";
        public static Guid Merchant3Id = Guid.Parse("877D7384-9A72-4A73-A275-9DB62BF32EDB");
        public static SettlementSchedule Merchant3SettlementSchedule = SettlementSchedule.Monthly;

        public static AddressResponse
            Merchant3Address = new() { AddressLine1 = "Address Line 3", Town = "Test Town 3" };

        public static DateTime FileImportLogQueryStartDate = DateTime.Now.AddDays(-1);
        public static DateTime FileImportLogQueryEndDate = DateTime.Now;

        public static ContactResponse Merchant3Contact = new() { ContactName = "Contact 3" };

        public static EstateResponse EstateResponse =>
            new EstateResponse {
                EstateName = TestData.EstateName,
                Operators = new List<EstateOperatorResponse> {
                    new EstateOperatorResponse {
                        OperatorId = Operator1Id,
                        Name = Operator1Name,
                        RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                        RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber
                    }
                },
                EstateId = TestData.EstateId,
                SecurityUsers = new List<SecurityUserResponse> {
                    new SecurityUserResponse {
                        EmailAddress = TestData.EmailAddress, SecurityUserId = TestData.SecurityUserId
                    }
                }
            };


        public static OperatorResponse OperatorResponse =>
            new OperatorResponse {
                Name = Operator1Name,
                OperatorId = Operator1Id,
                RequireCustomMerchantNumber = false,
                RequireCustomTerminalNumber = false
            };

        public static List<OperatorResponse> OperatorResponses =>
            new List<OperatorResponse> {
                new OperatorResponse {
                    Name = Operator1Name,
                    OperatorId = Operator1Id,
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                },
                new OperatorResponse {
                    Name = Operator2Name,
                    OperatorId = Operator2Id,
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                }
            };

        public static List<MerchantResponse> MerchantResponses =>
            new List<MerchantResponse> {
                new MerchantResponse {
                    SettlementSchedule = Merchant1SettlementSchedule,
                    MerchantReference = Merchant1Reference,
                    MerchantName = Merchant1Name,
                    MerchantId = Merchant1Id,
                    Addresses = [Merchant1Address],
                    Contacts = [Merchant1Contact]
                },
                new MerchantResponse {
                    SettlementSchedule = Merchant2SettlementSchedule,
                    MerchantReference = Merchant2Reference,
                    MerchantName = Merchant2Name,
                    MerchantId = Merchant2Id,
                    Addresses = [Merchant2Address],
                    Contacts = [Merchant2Contact]
                },
                new MerchantResponse {
                    SettlementSchedule = Merchant3SettlementSchedule,
                    MerchantReference = Merchant3Reference,
                    MerchantName = Merchant3Name,
                    MerchantId = Merchant3Id,
                    Addresses = [Merchant3Address],
                    Contacts = [Merchant3Contact]
                }
            };

        public static Guid Contract1Id = Guid.Parse("82ED1302-491D-48F4-8FC7-58A22EB1CF4C");
        public static String Contract1Description = "Test Contract 1";

        public static String Contract1Product1Name = "Contract 1 Product 1";
        public static String Contract1Product1DisplayText = "Contract 1 Prod 1";
        public static Guid Contract1Product1Id = Guid.Parse("49E996AC-7068-42E5-950F-5E73E8909730");
        public static ProductType Contract1Product1ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product1Value = 100.00m;

        public static String Contract1Product2Name = "Contract 1 Product 2";
        public static String Contract1Product2DisplayText = "Contract 1 Prod 2";
        public static Guid Contract1Product2Id = Guid.Parse("71159369-A719-46E3-9D86-E681307EEFAF");
        public static ProductType Contract1Product2ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product2Value = 200.00m;

        public static String Contract1Product3Name = "Contract 1 Product 3";
        public static String Contract1Product3DisplayText = "Contract 1 Prod 3";
        public static Guid Contract1Product3Id = Guid.Parse("29A7B01D-825C-4CD2-A6CE-F18D1DE9087B");
        public static ProductType Contract1Product3ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product3Value = null;

        public static Guid Contract2Id = Guid.Parse("A3A9ED76-EB64-464D-A958-95A496FD63D5");
        public static String Contract2Description = "Test Contract 2";

        public static Guid Contract3Id = Guid.Parse("9070D695-30F1-4554-B595-28019E3237C4");
        public static String Contract3Description = "Test Contract 3";

        public static Decimal TransactionFee1Value = 0.5m;
        public static String TransactionFee1Description = "Merchant Commission";
        public static FeeType TransactionFee1Type = FeeType.Merchant;
        public static CalculationType TransactionFee1CalculationType = CalculationType.Fixed;
        public static Guid TransactionFee1Id = Guid.Parse("61D0AF6D-1303-459F-87E2-4B686BB0585E");

        public static ContractProductTransactionFee ContractProductTransactionFee1 =>
            new ContractProductTransactionFee {
                Value = TransactionFee1Value,
                Description = TransactionFee1Description,
                FeeType = TransactionFee1Type,
                CalculationType = TransactionFee1CalculationType,
                TransactionFeeId = TransactionFee1Id,
            };

        public static ContractProduct ContractProduct1 =>
            new ContractProduct {
                Name = Contract1Product1Name,
                DisplayText = Contract1Product1DisplayText,
                ProductId = Contract1Product1Id,
                ProductType = Contract1Product1ProductType,
                Value = Contract1Product1Value,
                TransactionFees = new List<ContractProductTransactionFee> {
                    ContractProductTransactionFee1
                }
            };

        public static ContractProduct ContractProduct2 =>
            new ContractProduct
            {
                Name = Contract1Product2Name,
                DisplayText = Contract1Product2DisplayText,
                ProductId = Contract1Product2Id,
                ProductType = Contract1Product2ProductType,
                Value = Contract1Product2Value
            };

        public static ContractProduct ContractProduct3 =>
            new ContractProduct
            {
                Name = Contract1Product3Name,
                DisplayText = Contract1Product3DisplayText,
                ProductId = Contract1Product3Id,
                ProductType = Contract1Product3ProductType,
                Value = Contract1Product3Value
            };


        public static ContractResponse ContractResponse1 =>
            new ContractResponse {
                OperatorId = Operator1Id,
                OperatorName = Operator1Name,
                ContractId = Contract1Id,
                Description = Contract1Description,
                Products = new List<ContractProduct> { ContractProduct1, ContractProduct2, ContractProduct3 }
            };

        public static List<ContractResponse> ContractResponses =>
            new List<ContractResponse>() {
                ContractResponse1
            };

        public static ContractResponse ContractResponseNullProducts =>
            new ContractResponse {
                OperatorId = Operator2Id,
                OperatorName = Operator2Name,
                ContractId = Contract2Id,
                Description = Contract2Description,
                Products = null
            };

        public static ContractResponse ContractResponseEmptyProducts =>
            new ContractResponse {
                OperatorId = Operator2Id,
                OperatorName = Operator2Name,
                ContractId = Contract2Id,
                Description = Contract2Description,
                Products = null
            };

        public static FileImportLogList FileImportLogList =>
            new FileImportLogList() { FileImportLogs = new List<FileImportLog> { FileImportLog1 } };

        public static DateTime ImportLogDateTime = DateTime.Now;
        public static Guid FileImportLogId = Guid.Parse("6D747595-C718-4CB0-B7BA-8FB470E5DB0E");
        public static Guid FileProfileId1 = Guid.Parse("55036A79-DFAA-471D-8EB0-4E3DFBF9685B");
        public static String FileProfileName1 = "File Profile 1";

        public static FileImportLogFile FileImportLogFile1 = new FileImportLogFile {
            FileImportLogId = FileImportLogId,
            MerchantId = Merchant1Id,
            FileId = Guid.Parse("90152CBE-1598-41BF-B0A0-B7A9A99DF9E4"),
            FilePath = @"C:\Temp\File1.txt",
            FileProfileId = FileProfileId1,
            FileUploadedDateTime = DateTime.Now,
            OriginalFileName = "originalFile1",
            UserId = SecurityUserId
        };

        public static FileImportLogFile FileImportLogFile2 = new FileImportLogFile
        {
            FileImportLogId = FileImportLogId,
            MerchantId = Merchant1Id,
            FileId = Guid.Parse("31873942-4E19-4F53-A3EE-9EA19ECB4649"),
            FilePath = @"C:\Temp\File2.txt",
            FileProfileId = FileProfileId1,
            FileUploadedDateTime = DateTime.Now,
            OriginalFileName = "originalFile2",
            UserId = SecurityUserId
        };

        public static FileImportLog FileImportLog1 =>
            new FileImportLog {
                ImportLogDate = ImportLogDateTime.Date,
                ImportLogDateTime = ImportLogDateTime,
                FileCount = 0,
                FileImportLogId = FileImportLogId,
                Files = new List<FileImportLogFile>() {
                    FileImportLogFile1,
                    FileImportLogFile2
                },
                ImportLogTime = ImportLogDateTime.TimeOfDay
            };

        public static Guid FileId1 = Guid.Parse("90BEB4AA-2162-435E-816B-BEA12BAE0E30");
        public static String FileLocation1 = "C:\\Temp\\File1.txt";

        public static FileDetails FileDetails1 =>
            new FileDetails {
                EstateId = EstateId,
                FileId = FileId1,
                FileImportLogId = FileImportLogId,
                FileLocation = FileLocation1,
                FileProfileId = FileProfileId1,
                FileProfileName = FileProfileName1,
                MerchantId = Merchant1Id,
                MerchantName = Merchant1Name,
                ProcessingCompleted = true,
                UserEmailAddress = EmailAddress,
                UserId = SecurityUserId,
                ProcessingSummary =
                    new FileProcessingSummary {
                        FailedLines = 1,
                        IgnoredLines = 2,
                        NotProcessedLines = 4, // Includes Unknown
                        RejectedLines = 4,
                        SuccessfullyProcessedLines = 5,
                        TotalLines = 16
                    },
                FileLines = new List<FileLine> {
                    new FileLine {
                        LineNumber = 1,
                        ProcessingResult = FileLineProcessingResult.Failed,
                        RejectionReason = "Rejected",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 2,
                        ProcessingResult = FileLineProcessingResult.Ignored,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 3,
                        ProcessingResult = FileLineProcessingResult.Ignored,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 4,
                        ProcessingResult = FileLineProcessingResult.NotProcessed,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 5,
                        ProcessingResult = FileLineProcessingResult.NotProcessed,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 6,
                        ProcessingResult = FileLineProcessingResult.NotProcessed,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 7,
                        ProcessingResult = FileLineProcessingResult.Rejected,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 8,
                        ProcessingResult = FileLineProcessingResult.Rejected,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 9,
                        ProcessingResult = FileLineProcessingResult.Rejected,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 10,
                        ProcessingResult = FileLineProcessingResult.Rejected,
                        RejectionReason = "",
                        TransactionId = Guid.Empty,
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 11,
                        ProcessingResult = FileLineProcessingResult.Successful,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 12,
                        ProcessingResult = FileLineProcessingResult.Successful,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 13,
                        ProcessingResult = FileLineProcessingResult.Successful,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 14,
                        ProcessingResult = FileLineProcessingResult.Successful,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 15,
                        ProcessingResult = FileLineProcessingResult.Successful,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                    new FileLine {
                        LineNumber = 16,
                        ProcessingResult = FileLineProcessingResult.Unknown,
                        RejectionReason = "",
                        TransactionId = Guid.NewGuid(),
                        LineData = "1,2,3"
                    },
                }
            };

        public static TodaysSettlement TodaysSettlement => new TodaysSettlement
        {
            ComparisonSettlementCount = 100,
            ComparisonSettlementValue = 1000,
            TodaysSettlementCount = 50,
            TodaysSettlementValue = 500
        };

        public static TodaysSales TodaysSales => new TodaysSales
        {
            ComparisonSalesCount = 100,
            ComparisonSalesValue = 1000,
            TodaysSalesCount = 50,
            TodaysSalesValue = 500
        };

        public static List<ComparisonDate> ComparisonDates => new List<ComparisonDate> {
            new ComparisonDate {
                Date = new DateTime(2024, 1, 1), Description = new DateTime(2024, 1, 1).ToString(), OrderValue = 0
            },
            new ComparisonDate {
                Date = new DateTime(2024, 1, 2), Description = new DateTime(2024, 1, 2).ToString(), OrderValue = 0
            }
        };

        public static List<TodaysSalesCountByHour> TodaysSalesCountByHour => new List<TodaysSalesCountByHour> {
            new TodaysSalesCountByHour { TodaysSalesCount = 100, ComparisonSalesCount = 85, Hour = 0 },
            new TodaysSalesCountByHour { TodaysSalesCount = 90, ComparisonSalesCount = 87, Hour = 1 }
        };

        public static List<TodaysSalesValueByHour> TodaysSalesValueByHour => new List<TodaysSalesValueByHour> {
            new TodaysSalesValueByHour { TodaysSalesValue = 100, ComparisonSalesValue = 85, Hour = 0 },
            new TodaysSalesValueByHour { TodaysSalesValue = 90, ComparisonSalesValue = 87, Hour = 1 }
        };

        public static List<TopBottomProductData> TopBottomProductDataList => new List<TopBottomProductData>{
            new TopBottomProductData(){
                ProductName = "Product 1",
                SalesValue = 100
            },
            new TopBottomProductData(){
                ProductName = "Product 2",
                SalesValue = 200
            }
        };

        public static List<TopBottomMerchantData> TopBottomMerchantDataList => new List<TopBottomMerchantData>{
            new TopBottomMerchantData(){
                MerchantName = "Merchant 1",
                SalesValue = 100
            },
            new TopBottomMerchantData(){
                MerchantName = "Merchant 2",
                SalesValue = 200
            }
        };

        public static List<TopBottomOperatorData> TopBottomOperatorDataList => new List<TopBottomOperatorData>{
            new TopBottomOperatorData(){
                OperatorName = "Operator 1",
                SalesValue = 100
            },
            new TopBottomOperatorData(){
                OperatorName = "Operator 2",
                SalesValue = 200
            }
        };

        public static MerchantKpi MerchantKpi => new MerchantKpi()
        {
            MerchantsWithNoSaleInLast7Days = 1,
            MerchantsWithNoSaleToday = 2,
            MerchantsWithSaleInLastHour = 3
        };

        public static LastSettlement LastSettlement =>
            new LastSettlement
            {
                SettlementDate = new DateTime(2024, 1, 1),
                FeesValue = 100.00m,
                SalesCount = 100,
                SalesValue = 1000.00m
            };
    }
}