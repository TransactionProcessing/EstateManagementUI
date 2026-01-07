using Azure.Core;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Operator;
using FileImportLogList = FileProcessor.DataTransferObjects.Responses.FileImportLogList;
using FileLine = FileProcessor.DataTransferObjects.Responses.FileLine;
using FileLineProcessingResult = FileProcessor.DataTransferObjects.Responses.FileLineProcessingResult;
using LastSettlement = EstateReportingAPI.DataTransferObjects.LastSettlement;
using MerchantKpi = EstateReportingAPI.DataTransferObjects.MerchantKpi;
using SettlementSchedule = EstateManagementUI.BusinessLogic.Models.SettlementSchedule;
using TodaysSales = EstateReportingAPI.DataTransferObjects.TodaysSales;
using TodaysSalesCountByHour = EstateReportingAPI.DataTransferObjects.TodaysSalesCountByHour;
using TodaysSalesCountByHourModel = EstateManagementUI.BusinessLogic.Models.TodaysSalesCountByHourModel;
using TodaysSalesValueByHour = EstateReportingAPI.DataTransferObjects.TodaysSalesValueByHour;
using TodaysSalesValueByHourModel = EstateManagementUI.BusinessLogic.Models.TodaysSalesValueByHourModel;
using TodaysSettlement = EstateReportingAPI.DataTransferObjects.TodaysSettlement;

namespace EstateManagementUI.Testing
{
    public static class TestData {
        public static IReadOnlyDictionary<String, String> DefaultAppSettings =>
            new Dictionary<String, String>
            {
                ["AppSettings:ClientId"] = "clientId",
                ["AppSettings:ClientSecret"] = "clientSecret",
                ["AppSettings:BackEndClientId"] = "clientId",
                ["AppSettings:BackEndClientSecret"] = "clientSecret",
                ["AppSettings:UseConnectionStringConfig"] = "false",
                ["EventStoreSettings:ConnectionString"] = "esdb://127.0.0.1:2113",
                ["SecurityConfiguration:Authority"] = "https://127.0.0.1",
                ["AppSettings:EstateManagementApi"] = "http://127.0.0.1",
                ["AppSettings:SecurityService"] = "http://127.0.0.1",
                ["AppSettings:ContractProductFeeCacheExpiryInHours"] = "",
                ["AppSettings:ContractProductFeeCacheEnabled"] = "",
                ["ConnectionStrings:HealthCheck"] = "HealthCheck",
                ["ConnectionStrings:EstateReportingReadModel"] = "",
                ["ConnectionStrings:TransactionProcessorReadModel"] = ""
            };
        public static DateTime ComparisonDate = new DateTime(2024,1,1);
        public static String AccessToken = "token1";
        public static CorrelationId CorrelationId = CorrelationIdHelper.New();
        public static Guid EstateId = Guid.Parse("BD6F1ED7-6290-4285-A200-E4F8D25F4CBE");
        public static Queries.GetEstateQuery GetEstateQuery => new(CorrelationIdHelper.New(), AccessToken, EstateId);
        public static Queries.GetMerchantsQuery GetMerchantsQuery => new(CorrelationId,AccessToken, EstateId);

        public static Queries.GetMerchantQuery GetMerchantQuery => new(CorrelationId, AccessToken, EstateId, Merchant1Id);
        public static Queries.GetOperatorsQuery GetOperatorsQuery => new(CorrelationId, AccessToken, EstateId);
        public static Queries.GetOperatorQuery GetOperatorQuery => new(CorrelationId, AccessToken, EstateId, Operator1Id);
        public static Queries.GetContractsQuery GetContractsQuery => new(CorrelationId, AccessToken, EstateId);
        public static Queries.GetContractQuery GetContractQuery => new(CorrelationId, AccessToken, EstateId, Contract1Id);

        public static Queries.GetFileImportLogsListQuery GetFileImportLogsListQuery =>
            new Queries.GetFileImportLogsListQuery(CorrelationId, AccessToken, EstateId, Merchant1Id, FileImportLogQueryStartDate,
                FileImportLogQueryEndDate);

        public static Queries.GetFileImportLogQuery GetFileImportLogQuery =>
            new Queries.GetFileImportLogQuery(CorrelationId, AccessToken, EstateId, Merchant1Id, FileImportLogId);

        public static Queries.GetFileDetailsQuery GetFileDetailsQuery =>
            new(CorrelationId, AccessToken, EstateId, FileImportLogFile1.FileId);

