using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagementUI.Testing;
using EstateManagementUI.BusinessLogic.Models;
using Shouldly;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagement.DataTransferObjects.Responses.Operator;
using SimpleResults;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateReportingAPI.DataTransferObjects;
using FileProcessor.DataTransferObjects.Responses;
using EstateManagementUI.ViewModels;
using EstateReportingAPI.DataTrasferObjects;
using ContractProduct = EstateManagement.DataTransferObjects.Responses.Contract.ContractProduct;
using ContractProductTransactionFee = EstateManagement.DataTransferObjects.Responses.Contract.ContractProductTransactionFee;
using FileImportLogList = FileProcessor.DataTransferObjects.Responses.FileImportLogList;
using MerchantKpi = EstateReportingAPI.DataTransferObjects.MerchantKpi;
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

            Should.Throw<ArgumentNullException>(() => {
                ModelFactory.ConvertFrom(response);
            });
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
        public void ModelFactory_ConvertFrom_TodaysSettlement_ResponseIsNull_ErrorThrown()
        {
            TodaysSettlement response = null;

            Should.Throw<ArgumentNullException>(() => {
                ModelFactory.ConvertFrom(response);
            });
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
                SecurityUserModel securityUserModel =
                    model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
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
                EstateOperatorModel operatorModel =
                    model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
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
                EstateOperatorModel operatorModel =
                    model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel =
                    model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
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
                SecurityUserModel securityUserModel =
                    model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
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
                EstateOperatorModel operatorModel =
                    model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
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
                model.AddressLine1.ShouldBe(merchantResponse.Addresses.First().AddressLine1);
                model.Town.ShouldBe(merchantResponse.Addresses.First().Town);
                model.ContactName.ShouldBe(merchantResponse.Contacts.First().ContactName);
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
                ContractProductTransactionFeeModel? modelFee = model.ContractProductTransactionFees.SingleOrDefault(p =>
                    p.ContractProductTransactionFeeId == contractProductTransactionFee.TransactionFeeId);
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
                FileImportLogModel? fileImportLogModel =
                    model.SingleOrDefault(m => m.FileImportLogId == fileImportLog.FileImportLogId);
                fileImportLogModel.ShouldNotBeNull();
                fileImportLogModel.ImportLogDateTime.ShouldBe(fileImportLog.ImportLogDateTime);
                fileImportLogModel.FileCount.ShouldBe(fileImportLog.FileCount);
                fileImportLogModel.ImportLogDate.ShouldBe(fileImportLog.ImportLogDate);
                fileImportLogModel.ImportLogTime.ShouldBe(fileImportLog.ImportLogTime);

                foreach (FileImportLogFile fileImportLogFile in fileImportLog.Files) {
                    FileImportLogFileModel? fileModel =
                        fileImportLogModel.Files.SingleOrDefault(f => f.FileId == fileImportLogFile.FileId);
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

            foreach (TodaysSalesValueByHour todaysSalesValueByHour in response)
            {
                TodaysSalesValueByHourModel? hourModel = model.SingleOrDefault(m => m.Hour == todaysSalesValueByHour.Hour);
                hourModel.ShouldNotBeNull();
                hourModel.TodaysSalesValue.ShouldBe(todaysSalesValueByHour.TodaysSalesValue);
                hourModel.ComparisonSalesValue.ShouldBe(todaysSalesValueByHour.ComparisonSalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesValueByHourModel_NullResponse_ModelIsConverted()
        {
            List<TodaysSalesValueByHour> response = null;

            List<TodaysSalesValueByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TodaysSalesValueByHourModel_EmptyList_ModelIsConverted()
        {
            List<TodaysSalesValueByHour> response = new List<TodaysSalesValueByHour>();

            List<TodaysSalesValueByHourModel> model = ModelFactory.ConvertFrom(response);

            model.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantKpi_IsConverted()
        {

            MerchantKpi model = new MerchantKpi()
            {
                MerchantsWithNoSaleInLast7Days = 1,
                MerchantsWithNoSaleToday = 2,
                MerchantsWithSaleInLastHour = 3
            };
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldNotBeNull();
            result.MerchantsWithNoSaleInLast7Days.ShouldBe(model.MerchantsWithNoSaleInLast7Days);
            result.MerchantsWithNoSaleToday.ShouldBe(model.MerchantsWithNoSaleToday);
            result.MerchantsWithSaleInLastHour.ShouldBe(model.MerchantsWithSaleInLastHour);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_MerchantKpi_ModelIsNull_ErrorThrown()
        {

            MerchantKpi model = null;
            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(model); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_IsConverted() {
            List<TopBottomOperatorData> model = TestData.TopBottomOperatorDataList;
            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomOperatorData topBottomOperatorData in model)
            {
                var d = result.SingleOrDefault(r => r.OperatorName == topBottomOperatorData.OperatorName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomOperatorData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_ModelIsNull_NullReturned()
        {

            List<TopBottomOperatorData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomOperatorData_ModelIsEmpty_NullReturned()
        {

            List<TopBottomOperatorData> model = new List<TopBottomOperatorData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_IsConverted() {
            List<TopBottomMerchantData> model = TestData.TopBottomMerchantDataList;
            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomMerchantData topBottomMerchantData in model)
            {
                var d = result.SingleOrDefault(r => r.MerchantName == topBottomMerchantData.MerchantName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomMerchantData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_ModelIsNull_NullReturned()
        {

            List<TopBottomMerchantData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomMerchantData_ModelIsEmpty_NullReturned()
        {

            List<TopBottomMerchantData> model = new List<TopBottomMerchantData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }


        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_IsConverted() {
            List<TopBottomProductData> model = TestData.TopBottomProductDataList;

            var result = ModelFactory.ConvertFrom(model);

            result.Count.ShouldBe(model.Count);
            foreach (TopBottomProductData topBottomProductData in model)
            {
                var d = result.SingleOrDefault(r => r.ProductName == topBottomProductData.ProductName);
                d.ShouldNotBeNull();
                d.SalesValue.ShouldBe(topBottomProductData.SalesValue);
            }
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_ModelIsNull_NullReturned()
        {

            List<TopBottomProductData> model = null;
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_TopBottomProductData_ModelIsEmpty_NullReturned()
        {

            List<TopBottomProductData> model = new List<TopBottomProductData>();
            var result = ModelFactory.ConvertFrom(model);
            result.ShouldBeNull();
        }
    }
}