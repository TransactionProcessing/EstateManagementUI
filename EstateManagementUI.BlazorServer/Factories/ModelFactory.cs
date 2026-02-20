using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Models;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using ComparisonDateModel = EstateManagementUI.BlazorServer.Models.ComparisonDateModel;
using ContractDropDownModel = EstateManagementUI.BlazorServer.Models.ContractModels.ContractDropDownModel;
using OperatorDropDownModel = EstateManagementUI.BlazorServer.Models.OperatorModels.OperatorDropDownModel;
using ContractModel = EstateManagementUI.BlazorServer.Models.ContractModels.ContractModel;
using ContractProductModel = EstateManagementUI.BlazorServer.Models.ContractModels.ContractProductModel;
using ContractProductTransactionFeeModel = EstateManagementUI.BlazorServer.Models.ContractModels.ContractProductTransactionFeeModel;
using FileDetailsModel = EstateManagementUI.BlazorServer.Models.FileDetailsModel;
using FileImportLogModel = EstateManagementUI.BlazorServer.Models.FileImportLogModel;
using MerchantContractModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantContractModel;
using MerchantContractProductModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantContractProductModel;
using MerchantDeviceModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantDeviceModel;
using MerchantDropDownModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantDropDownModel;
using MerchantKpiModel = EstateManagementUI.BlazorServer.Models.TransactionModels.MerchantKpiModel;
using MerchantListModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantListModel;
using MerchantModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantModel;
using MerchantOperatorModel = EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantOperatorModel;
using MerchantSettlementHistoryModel = EstateManagementUI.BlazorServer.Models.MerchantSettlementHistoryModel;
using OperatorModel = EstateManagementUI.BlazorServer.Models.OperatorModels.OperatorModel;
//using OperatorTransactionSummaryModel = EstateManagementUI.BlazorServer.Models.OperatorTransactionSummaryModel;
using ProductPerformanceResponse = EstateManagementUI.BlazorServer.Models.TransactionModels.ProductPerformanceResponse;
using RecentContractModel = EstateManagementUI.BlazorServer.Models.ContractModels.RecentContractModel;
using RecentMerchantsModel = EstateManagementUI.BlazorServer.Models.MerchantModels.RecentMerchantsModel;
using SettlementSummaryModel = EstateManagementUI.BlazorServer.Models.SettlementSummaryModel;
using TodaysSalesModel = EstateManagementUI.BlazorServer.Models.TransactionModels.TodaysSalesModel;
using TodaysSettlementModel = EstateManagementUI.BlazorServer.Models.TransactionModels.TodaysSettlementModel;
using TransactionDetailModel = EstateManagementUI.BlazorServer.Models.TransactionModels.TransactionDetailModel;
using TransactionModels = EstateManagementUI.BlazorServer.Models.TransactionModels;

namespace EstateManagementUI.BlazorServer.Factories;

public static class ModelFactory {
    //public static EstateModel ConvertFrom(BusinessLogic.Models.EstateModels.EstateModel model) {
    //    EstateModel result = new EstateModel(model.EstateId, model.EstateName, model.Reference);
    //    result = result with { AllOperators = new List<OperatorDropDownModel>(), AssignedOperators = new List<OperatorModel>(), RecentContracts = new List<RecentContractModel>(), RecentMerchants = new List<RecentMerchantsModel>() };
    //    if (model.Operators != null && model.Operators.Any()) {
    //        model.Operators.ForEach((o) => result.Operators.Add(ConvertFrom(o)));
    //    }
    //    if (model.Merchants != null && model.Merchants.Any()) {
    //        model.Merchants.ForEach((m) => result.Merchants.Add(ConvertFrom(m)));
    //    }
    //    if (model.Contracts != null && model.Contracts.Any()) {
    //        model.Contracts.ForEach((m) => result.Contracts.Add(ConvertFrom(m)));
    //    }
    //    if (model.Users != null && model.Users.Any())
    //    {
    //        model.Users.ForEach((m) => result.Users.Add(ConvertFrom(m)));
    //    }

    //    return result;
    //}

    private static EstateModels.EstateUserModel ConvertFrom(BusinessLogic.Models.EstateModels.EstateUserModel model) {
        return new EstateModels.EstateUserModel() { CreatedDateTime = model.CreatedDateTime, EmailAddress = model.EmailAddress, UserId = model.UserId };
    }