        public static Queries.GetComparisonDatesQuery GetComparisonDatesQuery => new(CorrelationId, AccessToken, EstateId);
        public static Queries.GetTodaysSalesQuery GetTodaysSalesQuery => new(CorrelationId, AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSettlementQuery GetTodaysSettlementQuery => new(CorrelationId, AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSalesCountByHourQuery GetTodaysSalesCountByHourQuery => new(CorrelationId, AccessToken, EstateId, DateTime.Now);
        public static Queries.GetTodaysSalesValueByHourQuery GetTodaysSalesValueByHourQuery => new(CorrelationId, AccessToken, EstateId, DateTime.Now);

        public static Queries.GetMerchantKpiQuery GetMerchantKpiQuery => new (CorrelationId, AccessToken, EstateId);

        public static Queries.GetTodaysFailedSalesQuery GetTodaysFailedSalesQuery =>
            new(CorrelationId, AccessToken, EstateId, "1009", DateTime.Now);

        public static Queries.GetTopProductDataQuery GetTopProductDataQuery =>
            new Queries.GetTopProductDataQuery(CorrelationId, AccessToken, EstateId, 1);

        public static Queries.GetBottomProductDataQuery GetBottomProductDataQuery => new(CorrelationId, AccessToken, EstateId, 1);
        public static Queries.GetTopMerchantDataQuery GetTopMerchantDataQuery => new(CorrelationId, AccessToken, EstateId, 1);
        public static Queries.GetBottomMerchantDataQuery GetBottomMerchantDataQuery => new(CorrelationId, AccessToken, EstateId, 1);
        public static Queries.GetTopOperatorDataQuery GetTopOperatorDataQuery => new(CorrelationId, AccessToken, EstateId, 1);
        public static Queries.GetBottomOperatorDataQuery GetBottomOperatorDataQuery => new(CorrelationId, AccessToken, EstateId, 1);

        public static Queries.GetLastSettlementQuery GetLastSettlementQuery => new(CorrelationId, AccessToken, EstateId);

        public static Commands.AddMerchantCommand AddNewMerchantCommand => new(CorrelationId, AccessToken, EstateId, CreateMerchantModel(BusinessLogic.Models.SettlementSchedule.Immediate));

        public static Commands.UpdateMerchantCommand UpdateMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, new UpdateMerchantModel {
            MerchantName = Merchant1Name,
            SettlementSchedule = EstateManagementUI.BusinessLogic.Models.SettlementSchedule.Immediate
        });

        public static Commands.UpdateMerchantAddressCommand UpdateMerchantAddressCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, new AddressModel
        {
            AddressLine1 = Merchant1AddressLine1,
            AddressLine2 = Merchant1AddressLine2,
            AddressLine3 = Merchant1AddressLine3,
            AddressLine4 = Merchant1AddressLine4,
            Country = Merchant1Country,
            PostalCode = Merchant1PostalCode,
            Region = Merchant1Region,
            Town = Merchant1Town
        });

        public static Commands.UpdateMerchantContactCommand UpdateMerchantContactCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, new ContactModel
        {
            ContactName = Merchant1ContactName,
            ContactPhoneNumber = Merchant1ContactPhoneNumber,
            ContactEmailAddress = Merchant1ContactEmailAddress
        });

