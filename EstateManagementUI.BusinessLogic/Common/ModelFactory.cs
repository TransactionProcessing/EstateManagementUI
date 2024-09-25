using System.Runtime.CompilerServices;
using Azure;
using EstateManagement.DataTransferObjects.Requests.Merchant;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.DataTransferObjects.Responses;
using SimpleResults;
using FileLineProcessingResult = FileProcessor.DataTransferObjects.Responses.FileLineProcessingResult;
using SettlementSchedule = EstateManagement.DataTransferObjects.Responses.Merchant.SettlementSchedule;

namespace EstateManagementUI.BusinessLogic.Common;

public static class ModelFactory
{
    public static SettlementSchedule ConvertFrom(Models.SettlementSchedule settlementSchedule)
    {
        return settlementSchedule switch
        {
            Models.SettlementSchedule.Immediate => SettlementSchedule.Immediate,
            Models.SettlementSchedule.Weekly => SettlementSchedule.Weekly,
            Models.SettlementSchedule.Monthly => SettlementSchedule.Monthly,
            _ => SettlementSchedule.Immediate
        };
    }

    public static CreateMerchantRequest ConvertFrom(CreateMerchantModel source)
    {
        if (source == null) {
            return null;
        }

        CreateMerchantRequest apiRequest = new CreateMerchantRequest
        {
            Address = new Address
            {
                AddressLine1 = source.Address.AddressLine1,
                AddressLine2 = source.Address.AddressLine2,
                AddressLine3 = source.Address.AddressLine3,
                AddressLine4 = source.Address.AddressLine4,
                Country = source.Address.Country,
                PostalCode = source.Address.PostalCode,
                Region = source.Address.Region,
                Town = source.Address.Town
            },
            Contact = new Contact
            {
                ContactName = source.Contact.ContactName,
                EmailAddress = source.Contact.ContactEmailAddress,
                PhoneNumber = source.Contact.ContactPhoneNumber
            },
            Name = source.MerchantName,
            SettlementSchedule = ConvertFrom(source.SettlementSchedule)
        };

        return apiRequest;
    }
    public static List<ComparisonDateModel> ConvertFrom(List<ComparisonDate> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<ComparisonDateModel> models = new List<ComparisonDateModel>();
        source.ForEach(s => {
            models.Add(new ComparisonDateModel
            {
                Date = s.Date,
                Description = s.Description,
                OrderValue = s.OrderValue
            });
        });

        return models;
    }
    public static EstateModel ConvertFrom(EstateResponse source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        EstateModel model = new EstateModel
        {
            EstateId = source.EstateId,
            EstateName = source.EstateName,
            Reference = source.EstateReference,
            Operators = ConvertOperators(source.Operators),
            SecurityUsers = ConvertSecurityUsers(source.SecurityUsers)
        };
        return model;
    }

    public static List<MerchantModel> ConvertFrom(List<MerchantResponse> source) {
        if (source == null || source.Any() == false) {
            return new List<MerchantModel>();
        }

        List<MerchantModel> models = new List<MerchantModel>();
        foreach (MerchantResponse merchantResponse in source) {
            MerchantModel model = new MerchantModel {
                MerchantId = merchantResponse.MerchantId, 
                MerchantName = merchantResponse.MerchantName,
                MerchantReference = merchantResponse.MerchantReference,
                SettlementSchedule = merchantResponse.SettlementSchedule.ToString(),
                AddressLine1 = merchantResponse.Addresses.FirstOrDefault().AddressLine1,
                ContactName = merchantResponse.Contacts.FirstOrDefault().ContactName,
                Town = merchantResponse.Addresses.FirstOrDefault().Town
            };
            models.Add(model);
        }
        
        return models;
    }

