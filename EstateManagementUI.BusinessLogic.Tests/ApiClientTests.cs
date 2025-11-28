using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Merchant;
using EstateManagementUI.Testing;
using EstateReportingAPI.Client;
using EstateReportingAPI.DataTransferObjects;
using FileProcessor.Client;
using Moq;
using SecurityService.Client;
using Shared.Logger;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Operator;
using CreateMerchantModel = EstateManagementUI.BusinessLogic.Models.CreateMerchantModel;
using SettlementSchedule = EstateManagementUI.BusinessLogic.Models.SettlementSchedule;

namespace EstateManagementUI.BusinessLogic.Tests;

public class ApiClientTests {
    private readonly IApiClient ApiClient;
    private readonly Mock<ITransactionProcessorClient> TransactionProcessorClient;
    private readonly Mock<IFileProcessorClient> FileProcessorClient;
    private readonly Mock<IEstateReportingApiClient> EstateReportingApiClient;
    private readonly Mock<ISecurityServiceClient> SecurityServiceClient;

    public ApiClientTests() {
        Logger.Initialise(NullLogger.Instance);

        this.TransactionProcessorClient = new Mock<ITransactionProcessorClient>();
        this.FileProcessorClient = new Mock<IFileProcessorClient>();
        this.EstateReportingApiClient = new Mock<IEstateReportingApiClient>();
        this.SecurityServiceClient = new Mock<ISecurityServiceClient>();

        this.ApiClient = new ApiClient(this.TransactionProcessorClient.Object, this.FileProcessorClient.Object, this.EstateReportingApiClient.Object,
            this.SecurityServiceClient.Object);
    }


