
using EstateManagementUI.BlazorServer.Models;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

namespace EstateManagementUI.BlazorServer.Factories {
    public static class ModelFactory {
        public static EstateModel ConvertFrom(BusinessLogic.Models.EstateModel model) {
            EstateModel result = new EstateModel { EstateId = model.EstateId, EstateName = model.EstateName, 
                Operators = new List<EstateOperatorModel>(), 
                Contracts = new(),
                Merchants = new(),
                Users = new(),
                Reference = model.Reference };
            if (model.Operators != null && model.Operators.Any()) {
                model.Operators.ForEach((o) => result.Operators.Add(ConvertFrom(o)));
            }
            if (model.Merchants != null && model.Merchants.Any()) {
                model.Merchants.ForEach((m) => result.Merchants.Add(ConvertFrom(m)));
            }
            if (model.Contracts != null && model.Contracts.Any()) {
                model.Contracts.ForEach((m) => result.Contracts.Add(ConvertFrom(m)));
            }
            if (model.Users != null && model.Users.Any())
            {
                model.Users.ForEach((m) => result.Users.Add(ConvertFrom(m)));
            }

            return result;
        }

        private static EstateUserModel ConvertFrom(BusinessLogic.Models.EstateUserModel model) {
            return new EstateUserModel() { CreatedDateTime = model.CreatedDateTime, EmailAddress = model.EmailAddress, UserId = model.UserId };
        }

        private static EstateContractModel ConvertFrom(BusinessLogic.Models.EstateContractModel model) {
            return new EstateContractModel() { ContractId = model.ContractId, Name = model.Name, OperatorId = model.OperatorId, OperatorName = model.OperatorName };
        }

        private static EstateMerchantModel ConvertFrom(BusinessLogic.Models.EstateMerchantModel model) {
            return new EstateMerchantModel() { Reference = model.Reference, Name = model.Name, MerchantId = model.MerchantId };
        }

        public static EstateOperatorModel ConvertFrom(BusinessLogic.Models.EstateOperatorModel model) {
            return new EstateOperatorModel() { Name = model.Name, OperatorId = model.OperatorId, RequireCustomMerchantNumber = model.RequireCustomMerchantNumber, RequireCustomTerminalNumber = model.RequireCustomTerminalNumber };
        }