    public static List<EstateOperatorModel> ConvertOperators(List<EstateOperatorResponse> estateResponseOperators)
    {
        if (estateResponseOperators == null || estateResponseOperators.Any() == false)
        {
            return null;
        }

        List<EstateOperatorModel> models = new List<EstateOperatorModel>();
        foreach (EstateOperatorResponse estateOperatorResponse in estateResponseOperators)
        {
            models.Add(new EstateOperatorModel
            {
                Name = estateOperatorResponse.Name,
                OperatorId = estateOperatorResponse.OperatorId,
                RequireCustomMerchantNumber = estateOperatorResponse.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperatorResponse.RequireCustomTerminalNumber
            });
        }

        return models;
    }

    public static List<SecurityUserModel> ConvertSecurityUsers(List<SecurityUserResponse> estateResponseSecurityUsers)
    {
        if (estateResponseSecurityUsers == null || estateResponseSecurityUsers.Any() == false)
        {
            return null;
        }

        List<SecurityUserModel> models = new List<SecurityUserModel>();
        foreach (SecurityUserResponse estateResponseSecurityUser in estateResponseSecurityUsers)
        {
            models.Add(new SecurityUserModel
            {
                EmailAddress = estateResponseSecurityUser.EmailAddress,
                SecurityUserId = estateResponseSecurityUser.SecurityUserId
            });
        }

        return models;
    }

    public static List<OperatorModel> ConvertFrom(List<OperatorResponse> operators) {
        if (operators == null || operators.Any() == false)
        {
            return new List<OperatorModel>();
        }

        List<OperatorModel> models = new List<OperatorModel>();
        foreach (OperatorResponse @operator in operators)
        {
            models.Add(ConvertFrom(@operator));
        }

        return models;
    }

    public static ContractModel ConvertFrom(ContractResponse contract) {
        ContractModel model = new ContractModel {
            Description = contract.Description,
            OperatorName = contract.OperatorName,
            ContractId = contract.ContractId,
        };

        if (contract.Products != null && contract.Products.Any()) {
            model.ContractProducts = new List<ContractProductModel>();
            model.NumberOfProducts = contract.Products.Count;
            // Convert the products as well
            foreach (ContractProduct contractProduct in contract.Products) {
                model.ContractProducts.Add(ConvertFrom(contractProduct));
            }
        }
        else {
            model.ContractProducts = new List<ContractProductModel>();
        }
        
        return model;
    }

    public static ContractProductModel ConvertFrom(ContractProduct contractProduct) {
        ContractProductModel model = new ContractProductModel {
            ProductName = contractProduct.Name,
            ContractProductId = contractProduct.ProductId,
            DisplayText = contractProduct.DisplayText,
            ProductType = contractProduct.ProductType.ToString(),
            Value = contractProduct.Value.HasValue ? contractProduct.Value.Value.ToString() : "Variable",
        };

        if (contractProduct.TransactionFees != null && contractProduct.TransactionFees.Any()) {
            // TODO: Convert the fees
            model.NumberOfFees = contractProduct.TransactionFees.Count;
            model.ContractProductTransactionFees = new List<ContractProductTransactionFeeModel>();
            foreach (ContractProductTransactionFee contractProductTransactionFee in contractProduct.TransactionFees) {
                model.ContractProductTransactionFees.Add(ConvertFrom(contractProductTransactionFee));
            }
        }
        else {
            model.ContractProductTransactionFees = new List<ContractProductTransactionFeeModel>();
        }
        return model;

    }

    public static ContractProductTransactionFeeModel ConvertFrom(ContractProductTransactionFee transactionFee) {
        ContractProductTransactionFeeModel model = new ContractProductTransactionFeeModel {
            Description = transactionFee.Description,
            Value = transactionFee.Value,
            CalculationType = transactionFee.CalculationType.ToString(),
            ContractProductTransactionFeeId = transactionFee.TransactionFeeId,
            FeeType = transactionFee.FeeType.ToString()
        };
        return model;
    }

    public static List<ContractModel> ConvertFrom(List<ContractResponse> contracts)
    {
        if (contracts == null || contracts.Any() == false)
        {
            return new List<ContractModel>();
        }

        List<ContractModel> models = new List<ContractModel>();
        foreach (ContractResponse contract in contracts)
        {
            models.Add(ConvertFrom(contract));
        }

        return models;
    }