    private static EstateModels.EstateContractModel ConvertFrom(BusinessLogic.Models.EstateModels.EstateContractModel model) {
        return new EstateModels.EstateContractModel() { ContractId = model.ContractId, Name = model.Name, OperatorId = model.OperatorId, OperatorName = model.OperatorName };
    }

    private static EstateModels.EstateMerchantModel ConvertFrom(BusinessLogic.Models.EstateModels.EstateMerchantModel model) {
        return new EstateModels.EstateMerchantModel() { Reference = model.Reference, Name = model.Name, MerchantId = model.MerchantId };
    }

    public static EstateModels.EstateOperatorModel ConvertFrom(BusinessLogic.Models.EstateModels.EstateOperatorModel model) {
        return new EstateModels.EstateOperatorModel() { Name = model.Name, OperatorId = model.OperatorId, RequireCustomMerchantNumber = model.RequireCustomMerchantNumber, RequireCustomTerminalNumber = model.RequireCustomTerminalNumber };
    }

    public static MerchantModel ConvertFrom(BusinessLogic.Models.MerchantModels.MerchantModel model) {
        return new MerchantModel() {
            MerchantId = model.MerchantId,
            MerchantName = model.MerchantName,
            MerchantReference = model.MerchantReference,
            Balance = model.Balance,
            AvailableBalance = model.AvailableBalance,
            SettlementSchedule = model.SettlementSchedule,
            AddressLine1 = model.AddressLine1,
            AddressLine2 = model.AddressLine2,
            Town = model.Town,
            Region = model.Region,
            PostalCode = model.PostalCode,
            Country = model.Country,
            ContactName = model.ContactName,
            ContactEmailAddress = model.ContactEmailAddress,
            ContactPhoneNumber = model.ContactPhoneNumber,
            AddressId = model.AddressId,
            ContactId = model.ContactId
        };
    }