        public static Commands.AssignContractToMerchantCommand AssignContractToMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, new AssignContractToMerchantModel {
            ContractId = Contract1Id
        });

        public static Commands.RemoveContractFromMerchantCommand RemoveContractFromMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, Contract1Id);

        public static Commands.AssignDeviceToMerchantCommand AssignDeviceToMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, AssignDeviceToMerchantModel);

        public static AssignOperatorToMerchantModel AssignOperatorToMerchantModel => new AssignOperatorToMerchantModel { OperatorId = Operator1Id };
        public static AssignOperatorToEstateModel AssignOperatorToEstateModel => new AssignOperatorToEstateModel { OperatorId = Operator1Id };
        public static AssignContractToMerchantModel AssignContractToMerchantModel => new AssignContractToMerchantModel { ContractId = TestData.Contract1Id };
        public static String DeviceIdentifier = "123456ABCDEF";
        public static AssignDeviceToMerchantModel AssignDeviceToMerchantModel => new AssignDeviceToMerchantModel { DeviceIdentifier = DeviceIdentifier };

        public static Commands.AssignOperatorToMerchantCommand AssignOperatorToMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, new AssignOperatorToMerchantModel {
            OperatorId = Operator1Id
        });

        public static Commands.RemoveOperatorFromMerchantCommand RemoveOperatorFromMerchantCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, Operator1Id);

        public static Commands.AssignOperatorToEstateCommand AssignOperatorToEstateCommand => new(CorrelationId, AccessToken, EstateId, new AssignOperatorToEstateModel {
            OperatorId = Operator1Id
        });

        public static Commands.RemoveOperatorFromEstateCommand RemoveOperatorFromEstateCommand => new(CorrelationId, AccessToken, EstateId, Operator1Id);

        public static Commands.CreateContractCommand CreateContractCommand => new(CorrelationId, AccessToken, EstateId, CreateContractModel);
        public static Commands.CreateContractProductCommand CreateContractProductCommand => new(CorrelationId, AccessToken, EstateId, Contract1Id, CreateContractProductModel);

        public static Commands.CreateContractProductTransactionFeeCommand CreateContractProductTransactionFeeCommand => new(CorrelationId, AccessToken, EstateId, Contract1Id, Contract1Product1Id, CreateContractProductTransactionFeeModel);

        public static Commands.MakeDepositCommand MakeDepositCommand => new(CorrelationId, AccessToken, EstateId, Merchant1Id, MakeDepositModel);

        public static CreateMerchantResponse CreateMerchantResponse =>
            new() { EstateId = EstateId, MerchantId = Merchant1Id };

        public static Decimal DepositAmount = 100.00m;
        public static DateTime DepositDateTime = DateTime.Now;
        public static String DepositReference = "Deposit Reference 1";
        public static MakeDepositModel MakeDepositModel => new() { Amount = TestData.DepositAmount, Date = TestData.DepositDateTime, Reference = TestData.DepositReference };
        public static CreateContractModel CreateContractModel => new() { Description = Contract1Description, OperatorId = Operator1Id };

        public static CreateContractProductModel CreateContractProductModel => new() { DisplayText = Contract1Product1DisplayText, Name = Contract1Product1Name, Type = (Int32)Contract1Product1ProductType, Value = Contract1Product1Value };
        public static CreateContractProductTransactionFeeModel CreateContractProductTransactionFeeModel => new() { CalculationType = TransactionFee1CalculationType, Description = TransactionFee1Description, FeeType = TransactionFee1Type, Value = TransactionFee1Value };

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

        public static UpdateMerchantModel UpdateMerchantModel(BusinessLogic.Models.SettlementSchedule settlementSchedule) =>
            new UpdateMerchantModel
            {
                MerchantName = Merchant1Name,
                SettlementSchedule = settlementSchedule
            };

        public static AddressModel AddressModel =>
            new AddressModel
            {
                AddressLine1 = Merchant1AddressLine1,
                AddressLine2 = Merchant1AddressLine2,
                AddressLine3 = Merchant1AddressLine3,
                AddressLine4 = Merchant1AddressLine4,
                Country = Merchant1Country,
                PostalCode = Merchant1PostalCode,
                Region = Merchant1Region,
                Town = Merchant1Town
            };

        public static ContactModel ContactModel => new ContactModel { ContactEmailAddress = Merchant1ContactEmailAddress, ContactName = Merchant1ContactName, ContactPhoneNumber = Merchant1ContactPhoneNumber };

        public static Commands.AddNewOperatorCommand AddNewOperatorCommand =>
            new(CorrelationId, AccessToken, EstateId, Operator1Id, Operator1Name, RequireCustomMerchantNumber,
                RequireCustomTerminalNumber);

        public static Commands.UpdateOperatorCommand UpdateOperatorCommand =>
            new(CorrelationId, AccessToken, EstateId, Operator1Id, Operator1Name, RequireCustomMerchantNumber,
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
        public static TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule Merchant1SettlementSchedule = TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate;

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

        public static Dictionary<Guid, String> Merchant1Devices = new Dictionary<Guid, String> {
            { Guid.Parse("A5B6A7FD-1D45-4781-A9CC-0A09C206737E"), "Device 1" }
        };

        public static MerchantContractResponse Merchant1Contract = new()
        {
            ContractId = Contract1Id
        };

        public static MerchantOperatorResponse Merchant1Operator = new()
        {
            OperatorId = Operator1Id
        };

        public static ContactResponse Merchant1Contact = new() { ContactName = "Contact 1" };

        public static String Merchant2Reference = "Reference2";
        public static String Merchant2Name = "Test Merchant 2";
        public static Guid Merchant2Id = Guid.Parse("8959608C-2448-48EA-AFB4-9D10FFFB6140");
        public static TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule Merchant2SettlementSchedule = TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly;

        public static AddressResponse
            Merchant2Address = new() { AddressLine1 = "Address Line 2", Town = "Test Town 2" };

        public static ContactResponse Merchant2Contact = new() { ContactName = "Contact 2" };

        public static String Merchant3Reference = "Reference3";
        public static String Merchant3Name = "Test Merchant 3";
        public static Guid Merchant3Id = Guid.Parse("877D7384-9A72-4A73-A275-9DB62BF32EDB");
        public static TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule Merchant3SettlementSchedule = TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly;

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

        public static MerchantResponse MerchantResponse =>
            new MerchantResponse {
                SettlementSchedule = Merchant1SettlementSchedule,
                MerchantReference = Merchant1Reference,
                MerchantName = Merchant1Name,
                MerchantId = Merchant1Id,
                Addresses = [Merchant1Address],
                Contacts = [Merchant1Contact],
                Devices = Merchant1Devices,
                Contracts = [Merchant1Contract],
                Operators = [Merchant1Operator]
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

        public static TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProductTransactionFee ContractProductTransactionFee1 =>
            new TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProductTransactionFee {
                Value = TransactionFee1Value,
                Description = TransactionFee1Description,
                FeeType = TransactionFee1Type,
                CalculationType = TransactionFee1CalculationType,
                TransactionFeeId = TransactionFee1Id,
            };

        public static TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct ContractProduct1 =>
            new TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct {
                Name = Contract1Product1Name,
                DisplayText = Contract1Product1DisplayText,
                ProductId = Contract1Product1Id,
                ProductType = Contract1Product1ProductType,
                Value = Contract1Product1Value,
                TransactionFees = new List<TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProductTransactionFee> {
                    ContractProductTransactionFee1
                }
            };

        public static TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct ContractProduct2 =>
            new TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct
            {
                Name = Contract1Product2Name,
                DisplayText = Contract1Product2DisplayText,
                ProductId = Contract1Product2Id,
                ProductType = Contract1Product2ProductType,
                Value = Contract1Product2Value
            };

        public static TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct ContractProduct3 =>
            new TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct
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
                Products = new List<TransactionProcessor.DataTransferObjects.Responses.Contract.ContractProduct> { ContractProduct1, ContractProduct2, ContractProduct3 }
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

        public static FileDetails FileDetails => new FileDetails
        {
            FileId = Guid.NewGuid(),
            FileProfileId = Guid.NewGuid(),
            FileProfileName = "Test Profile",
            FileImportLogId = Guid.NewGuid(),
            FileLocation = "Test Location",
            ProcessingCompleted = true,
            EstateId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserEmailAddress = "test@example.com",
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            ProcessingSummary = new FileProcessingSummary
            {
                FailedLines = 1,
                IgnoredLines = 2,
                NotProcessedLines = 3,
                RejectedLines = 4,
                SuccessfullyProcessedLines = 5,
                TotalLines = 15
            },
            FileLines = new List<FileLine>
            {
                new FileLine
                {
                    LineData = "Line 1 Data",
                    LineNumber = 1,
                    RejectionReason = "Reason 1",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.Successful
                },
                new FileLine
                {
                    LineData = "Line 2 Data",
                    LineNumber = 2,
                    RejectionReason = "Reason 2",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.Failed
                },
                new FileLine
                {
                    LineData = "Line 3 Data",
                    LineNumber = 3,
                    RejectionReason = "Reason 3",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.Ignored
                },
                new FileLine
                {
                    LineData = "Line 4 Data",
                    LineNumber = 4,
                    RejectionReason = "Reason 4",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.NotProcessed
                },
                new FileLine
                {
                    LineData = "Line 5 Data",
                    LineNumber = 5,
                    RejectionReason = "Reason 5",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.Rejected
                },
                new FileLine
                {
                    LineData = "Line 6 Data",
                    LineNumber = 6,
                    RejectionReason = "Reason 6",
                    TransactionId = Guid.NewGuid(),
                    ProcessingResult = FileLineProcessingResult.Unknown
                }
            }
        };

        public static List<ComparisonDateModel> ComparisonDates1 => new List<ComparisonDateModel>
        {
            new ComparisonDateModel { Date = DateTime.Parse("2023-01-01"), Description = "2023-01-01", OrderValue = 1 }
        };

        public static TodaysSalesModel TodaysSales1 => new TodaysSalesModel
        {
            TodaysSalesValue = 100,
            ComparisonSalesValue = 80
        };

        public static MerchantKpiModel MerchantKpi1 => new MerchantKpiModel
        {
            MerchantsWithNoSaleInLast7Days = 5,
            MerchantsWithNoSaleToday = 3,
            MerchantsWithSaleInLastHour = 2
        };

        public static List<TopBottomMerchantDataModel> BottomMerchants1 => new List<TopBottomMerchantDataModel>
        {
            new TopBottomMerchantDataModel { MerchantName = "Merchant1", SalesValue = 50 },
            new TopBottomMerchantDataModel { MerchantName = "Merchant2", SalesValue = 30 }
        };

        public static List<TopBottomOperatorDataModel> BottomOperators1 => new List<TopBottomOperatorDataModel>
        {
            new TopBottomOperatorDataModel { OperatorName = "Operator1", SalesValue = 40 },
            new TopBottomOperatorDataModel { OperatorName = "Operator2", SalesValue = 20 }
        };

        public static List<TopBottomProductDataModel> BottomProducts1 => new List<TopBottomProductDataModel>
        {
            new TopBottomProductDataModel { ProductName = "Product1", SalesValue = 60 },
            new TopBottomProductDataModel { ProductName = "Product2", SalesValue = 25 }
        };

        public static TodaysSalesModel TodaysFailedSales1 => new TodaysSalesModel
        {
            TodaysSalesValue = 70,
            ComparisonSalesValue = 50
        };

        public static TodaysSettlementModel TodaysSettlement1 => new TodaysSettlementModel
        {
            TodaysSettlementValue = 1000,
            ComparisonSettlementValue = 800
        };

        public static LastSettlementModel LastSettlement1 => new LastSettlementModel
        {
            SettlementDate = DateTime.Parse("2023-01-01"),
            SalesValue = 1500,
            FeesValue = 100
        };

        public static OperatorModel Operator => new OperatorModel
        {
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };

        public static Result<OperatorModel> OperatorResult => Result.Success(Operator);

        public static List<OperatorModel> Operators => new List<OperatorModel>
        {
            new OperatorModel { OperatorId = Guid.NewGuid(), Name = "Operator1", RequireCustomMerchantNumber = true, RequireCustomTerminalNumber = false },
            new OperatorModel { OperatorId = Guid.NewGuid(), Name = "Operator2", RequireCustomMerchantNumber = false, RequireCustomTerminalNumber = true }
        };

        public static Result<List<OperatorModel>> OperatorsResult => Result.Success(Operators);

        public static List<MerchantModel> Merchants => new List<MerchantModel>
        {
            new MerchantModel { MerchantId = Guid.NewGuid(), MerchantName = "Merchant1", MerchantReference  = "Reference1",Address = new AddressModel{AddressLine1 = "AddressLine1", Town = "Town1"}, Contact = new ContactModel{ContactName = "Contact1"}, SettlementSchedule = "Immediate"},
            new MerchantModel { MerchantId = Guid.NewGuid(), MerchantName = "Merchant2", MerchantReference  = "Reference2",Address = new AddressModel{AddressLine1 = "AddressLine2", Town = "Town2"}, Contact = new ContactModel{ContactName = "Contact2"}, SettlementSchedule = "Monthly"}
        };

        public static Result<List<MerchantModel>> MerchantsResult => Result.Success(Merchants);

        public static MerchantModel Merchant => new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            MerchantReference = "Ref123",
            SettlementSchedule = "Immediate",
            Address = new AddressModel
            {
                AddressLine1 = "123 Main St",
                AddressLine2 = "Suite 100",
                Town = "Anytown",
                Region = "Anystate",
                Country = "USA",
                PostalCode = "12345",
                AddressId = Guid.NewGuid()
            },
            Contact = new ContactModel
            {
                ContactName = "John Doe",
                ContactEmailAddress = "john.doe@example.com",
                ContactPhoneNumber = "555-1234",
                ContactId = Guid.NewGuid()
            },
            Operators = new List<MerchantOperatorModel>
            {
                new MerchantOperatorModel
                {
                    OperatorId = Guid.NewGuid(),
                    Name = "Operator1",
                    MerchantNumber = "123456",
                    TerminalNumber = "7890",
                    IsDeleted = false
                }
            },
            Contracts = new List<MerchantContractModel>
            {
                new MerchantContractModel
                {
                    ContractId = Guid.NewGuid(),
                    Name = "Contract1",
                    IsDeleted = false
                }
            },
            Devices = new Dictionary<Guid, string>
            {
                { Guid.NewGuid(), "Device1" }
            }
        };

        public static Result<MerchantModel> MerchantResult => Result.Success(Merchant);


        public static List<ContractModel> Contracts => new List<ContractModel>
        {
            new ContractModel { ContractId = Guid.NewGuid(), Description = "Contract1" },
            new ContractModel { ContractId = Guid.NewGuid(), Description = "Contract2" }
        };

        public static Result<List<ContractModel>> ContractsResult => Result.Success(Contracts);

        public static List<FileImportLogModel> FileImportLogs => new List<FileImportLogModel>
        {
            new FileImportLogModel
            {
                FileImportLogId = Guid.NewGuid(),
                ImportLogDateTime = DateTime.Now,
                ImportLogDate = DateTime.Now.Date,
                ImportLogTime = DateTime.Now.TimeOfDay,
                FileCount = 5
            },
            new FileImportLogModel
            {
                FileImportLogId = Guid.NewGuid(),
                ImportLogDateTime = DateTime.Now.AddDays(-1),
                ImportLogDate = DateTime.Now.AddDays(-1).Date,
                ImportLogTime = DateTime.Now.AddDays(-1).TimeOfDay,
                FileCount = 3
            }
        };

        public static Result<List<FileImportLogModel>> FileImportLogsResult => Result.Success(FileImportLogs);

        public static FileImportLogModel FileImportLog => new FileImportLogModel
        {
            FileImportLogId = Guid.Parse("C9EDAF44-63A7-4037-8E0F-4EC0707474EE"),
            ImportLogDateTime = DateTime.Now,
            ImportLogDate = DateTime.Now.Date,
            ImportLogTime = DateTime.Now.TimeOfDay,
            FileCount = 5,
            Files = new List<FileImportLogFileModel> {
                new FileImportLogFileModel {
                    OriginalFileName = "File1.txt",
                    FileUploadedDateTime = new DateTime(2024,12,25),
                    FileProfileId = Guid.Parse("B2A59ABF-293D-4A6B-B81B-7007503C3476"),
                    UserId = Guid.Parse("01A5BD19-0A11-4814-B27B-75A53E237CE6"),
                },
                new FileImportLogFileModel {
                    OriginalFileName = "File2.txt",
                    FileUploadedDateTime = new DateTime(2024,12,26),
                    FileProfileId = Guid.Parse("B2A59ABF-293D-4A6B-B81B-7007503C3476"),
                    UserId = Guid.Parse("01A5BD19-0A11-4814-B27B-75A53E237CE6"),
                },
                new FileImportLogFileModel {
                    OriginalFileName = "File3.txt",
                    FileUploadedDateTime = new DateTime(2024,12,27),
                    FileProfileId = Guid.Parse("B2A59ABF-293D-4A6B-B81B-7007503C3476"),
                    UserId = Guid.Parse("01A5BD19-0A11-4814-B27B-75A53E237CE6"),
                },
                new FileImportLogFileModel {
                    OriginalFileName = "File4.txt",
                    FileUploadedDateTime = new DateTime(2024,12,28),
                    FileProfileId = Guid.Parse("B2A59ABF-293D-4A6B-B81B-7007503C3476"),
                    UserId = Guid.Parse("01A5BD19-0A11-4814-B27B-75A53E237CE6"),
                },
                new FileImportLogFileModel {
                    OriginalFileName = "File5.txt",
                    FileUploadedDateTime = new DateTime(2024,12,29),
                    FileProfileId = Guid.Parse("B2A59ABF-293D-4A6B-B81B-7007503C3476"),
                    UserId = Guid.Parse("01A5BD19-0A11-4814-B27B-75A53E237CE6"),
                }
            }
        };

        public static Result<FileImportLogModel> FileImportLogResult => Result.Success(FileImportLog);

        public static EstateModel Estate => new EstateModel
        {
            EstateId = Guid.NewGuid(),
            EstateName = "Test Estate",
            Reference = "Ref123",
            SecurityUsers = new List<SecurityUserModel>
            {
                new SecurityUserModel
                {
                    SecurityUserId = Guid.NewGuid(),
                    EmailAddress = "user1@example.com"
                },
                new SecurityUserModel
                {
                    SecurityUserId = Guid.NewGuid(),
                    EmailAddress = "user2@example.com"
                }
            },
            Operators = new List<EstateOperatorModel> {
                new EstateOperatorModel {
                    Name = "Operator1"
                },
                new EstateOperatorModel {
                    Name = "Operator2"
                }
            }
        };

        public static Result<EstateModel> EstateResult => Result.Success(Estate);

        public static FileDetailsModel FileDetailsWith2Lines => new FileDetailsModel
        {
            FileId = Guid.NewGuid(),
            FileLocation = "path/to/file",
            MerchantName = "Test Merchant",
            FileProfileName = "Test Profile",
            UserEmailAddress = "user@example.com",
            FileLines = new List<FileLineModel>
            {
                new FileLineModel
                {
                    LineNumber = 1,
                    LineData = "data1",
                    ProcessingResult = BusinessLogic.Models.FileLineProcessingResult.Successful,
                    TransactionId = Guid.NewGuid(),
                    RejectionReason = "None"
                },
                new FileLineModel
                {
                    LineNumber = 2,
                    LineData = "data2",
                    ProcessingResult = BusinessLogic.Models.FileLineProcessingResult.Failed,
                    TransactionId = Guid.NewGuid(),
                    RejectionReason = "Error"
                }
            }
        };

        public static Result<FileDetailsModel> FileDetailsResult => Result.Success(FileDetailsWith2Lines);


        public static EstateModel ViewEstate => new EstateModel
        {
            EstateId = Guid.Parse("D2ED8F42-BA29-47A6-B781-FE115CD145D0"),
            EstateName = "Test Estate",
            Reference = "Ref123"
        };

        public static Result<EstateModel> ViewEstateResult => Result.Success(ViewEstate);


        public static List<ContractProductModel> ContractProducts => new List<ContractProductModel>
        {
            new ContractProductModel {
                ContractProductId = Guid.NewGuid(), ProductName = "Product1", ProductType = "Type1",
                NumberOfFees = 2, DisplayText = "Product 1", Value = "100 KES"
            },
            new ContractProductModel {
                ContractProductId = Guid.NewGuid(), ProductName = "Product2", ProductType = "Type2", NumberOfFees = 3
                , DisplayText = "Product 2", Value = "200 KES"
            }
        };

        public static ContractModel ContractsResultWithProducts => new ContractModel() {
            ContractId = Guid.NewGuid(), Description = "Contract1",
            ContractProducts = ContractProducts
        };

        public static ContractListModel ContractList => new ContractListModel
        {
            Contracts = new List<SelectListItem>
            {
                new SelectListItem { Text = "Contract1", Value = Guid.NewGuid().ToString() },
                new SelectListItem { Text = "Contract2", Value = Guid.NewGuid().ToString() }
            },
            ContractId = Guid.NewGuid().ToString()
        };

        public static Guid ContractId => Guid.Parse("BA93C499-9921-468E-960E-03D1063E7FF8");
        public static Guid ContractProductId => Guid.Parse("55B77A81-A218-4874-88A4-EA86B3C8181D");

        public static ContractModel GetContractModel()
        {
            return new ContractModel
            {
                ContractId = ContractId,
                ContractProducts = new List<ContractProductModel>
                {
                    new ContractProductModel
                    {
                        ContractProductId = ContractProductId,
                        ProductName = "Product1",
                        ContractProductTransactionFees = new List<ContractProductTransactionFeeModel>
                        {
                            new ContractProductTransactionFeeModel
                            {
                                ContractProductTransactionFeeId = Guid.NewGuid(),
                                CalculationType = "Type1",
                                Description = "Description1",
                                Value = 10.0m,
                                FeeType = "FeeType1"
                            },
                            new ContractProductTransactionFeeModel
                            {
                                ContractProductTransactionFeeId = Guid.NewGuid(),
                                CalculationType = "Type2",
                                Description = "Description2",
                                Value = 20.0m,
                                FeeType = "FeeType2"
                            }
                        }
                    }
                }
            };
        }

        public static List<ContractModel> GetContracts()
        {
            return new List<ContractModel>
            {
                new ContractModel
                {
                    ContractId = Guid.NewGuid(),
                    OperatorName = "Operator1",
                    Description = "Description1",
                    NumberOfProducts = 2
                },
                new ContractModel
                {
                    ContractId = Guid.NewGuid(),
                    OperatorName = "Operator2",
                    Description = "Description2",
                    NumberOfProducts = 3
                }
            };
        }

        public static List<OperatorModel> GetOperatorModels()
        {
            return new List<OperatorModel>
            {
                new OperatorModel
                {
                    OperatorId = Guid.NewGuid(),
                    Name = "Operator1"
                },
                new OperatorModel
                {
                    OperatorId = Guid.NewGuid(),
                    Name = "Operator2"
                }
            };
        }

        public static AssignOperatorToMerchantModel GetAssignOperatorToMerchantModel()
        {
            return new AssignOperatorToMerchantModel
            {
                OperatorId = Guid.NewGuid(),
                MerchantNumber = "123456",
                TerminalNumber = "7890"
            };
        }

        public static List<OperatorListModel> GetOperatorListModels()
        {
            return new List<OperatorListModel>
            {
                new OperatorListModel
                {
                    OperatorId = "64BA00F0-2DB1-4706-BB96-6A925BA25910",
                    Operators = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "64BA00F0-2DB1-4706-BB96-6A925BA25910", Text = "Operator1" },
                        new SelectListItem { Value = "1136657A-16A1-4416-A9A0-C8939A79E3FE", Text = "Operator2" }
                    }
                }
            };
        }

        public static List<MerchantModel> GetMerchants()
        {
            return new List<MerchantModel>
            {
                new MerchantModel
                {
                    MerchantId = Guid.NewGuid(),
                    MerchantName = "Merchant1",
                    MerchantReference = "Reference1",
                    Address = new AddressModel
                    {
                        AddressLine1 = "Address Line 1",
                        Town = "Town1",
                        Region = "Region1",
                        Country = "Country1",
                        PostalCode = "PostalCode1"
                    },
                    Contact = new ContactModel
                    {
                        ContactName = "Contact1",
                        ContactPhoneNumber = "1234567890",
                        ContactEmailAddress = "contact1@example.com"
                    },
                    SettlementSchedule = "Immediate",
                    Operators = new List<MerchantOperatorModel>
                    {
                        new MerchantOperatorModel
                        {
                            OperatorId = Guid.NewGuid(),
                            Name = "Operator1",
                            MerchantNumber = "123456",
                            TerminalNumber = "7890",
                            IsDeleted = false
                        }
                    },
                    Contracts = new List<MerchantContractModel>
                    {
                        new MerchantContractModel
                        {
                            ContractId = Guid.NewGuid(),
                            Name = "Contract1",
                            IsDeleted = false
                        }
                    },
                    Devices = new Dictionary<Guid, string>
                    {
                        { Guid.NewGuid(), "Device1" }
                    }
                },
                new MerchantModel
                {
                    MerchantId = Guid.NewGuid(),
                    MerchantName = "Merchant2",
                    MerchantReference = "Reference2",
                    Address = new AddressModel
                    {
                        AddressLine1 = "Address Line 2",
                        Town = "Town2",
                        Region = "Region2",
                        Country = "Country2",
                        PostalCode = "PostalCode2"
                    },
                    Contact = new ContactModel
                    {
                        ContactName = "Contact2",
                        ContactPhoneNumber = "0987654321",
                        ContactEmailAddress = "contact2@example.com"
                    },
                    SettlementSchedule = "Monthly",
                    Operators = new List<MerchantOperatorModel>
                    {
                        new MerchantOperatorModel
                        {
                            OperatorId = Guid.NewGuid(),
                            Name = "Operator2",
                            MerchantNumber = "654321",
                            TerminalNumber = "0987",
                            IsDeleted = false
                        }
                    },
                    Contracts = new List<MerchantContractModel>
                    {
                        new MerchantContractModel
                        {
                            ContractId = Guid.NewGuid(),
                            Name = "Contract2",
                            IsDeleted = false
                        }
                    },
                    Devices = new Dictionary<Guid, string>
                    {
                        { Guid.NewGuid(), "Device2" }
                    }
                }
            };
        }

        public static List<OperatorModel> GetOperators()
        {
            return new List<OperatorModel>
            {
                new OperatorModel
                {
                    OperatorId = Guid.NewGuid(),
                    Name = "Operator1",
                    RequireCustomMerchantNumber = true,
                    RequireCustomTerminalNumber = false
                },
                new OperatorModel
                {
                    OperatorId = Guid.NewGuid(),
                    Name = "Operator2",
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = true
                }
            };
        }

        public static List<ComparisonDateModel> GetComparisonDates()
        {
            return new List<ComparisonDateModel>
            {
                new ComparisonDateModel
                {
                    Date = new DateTime(2024, 1, 1),
                    Description = "2024-01-01",
                    OrderValue = 1
                },
                new ComparisonDateModel
                {
                    Date = new DateTime(2024, 1, 2),
                    Description = "2024-01-02",
                    OrderValue = 2
                }
            };
        }

        public static TodaysSalesModel GetTodaysSalesModel()
        {
            return new TodaysSalesModel
            {
                TodaysSalesValue = 1000m,
                ComparisonSalesValue = 800m
            };
        }

        public static TodaysSettlementModel GetTodaysSettlementModel()
        {
            return new TodaysSettlementModel
            {
                TodaysSettlementValue = 500m,
                ComparisonSettlementValue = 400m
            };
        }

        public static List<TodaysSalesCountByHourModel> GetTodaysSalesCountByHourModels()
        {
            return new List<TodaysSalesCountByHourModel>
        {
            new TodaysSalesCountByHourModel { Hour = 1, TodaysSalesCount = 10, ComparisonSalesCount = 8 },
            new TodaysSalesCountByHourModel { Hour = 2, TodaysSalesCount = 20, ComparisonSalesCount = 15 }
        };
        }

        public static List<TodaysSalesValueByHourModel> GetTodaysSalesValueByHourModels()
        {
            return new List<TodaysSalesValueByHourModel>
        {
            new TodaysSalesValueByHourModel { Hour = 1, TodaysSalesValue = 100m, ComparisonSalesValue = 80m },
            new TodaysSalesValueByHourModel { Hour = 2, TodaysSalesValue = 200m, ComparisonSalesValue = 150m }
        };
        }
    }
}