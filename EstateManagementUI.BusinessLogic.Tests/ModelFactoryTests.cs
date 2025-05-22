using EstateManagementUI.Testing;
using EstateManagementUI.BusinessLogic.Models;
using Shouldly;
using EstateManagementUI.BusinessLogic.Common;
using EstateReportingAPI.DataTransferObjects;
using FileProcessor.DataTransferObjects.Responses;
using EstateReportingAPI.DataTrasferObjects;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Operator;
using FileImportLogList = FileProcessor.DataTransferObjects.Responses.FileImportLogList;
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

namespace EstateManagementUI.BusinessLogic.Tests {
    public class ModelFactoryTests {

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSettlement_ModelIsConverted() {
            TodaysSettlement response = TestData.TodaysSettlement;

            TodaysSettlementModel model = ModelFactory.ConvertFrom(response);
            model.ComparisonSettlementValue.ShouldBe(response.ComparisonSettlementValue);
            model.ComparisonSettlementCount.ShouldBe(response.ComparisonSettlementCount);
            model.TodaysSettlementCount.ShouldBe(response.TodaysSettlementCount);
            model.TodaysSettlementValue.ShouldBe(response.TodaysSettlementValue);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSales_ResponseIsNull_ErrorThrown() {
            TodaysSales response = null;

            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(response); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSales_ModelIsConverted() {
            TodaysSales response = TestData.TodaysSales;

            TodaysSalesModel model = ModelFactory.ConvertFrom(response);
            model.ComparisonSalesValue.ShouldBe(response.ComparisonSalesValue);
            model.ComparisonSalesCount.ShouldBe(response.ComparisonSalesCount);
            model.TodaysSalesCount.ShouldBe(response.TodaysSalesCount);
            model.TodaysSalesValue.ShouldBe(response.TodaysSalesValue);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSettlement_ResponseIsNull_ErrorThrown() {
            TodaysSettlement response = null;

            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(response); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_EmptyOperators_ModelIsConverted() {
            EstateResponse response = TestData.EstateResponse;
            response.Operators = new List<EstateOperatorResponse>();

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.ShouldBeNull();
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_EmptySecurityUsers_ModelIsConverted() {
            EstateResponse response = TestData.EstateResponse;
            response.SecurityUsers = new List<SecurityUserResponse>();

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            model.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_ModelIsConverted() {
            EstateResponse response = TestData.EstateResponse;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullOperators_ModelIsConverted() {
            EstateResponse response = TestData.EstateResponse;
            response.Operators = null;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.ShouldBeNull();
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullResponse_ErrorThrown() {
            EstateResponse response = null;

            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(response); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullSecurityUsers_ModelIsConverted() {
            EstateResponse response = TestData.EstateResponse;
            response.SecurityUsers = null;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            model.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantResponsesList_ModelIsConverted() {
            List<MerchantResponse> response = TestData.MerchantResponses;

            List<MerchantModel> models = ModelFactory.ConvertFrom(response);

            foreach (MerchantResponse merchantResponse in response) {
                MerchantModel? model = models.SingleOrDefault(m => m.MerchantId == merchantResponse.MerchantId);
                model.ShouldNotBeNull();
                model.MerchantName.ShouldBe(merchantResponse.MerchantName.ToString());
                model.SettlementSchedule.ShouldBe(merchantResponse.SettlementSchedule.ToString());
                model.MerchantReference.ShouldBe(merchantResponse.MerchantReference.ToString());
                model.Address.AddressLine1.ShouldBe(merchantResponse.Addresses.First().AddressLine1);
                model.Address.Town.ShouldBe(merchantResponse.Addresses.First().Town);
                model.Contact.ContactName.ShouldBe(merchantResponse.Contacts.First().ContactName);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantResponsesList_NullList_ModelIsConverted() {
            List<MerchantResponse> response = null;

            List<MerchantModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantResponsesList_EmptyList_ModelIsConverted() {
            List<MerchantResponse> response = new List<MerchantResponse>();

            List<MerchantModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_OperatorResponsesList_ModelIsConverted() {
            List<OperatorResponse> response = TestData.OperatorResponses;

            List<OperatorModel> models = ModelFactory.ConvertFrom(response);

            foreach (OperatorResponse operatorResponse in response) {
                OperatorModel? model = models.SingleOrDefault(m => m.OperatorId == operatorResponse.OperatorId);
                model.ShouldNotBeNull();
                model.Name.ShouldBe(operatorResponse.Name);
                model.RequireCustomTerminalNumber.ShouldBe(operatorResponse.RequireCustomTerminalNumber);
                model.RequireCustomMerchantNumber.ShouldBe(operatorResponse.RequireCustomMerchantNumber);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_OperatorResponsesList_NullList_ModelIsConverted() {
            List<OperatorResponse> response = null;

            List<OperatorModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_OperatorResponsesList_EmptyList_ModelIsConverted() {
            List<OperatorResponse> response = new List<OperatorResponse>();

            List<OperatorModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_OperatorResponse_OperatorIsNull_ModelIsConverted() {
            OperatorResponse response = null;

            OperatorModel model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }


        [Fact]
        public void ModelFactory_ConvertFrom_ContractResponsesList_ModelIsConverted() {
            List<ContractResponse> response = TestData.ContractResponses;

            List<ContractModel> models = ModelFactory.ConvertFrom(response);

            foreach (ContractResponse contractResponse in response) {
                ContractModel? model = models.SingleOrDefault(m => m.ContractId == contractResponse.ContractId);
                model.ShouldNotBeNull();
                model.Description.ShouldBe(contractResponse.Description);
                model.OperatorName.ShouldBe(contractResponse.OperatorName);
                model.NumberOfProducts.ShouldBe(contractResponse.Products.Count);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ContractResponsesList_NullList_ModelIsConverted() {
            List<ContractResponse> response = null;

            List<ContractModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ContractResponsesList_EmptyList_ModelIsConverted() {
            List<ContractResponse> response = new List<ContractResponse>();

            List<ContractModel> models = ModelFactory.ConvertFrom(response);

            models.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ContractResponse_NullProducts_ModelIsConverted() {
            ContractResponse response = TestData.ContractResponseNullProducts;

            ContractModel model = ModelFactory.ConvertFrom(response);

            model.ShouldNotBeNull();
            model.Description.ShouldBe(response.Description);
            model.OperatorName.ShouldBe(response.OperatorName);
            model.NumberOfProducts.ShouldBe(0);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ContractResponse_EmptyProducts_ModelIsConverted() {
            ContractResponse response = TestData.ContractResponseEmptyProducts;

            ContractModel model = ModelFactory.ConvertFrom(response);

            model.ShouldNotBeNull();
            model.Description.ShouldBe(response.Description);
            model.OperatorName.ShouldBe(response.OperatorName);
            model.NumberOfProducts.ShouldBe(0);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ContractProduct_ModelIsConverted() {
            ContractProduct contractProduct = TestData.ContractProduct1;

            ContractProductModel model = ModelFactory.ConvertFrom(contractProduct);
            model.ShouldNotBeNull();
            model.Value.ShouldBe(contractProduct.Value.ToString());
            model.DisplayText.ShouldBe(contractProduct.DisplayText);
            model.ContractProductId.ShouldBe(contractProduct.ProductId);
            model.ProductName.ShouldBe(contractProduct.Name);
            model.ProductType.ShouldBe(contractProduct.ProductType.ToString());
            model.NumberOfFees.ShouldBe(contractProduct.TransactionFees.Count);

            foreach (ContractProductTransactionFee contractProductTransactionFee in contractProduct.TransactionFees) {
                ContractProductTransactionFeeModel? modelFee = model.ContractProductTransactionFees.SingleOrDefault(p => p.ContractProductTransactionFeeId == contractProductTransactionFee.TransactionFeeId);
                modelFee.ShouldNotBeNull();
                modelFee.Value.ShouldBe(contractProductTransactionFee.Value);
                modelFee.CalculationType.ShouldBe(contractProductTransactionFee.CalculationType.ToString());
                modelFee.Description.ShouldBe(contractProductTransactionFee.Description);
                modelFee.FeeType.ShouldBe(contractProductTransactionFee.FeeType.ToString());
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_FileImportLogList_ModelIsConverted() {
            FileImportLogList fileImportLogList = TestData.FileImportLogList;

            List<FileImportLogModel> model = ModelFactory.ConvertFrom(fileImportLogList);

            model.ShouldNotBeNull();
            model.Count.ShouldBe(fileImportLogList.FileImportLogs.Count);

            foreach (FileImportLog fileImportLog in fileImportLogList.FileImportLogs) {
                FileImportLogModel? fileImportLogModel = model.SingleOrDefault(m => m.FileImportLogId == fileImportLog.FileImportLogId);
                fileImportLogModel.ShouldNotBeNull();
                fileImportLogModel.ImportLogDateTime.ShouldBe(fileImportLog.ImportLogDateTime);
                fileImportLogModel.FileCount.ShouldBe(fileImportLog.FileCount);
                fileImportLogModel.ImportLogDate.ShouldBe(fileImportLog.ImportLogDate);
                fileImportLogModel.ImportLogTime.ShouldBe(fileImportLog.ImportLogTime);

                foreach (FileImportLogFile fileImportLogFile in fileImportLog.Files) {
                    FileImportLogFileModel? fileModel = fileImportLogModel.Files.SingleOrDefault(f => f.FileId == fileImportLogFile.FileId);
                    fileModel.ShouldNotBeNull();
                    fileModel.MerchantId.ShouldBe(fileImportLogFile.MerchantId);
                    fileModel.FilePath.ShouldBe(fileImportLogFile.FilePath);
                    fileModel.OriginalFileName.ShouldBe(fileImportLogFile.OriginalFileName);
                    fileModel.FileUploadedDateTime.ShouldBe(fileImportLogFile.FileUploadedDateTime);
                    fileModel.UserId.ShouldBe(fileImportLogFile.UserId);
                }
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_FileImportLogList_NullInput_ModelIsConverted() {
            FileImportLogList fileImportLogList = null;
            List<FileImportLogModel> model = ModelFactory.ConvertFrom(fileImportLogList);
            model.ShouldNotBeNull();
            model.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ComparisonDates_ModelIsConverted() {
            List<ComparisonDate> response = TestData.ComparisonDates;

            List<ComparisonDateModel> model = ModelFactory.ConvertFrom(response);

            foreach (ComparisonDate comparisonDate in response) {
                ComparisonDateModel? dateModel = model.SingleOrDefault(m => m.Date == comparisonDate.Date);
                dateModel.ShouldNotBeNull();
                dateModel.Description.ShouldBe(comparisonDate.Description);
                dateModel.OrderValue.ShouldBe(comparisonDate.OrderValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ComparisonDates_NullList_ModelIsConverted() {
            List<ComparisonDate> response = null;

            List<ComparisonDateModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_ComparisonDates_EmptyList_ModelIsConverted() {
            List<ComparisonDate> response = new List<ComparisonDate>();

            List<ComparisonDateModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesCountByHourModel_ModelIsConverted() {
            List<TodaysSalesCountByHour> response = TestData.TodaysSalesCountByHour;

            List<TodaysSalesCountByHourModel> model = ModelFactory.ConvertFrom(response);

            foreach (TodaysSalesCountByHour todaysSalesCountByHour in response) {
                TodaysSalesCountByHourModel? hourModel = model.SingleOrDefault(m => m.Hour == todaysSalesCountByHour.Hour);
                hourModel.ShouldNotBeNull();
                hourModel.TodaysSalesCount.ShouldBe(todaysSalesCountByHour.TodaysSalesCount);
                hourModel.ComparisonSalesCount.ShouldBe(todaysSalesCountByHour.ComparisonSalesCount);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesCountByHourModel_NullResponse_ModelIsConverted() {
            List<TodaysSalesCountByHour> response = null;

            List<TodaysSalesCountByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesCountByHourModel_EmptyList_ModelIsConverted() {
            List<TodaysSalesCountByHour> response = new List<TodaysSalesCountByHour>();

            List<TodaysSalesCountByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }




        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesValueByHourModel_ModelIsConverted() {
            List<TodaysSalesValueByHour> response = TestData.TodaysSalesValueByHour;

            List<TodaysSalesValueByHourModel> model = ModelFactory.ConvertFrom(response);

            foreach (TodaysSalesValueByHour todaysSalesValueByHour in response) {
                TodaysSalesValueByHourModel? hourModel = model.SingleOrDefault(m => m.Hour == todaysSalesValueByHour.Hour);
                hourModel.ShouldNotBeNull();
                hourModel.TodaysSalesValue.ShouldBe(todaysSalesValueByHour.TodaysSalesValue);
                hourModel.ComparisonSalesValue.ShouldBe(todaysSalesValueByHour.ComparisonSalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesValueByHourModel_NullResponse_ModelIsConverted() {
            List<TodaysSalesValueByHour> response = null;

            List<TodaysSalesValueByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesValueByHourModel_EmptyList_ModelIsConverted() {
            List<TodaysSalesValueByHour> response = new List<TodaysSalesValueByHour>();

            List<TodaysSalesValueByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantKpi_IsConverted() {

            MerchantKpi model = new MerchantKpi() { MerchantsWithNoSaleInLast7Days = 1, MerchantsWithNoSaleToday = 2, MerchantsWithSaleInLastHour = 3 };
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldNotBeNull();
            result.MerchantsWithNoSaleInLast7Days.ShouldBe(model.MerchantsWithNoSaleInLast7Days);
            result.MerchantsWithNoSaleToday.ShouldBe(model.MerchantsWithNoSaleToday);
            result.MerchantsWithSaleInLastHour.ShouldBe(model.MerchantsWithSaleInLastHour);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantKpi_ModelIsNull_ErrorThrown() {

            MerchantKpi model = null;
            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(model); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_IsConverted() {
            List<TopBottomOperatorData> model = TestData.TopBottomOperatorDataList;
            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomOperatorData topBottomOperatorData in model) {
                var d = result.SingleOrDefault(r => r.OperatorName == topBottomOperatorData.OperatorName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomOperatorData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_ModelIsNull_NullReturned() {

            List<TopBottomOperatorData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_ModelIsEmpty_NullReturned() {

            List<TopBottomOperatorData> model = new List<TopBottomOperatorData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_IsConverted() {
            List<TopBottomMerchantData> model = TestData.TopBottomMerchantDataList;
            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomMerchantData topBottomMerchantData in model) {
                var d = result.SingleOrDefault(r => r.MerchantName == topBottomMerchantData.MerchantName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomMerchantData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_ModelIsNull_NullReturned() {

            List<TopBottomMerchantData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_ModelIsEmpty_NullReturned() {

            List<TopBottomMerchantData> model = new List<TopBottomMerchantData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }


        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_IsConverted() {
            List<TopBottomProductData> model = TestData.TopBottomProductDataList;

            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomProductData topBottomProductData in model) {
                var d = result.SingleOrDefault(r => r.ProductName == topBottomProductData.ProductName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomProductData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_ModelIsNull_NullReturned() {

            List<TopBottomProductData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_ModelIsEmpty_NullReturned() {

            List<TopBottomProductData> model = new List<TopBottomProductData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_LastSettlement_IsConverted() {
            LastSettlement model = TestData.LastSettlement;

            LastSettlementModel result = ModelFactory.ConvertFrom(model);
            result.ShouldNotBeNull();
            result.FeesValue.ShouldBe(model.FeesValue);
            result.SalesCount.ShouldBe(model.SalesCount);
            result.SalesValue.ShouldBe(model.SalesValue);
            result.SettlementDate.ShouldBe(model.SettlementDate);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_LastSettlement_ModelIsNull_IsConverted() {
            LastSettlement model = null;

            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Theory]
        [InlineData(Models.SettlementSchedule.Immediate, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        [InlineData(Models.SettlementSchedule.Monthly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly)]
        [InlineData(Models.SettlementSchedule.Weekly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly)]
        [InlineData((Models.SettlementSchedule)99, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        public void ModelFactory_ConvertFrom_SettlementSchedule_IsConverted(Models.SettlementSchedule settlementSchedule,
                                                                            TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule expectedResult) {
            TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule result = ModelFactory.ConvertFrom(settlementSchedule);
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Immediate, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Weekly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly)]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Monthly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly)]
        public void ModelFactory_ConvertFrom_CreateMerchantModel_ModelIsConverted(BusinessLogic.Models.SettlementSchedule settlementSchedule,
                                                                                  TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule expectedSettlementSchedule) {
            CreateMerchantModel model = TestData.CreateMerchantModel(settlementSchedule);

            CreateMerchantRequest request = ModelFactory.ConvertFrom(model);

            request.Contact.ShouldNotBeNull();
            request.Contact.EmailAddress.ShouldBe(model.Contact.ContactEmailAddress);
            request.Contact.PhoneNumber.ShouldBe(model.Contact.ContactPhoneNumber);
            request.Contact.ContactName.ShouldBe(model.Contact.ContactName);

            request.Address.ShouldNotBeNull();
            request.Address.AddressLine1.ShouldBe(model.Address.AddressLine1);
            request.Address.AddressLine2.ShouldBe(model.Address.AddressLine2);
            request.Address.AddressLine3.ShouldBe(model.Address.AddressLine3);
            request.Address.AddressLine4.ShouldBe(model.Address.AddressLine4);
            request.Address.Town.ShouldBe(model.Address.Town);
            request.Address.Region.ShouldBe(model.Address.Region);
            request.Address.Country.ShouldBe(model.Address.Country);
            request.Address.PostalCode.ShouldBe(model.Address.PostalCode);

            request.Name.ShouldBe(model.MerchantName);
            request.SettlementSchedule.ShouldBe(expectedSettlementSchedule);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_CreateMerchantModel_ModelIsNull_ModelIsConverted() {
            CreateMerchantModel model = null;

            CreateMerchantRequest request = ModelFactory.ConvertFrom(model);

            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_Address_ModelIsConverted() {
            CreateMerchantModel model = TestData.CreateMerchantModel(SettlementSchedule.Immediate);

            Address request = ModelFactory.ConvertFrom(model.Address);

            request.ShouldNotBeNull();
            request.AddressLine1.ShouldBe(model.Address.AddressLine1);
            request.AddressLine2.ShouldBe(model.Address.AddressLine2);
            request.AddressLine3.ShouldBe(model.Address.AddressLine3);
            request.AddressLine4.ShouldBe(model.Address.AddressLine4);
            request.Town.ShouldBe(model.Address.Town);
            request.Region.ShouldBe(model.Address.Region);
            request.Country.ShouldBe(model.Address.Country);
            request.PostalCode.ShouldBe(model.Address.PostalCode);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_Address_AddressIsNull_ModelIsConverted() {
            CreateMerchantModel model = TestData.CreateMerchantModel(SettlementSchedule.Immediate);
            model.Address = null;

            var request = ModelFactory.ConvertFrom(model.Address);

            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_Contact_ModelIsConverted() {
            CreateMerchantModel model = TestData.CreateMerchantModel(SettlementSchedule.Immediate);

            Contact request = ModelFactory.ConvertFrom(model.Contact);

            request.ShouldNotBeNull();
            request.EmailAddress.ShouldBe(model.Contact.ContactEmailAddress);
            request.PhoneNumber.ShouldBe(model.Contact.ContactPhoneNumber);
            request.ContactName.ShouldBe(model.Contact.ContactName);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_Contact_ContactIsNull_ModelIsConverted() {
            CreateMerchantModel model = TestData.CreateMerchantModel(SettlementSchedule.Immediate);
            model.Contact = null;
            Contact request = ModelFactory.ConvertFrom(model.Contact);

            request.ShouldBeNull();
        }

        [Theory]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Immediate, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Immediate)]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Weekly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Weekly)]
        [InlineData(BusinessLogic.Models.SettlementSchedule.Monthly, TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule.Monthly)]
        public void ModelFactory_ConvertFrom_UpdateMerchantModel_ModelIsConverted(BusinessLogic.Models.SettlementSchedule settlementSchedule,
                                                                                  TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule expectedSettlementSchedule) {
            UpdateMerchantModel model = TestData.UpdateMerchantModel(settlementSchedule);

            var request = ModelFactory.ConvertFrom(model);

            request.Name.ShouldBe(model.MerchantName);
            request.SettlementSchedule.ShouldBe(expectedSettlementSchedule);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_UpdateMerchantModel_ModelIsNull_ModelIsConverted() {
            UpdateMerchantModel model = null;

            var request = ModelFactory.ConvertFrom(model);

            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantResponse_ResponseIsConverted() {
            MerchantResponse response = TestData.MerchantResponse;

            var model = ModelFactory.ConvertFrom(response);

            model.ShouldNotBeNull();
            model.MerchantId.ShouldBe(response.MerchantId);
            model.MerchantName.ShouldBe(response.MerchantName);
            model.SettlementSchedule.ShouldBe(response.SettlementSchedule.ToString());
            model.MerchantReference.ShouldBe(response.MerchantReference);
            model.Address.AddressLine1.ShouldBe(response.Addresses.First().AddressLine1);
            model.Address.AddressLine2.ShouldBe(response.Addresses.First().AddressLine2);
            model.Address.AddressLine3.ShouldBe(response.Addresses.First().AddressLine3);
            model.Address.AddressLine4.ShouldBe(response.Addresses.First().AddressLine4);
            model.Address.Town.ShouldBe(response.Addresses.First().Town);
            model.Address.Region.ShouldBe(response.Addresses.First().Region);
            model.Address.Country.ShouldBe(response.Addresses.First().Country);
            model.Address.PostalCode.ShouldBe(response.Addresses.First().PostalCode);
            model.Contact.ContactName.ShouldBe(response.Contacts.First().ContactName);
            model.Contact.ContactEmailAddress.ShouldBe(response.Contacts.First().ContactEmailAddress);
            model.Contact.ContactPhoneNumber.ShouldBe(response.Contacts.First().ContactPhoneNumber);

            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                var operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.Name.ShouldBe(o.Name);
                operatorModel.IsDeleted.ShouldBe(o.IsDeleted);
                operatorModel.MerchantNumber.ShouldBe(o.MerchantNumber);
                operatorModel.TerminalNumber.ShouldBe(o.TerminalNumber);
            });

            model.Devices.Count.ShouldBe(response.Devices.Count);
            foreach (var device in response.Devices) {
                model.Devices.ShouldContainKeyAndValue(device.Key, device.Value);
            }

            model.Contracts.Count.ShouldBe(response.Contracts.Count);
            response.Contracts.ForEach(c => {
                var contractModel = model.Contracts.SingleOrDefault(mc => mc.ContractId == c.ContractId);
                contractModel.ShouldNotBeNull();
                contractModel.IsDeleted.ShouldBe(c.IsDeleted);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantResponse_ResponseIsNull_ResponseIsConverted() {
            MerchantResponse response = null;

            var model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignOperatorToMerchantModel_ModelIsConverted()
        {
            var model = TestData.AssignOperatorToMerchantModel;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldNotBeNull();
            request.OperatorId.ShouldBe(model.OperatorId);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignOperatorToMerchantModel_ModelIsNull_ModelIsConverted() {
            AssignOperatorToMerchantModel model = null;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignContractToMerchantModel_ModelIsConverted()
        {
            var model = TestData.AssignContractToMerchantModel;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldNotBeNull();
            request.ContractId.ShouldBe(model.ContractId);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignContractToMerchantModel_ModelIsNull_ModelIsConverted()
        {
            AssignContractToMerchantModel model = null;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignDeviceToMerchantModel_ModelIsConverted()
        {
            var model = TestData.AssignDeviceToMerchantModel;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldNotBeNull();
            request.DeviceIdentifier.ShouldBe(model.DeviceIdentifier);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_AssignDeviceToMerchantModel_ModelIsNull_ModelIsConverted()
        {
            AssignDeviceToMerchantModel model = null;
            var request = ModelFactory.ConvertFrom(model);
            request.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_FileDetails_SourceIsNull_ReturnsNull()
        {
            FileDetails source = null;

            var result = ModelFactory.ConvertFrom(source);

            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_FileDetails_SourceIsConverted()
        {
            FileDetails source = TestData.FileDetails;

            var result = ModelFactory.ConvertFrom(source);

            result.ShouldNotBeNull();
            result.FileId.ShouldBe(source.FileId);
            result.FileProfileId.ShouldBe(source.FileProfileId);
            result.FileProfileName.ShouldBe(source.FileProfileName);
            result.FileImportLogId.ShouldBe(source.FileImportLogId);
            result.FileLocation.ShouldBe(source.FileLocation);
            result.ProcessingCompleted.ShouldBe(source.ProcessingCompleted);
            result.EstateId.ShouldBe(source.EstateId);
            result.UserId.ShouldBe(source.UserId);
            result.UserEmailAddress.ShouldBe(source.UserEmailAddress);
            result.MerchantId.ShouldBe(source.MerchantId);
            result.MerchantName.ShouldBe(source.MerchantName);
            result.ProcessingSummary.FailedLines.ShouldBe(source.ProcessingSummary.FailedLines);
            result.ProcessingSummary.IgnoredLines.ShouldBe(source.ProcessingSummary.IgnoredLines);
            result.ProcessingSummary.NotProcessedLines.ShouldBe(source.ProcessingSummary.NotProcessedLines);
            result.ProcessingSummary.RejectedLines.ShouldBe(source.ProcessingSummary.RejectedLines);
            result.ProcessingSummary.SuccessfullyProcessedLines.ShouldBe(source.ProcessingSummary.SuccessfullyProcessedLines);
            result.ProcessingSummary.TotalLines.ShouldBe(source.ProcessingSummary.TotalLines);

            result.FileLines.Count.ShouldBe(source.FileLines.Count);
            foreach (var fileLine in source.FileLines)
            {
                FileLineModel? fileLineModel = result.FileLines.SingleOrDefault(fl => fl.LineNumber == fileLine.LineNumber);
                fileLineModel.ShouldNotBeNull();
                fileLineModel.LineData.ShouldBe(fileLine.LineData);
                fileLineModel.RejectionReason.ShouldBe(fileLine.RejectionReason);
                fileLineModel.TransactionId.ShouldBe(fileLine.TransactionId);
                fileLineModel.ProcessingResult.ShouldBe(fileLine.ProcessingResult switch
                {
                    FileLineProcessingResult.Failed => Models.FileLineProcessingResult.Failed,
                    FileLineProcessingResult.Ignored => Models.FileLineProcessingResult.Ignored,
                    FileLineProcessingResult.NotProcessed => Models.FileLineProcessingResult.NotProcessed,
                    FileLineProcessingResult.Rejected => Models.FileLineProcessingResult.Rejected,
                    FileLineProcessingResult.Successful => Models.FileLineProcessingResult.Successful,
                    _ => Models.FileLineProcessingResult.Unknown
                });
            }
        }
    }
}