    public static List<OperatorModel> ConvertFrom(List<BusinessLogic.Models.OperatorModels.OperatorModel> models)
    {
        List<OperatorModel> result = new List<OperatorModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static OperatorModel ConvertFrom(BusinessLogic.Models.OperatorModels.OperatorModel model) {
        return new OperatorModel() { OperatorId = model.OperatorId, Name = model.Name, RequireCustomMerchantNumber = model.RequireCustomMerchantNumber, RequireCustomTerminalNumber = model.RequireCustomTerminalNumber };
    }

    public static ContractModel ConvertFrom(BusinessLogic.Models.ContractModels.ContractModel model) {
        ContractModel result = new ContractModel() {
            ContractId = model.ContractId,
            Description = model.Description,
            OperatorId = model.OperatorId,
            OperatorName = model.OperatorName,
            Products = new List<ContractProductModel>()
        };
        if (model.Products != null && model.Products.Any()) {
            model.Products.ForEach(p => result.Products.Add(ConvertFrom(p)));
        }

        return result;
    }

    public static List<ContractProductModel> ConvertFrom(List<BusinessLogic.Models.ContractModels.ContractProductModel> models)
    {
        List<ContractProductModel> result = new List<ContractProductModel>();

        models.ForEach(p => result.Add(ConvertFrom(p)));

        return result;
    }

    public static ContractProductModel ConvertFrom(BusinessLogic.Models.ContractModels.ContractProductModel model) {
        var result = new ContractProductModel() {
            ContractProductId = model.ContractProductId,
            ProductName = model.ProductName,
            DisplayText = model.DisplayText,
            Value = model.Value,
            NumberOfFees = model.NumberOfFees,
            ProductType = Enum.Parse<ProductType>(model.ProductType),
            TransactionFees = new List<ContractProductTransactionFeeModel>()
        };
        if (model.TransactionFees != null && model.TransactionFees.Any()) {
            model.TransactionFees.ForEach(f => result.TransactionFees.Add(ConvertFrom(f)));
        }

        return result;
    }

    public static ContractProductTransactionFeeModel ConvertFrom(BusinessLogic.Models.ContractModels.ContractProductTransactionFeeModel model) {
        return new ContractProductTransactionFeeModel() {
            CalculationType = (CalculationType)model.CalculationType,
            Description = model.Description,
            FeeType = (FeeType)model.FeeType,
            TransactionFeeId = model.TransactionFeeId,
            Value = model.Value
        };
    }

    public static FileImportLogModel ConvertFrom(BusinessLogic.Models.FileImportLogModel model) {
        return new FileImportLogModel() { FileImportLogId = model.FileImportLogId, FileCount = model.FileCount, FileUploadedDateTime = model.FileUploadedDateTime, ImportLogDateTime = model.ImportLogDateTime };
    }

    public static FileDetailsModel ConvertFrom(BusinessLogic.Models.FileDetailsModel model) {
        return new FileDetailsModel() {
            FileId = model.FileId,
            FileUploadedDateTime = model.FileUploadedDateTime,
            FailedLines = model.FailedLines,
            FileLocation = model.FileLocation,
            FileProfileName = model.FileProfileName,
            IgnoredLines = model.IgnoredLines,
            MerchantName = model.MerchantName,
            ProcessingCompletedDateTime = model.ProcessingCompletedDateTime,
            SuccessfullyProcessedLines = model.SuccessfullyProcessedLines,
            TotalLines = model.TotalLines,
            UserId = model.UserId
        };
    }

    public static ComparisonDateModel ConvertFrom(BusinessLogic.Models.ComparisonDateModel model) {
        return new ComparisonDateModel() { Description = model.Description, Date = model.Date };
    }

    public static TodaysSalesModel ConvertFrom(BusinessLogic.Models.TodaysSalesModel model) {
        return new TodaysSalesModel() {
            ComparisonAverageValue = model.ComparisonAverageValue,
            ComparisonSalesCount = model.ComparisonSalesCount,
            ComparisonSalesValue = model.ComparisonSalesValue,
            TodaysAverageValue = model.TodaysAverageValue,
            TodaysSalesCount = model.TodaysSalesCount,
            TodaysSalesValue = model.TodaysSalesValue
        };
    }

    public static TodaysSettlementModel ConvertFrom(BusinessLogic.Models.TodaysSettlementModel model) {
        return new TodaysSettlementModel() {
            ComparisonSettlementCount = model.ComparisonSettlementCount,
            ComparisonSettlementValue = model.ComparisonSettlementValue,
            TodaysSettlementCount = model.TodaysSettlementCount,
            TodaysSettlementValue = model.TodaysSettlementValue,
            ComparisonPendingSettlementCount = model.ComparisonPendingSettlementCount,
            ComparisonPendingSettlementValue = model.ComparisonPendingSettlementValue,
            TodaysPendingSettlementCount = model.TodaysPendingSettlementCount,
            TodaysPendingSettlementValue = model.TodaysPendingSettlementValue
        };
    }
    
    public static MerchantKpiModel ConvertFrom(BusinessLogic.Models.MerchantModels.MerchantKpiModel model) {
        return new MerchantKpiModel() { MerchantsWithNoSaleInLast7Days = model.MerchantsWithNoSaleInLast7Days, MerchantsWithNoSaleToday = model.MerchantsWithNoSaleToday, MerchantsWithSaleInLastHour = model.MerchantsWithSaleInLastHour };
    }

    public static List<SettlementSummaryModel> ConvertFrom(List<BusinessLogic.Models.SettlementSummaryModel> models) {
        List<SettlementSummaryModel> result = new List<SettlementSummaryModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static SettlementSummaryModel ConvertFrom(BusinessLogic.Models.SettlementSummaryModel model) {
        return new SettlementSummaryModel() {
            MerchantName = model.MerchantName,
            GrossTransactionValue = model.GrossTransactionValue,
            MerchantId = model.MerchantId,
            NetSettlementAmount = model.NetSettlementAmount,
            SettlementPeriodEnd = model.SettlementPeriodEnd,
            SettlementPeriodStart = model.SettlementPeriodStart,
            TotalFees = model.TotalFees,
            SettlementStatus = model.SettlementStatus
        };
    }

    public static List<MerchantModel> ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantModel> models) {
        List<MerchantModel> result = new List<MerchantModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static List<ComparisonDateModel> ConvertFrom(List<BusinessLogic.Models.ComparisonDateModel> models) {
        List<ComparisonDateModel> result = new List<ComparisonDateModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static List<ContractModel> ConvertFrom(List<BusinessLogic.Models.ContractModels.ContractModel> models) {
        List<ContractModel> result = new List<ContractModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static List<MerchantSettlementHistoryModel> ConvertFrom(List<BusinessLogic.Models.MerchantSettlementHistoryModel> models) {
        List<MerchantSettlementHistoryModel> result = new List<MerchantSettlementHistoryModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static MerchantSettlementHistoryModel ConvertFrom(BusinessLogic.Models.MerchantSettlementHistoryModel model) {
        return new MerchantSettlementHistoryModel() { TransactionCount = model.TransactionCount, NetAmountPaid = model.NetAmountPaid, SettlementDate = model.SettlementDate, SettlementReference = model.SettlementReference };
    }

    public static List<TransactionDetailModel> ConvertFrom(List<BusinessLogic.Models.TransactionDetailModel> models) {
        List<TransactionDetailModel> result = new List<TransactionDetailModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    private static TransactionDetailModel ConvertFrom(BusinessLogic.Models.TransactionDetailModel model) {
        return new TransactionDetailModel() {
            MerchantName = model.MerchantName,
            ProductName = model.ProductName,
            MerchantId = model.MerchantId,
            OperatorId = model.OperatorId,
            SettlementReference = model.SettlementReference,
            FeesCommission = model.FeesCommission,
            GrossAmount = model.GrossAmount,
            TransactionType = model.TransactionType,
            TransactionStatus = model.TransactionStatus,
            TransactionId = model.TransactionId,
            ProductId = model.ProductId,
            OperatorName = model.OperatorName,
            TransactionDateTime = model.TransactionDateTime,
            NetAmount = model.NetAmount,
            ResponseCode = model.ResponseCode,
            SettlementDateTime = model.SettlementDateTime
        };


    }

    public static List<OperatorTransactionSummaryModel> ConvertFrom(List<BusinessLogic.Models.OperatorTransactionSummaryModel> models) {
        List<OperatorTransactionSummaryModel> result = new List<OperatorTransactionSummaryModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    private static OperatorTransactionSummaryModel ConvertFrom(BusinessLogic.Models.OperatorTransactionSummaryModel model) {
        return new OperatorTransactionSummaryModel() {
            AverageTransactionValue = model.AverageTransactionValue,
            FailedTransactionCount = model.FailedTransactionCount,
            OperatorId = model.OperatorId,
            OperatorName = model.OperatorName,
            SuccessfulTransactionCount = model.SuccessfulTransactionCount,
            TotalFeesEarned = model.TotalFeesEarned,
            TotalTransactionCount = model.TotalTransactionCount,
            TotalTransactionValue = model.TotalTransactionValue
        };
    }

    //public static List<MerchantTransactionSummaryModel> ConvertFrom(List<BusinessLogic.Models.MerchantTransactionSummaryModel> models) {
    //    List<MerchantTransactionSummaryModel> result = new List<MerchantTransactionSummaryModel>();
    //    models.ForEach(m => result.Add(ConvertFrom(m)));
    //    return result;
    //}

    //private static MerchantTransactionSummaryModel ConvertFrom(BusinessLogic.Models.MerchantTransactionSummaryModel model) {
    //    return new MerchantTransactionSummaryModel() {
    //        MerchantName = model.MerchantName,
    //        TotalTransactionCount = model.TotalTransactionCount,
    //        TotalTransactionValue = model.TotalTransactionValue,
    //        MerchantId = model.MerchantId,
    //        AverageTransactionValue = model.AverageTransactionValue,
    //        FailedTransactionCount = model.FailedTransactionCount,
    //        SuccessfulTransactionCount = model.SuccessfulTransactionCount
    //    };
    //}

    public static List<FileImportLogModel> ConvertFrom(List<BusinessLogic.Models.FileImportLogModel> models) {
        List<FileImportLogModel> result = new List<FileImportLogModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    public static List<RecentMerchantsModel> ConvertFrom(List<BusinessLogic.Models.MerchantModels.RecentMerchantsModel> models) {
        List<RecentMerchantsModel> result = new List<RecentMerchantsModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }

    private static RecentMerchantsModel ConvertFrom(BusinessLogic.Models.MerchantModels.RecentMerchantsModel model) {
        RecentMerchantsModel result = new RecentMerchantsModel() {
            CreatedDateTime = model.CreatedDateTime,
            MerchantId = model.MerchantId,
            Name = model.Name,
            Reference = model.Reference
        };
        return result;
    }

    public static List<RecentContractModel> ConvertFrom(List<BusinessLogic.Models.ContractModels.RecentContractModel> models) {
        List<RecentContractModel> result = new List<RecentContractModel>();
        models.ForEach(m => result.Add(ConvertFrom(m)));
        return result;
    }
        
    private static RecentContractModel ConvertFrom(BusinessLogic.Models.ContractModels.RecentContractModel model) {
        return new RecentContractModel { OperatorName = model.OperatorName, Description = model.Description, ContractId = model.ContractId };
    }

    public static List<MerchantListModel>? ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantListModel> resultData) {
        List<MerchantListModel> merchantList = new();
        foreach (BusinessLogic.Models.MerchantModels.MerchantListModel merchantListModel in resultData) {
            merchantList.Add(new MerchantListModel {
                CreatedDateTime = merchantListModel.CreatedDateTime,
                AvailableBalance = merchantListModel.AvailableBalance,
                Balance = merchantListModel.Balance,
                MerchantId = merchantListModel.MerchantId,
                MerchantName = merchantListModel.MerchantName,
                MerchantReference = merchantListModel.MerchantReference,
                Region = merchantListModel.Region,
                PostalCode = merchantListModel.PostalCode,
                SettlementSchedule = merchantListModel.SettlementSchedule
            });
        }

        return merchantList;
    }

    public static List<MerchantDropDownModel>? ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantDropDownModel> resultData) {
        List<MerchantDropDownModel> merchantList = new();
        foreach (BusinessLogic.Models.MerchantModels.MerchantDropDownModel merchantDropDownModel in resultData) {
            merchantList.Add(new MerchantDropDownModel {
                MerchantId = merchantDropDownModel.MerchantId,
                MerchantReportingId = merchantDropDownModel.MerchantReportingId,
                MerchantName = merchantDropDownModel.MerchantName
            });
        }
        return merchantList;
    }

    public static List<MerchantOperatorModel> ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantOperatorModel> resultData) {
        List<MerchantOperatorModel> merchantOperators = new();
        foreach (BusinessLogic.Models.MerchantModels.MerchantOperatorModel merchantOperatorModel in resultData)
        {
            merchantOperators.Add(new MerchantOperatorModel
            {
                OperatorName = merchantOperatorModel.OperatorName,
                OperatorId = merchantOperatorModel.OperatorId,
                MerchantNumber = merchantOperatorModel.MerchantNumber,
                TerminalNumber = merchantOperatorModel.TerminalNumber
            });
        }
        return merchantOperators;
    }

    public static List<MerchantDeviceModel> ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantDeviceModel> resultData) {
        List<MerchantDeviceModel> deviceList = new();
        foreach (var merchantDevice in resultData)
        {
            deviceList.Add(new MerchantDeviceModel() {
                DeviceId = merchantDevice.DeviceId,
                DeviceIdentifier = merchantDevice.DeviceIdentifier,
                IsDeleted = merchantDevice.IsDeleted,
                MerchantId = merchantDevice.MerchantId
            });
        }

        return deviceList;
    }

    public static List<MerchantContractModel> ConvertFrom(List<BusinessLogic.Models.MerchantModels.MerchantContractModel> resultData) {
        List<MerchantContractModel> merchantContracts = new();
        foreach (BusinessLogic.Models.MerchantModels.MerchantContractModel merchantContractModel in resultData)
        {
            var cm = new MerchantContractModel
            {
                ContractId = merchantContractModel.ContractId,
                OperatorName = merchantContractModel.OperatorName,
                ContractName = merchantContractModel.ContractName,
                ContractProducts = new List<MerchantContractProductModel>()
            };
            foreach (BusinessLogic.Models.MerchantModels.MerchantContractProductModel merchantContractProductModel in merchantContractModel.ContractProducts)
            {
                cm.ContractProducts.Add(new MerchantContractProductModel
                {
                    ProductName = merchantContractProductModel.ProductName,
                    DisplayText = merchantContractProductModel.DisplayText,
                    ProductId = merchantContractProductModel.ProductId,
                    ProductType = merchantContractProductModel.ProductType,
                    ContractId = merchantContractProductModel.ContractId,
                    Value = merchantContractProductModel.Value,
                    MerchantId = merchantContractProductModel.MerchantId
                });
            }
            merchantContracts.Add(cm);
        }
        return merchantContracts;
    }

    public static List<ContractDropDownModel> ConvertFrom(List<BusinessLogic.Models.ContractModels.ContractDropDownModel> resultData) {
        List<ContractDropDownModel> contractList = new();
        foreach (BusinessLogic.Models.ContractModels.ContractDropDownModel contractDropDownModel in resultData) {
            contractList.Add(new ContractDropDownModel() {
                ContractId = contractDropDownModel.ContractId,
                Description = contractDropDownModel.Description,
                OperatorName = contractDropDownModel.OperatorName
            });
        }

        return contractList;
    }

    public static List<OperatorDropDownModel>? ConvertFrom(List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel> resultData) {
        List<OperatorDropDownModel> operatorList = new();
        foreach (BusinessLogic.Models.OperatorModels.OperatorDropDownModel operatorDropDownModel in resultData)
        {
            operatorList.Add(new OperatorDropDownModel()
            {
                OperatorId = operatorDropDownModel.OperatorId,
                OperatorReportingId = operatorDropDownModel.OperatorReportingId,
                OperatorName = operatorDropDownModel.OperatorName
            });
        }

        return operatorList;
    }

    public static TransactionModels.TransactionDetailReportResponse? ConvertFrom(BusinessLogic.Models.TransactionModels.TransactionDetailReportResponse resultData) {
        TransactionModels.TransactionDetailReportResponse model = new();
        model.Summary = new TransactionModels.TransactionDetailSummary {
            TotalFees = resultData.Summary.TotalFees,
            TotalValue = resultData.Summary.TotalValue,
            TransactionCount = resultData.Summary.TransactionCount
        };
        model.Transactions = new List<TransactionModels.TransactionDetail>();
        foreach (BusinessLogic.Models.TransactionModels.TransactionDetail resultDataTransaction in resultData.Transactions) {
            model.Transactions.Add(new TransactionModels.TransactionDetail {
                TotalFees = resultDataTransaction.TotalFees,
                Value = resultDataTransaction.Value,
                Status = resultDataTransaction.Status,
                Type = resultDataTransaction.Type,
                ProductReportingId = resultDataTransaction.ProductReportingId,
                ProductId = resultDataTransaction.ProductId,
                Product = resultDataTransaction.Product,
                OperatorReportingId = resultDataTransaction.OperatorReportingId,
                OperatorId = resultDataTransaction.OperatorId,
                Operator = resultDataTransaction.Operator,
                MerchantReportingId = resultDataTransaction.MerchantReportingId,
                MerchantId = resultDataTransaction.MerchantId,
                Merchant = resultDataTransaction.Merchant,
                DateTime = resultDataTransaction.DateTime,
                Id = resultDataTransaction.Id,
                SettlementReference = resultDataTransaction.SettlementReference,
                NetAmount = resultDataTransaction.Value - resultDataTransaction.TotalFees
            });
        }
        return model;
    }

    public static TransactionModels.TransactionSummaryByMerchantResponse ConvertFrom(BusinessLogic.Models.TransactionModels.TransactionSummaryByMerchantResponse resultData) {
        TransactionModels.TransactionSummaryByMerchantResponse model = new();
        model.Summary = new TransactionModels.MerchantDetailSummary {
            AverageValue = resultData.Summary.AverageValue,
            TotalCount = resultData.Summary.TotalCount,
            TotalMerchants = resultData.Summary.TotalMerchants,
            TotalValue = resultData.Summary.TotalValue
        };
        model.Merchants = new();
        foreach (BusinessLogic.Models.TransactionModels.MerchantDetail resultDataMerchant in resultData.Merchants) {
            model.Merchants.Add(new TransactionModels.MerchantDetail() {
                AverageValue = resultDataMerchant.AverageValue,
                DeclinedCount = resultDataMerchant.DeclinedCount,
                MerchantId = resultDataMerchant.MerchantId,
                MerchantReportingId = resultDataMerchant.MerchantReportingId,
                MerchantName = resultDataMerchant.MerchantName,
                AuthorisedPercentage = resultDataMerchant.AuthorisedPercentage,
                AuthorisedCount = resultDataMerchant.AuthorisedCount,
                TotalCount = resultDataMerchant.TotalCount,
                TotalValue= resultDataMerchant.TotalValue
            });
        }

        return model;
    }

    public static TransactionModels.TransactionSummaryByOperatorResponse ConvertFrom(BusinessLogic.Models.TransactionModels.TransactionSummaryByOperatorResponse resultData) {
        TransactionModels.TransactionSummaryByOperatorResponse model = new();
        model.Summary = new TransactionModels.OperatorDetailSummary
        {
            AverageValue = resultData.Summary.AverageValue,
            TotalCount = resultData.Summary.TotalCount,
            TotalOperators = resultData.Summary.TotalOperators,
            TotalValue = resultData.Summary.TotalValue
        };
        model.Operators = new();
        foreach (BusinessLogic.Models.TransactionModels.OperatorDetail resultDataOperator in resultData.Operators)
        {
            model.Operators.Add(new TransactionModels.OperatorDetail()
            {
                AverageValue = resultDataOperator.AverageValue,
                DeclinedCount = resultDataOperator.DeclinedCount,
                OperatorId = resultDataOperator.OperatorId,
                OperatorReportingId = resultDataOperator.OperatorReportingId,
                OperatorName = resultDataOperator.OperatorName,
                AuthorisedPercentage = resultDataOperator.AuthorisedPercentage,
                AuthorisedCount = resultDataOperator.AuthorisedCount,
                TotalCount = resultDataOperator.TotalCount,
                TotalValue = resultDataOperator.TotalValue
            });
        }

        return model;
    }

    public static ProductPerformanceResponse ConvertFrom(BusinessLogic.Models.TransactionModels.ProductPerformanceResponse resultData) {

        var model = new ProductPerformanceResponse { Summary = new TransactionModels.ProductPerformanceSummary() { TotalValue = resultData.Summary.TotalValue, TotalCount = resultData.Summary.TotalCount, AveragePerProduct = resultData.Summary.AveragePerProduct, TotalProducts = resultData.Summary.TotalProducts } };
        model.ProductDetails = new List<TransactionModels.ProductPerformanceDetail>();
        foreach (BusinessLogic.Models.TransactionModels.ProductPerformanceDetail productPerformanceDetail in resultData.ProductDetails) {
            model.ProductDetails.Add(new TransactionModels.ProductPerformanceDetail() {
                ProductId = productPerformanceDetail.ProductId,
                ProductName = productPerformanceDetail.ProductName,
                TransactionCount = productPerformanceDetail.TransactionCount,
                ContractId = productPerformanceDetail.ContractId,
                ContractReportingId = productPerformanceDetail.ContractReportingId,
                ProductReportingId = productPerformanceDetail.ProductReportingId,
                TransactionValue = productPerformanceDetail.TransactionValue,
                PercentageOfTotal = productPerformanceDetail.PercentageOfTotal,
            });
        }

        return model;
    }

    public static List<TransactionModels.TodaysSalesByHourModel>? ConvertFrom(List<BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel> resultData) {
        List<TransactionModels.TodaysSalesByHourModel> todaysSalesByHourModels = new();
        foreach (BusinessLogic.Models.TransactionModels.TodaysSalesByHourModel todaysSalesCountByHourModel in resultData) {
            todaysSalesByHourModels.Add(new TransactionModels.TodaysSalesByHourModel {
                Hour = todaysSalesCountByHourModel.Hour,
                TodaysSalesCount = todaysSalesCountByHourModel.TodaysSalesCount,
                ComparisonSalesCount = todaysSalesCountByHourModel.ComparisonSalesCount,
                ComparisonSalesValue = todaysSalesCountByHourModel.ComparisonSalesValue,
                TodaysSalesValue = todaysSalesCountByHourModel.TodaysSalesValue
            });
        }

        return todaysSalesByHourModels;
    }
}