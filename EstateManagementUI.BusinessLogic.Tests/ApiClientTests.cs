﻿using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Requests.Operator;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Merchant;
using EstateManagementUI.Testing;
using EstateReportingAPI.Client;
using FileProcessor.Client;
using Moq;
using Shared.Logger;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class ApiClientTests {
        private IApiClient ApiClient;
        private Mock<IEstateClient> EstateClient;
        private Mock<IFileProcessorClient> FileProcessorClient;
        private Mock<IEstateReportingApiClient> EstateReportingApiClient;

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
                    It.IsAny<CancellationToken>())).ReturnsAsync(new CreateOperatorResponse {
                    OperatorId = TestData.Operator1Id, EstateId = TestData.EstateId
                });

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
                It.IsAny<UpdateOperatorRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

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
    }
}