    public static OperatorModel ConvertFrom(OperatorResponse @operator) {

        if (@operator == null)
            return null;

        OperatorModel model = new OperatorModel {
            Name = @operator.Name,
            RequireCustomTerminalNumber = @operator.RequireCustomTerminalNumber,
            RequireCustomMerchantNumber = @operator.RequireCustomMerchantNumber,
            OperatorId = @operator.OperatorId
        };
        return model;
    }

    public static List<FileImportLogModel> ConvertFrom(FileImportLogList source)
    {
        if (source == null) {
            return new List<FileImportLogModel>();
        }

        List<FileImportLogModel> models = new List<FileImportLogModel>();

        if (source.FileImportLogs.Any())
        {
            foreach (FileImportLog sourceFileImportLog in source.FileImportLogs)
            {
                models.Add(ConvertFrom(sourceFileImportLog));
            }
        }

        return models;
    }

    public static FileImportLogModel ConvertFrom(FileImportLog source)
    {
        FileImportLogModel model = new FileImportLogModel
        {
            FileCount = source.FileCount,
            FileImportLogId = source.FileImportLogId,
            ImportLogDate = source.ImportLogDate,
            ImportLogDateTime = source.ImportLogDateTime,
            ImportLogTime = source.ImportLogTime
        };

        if (source.Files.Any())
        {
            model.Files = new List<FileImportLogFileModel>();
            foreach (FileImportLogFile fileImportLogFile in source.Files)
            {
                model.Files.Add(new FileImportLogFileModel
                {
                    FileImportLogId = fileImportLogFile.FileImportLogId,
                    FileId = fileImportLogFile.FileId,
                    FilePath = fileImportLogFile.FilePath,
                    FileProfileId = fileImportLogFile.FileProfileId,
                    FileUploadedDateTime = fileImportLogFile.FileUploadedDateTime,
                    MerchantId = fileImportLogFile.MerchantId,
                    OriginalFileName = fileImportLogFile.OriginalFileName,
                    UserId = fileImportLogFile.UserId
                });
            }
        }

        return model;
    }

    public static FileDetailsModel ConvertFrom(FileDetails source) {
        FileDetailsModel model = new FileDetailsModel {
            EstateId = source.EstateId,
            FileId = source.FileId,
            FileImportLogId = source.FileImportLogId,
            FileLocation = source.FileLocation,
            FileProfileId = source.FileProfileId,
            FileProfileName = source.FileProfileName,
            MerchantId = source.MerchantId,
            MerchantName = source.MerchantName,
            ProcessingCompleted = source.ProcessingCompleted,
            UserEmailAddress = source.UserEmailAddress,
            UserId = source.UserId,
            FileLines = new List<FileLineModel>(),
            ProcessingSummary = new FileProcessingSummaryModel {
                FailedLines = source.ProcessingSummary.FailedLines,
                IgnoredLines = source.ProcessingSummary.IgnoredLines,
                NotProcessedLines = source.ProcessingSummary.NotProcessedLines,
                RejectedLines = source.ProcessingSummary.RejectedLines,
                SuccessfullyProcessedLines = source.ProcessingSummary.SuccessfullyProcessedLines,
                TotalLines = source.ProcessingSummary.TotalLines
            }
        };

        foreach (FileLine sourceFileLine in source.FileLines) {
            model.FileLines.Add(new FileLineModel {
                LineData = sourceFileLine.LineData,
                LineNumber = sourceFileLine.LineNumber,
                RejectionReason = sourceFileLine.RejectionReason,
                TransactionId = sourceFileLine.TransactionId,
                ProcessingResult = sourceFileLine.ProcessingResult switch {
                    FileLineProcessingResult.Failed => Models.FileLineProcessingResult.Failed,
                    FileLineProcessingResult.Ignored => Models.FileLineProcessingResult.Ignored,
                    FileLineProcessingResult.NotProcessed => Models.FileLineProcessingResult.NotProcessed,
                    FileLineProcessingResult.Rejected => Models.FileLineProcessingResult.Rejected,
                    FileLineProcessingResult.Successful => Models.FileLineProcessingResult.Successful,
                    _ => Models.FileLineProcessingResult.Unknown
                }
            });
        }

        return model;
    }

