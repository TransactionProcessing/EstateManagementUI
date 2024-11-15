using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Requests.Merchant;
using EstateManagement.DataTransferObjects.Requests.Operator;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Merchant;
using EstateManagementUI.Testing;
using EstateReportingAPI.Client;
using EstateReportingAPI.DataTransferObjects;
using FileProcessor.Client;
using Moq;
using Shared.Logger;
using Shouldly;
using SimpleResults;
using CreateMerchantModel = EstateManagementUI.BusinessLogic.Models.CreateMerchantModel;
using SettlementSchedule = EstateManagementUI.BusinessLogic.Models.SettlementSchedule;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class ApiClientTests {
        private readonly IApiClient ApiClient;
        private readonly Mock<IEstateClient> EstateClient;
        private readonly Mock<IFileProcessorClient> FileProcessorClient;
        private readonly Mock<IEstateReportingApiClient> EstateReportingApiClient;

        public ApiClientTests() {
            Logger.Initialise(NullLogger.Instance);

            this.EstateClient = new Mock<IEstateClient>();
            this.FileProcessorClient = new Mock<IFileProcessorClient>();
            this.EstateReportingApiClient = new Mock<IEstateReportingApiClient>();

            this.ApiClient = new ApiClient(this.EstateClient.Object, this.FileProcessorClient.Object,
                    this.EstateReportingApiClient.Object);
        }

        [Fact]
        public async Task ApiClient_GetEstate_EstateReturned() {
            this.EstateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.EstateResponse);
            
            EstateModel estate = await this.ApiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            estate.ShouldNotBeNull();
            estate.EstateName.ShouldBe(TestData.EstateName);
        }

        [Fact]
        public async Task ApiClient_GetMerchants_MerchantsReturned()
        {
            this.EstateClient.Setup(e => e.GetMerchants(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.MerchantResponses);

            List<MerchantModel> merchantList = await this.ApiClient.GetMerchants(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.Count.ShouldBe(3);

            foreach (MerchantResponse merchantResponse in TestData.MerchantResponses) {
                MerchantModel? merchant = merchantList.SingleOrDefault(m => m.MerchantId == merchantResponse.MerchantId);
                merchant.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ApiClient_GetOperators_OperatorsReturned()
        {
            this.EstateClient.Setup(e => e.GetOperators(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.OperatorResponses);


            List<OperatorModel> operatorsList = await this.ApiClient.GetOperators(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            operatorsList.ShouldNotBeNull();
            operatorsList.ShouldNotBeEmpty();
            operatorsList.Count.ShouldBe(2);

            foreach (OperatorResponse operatorResponse in TestData.OperatorResponses)
            {
                OperatorModel? @operatorModel = operatorsList.SingleOrDefault(m => m.OperatorId == operatorResponse.OperatorId);
                operatorModel.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ApiClient_GetOperator_OperatorIsReturned()
        {
            this.EstateClient.Setup(e => e.GetOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.OperatorResponse);


            OperatorModel @operator = await this.ApiClient.GetOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Operator1Id, CancellationToken.None);

            @operator.ShouldNotBeNull();
        }

        [Fact]
        public async Task ApiClient_GetContracts_ContractsReturned()
        {
            this.EstateClient.Setup(e => e.GetContracts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.ContractResponses);


            List<ContractModel> contractsList = await this.ApiClient.GetContracts(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            contractsList.ShouldNotBeNull();
            contractsList.ShouldNotBeEmpty();
            contractsList.Count.ShouldBe(1);

            foreach (ContractResponse contractResponse in TestData.ContractResponses)
            {
                ContractModel? contractModel = contractsList.SingleOrDefault(m => m.ContractId == contractResponse.ContractId);
                contractModel.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ApiClient_GetContract_ContractIsReturned() {
            this.EstateClient
                .Setup(e => e.GetContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractResponse1);


            ContractModel contractModel = await this.ApiClient.GetContract(TestData.AccessToken, Guid.NewGuid(),
                TestData.EstateId, TestData.Contract1Id, CancellationToken.None);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(TestData.ContractResponse1.ContractId);
            contractModel.Description.ShouldBe(TestData.ContractResponse1.Description);
            contractModel.OperatorName.ShouldBe(TestData.ContractResponse1.OperatorName);
            contractModel.NumberOfProducts.ShouldBe(TestData.ContractResponse1.Products.Count);
            foreach (ContractProduct contractResponse1Product in TestData.ContractResponse1.Products) {
                ContractProductModel? modelProduct = contractModel.ContractProducts.SingleOrDefault(cp =>
                    cp.ContractProductId == contractResponse1Product.ProductId);
                modelProduct.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ApiClient_CreateOperator_OperatorIsCreated() {

            this.EstateClient
                .Setup(e => e.CreateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateOperatorRequest>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            CreateOperatorModel createOperatorModel = new CreateOperatorModel {
                RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                OperatorName = TestData.Operator1Name,
                OperatorId = TestData.Operator1Id
            };

            Result result = await this.ApiClient.CreateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, createOperatorModel,
                CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_CreateOperator_ErrorAtServer_OperatorIsNotCreated() {

            Logger.Initialise(NullLogger.Instance);
            this.EstateClient.Setup(e => e.CreateOperator(It.IsAny<String>(), It.IsAny<Guid>(),
                It.IsAny<CreateOperatorRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            CreateOperatorModel createOperatorModel = new CreateOperatorModel
            {
                RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                OperatorName = TestData.Operator1Name,
                OperatorId = TestData.Operator1Id
            };

            Result result = await this.ApiClient.CreateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, createOperatorModel,
                CancellationToken.None);

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_UpdateOperator_OperatorIsUpdated()
        {
            this.EstateClient.Setup(e => e.UpdateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
                It.IsAny<UpdateOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success());

            UpdateOperatorModel updateOperatorModel = new UpdateOperatorModel
            {
                RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                OperatorName = TestData.Operator1Name,
                OperatorId = TestData.Operator1Id
            };

            Result result = await this.ApiClient.UpdateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, updateOperatorModel,
                CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_UpdateOperator_ErrorAtServer_OperatorIsNotUpdated()
        {
            this.EstateClient.Setup(e => e.UpdateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
                It.IsAny<UpdateOperatorRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            UpdateOperatorModel updateOperatorModel = new UpdateOperatorModel
            {
                RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber,
                OperatorName = TestData.Operator1Name,
                OperatorId = TestData.Operator1Id
            };

            Result result = await this.ApiClient.UpdateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, updateOperatorModel,
                CancellationToken.None);

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileImportLogList_DataIsReturned()
        {
            this.FileProcessorClient.Setup(e => e.GetFileImportLogs(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.FileImportLogList);
            
            Result<List<FileImportLogModel>> result = await this.ApiClient.GetFileImportLogList(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogQueryStartDate,
                TestData.FileImportLogQueryEndDate, System.Threading.CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileImportLogList_ErrorAtServer_NoDataIsReturned()
        {
            this.FileProcessorClient.Setup(e => e.GetFileImportLogs(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<FileImportLogModel>> result = await this.ApiClient.GetFileImportLogList(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogQueryStartDate,
                TestData.FileImportLogQueryEndDate, System.Threading.CancellationToken.None);

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileImportLog_DataIsReturned()
        {
            this.FileProcessorClient.Setup(e => e.GetFileImportLog(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.FileImportLog1);

            Result<FileImportLogModel> result = await this.ApiClient.GetFileImportLog(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogId, System.Threading.CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileImportLog_ErrorAtServer_NoDataIsReturned()
        {
            this.FileProcessorClient.Setup(e => e.GetFileImportLog(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<FileImportLogModel> result = await this.ApiClient.GetFileImportLog(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogId, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileDetails_DataIsReturned() {
            
            this.FileProcessorClient.Setup(e => e.GetFile(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.FileDetails1);

            Result<FileDetailsModel> result = await this.ApiClient.GetFileDetails(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId,  TestData.FileId1, System.Threading.CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetFileDetails_ErrorAtServer_NoDataIsReturned()
        {
            this.FileProcessorClient.Setup(e => e.GetFile(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<FileDetailsModel> result = await this.ApiClient.GetFileDetails(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.FileId1, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }


        [Fact]
        public async Task ApiClient_GetComparisonDates_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ComparisonDates);

            Result<List<ComparisonDateModel>> result = await this.ApiClient.GetComparisonDates(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetComparisonDates_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<ComparisonDateModel>> result = await this.ApiClient.GetComparisonDates(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSales_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(),It.IsAny<DateTime>(),It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSales);

            Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysSales(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSales_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysSales(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSettlement_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSettlement);

            Result<TodaysSettlementModel> result = await this.ApiClient.GetTodaysSettlement(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, 
                null, null, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSettlement_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(),It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<TodaysSettlementModel> result = await this.ApiClient.GetTodaysSettlement(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, 
                null, null, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSalesCountByHour_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesCountByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSalesCountByHour);

            Result<List<TodaysSalesCountByHourModel>> result = await this.ApiClient.GetTodaysSalesCountByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId,TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSalesCountByHour_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesCountByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<TodaysSalesCountByHourModel>> result = await this.ApiClient.GetTodaysSalesCountByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact] 
        public async Task ApiClient_GetTodaysSalesValueByHour_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesValueByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSalesValueByHour);

            Result<List<TodaysSalesValueByHourModel>> result = await this.ApiClient.GetTodaysSalesValueByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysSalesValueByHour_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesValueByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<TodaysSalesValueByHourModel>> result = await this.ApiClient.GetTodaysSalesValueByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetMerchantKpi_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetMerchantKpi(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantKpi);

            Result<MerchantKpiModel> result = await this.ApiClient.GetMerchantKpi(TestData.AccessToken, TestData.EstateId, System.Threading.CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetMerchantKpi_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetMerchantKpi(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<MerchantKpiModel> result = await this.ApiClient.GetMerchantKpi(TestData.AccessToken, TestData.EstateId, System.Threading.CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }


        [Fact]
        public async Task ApiClient_GetTodaysFailedSales_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysFailedSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSales);

            Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysFailedSales(TestData.AccessToken, TestData.EstateId, "1009",TestData.ComparisonDate,CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTodaysFailedSales_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTodaysFailedSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysFailedSales(TestData.AccessToken, TestData.EstateId, "1009", TestData.ComparisonDate, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }


        [Fact]
        public async Task ApiClient_GetTopBottomMerchantData_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomMerchantData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomMerchantDataList);

            Result<List<TopBottomMerchantDataModel>> result = await this.ApiClient.GetTopBottomMerchantData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTopBottomMerchantData_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomMerchantData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<TopBottomMerchantDataModel>> result = await this.ApiClient.GetTopBottomMerchantData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTopBottomProductData_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomProductData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomProductDataList);

            Result<List<TopBottomProductDataModel>> result = await this.ApiClient.GetTopBottomProductData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTopBottomProductData_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomProductData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<TopBottomProductDataModel>> result = await this.ApiClient.GetTopBottomProductData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTopBottomOperatorData_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomOperatorData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomOperatorDataList);

            Result<List<TopBottomOperatorDataModel>> result = await this.ApiClient.GetTopBottomOperatorData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetTopBottomOperatorData_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetTopBottomOperatorData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<List<TopBottomOperatorDataModel>> result = await this.ApiClient.GetTopBottomOperatorData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetLastSettlement_DataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetLastSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.LastSettlement);

            Result<LastSettlementModel> result = await this.ApiClient.GetLastSettlement(TestData.AccessToken, TestData.EstateId, null, null, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_GetLastSettlement_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateReportingApiClient.Setup(e => e.GetLastSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result<LastSettlementModel> result = await this.ApiClient.GetLastSettlement(TestData.AccessToken, TestData.EstateId, null, null, CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_CreateMerchant_DataIsReturned()
        {
            this.EstateClient.Setup(e => e.CreateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateMerchantRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            Result result = await this.ApiClient.CreateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ApiClient_CreateMerchant_ErrorAtServer_NoDataIsReturned()
        {
            this.EstateClient.Setup(e => e.CreateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateMerchantRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Result result = await this.ApiClient.CreateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
            result.IsFailed.ShouldBeTrue();
        }
    }
}
