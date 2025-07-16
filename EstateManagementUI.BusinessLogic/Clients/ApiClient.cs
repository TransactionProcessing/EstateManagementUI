using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.Models;
using EstateReportingAPI.Client;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.Client;
using FileProcessor.DataTransferObjects.Responses;
using Shared.Logger;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Operator;

namespace EstateManagementUI.BusinessLogic.Clients;

public class ApiClient : IApiClient {
    private readonly ITransactionProcessorClient TransactionProcessorClient;
    private readonly IFileProcessorClient FileProcessorClient;
    private readonly IEstateReportingApiClient EstateReportingApiClient;

    public ApiClient(ITransactionProcessorClient transactionProcessorClient,
                     IFileProcessorClient fileProcessorClient,
                     IEstateReportingApiClient estateReportingApiClient) {
        this.TransactionProcessorClient = transactionProcessorClient;
        this.FileProcessorClient = fileProcessorClient;
        this.EstateReportingApiClient = estateReportingApiClient;
    }

    public async Task<Result> AssignOperatorToMerchant(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid merchantId,
                                                       AssignOperatorToMerchantModel assignOperatorToMerchantModel,
                                                       CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            AssignOperatorRequest apiRequest = ModelFactory.ConvertFrom(assignOperatorToMerchantModel);

            return await this.TransactionProcessorClient.AssignOperatorToMerchant(accessToken, estateId, merchantId, apiRequest, cancellationToken);
        }