    public static TodaysSettlementModel ConvertFrom(TodaysSettlement source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        TodaysSettlementModel model = new TodaysSettlementModel
        {
            ComparisonSettlementCount = source.ComparisonSettlementCount,
            ComparisonSettlementValue = source.ComparisonSettlementValue,
            TodaysSettlementCount = source.TodaysSettlementCount,
            TodaysSettlementValue = source.TodaysSettlementValue
        };
        return model;
    }

    public static TodaysSalesModel ConvertFrom(TodaysSales source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        TodaysSalesModel model = new TodaysSalesModel
        {
            TodaysSalesCount = source.TodaysSalesCount,
            ComparisonSalesCount = source.ComparisonSalesCount,
            ComparisonSalesValue = source.ComparisonSalesValue,
            TodaysSalesValue = source.TodaysSalesValue
        };
        return model;
    }

    public static List<TodaysSalesCountByHourModel> ConvertFrom(List<TodaysSalesCountByHour> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<TodaysSalesCountByHourModel> models = new List<TodaysSalesCountByHourModel>();

        source.ForEach(s => {
            models.Add(new TodaysSalesCountByHourModel
            {
                ComparisonSalesCount = s.ComparisonSalesCount,
                Hour = s.Hour,
                TodaysSalesCount = s.TodaysSalesCount,
            });
        });
        return models;
    }

    public static List<TodaysSalesValueByHourModel> ConvertFrom(List<TodaysSalesValueByHour> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<TodaysSalesValueByHourModel> models = new List<TodaysSalesValueByHourModel>();
        source.ForEach(s => {
            models.Add(new TodaysSalesValueByHourModel
            {
                ComparisonSalesValue = s.ComparisonSalesValue,
                Hour = s.Hour,
                TodaysSalesValue = s.TodaysSalesValue,
            });
        });
        return models;
    }

    public static MerchantKpiModel ConvertFrom(MerchantKpi source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        MerchantKpiModel model = new MerchantKpiModel
        {
            MerchantsWithNoSaleInLast7Days = source.MerchantsWithNoSaleInLast7Days,
            MerchantsWithNoSaleToday = source.MerchantsWithNoSaleToday,
            MerchantsWithSaleInLastHour = source.MerchantsWithSaleInLastHour
        };
        return model;
    }

    public static List<TopBottomOperatorDataModel> ConvertFrom(List<TopBottomOperatorData> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<TopBottomOperatorDataModel> models = new List<TopBottomOperatorDataModel>();
        source.ForEach(s => {
            models.Add(new TopBottomOperatorDataModel
            {
                SalesValue = s.SalesValue,
                OperatorName = s.OperatorName,
            });
        });
        return models;
    }

    public static List<TopBottomMerchantDataModel> ConvertFrom(List<TopBottomMerchantData> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<TopBottomMerchantDataModel> models = new List<TopBottomMerchantDataModel>();
        source.ForEach(s => {
            models.Add(new TopBottomMerchantDataModel
            {
                SalesValue = s.SalesValue,
                MerchantName = s.MerchantName,
            });
        });
        return models;
    }

    public static List<TopBottomProductDataModel> ConvertFrom(List<TopBottomProductData> source)
    {
        if (source == null || source.Any() == false)
        {
            return null;
        }

        List<TopBottomProductDataModel> models = new List<TopBottomProductDataModel>();
        source.ForEach(s => {
            models.Add(new TopBottomProductDataModel
            {
                SalesValue = s.SalesValue,
                ProductName = s.ProductName,
            });
        });
        return models;
    }

    public static LastSettlementModel ConvertFrom(LastSettlement source)
    {
        if (source == null)
        {
            return null;
        }

        LastSettlementModel model = new LastSettlementModel
        {
            FeesValue = source.FeesValue,
            SalesCount = source.SalesCount,
            SalesValue = source.SalesValue,
            SettlementDate = source.SettlementDate
        };
        return model;
    }
}