    [Fact]
    public async Task ApiClient_GetEstate_EstateReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TestData.EstateResponse));

        var result= await this.ApiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.EstateName.ShouldBe(TestData.EstateName);
    }

    [Fact]
    public async Task ApiClient_GetEstate_ClientCallFailed_ResultFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        var result = await this.ApiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetEstate_ClientCallThrewException_ResultFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var result = await this.ApiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetMerchants_MerchantsReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetMerchants(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantResponses));

        var result = await this.ApiClient.GetMerchants(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldNotBeEmpty();
        result.Data.Count.ShouldBe(3);

        foreach (MerchantResponse merchantResponse in TestData.MerchantResponses) {
            MerchantModel? merchant = result.Data.SingleOrDefault(m => m.MerchantId == merchantResponse.MerchantId);
            merchant.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task ApiClient_GetMerchants_ClientCallFailed_ResultFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.GetMerchants(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result<List<MerchantModel>> result = await this.ApiClient.GetMerchants(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetMerchant_ClientCallFailed_ResultFailed() {
        this.TransactionProcessorClient.Setup(e => e.GetMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result<MerchantModel> result = await this.ApiClient.GetMerchant(TestData.AccessToken, Guid.NewGuid(), Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetMerchant_MerchantReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantResponse));

        var result= await this.ApiClient.GetMerchant(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task ApiClient_GetOperators_OperatorsReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetOperators(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.OperatorResponses));
        
        var result = await this.ApiClient.GetOperators(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldNotBeEmpty();
        result.Data.Count.ShouldBe(2);

        foreach (OperatorResponse operatorResponse in TestData.OperatorResponses) {
            OperatorModel? @operatorModel = result.Data.SingleOrDefault(m => m.OperatorId == operatorResponse.OperatorId);
            operatorModel.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task ApiClient_GetOperators_ClientCallFailed_ResultFailed() {
        this.TransactionProcessorClient.Setup(e => e.GetOperators(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());


        Result<List<OperatorModel>> result = await this.ApiClient.GetOperators(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetOperator_OperatorIsReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.OperatorResponse));
        
        Result<OperatorModel> @operator = await this.ApiClient.GetOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Operator1Id, CancellationToken.None);
        @operator.IsSuccess.ShouldBeTrue();
        @operator.ShouldNotBeNull();
    }

    [Fact]
    public async Task ApiClient_GetOperator_ClientCallFailed_ResultFailed() {
        this.TransactionProcessorClient.Setup(e => e.GetOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());
        
        Result<OperatorModel> result = await this.ApiClient.GetOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Operator1Id, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetContracts_ContractsReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetContracts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractResponses));


        var result = await this.ApiClient.GetContracts(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();

        result.Data.ShouldNotBeNull();
        result.Data.ShouldNotBeEmpty();
        result.Data.Count.ShouldBe(1);

        foreach (ContractResponse contractResponse in TestData.ContractResponses) {
            ContractModel? contractModel = result.Data.SingleOrDefault(m => m.ContractId == contractResponse.ContractId);
            contractModel.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task ApiClient_GetContracts_ClientCallFailed_ResultFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.GetContracts(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        var result = await this.ApiClient.GetContracts(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetContract_ContractIsReturned() {
        this.TransactionProcessorClient.Setup(e => e.GetContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractResponse1));
        
        var result = await this.ApiClient.GetContract(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Contract1Id, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ContractId.ShouldBe(TestData.ContractResponse1.ContractId);
        result.Data.Description.ShouldBe(TestData.ContractResponse1.Description);
        result.Data.OperatorName.ShouldBe(TestData.ContractResponse1.OperatorName);
        result.Data.NumberOfProducts.ShouldBe(TestData.ContractResponse1.Products.Count);
        foreach (ContractProduct contractResponse1Product in TestData.ContractResponse1.Products) {
            ContractProductModel? modelProduct = result.Data.ContractProducts.SingleOrDefault(cp => cp.ContractProductId == contractResponse1Product.ProductId);
            modelProduct.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task ApiClient_GetContract_ClientCallFailed_ResultFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.GetContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());


        var result= await this.ApiClient.GetContract(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Contract1Id, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateOperator_OperatorIsCreated() {
        this.TransactionProcessorClient.Setup(e => e.CreateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        CreateOperatorModel createOperatorModel = new() { RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber, RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber, OperatorName = TestData.Operator1Name, OperatorId = TestData.Operator1Id };

        Result result = await this.ApiClient.CreateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, createOperatorModel, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateOperator_ErrorAtServer_OperatorIsNotCreated() {
        Logger.Initialise(NullLogger.Instance);
        this.TransactionProcessorClient.Setup(e => e.CreateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        CreateOperatorModel createOperatorModel = new() { RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber, RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber, OperatorName = TestData.Operator1Name, OperatorId = TestData.Operator1Id };

        Result result = await this.ApiClient.CreateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, createOperatorModel, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateOperator_OperatorIsUpdated() {
        this.TransactionProcessorClient.Setup(e => e.UpdateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success());

        UpdateOperatorModel updateOperatorModel = new() { RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber, RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber, OperatorName = TestData.Operator1Name, OperatorId = TestData.Operator1Id };

        Result result = await this.ApiClient.UpdateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, updateOperatorModel, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateOperator_ErrorAtServer_OperatorIsNotUpdated() {
        this.TransactionProcessorClient.Setup(e => e.UpdateOperator(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        UpdateOperatorModel updateOperatorModel = new() { RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber, RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber, OperatorName = TestData.Operator1Name, OperatorId = TestData.Operator1Id };

        Result result = await this.ApiClient.UpdateOperator(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, updateOperatorModel, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetFileImportLogList_DataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFileImportLogs(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync( Result.Success(TestData.FileImportLogList));

        Result<List<FileImportLogModel>> result = await this.ApiClient.GetFileImportLogList(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogQueryStartDate, TestData.FileImportLogQueryEndDate, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldNotBeEmpty();
        result.Data.Count.ShouldBe(TestData.FileImportLogList.FileImportLogs.Count);
    }

    [Fact]
    public async Task ApiClient_GetFileImportLogList_ErrorAtServer_NoDataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFileImportLogs(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());
        Result<List<FileImportLogModel>> result = await this.ApiClient.GetFileImportLogList(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogQueryStartDate, TestData.FileImportLogQueryEndDate, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetFileImportLog_DataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFileImportLog(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync( Result.Success(TestData.FileImportLog1));

        Result<FileImportLogModel> result = await this.ApiClient.GetFileImportLog(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogId, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetFileImportLog_ErrorAtServer_NoDataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFileImportLog(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result<FileImportLogModel> result = await this.ApiClient.GetFileImportLog(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.FileImportLogId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetFileDetails_DataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFile(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.FileDetails1));

        Result<FileDetailsModel> result = await this.ApiClient.GetFileDetails(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.FileId1, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetFileDetails_ErrorAtServer_NoDataIsReturned() {
        this.FileProcessorClient.Setup(e => e.GetFile(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result<FileDetailsModel> result = await this.ApiClient.GetFileDetails(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.FileId1, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }


    [Fact]
    public async Task ApiClient_GetComparisonDates_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ComparisonDates));

        Result<List<ComparisonDateModel>> result = await this.ApiClient.GetComparisonDates(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetComparisonDates_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<ComparisonDateModel>> result = await this.ApiClient.GetComparisonDates(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSales_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSales);

        Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysSales(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSales_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetComparisonDates(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysSales(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSettlement_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSettlement);

        Result<TodaysSettlementModel> result = await this.ApiClient.GetTodaysSettlement(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSettlement_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<TodaysSettlementModel> result = await this.ApiClient.GetTodaysSettlement(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, null, null, TestData.ComparisonDate, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSalesCountByHour_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesCountByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSalesCountByHour);

        Result<List<TodaysSalesCountByHourModel>> result = await this.ApiClient.GetTodaysSalesCountByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSalesCountByHour_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesCountByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<TodaysSalesCountByHourModel>> result = await this.ApiClient.GetTodaysSalesCountByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSalesValueByHour_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesValueByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSalesValueByHour);

        Result<List<TodaysSalesValueByHourModel>> result = await this.ApiClient.GetTodaysSalesValueByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysSalesValueByHour_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysSalesValueByHour(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<TodaysSalesValueByHourModel>> result = await this.ApiClient.GetTodaysSalesValueByHour(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, TestData.Merchant1Id, TestData.Operator1Id, TestData.ComparisonDate, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetMerchantKpi_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetMerchantKpi(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.MerchantKpi);

        Result<MerchantKpiModel> result = await this.ApiClient.GetMerchantKpi(TestData.AccessToken, TestData.EstateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetMerchantKpi_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetMerchantKpi(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<MerchantKpiModel> result = await this.ApiClient.GetMerchantKpi(TestData.AccessToken, TestData.EstateId, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }


    [Fact]
    public async Task ApiClient_GetTodaysFailedSales_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysFailedSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TodaysSales);

        Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysFailedSales(TestData.AccessToken, TestData.EstateId, "1009", TestData.ComparisonDate, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTodaysFailedSales_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTodaysFailedSales(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<TodaysSalesModel> result = await this.ApiClient.GetTodaysFailedSales(TestData.AccessToken, TestData.EstateId, "1009", TestData.ComparisonDate, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }


    [Fact]
    public async Task ApiClient_GetTopBottomMerchantData_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomMerchantData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomMerchantDataList);

        Result<List<TopBottomMerchantDataModel>> result = await this.ApiClient.GetTopBottomMerchantData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTopBottomMerchantData_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomMerchantData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<TopBottomMerchantDataModel>> result = await this.ApiClient.GetTopBottomMerchantData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTopBottomProductData_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomProductData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomProductDataList);

        Result<List<TopBottomProductDataModel>> result = await this.ApiClient.GetTopBottomProductData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTopBottomProductData_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomProductData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<TopBottomProductDataModel>> result = await this.ApiClient.GetTopBottomProductData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTopBottomOperatorData_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomOperatorData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TopBottomOperatorDataList);

        Result<List<TopBottomOperatorDataModel>> result = await this.ApiClient.GetTopBottomOperatorData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetTopBottomOperatorData_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetTopBottomOperatorData(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<TopBottom>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<List<TopBottomOperatorDataModel>> result = await this.ApiClient.GetTopBottomOperatorData(TestData.AccessToken, TestData.EstateId, TopBottom.Bottom, 10, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetLastSettlement_DataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetLastSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.LastSettlement);

        Result<LastSettlementModel> result = await this.ApiClient.GetLastSettlement(TestData.AccessToken, TestData.EstateId, null, null, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_GetLastSettlement_ErrorAtServer_NoDataIsReturned() {
        this.EstateReportingApiClient.Setup(e => e.GetLastSettlement(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result<LastSettlementModel> result = await this.ApiClient.GetLastSettlement(TestData.AccessToken, TestData.EstateId, null, null, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateMerchant_DataIsReturned() {
        this.TransactionProcessorClient.Setup(e => e.CreateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateMerchantRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.CreateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateMerchant_ErrorAtServer_NoDataIsReturned() {
        this.TransactionProcessorClient.Setup(e => e.CreateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateMerchantRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result result = await this.ApiClient.CreateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateMerchant_ClientThrowsException_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.CreateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateMerchantRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result result = await this.ApiClient.CreateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchant_MerchantIsUpdated()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateMerchantRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.UpdateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.UpdateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchant_ErrorAtServer_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateMerchantRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.UpdateMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.UpdateMerchantModel(SettlementSchedule.Immediate), CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchantAddress_MerchantAddressIsUpdated()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchantAddress(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Address>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.UpdateMerchantAddress(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.AddressModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchantAddress_ErrorAtServer_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchantAddress(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Address>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.UpdateMerchantAddress(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.AddressModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchantContact_MerchantContactIsUpdated()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchantContact(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Contact>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.UpdateMerchantContact(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.ContactModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_UpdateMerchantContact_ErrorAtServer_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.UpdateMerchantContact(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Contact>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.UpdateMerchantContact(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.ContactModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignContractToMerchant_ContractAssigned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddContractToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddMerchantContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.AssignContractToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignContractToMerchantModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignContractToMerchant_ClientCallFailed_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.AddContractToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddMerchantContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.AssignContractToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignContractToMerchantModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_RemoveContractFromMerchant_ContractRemoved()
    {
        this.TransactionProcessorClient.Setup(e => e.RemoveContractFromMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.RemoveContractFromMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.Contract1Id, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_RemoveContractFromMerchant_ClientCallFailed_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.RemoveContractFromMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.RemoveContractFromMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.Contract1Id, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignOperatorToMerchant_OperatorAssigned()
    {
        this.TransactionProcessorClient.Setup(e => e.AssignOperatorToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AssignOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.AssignOperatorToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignOperatorToMerchantModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignOperatorToMerchant_ClientCallFailed_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.AssignOperatorToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AssignOperatorRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.AssignOperatorToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignOperatorToMerchantModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_RemoveOperatorFromMerchant_OperatorRemoved()
    {
        this.TransactionProcessorClient.Setup(e => e.RemoveOperatorFromMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.RemoveOperatorFromMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.Operator1Id, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_RemoveOperatorFromMerchant_ClientCallFailed_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.RemoveOperatorFromMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.RemoveOperatorFromMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.Operator1Id, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignDeviceToMerchant_DeviceAssigned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddDeviceToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddMerchantDeviceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.AssignDeviceToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignDeviceToMerchantModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_AssignDeviceToMerchant_ClientCallFailed_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.AddDeviceToMerchant(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddMerchantDeviceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure);

        Result result = await this.ApiClient.AssignDeviceToMerchant(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id,
            TestData.AssignDeviceToMerchantModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContract_DataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.CreateContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.CreateContract(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateContractModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContract_ErrorAtServer_NoDataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.CreateContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result result = await this.ApiClient.CreateContract(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateContractModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContract_ClientThrowsException_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.CreateContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CreateContractRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result result = await this.ApiClient.CreateContract(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.CreateContractModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_MakeDeposit_DataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.MakeMerchantDeposit(It.IsAny<String>(), It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<MakeMerchantDepositRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.MakeDeposit(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.MakeDepositModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_MakeDeposit_ErrorAtServer_NoDataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.MakeMerchantDeposit(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<MakeMerchantDepositRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result result = await this.ApiClient.MakeDeposit(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.MakeDepositModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_MakeDeposit_ClientThrowsException_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.MakeMerchantDeposit(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<MakeMerchantDepositRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result result = await this.ApiClient.MakeDeposit(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Merchant1Id, TestData.MakeDepositModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProduct_DataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddProductToContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.CreateContractProduct(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.CreateContractProductModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProduct_ErrorAtServer_NoDataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddProductToContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result result = await this.ApiClient.CreateContractProduct(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.CreateContractProductModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProduct_ClientThrowsException_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.AddProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddProductToContractRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result result = await this.ApiClient.CreateContractProduct(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.CreateContractProductModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProductTransactionFee_DataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddTransactionFeeForProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddTransactionFeeForProductToContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

        Result result = await this.ApiClient.CreateContractProductTransactionFee(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.Contract1Product1Id, TestData.CreateContractProductTransactionFeeModel, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProductTransactionFee_ErrorAtServer_NoDataIsReturned()
    {
        this.TransactionProcessorClient.Setup(e => e.AddTransactionFeeForProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddTransactionFeeForProductToContractRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

        Result result = await this.ApiClient.CreateContractProductTransactionFee(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.Contract1Product1Id, TestData.CreateContractProductTransactionFeeModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ApiClient_CreateContractProductTransactionFee_ClientThrowsException_ResultIsFailed()
    {
        this.TransactionProcessorClient.Setup(e => e.AddTransactionFeeForProductToContract(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AddTransactionFeeForProductToContractRequest>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        Result result = await this.ApiClient.CreateContractProductTransactionFee(TestData.AccessToken, Guid.Empty, TestData.EstateId, TestData.Contract1Id, TestData.Contract1Product1Id, TestData.CreateContractProductTransactionFeeModel, CancellationToken.None);
        result.IsFailed.ShouldBeTrue();
    }
}