        ;

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> AssignContractToMerchant(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid merchantId,
                                                       AssignContractToMerchantModel assignContractToMerchantModel,
                                                       CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            AddMerchantContractRequest apiRequest = ModelFactory.ConvertFrom(assignContractToMerchantModel);

            return await this.TransactionProcessorClient.AddContractToMerchant(accessToken, estateId, merchantId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> AssignDeviceToMerchant(String accessToken,
                                                     Guid actionId,
                                                     Guid estateId,
                                                     Guid merchantId,
                                                     AssignDeviceToMerchantModel assignDeviceToMerchantModel,
                                                     CancellationToken cancellationToken) {
        async Task<Result> ClientMethod()
        {
            AddMerchantDeviceRequest apiRequest = ModelFactory.ConvertFrom(assignDeviceToMerchantModel);
            
            return await this.TransactionProcessorClient.AddDeviceToMerchant(accessToken, estateId, merchantId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> RemoveOperatorFromMerchant(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         Guid operatorId,
                                                         CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            return await this.TransactionProcessorClient.RemoveOperatorFromMerchant(accessToken, estateId, merchantId, operatorId, cancellationToken);
        }

        ;

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> RemoveContractFromMerchant(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         Guid contractId,
                                                         CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            return await this.TransactionProcessorClient.RemoveContractFromMerchant(accessToken, estateId, merchantId, contractId, cancellationToken);
        }

        ;

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<FileImportLogModel>>> GetFileImportLogList(String accessToken,
                                                                             Guid actionId,
                                                                             Guid estateId,
                                                                             Guid merchantId,
                                                                             DateTime startDate,
                                                                             DateTime endDate,
                                                                             CancellationToken cancellationToken) {
        async Task<Result<List<FileImportLogModel>>> ClientMethod() {
            Result<FileImportLogList>? result = await this.FileProcessorClient.GetFileImportLogs(accessToken, estateId, startDate, endDate, merchantId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<EstateModel>> GetEstate(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CancellationToken cancellationToken) {
        Logger.LogWarning("in GetEstate");
        
        async Task<Result<EstateModel>> ClientMethod() {
            Result<EstateResponse>? result = await this.TransactionProcessorClient.GetEstate(accessToken, estateId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<MerchantModel>>> GetMerchants(String accessToken,
                                                                Guid actionId,
                                                                Guid estateId,
                                                                CancellationToken cancellationToken) {
        async Task<Result<List<MerchantModel>>> ClientMethod() {
            Result<List<MerchantResponse>>? result = await this.TransactionProcessorClient.GetMerchants(accessToken, estateId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<MerchantModel>> GetMerchant(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         CancellationToken cancellationToken) {
        async Task<Result<MerchantModel>> ClientMethod() {
            Result<MerchantResponse>? result = await this.TransactionProcessorClient.GetMerchant(accessToken, estateId, merchantId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<OperatorModel>>> GetOperators(String accessToken,
                                                                Guid actionId,
                                                                Guid estateId,
                                                                CancellationToken cancellationToken) {
        async Task<Result<List<OperatorModel>>> ClientMethod() {
            Result<List<OperatorResponse>>? result = await this.TransactionProcessorClient.GetOperators(accessToken, estateId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<OperatorModel>> GetOperator(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid operatorId,
                                                         CancellationToken cancellationToken) {
        async Task<Result<OperatorModel>> ClientMethod() {
            Result<OperatorResponse>? result = await this.TransactionProcessorClient.GetOperator(accessToken, estateId, operatorId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<ContractModel>>> GetContracts(String accessToken,
                                                                Guid actionId,
                                                                Guid estateId,
                                                                CancellationToken cancellationToken) {
        async Task<Result<List<ContractModel>>> ClientMethod() {
            Result<List<ContractResponse>>? result = await this.TransactionProcessorClient.GetContracts(accessToken, estateId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<ContractModel>> GetContract(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid contractId,
                                                         CancellationToken cancellationToken) {
        async Task<Result<ContractModel>> ClientMethod() {
            Result<ContractResponse>? result = await this.TransactionProcessorClient.GetContract(accessToken, estateId, contractId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }
        
        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> CreateOperator(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CreateOperatorModel createOperatorModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            CreateOperatorRequest request = new() { RequireCustomMerchantNumber = createOperatorModel.RequireCustomMerchantNumber, RequireCustomTerminalNumber = createOperatorModel.RequireCustomTerminalNumber, Name = createOperatorModel.OperatorName, OperatorId = createOperatorModel.OperatorId };

            return await this.TransactionProcessorClient.CreateOperator(accessToken, estateId, request, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateOperator(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             UpdateOperatorModel updateOperatorModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            UpdateOperatorRequest request = new() { RequireCustomMerchantNumber = updateOperatorModel.RequireCustomMerchantNumber, RequireCustomTerminalNumber = updateOperatorModel.RequireCustomTerminalNumber, Name = updateOperatorModel.OperatorName };

            return await this.TransactionProcessorClient.UpdateOperator(accessToken, estateId, updateOperatorModel.OperatorId, request, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<FileImportLogModel>> GetFileImportLog(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   Guid merchantId,
                                                                   Guid fileImportLogId,
                                                                   CancellationToken cancellationToken) {
        async Task<Result<FileImportLogModel>> ClientMethod() {
            Result<FileImportLog>? result = await this.FileProcessorClient.GetFileImportLog(accessToken, fileImportLogId, estateId, merchantId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<FileDetailsModel>> GetFileDetails(String accessToken,
                                                               Guid actionId,
                                                               Guid estateId,
                                                               Guid fileId,
                                                               CancellationToken cancellationToken) {
        async Task<Result<FileDetailsModel>> ClientMethod() {
            Result<FileDetails>? result = await this.FileProcessorClient.GetFile(accessToken, estateId, fileId, cancellationToken);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            return ModelFactory.ConvertFrom(result.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<ComparisonDateModel>>> GetComparisonDates(String accessToken,
                                                                            Guid actionId,
                                                                            Guid estateId,
                                                                            CancellationToken cancellationToken) {
        async Task<Result<List<ComparisonDateModel>>> ClientMethod()
        {

            Result<List<ComparisonDate>> apiResponse = await this.EstateReportingApiClient.GetComparisonDates(accessToken, estateId, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<TodaysSalesModel>> GetTodaysSales(String accessToken,
                                                               Guid actionId,
                                                               Guid estateId,
                                                               Int32? merchantReportingId,
                                                               Int32? operatorReportingId,
                                                               DateTime comparisonDate,
                                                               CancellationToken cancellationToken) {
        async Task<Result<TodaysSalesModel>> ClientMethod() {
            Result<TodaysSales> apiResponse = await this.EstateReportingApiClient.GetTodaysSales(accessToken, estateId, merchantReportingId.GetValueOrDefault(0), operatorReportingId.GetValueOrDefault(0), comparisonDate, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<TodaysSettlementModel>> GetTodaysSettlement(String accessToken,
                                                                         Guid actionId,
                                                                         Guid estateId,
                                                                         Int32? merchantReportingId,
                                                                         Int32? operatorReportingId,
                                                                         DateTime comparisonDate,
                                                                         CancellationToken cancellationToken) {
        async Task<Result<TodaysSettlementModel>> ClientMethod() {
            Result<TodaysSettlement> apiResponse = await this.EstateReportingApiClient.GetTodaysSettlement(accessToken, estateId, merchantReportingId.GetValueOrDefault(0), operatorReportingId.GetValueOrDefault(0), comparisonDate, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> GetTodaysSalesCountByHour(String accessToken,
                                                                                           Guid actionId,
                                                                                           Guid estateId,
                                                                                           Guid? merchantId,
                                                                                           Guid? operatorId,
                                                                                           DateTime comparisonDate,
                                                                                           CancellationToken cancellationToken) {
        async Task<Result<List<TodaysSalesCountByHourModel>>> ClientMethod() {
            Result<List<TodaysSalesCountByHour>> apiResponse = await this.EstateReportingApiClient.GetTodaysSalesCountByHour(accessToken, estateId, 0, 0, comparisonDate, cancellationToken);

            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> GetTodaysSalesValueByHour(String accessToken,
                                                                                           Guid actionId,
                                                                                           Guid estateId,
                                                                                           Guid? merchantId,
                                                                                           Guid? operatorId,
                                                                                           DateTime comparisonDate,
                                                                                           CancellationToken cancellationToken) {
        async Task<Result<List<TodaysSalesValueByHourModel>>> ClientMethod() {
            Result<List<TodaysSalesValueByHour>> apiResponse = await this.EstateReportingApiClient.GetTodaysSalesValueByHour(accessToken, estateId, 0, 0, comparisonDate, cancellationToken);

            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    private async Task<Result<T>> CallClientMethod<T>(Func<Task<Result<T>>> clientMethod,
                                                      CancellationToken cancellationToken) {
        try {
            Result<T> clientResult = await clientMethod();
            return clientResult;
        }
        catch (Exception e) {
            Logger.LogError(e);
            return Result.Failure(e.Message);
        }
    }

    private async Task<Result> CallClientMethod(Func<Task<Result>> clientMethod,
                                                CancellationToken cancellationToken) {
        try {
            Result clientResult = await clientMethod();
            return clientResult;
        }
        catch (Exception e) {
            Logger.LogError(e);
            return Result.Failure(e.Message);
        }
    }

    public async Task<Result<TodaysSalesModel>> GetTodaysFailedSales(String accessToken,
                                                                     Guid estateId,
                                                                     String responseCode,
                                                                     DateTime comparisonDate,
                                                                     CancellationToken cancellationToken) {
        async Task<Result<TodaysSalesModel>> ClientMethod() {
            Result<TodaysSales> apiResponse = await this.EstateReportingApiClient.GetTodaysFailedSales(accessToken, estateId, 0, 0, responseCode, comparisonDate, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> GetTopBottomOperatorData(String accessToken,
                                                                                         Guid estateId,
                                                                                         TopBottom topBottom,
                                                                                         Int32 resultCount,
                                                                                         CancellationToken cancellationToken) {
        async Task<Result<List<TopBottomOperatorDataModel>>> ClientMethod() {
            Result<List<TopBottomOperatorData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomOperatorData(accessToken, estateId, topBottom, resultCount, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> GetTopBottomMerchantData(String accessToken,
                                                                                         Guid estateId,
                                                                                         TopBottom topBottom,
                                                                                         Int32 resultCount,
                                                                                         CancellationToken cancellationToken) {
        async Task<Result<List<TopBottomMerchantDataModel>>> ClientMethod() {
            Result<List<TopBottomMerchantData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomMerchantData(accessToken, estateId, topBottom, resultCount, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomProductDataModel>>> GetTopBottomProductData(String accessToken,
                                                                                       Guid estateId,
                                                                                       TopBottom topBottom,
                                                                                       Int32 resultCount,
                                                                                       CancellationToken cancellationToken) {
        async Task<Result<List<TopBottomProductDataModel>>> ClientMethod() {
            Result<List<TopBottomProductData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomProductData(accessToken, estateId, topBottom, resultCount, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken) {
        async Task<Result<MerchantKpiModel>> ClientMethod() {
            Result<MerchantKpi> apiResponse = await this.EstateReportingApiClient.GetMerchantKpi(accessToken, estateId, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    //public TopBottom Convert(TopBottom model) {
    //    return model switch {
    //        TopBottom.Bottom => TopBottom.Bottom,
    //        _ => TopBottom.Top
    //    };
    //}

    public async Task<Result<LastSettlementModel>> GetLastSettlement(String accessToken,
                                                                     Guid estateId,
                                                                     Int32? merchantReportingId,
                                                                     Int32? operatorReportingId,
                                                                     CancellationToken cancellationToken) {
        async Task<Result<LastSettlementModel>> ClientMethod() {
            Result<LastSettlement> apiResponse = await this.EstateReportingApiClient.GetLastSettlement(accessToken, estateId, cancellationToken);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }
    
    public async Task<Result> CreateContract(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CreateContractModel createContractModel,
                                             CancellationToken cancellationToken)
    {
        async Task<Result> ClientMethod()
        {
            CreateContractRequest apiRequest = ModelFactory.ConvertFrom(createContractModel);
            return await this.TransactionProcessorClient.CreateContract(accessToken, estateId, apiRequest, cancellationToken);
        }
        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }
    public async Task<Result> CreateMerchant(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CreateMerchantModel createMerchantModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            CreateMerchantRequest apiRequest = ModelFactory.ConvertFrom(createMerchantModel);

            return await this.TransactionProcessorClient.CreateMerchant(accessToken, estateId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchant(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             Guid merchantId,
                                             UpdateMerchantModel updateMerchantModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            UpdateMerchantRequest apiRequest = ModelFactory.ConvertFrom(updateMerchantModel);

            return await this.TransactionProcessorClient.UpdateMerchant(accessToken, estateId, merchantId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    AddressModel updateAddressModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            Address apiRequest = ModelFactory.ConvertFrom(updateAddressModel);

            return await this.TransactionProcessorClient.UpdateMerchantAddress(accessToken, estateId, merchantId, updateAddressModel.AddressId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchantContact(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    ContactModel updateContactModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            Contact apiRequest = ModelFactory.ConvertFrom(updateContactModel);

            return await this.TransactionProcessorClient.UpdateMerchantContact(accessToken, estateId, merchantId, updateContactModel.ContactId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> MakeDeposit(String accessToken,
                                          Guid actionId,
                                          Guid estateId,
                                          Guid merchantId,
                                          MakeDepositModel makeDepositModel,
                                          CancellationToken cancellationToken) {
        async Task<Result> ClientMethod()
        {
            MakeMerchantDepositRequest apiRequest = ModelFactory.ConvertFrom(makeDepositModel);
            
            return await this.TransactionProcessorClient.MakeMerchantDeposit(accessToken, estateId, merchantId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> CreateContractProduct(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid contractId,
                                                    CreateContractProductModel createContractProductModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod()
        {
            AddProductToContractRequest apiRequest = ModelFactory.ConvertFrom(createContractProductModel);

            return await this.TransactionProcessorClient.AddProductToContract(accessToken, estateId, contractId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> CreateContractProductTransactionFee(String accessToken,
                                                                  Guid actionId,
                                                                  Guid estateId,
                                                                  Guid contractId,
                                                                  Guid productId,
                                                                  CreateContractProductTransactionFeeModel createContractProductTransactionFeeModel,
                                                                  CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            AddTransactionFeeForProductToContractRequest apiRequest = ModelFactory.ConvertFrom(createContractProductTransactionFeeModel);

            return await this.TransactionProcessorClient.AddTransactionFeeForProductToContract(accessToken, estateId, contractId, productId, apiRequest, cancellationToken);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }
}