        public static MerchantModel ConvertFrom(BusinessLogic.Models.MerchantModel model) {
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

        public static List<OperatorModel> ConvertFrom(List<BusinessLogic.Models.OperatorModel> models)
        {
            List<OperatorModel> result = new List<OperatorModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static OperatorModel ConvertFrom(BusinessLogic.Models.OperatorModel model) {
            return new OperatorModel() { OperatorId = model.OperatorId, Name = model.Name, RequireCustomMerchantNumber = model.RequireCustomMerchantNumber, RequireCustomTerminalNumber = model.RequireCustomTerminalNumber };
        }

        public static ContractModel ConvertFrom(BusinessLogic.Models.ContractModel model) {
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

        public static List<ContractProductModel> ConvertFrom(List<BusinessLogic.Models.ContractProductModel> models)
        {
            List<ContractProductModel> result = new List<ContractProductModel>();

            models.ForEach(p => result.Add(ConvertFrom(p)));

            return result;
        }

        public static ContractProductModel ConvertFrom(BusinessLogic.Models.ContractProductModel model) {
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

        public static ContractProductTransactionFeeModel ConvertFrom(BusinessLogic.Models.ContractProductTransactionFeeModel model) {
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

        public static List<TodaysSalesCountByHourModel> ConvertFrom(List<BusinessLogic.Models.TodaysSalesCountByHourModel> models)
        {
            List<TodaysSalesCountByHourModel> result = new List<TodaysSalesCountByHourModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static List<TodaysSalesValueByHourModel> ConvertFrom(List<BusinessLogic.Models.TodaysSalesValueByHourModel> models)
        {
            List<TodaysSalesValueByHourModel> result = new List<TodaysSalesValueByHourModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static TodaysSalesCountByHourModel ConvertFrom(BusinessLogic.Models.TodaysSalesCountByHourModel model) {
            return new TodaysSalesCountByHourModel() { TodaysSalesCount = model.TodaysSalesCount, ComparisonSalesCount = model.ComparisonSalesCount, Hour = model.Hour };
        }

        public static TodaysSalesValueByHourModel ConvertFrom(BusinessLogic.Models.TodaysSalesValueByHourModel model) {
            return new TodaysSalesValueByHourModel() { TodaysSalesValue = model.TodaysSalesValue, ComparisonSalesValue = model.ComparisonSalesValue, Hour = model.Hour };
        }

        public static MerchantKpiModel ConvertFrom(BusinessLogic.Models.MerchantKpiModel model) {
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

        public static List<ProductPerformanceModel> ConvertFrom(List<BusinessLogic.Models.ProductPerformanceModel> models) {
            List<ProductPerformanceModel> result = new List<ProductPerformanceModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static ProductPerformanceModel ConvertFrom(BusinessLogic.Models.ProductPerformanceModel model) {
            return new ProductPerformanceModel() { PercentageContribution = model.PercentageContribution, ProductName = model.ProductName, TransactionCount = model.TransactionCount, TransactionValue = model.TransactionValue };
        }

        

        public static List<MerchantModel> ConvertFrom(List<BusinessLogic.Models.MerchantModel> models) {
            List<MerchantModel> result = new List<MerchantModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static List<ComparisonDateModel> ConvertFrom(List<BusinessLogic.Models.ComparisonDateModel> models) {
            List<ComparisonDateModel> result = new List<ComparisonDateModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static List<ContractModel> ConvertFrom(List<BusinessLogic.Models.ContractModel> models) {
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

        public static List<MerchantTransactionSummaryModel> ConvertFrom(List<BusinessLogic.Models.MerchantTransactionSummaryModel> models) {
            List<MerchantTransactionSummaryModel> result = new List<MerchantTransactionSummaryModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        private static MerchantTransactionSummaryModel ConvertFrom(BusinessLogic.Models.MerchantTransactionSummaryModel model) {
            return new MerchantTransactionSummaryModel() {
                MerchantName = model.MerchantName,
                TotalTransactionCount = model.TotalTransactionCount,
                TotalTransactionValue = model.TotalTransactionValue,
                MerchantId = model.MerchantId,
                AverageTransactionValue = model.AverageTransactionValue,
                FailedTransactionCount = model.FailedTransactionCount,
                SuccessfulTransactionCount = model.SuccessfulTransactionCount
            };
        }

        public static List<FileImportLogModel> ConvertFrom(List<BusinessLogic.Models.FileImportLogModel> models) {
            List<FileImportLogModel> result = new List<FileImportLogModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        public static List<RecentMerchantsModel> ConvertFrom(List<BusinessLogic.Models.RecentMerchantsModel> models) {
            List<RecentMerchantsModel> result = new List<RecentMerchantsModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }

        private static RecentMerchantsModel ConvertFrom(BusinessLogic.Models.RecentMerchantsModel model) {
            RecentMerchantsModel result = new RecentMerchantsModel() {
                CreatedDateTime = model.CreatedDateTime,
                MerchantId = model.MerchantId,
                Name = model.Name,
                Reference = model.Reference
            };
            return result;
        }

        public static List<RecentContractModel> ConvertFrom(List<BusinessLogic.Models.RecentContractModel> models) {
            List<RecentContractModel> result = new List<RecentContractModel>();
            models.ForEach(m => result.Add(ConvertFrom(m)));
            return result;
        }
        
        private static RecentContractModel ConvertFrom(BusinessLogic.Models.RecentContractModel model) {
            return new RecentContractModel { OperatorName = model.OperatorName, Description = model.Description, ContractId = model.ContractId };
        }

        public static List<MerchantListModel>? ConvertFrom(List<BusinessLogic.Models.MerchantListModel> resultData) {
            List<MerchantListModel> merchantList = new();
            foreach (BusinessLogic.Models.MerchantListModel merchantListModel in resultData) {
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

        public static List<MerchantDropDownModel>? ConvertFrom(List<BusinessLogic.Models.MerchantDropDownModel> resultData) {
            List<MerchantDropDownModel> merchantList = new();
            foreach (BusinessLogic.Models.MerchantDropDownModel merchantDropDownModel in resultData) {
                merchantList.Add(new MerchantDropDownModel {
                    MerchantId = merchantDropDownModel.MerchantId,
                    MerchantName = merchantDropDownModel.MerchantName
                });
            }
            return merchantList;
        }

        public static List<MerchantOperatorModel> ConvertFrom(List<BusinessLogic.Models.MerchantOperatorModel> resultData) {
            List<MerchantOperatorModel> merchantOperators = new();
            foreach (BusinessLogic.Models.MerchantOperatorModel merchantOperatorModel in resultData)
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

        public static List<MerchantDeviceModel> ConvertFrom(List<BusinessLogic.Models.MerchantDeviceModel> resultData) {
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

        public static List<MerchantContractModel> ConvertFrom(List<BusinessLogic.Models.MerchantContractModel> resultData) {
            List<MerchantContractModel> merchantContracts = new();
            foreach (BusinessLogic.Models.MerchantContractModel merchantContractModel in resultData)
            {
                var cm = new MerchantContractModel
                {
                    ContractId = merchantContractModel.ContractId,
                    OperatorName = merchantContractModel.OperatorName,
                    ContractName = merchantContractModel.ContractName,
                    ContractProducts = new List<MerchantContractProductModel>()
                };
                foreach (BusinessLogic.Models.MerchantContractProductModel merchantContractProductModel in merchantContractModel.ContractProducts)
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

        public static List<ContractDropDownModel> ConvertFrom(List<BusinessLogic.Models.ContractDropDownModel> resultData) {
            List<ContractDropDownModel> contractList = new();
            foreach (BusinessLogic.Models.ContractDropDownModel contractDropDownModel in resultData) {
                contractList.Add(new ContractDropDownModel() {
                    ContractId = contractDropDownModel.ContractId,
                    Description = contractDropDownModel.Description,
                    OperatorName = contractDropDownModel.OperatorName
                });
            }

            return contractList;
        }
    }
}