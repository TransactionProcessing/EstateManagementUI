using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.Models;
using EstateReportingAPI.Client;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.Client;
using FileProcessor.DataTransferObjects.Responses;
using SecurityService.Client;
using SecurityService.DataTransferObjects.Responses;
using Shared.General;
using Shared.Logger;
using Shared.Results;
using SimpleResults;
using System.Runtime.CompilerServices;
using System.Threading;
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
    private readonly ISecurityServiceClient SecurityServiceClient;

    public ApiClient(ITransactionProcessorClient transactionProcessorClient,
                     IFileProcessorClient fileProcessorClient,
                     IEstateReportingApiClient estateReportingApiClient,
                     ISecurityServiceClient securityServiceClient) {
        this.TransactionProcessorClient = transactionProcessorClient;
        this.FileProcessorClient = fileProcessorClient;
        this.EstateReportingApiClient = estateReportingApiClient;
        this.SecurityServiceClient = securityServiceClient;
    }

    public async Task<Result> AssignOperatorToMerchant(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid merchantId,
                                                       AssignOperatorToMerchantModel assignOperatorToMerchantModel,
                                                       CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            AssignOperatorRequest apiRequest = ModelFactory.ConvertFrom(assignOperatorToMerchantModel);

            return await this.TransactionProcessorClient.AssignOperatorToMerchant(token, estateId, merchantId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> AssignContractToMerchant(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid merchantId,
                                                       AssignContractToMerchantModel assignContractToMerchantModel,
                                                       CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            AddMerchantContractRequest apiRequest = ModelFactory.ConvertFrom(assignContractToMerchantModel);

            return await this.TransactionProcessorClient.AddContractToMerchant(token, estateId, merchantId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> AssignDeviceToMerchant(String accessToken,
                                                     Guid actionId,
                                                     Guid estateId,
                                                     Guid merchantId,
                                                     AssignDeviceToMerchantModel assignDeviceToMerchantModel,
                                                     CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct)
        {
            AddMerchantDeviceRequest apiRequest = ModelFactory.ConvertFrom(assignDeviceToMerchantModel);
            
            return await this.TransactionProcessorClient.AddDeviceToMerchant(token, estateId, merchantId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> RemoveOperatorFromMerchant(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         Guid operatorId,
                                                         CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            return await this.TransactionProcessorClient.RemoveOperatorFromMerchant(token, estateId, merchantId, operatorId, ct);
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
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            return await this.TransactionProcessorClient.RemoveContractFromMerchant(token, estateId, merchantId, contractId, ct);
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
        async Task<Result<List<FileImportLogModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<FileImportLogList>? result = await this.FileProcessorClient.GetFileImportLogs(token, estateId, startDate, endDate, merchantId, ct);
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
        
        async Task<Result<EstateModel>> ClientMethod(String token, CancellationToken ct) {
            Result<EstateResponse>? result = await this.TransactionProcessorClient.GetEstate(token, estateId, ct);
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
        async Task<Result<List<MerchantModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<MerchantResponse>>? result = await this.TransactionProcessorClient.GetMerchants(token, estateId, ct);
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
        async Task<Result<MerchantModel>> ClientMethod(String token, CancellationToken ct) {
            Result<MerchantResponse>? result = await this.TransactionProcessorClient.GetMerchant(token, estateId, merchantId, ct);
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
        async Task<Result<List<OperatorModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<OperatorResponse>>? result = await this.TransactionProcessorClient.GetOperators(token, estateId, ct);
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
        async Task<Result<OperatorModel>> ClientMethod(String token, CancellationToken ct) {
            Result<OperatorResponse>? result = await this.TransactionProcessorClient.GetOperator(token, estateId, operatorId, ct);
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
        async Task<Result<List<ContractModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<ContractResponse>>? result = await this.TransactionProcessorClient.GetContracts(token, estateId, ct);
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
        async Task<Result<ContractModel>> ClientMethod(String token, CancellationToken ct) {
            Result<ContractResponse>? result = await this.TransactionProcessorClient.GetContract(token, estateId, contractId, ct);
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
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            CreateOperatorRequest request = new() { RequireCustomMerchantNumber = createOperatorModel.RequireCustomMerchantNumber, RequireCustomTerminalNumber = createOperatorModel.RequireCustomTerminalNumber, Name = createOperatorModel.OperatorName, OperatorId = createOperatorModel.OperatorId };

            return await this.TransactionProcessorClient.CreateOperator(token, estateId, request, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateOperator(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             UpdateOperatorModel updateOperatorModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            UpdateOperatorRequest request = new() { RequireCustomMerchantNumber = updateOperatorModel.RequireCustomMerchantNumber, RequireCustomTerminalNumber = updateOperatorModel.RequireCustomTerminalNumber, Name = updateOperatorModel.OperatorName };

            return await this.TransactionProcessorClient.UpdateOperator(token, estateId, updateOperatorModel.OperatorId, request, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<FileImportLogModel>> GetFileImportLog(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   Guid merchantId,
                                                                   Guid fileImportLogId,
                                                                   CancellationToken cancellationToken) {
        async Task<Result<FileImportLogModel>> ClientMethod(String token, CancellationToken ct) {
            Result<FileImportLog>? result = await this.FileProcessorClient.GetFileImportLog(token, fileImportLogId, estateId, merchantId, ct);
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
        async Task<Result<FileDetailsModel>> ClientMethod(String token, CancellationToken ct) {
            Result<FileDetails>? result = await this.FileProcessorClient.GetFile(token, estateId, fileId, ct);
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
        async Task<Result<List<ComparisonDateModel>>> ClientMethod(String token, CancellationToken ct)
        {

            Result<List<ComparisonDate>> apiResponse = await this.EstateReportingApiClient.GetComparisonDates(token, estateId, ct);
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
        async Task<Result<TodaysSalesModel>> ClientMethod(String token, CancellationToken ct) {
            Result<TodaysSales> apiResponse = await this.EstateReportingApiClient.GetTodaysSales(token, estateId, merchantReportingId.GetValueOrDefault(0), operatorReportingId.GetValueOrDefault(0), comparisonDate, ct);
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
        async Task<Result<TodaysSettlementModel>> ClientMethod(String token, CancellationToken ct) {
            Result<TodaysSettlement> apiResponse = await this.EstateReportingApiClient.GetTodaysSettlement(token, estateId, merchantReportingId.GetValueOrDefault(0), 
                operatorReportingId.GetValueOrDefault(0), comparisonDate, ct);
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
        async Task<Result<List<TodaysSalesCountByHourModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<TodaysSalesCountByHour>> apiResponse = await this.EstateReportingApiClient.GetTodaysSalesCountByHour(token, estateId, 0, 0, comparisonDate, ct);

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
        async Task<Result<List<TodaysSalesValueByHourModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<TodaysSalesValueByHour>> apiResponse = await this.EstateReportingApiClient.GetTodaysSalesValueByHour(token, estateId, 0, 0, comparisonDate, ct);

            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    private async Task<Result<T>> CallClientMethod<T>(Func<String, CancellationToken, Task<Result<T>>> clientMethod,
                                                      CancellationToken cancellationToken) {
        try {
            Result<TokenResponse> token = await GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<T> clientResult = await clientMethod(token.Data.AccessToken, cancellationToken);
            return clientResult;
        }
        catch (Exception e) {
            Logger.LogError(e);
            return Result.Failure(e.Message);
        }
    }
    
    private async Task<Result> CallClientMethod(Func<String, CancellationToken, Task<Result>> clientMethod,
                                               CancellationToken cancellationToken) {
        try {
            Result<TokenResponse> token = await GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result clientResult = await clientMethod(token.Data.AccessToken, cancellationToken);
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
        async Task<Result<TodaysSalesModel>> ClientMethod(String token, CancellationToken ct) {
            Result<TodaysSales> apiResponse = await this.EstateReportingApiClient.GetTodaysFailedSales(token, estateId, 0, 0, responseCode, comparisonDate, ct);
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
        async Task<Result<List<TopBottomOperatorDataModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<TopBottomOperatorData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomOperatorData(token, estateId, topBottom, resultCount, ct);
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
        async Task<Result<List<TopBottomMerchantDataModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<TopBottomMerchantData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomMerchantData(token, estateId, topBottom, resultCount, ct);
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
        async Task<Result<List<TopBottomProductDataModel>>> ClientMethod(String token, CancellationToken ct) {
            Result<List<TopBottomProductData>> apiResponse = await this.EstateReportingApiClient.GetTopBottomProductData(token, estateId, topBottom, resultCount, ct);
            if (apiResponse.IsFailed)
                return ResultHelpers.CreateFailure(apiResponse);
            return ModelFactory.ConvertFrom(apiResponse.Data);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken) {
        async Task<Result<MerchantKpiModel>> ClientMethod(String token, CancellationToken ct) {
            Result<MerchantKpi> apiResponse = await this.EstateReportingApiClient.GetMerchantKpi(token, estateId, ct);
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
        async Task<Result<LastSettlementModel>> ClientMethod(String token, CancellationToken ct) {
            Result<LastSettlement> apiResponse = await this.EstateReportingApiClient.GetLastSettlement(token, estateId, ct);
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
        async Task<Result> ClientMethod(String token, CancellationToken ct)
        {
            CreateContractRequest apiRequest = ModelFactory.ConvertFrom(createContractModel);
            return await this.TransactionProcessorClient.CreateContract(token, estateId, apiRequest, ct);
        }
        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }
    public async Task<Result> CreateMerchant(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CreateMerchantModel createMerchantModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            CreateMerchantRequest apiRequest = ModelFactory.ConvertFrom(createMerchantModel);

            return await this.TransactionProcessorClient.CreateMerchant(token, estateId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchant(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             Guid merchantId,
                                             UpdateMerchantModel updateMerchantModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            UpdateMerchantRequest apiRequest = ModelFactory.ConvertFrom(updateMerchantModel);

            return await this.TransactionProcessorClient.UpdateMerchant(token, estateId, merchantId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchantAddress(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    AddressModel updateAddressModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            Address apiRequest = ModelFactory.ConvertFrom(updateAddressModel);

            return await this.TransactionProcessorClient.UpdateMerchantAddress(token, estateId, merchantId, updateAddressModel.AddressId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateMerchantContact(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    ContactModel updateContactModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            Contact apiRequest = ModelFactory.ConvertFrom(updateContactModel);

            return await this.TransactionProcessorClient.UpdateMerchantContact(token, estateId, merchantId, updateContactModel.ContactId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> MakeDeposit(String accessToken,
                                          Guid actionId,
                                          Guid estateId,
                                          Guid merchantId,
                                          MakeDepositModel makeDepositModel,
                                          CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct)
        {
            MakeMerchantDepositRequest apiRequest = ModelFactory.ConvertFrom(makeDepositModel);
            
            return await this.TransactionProcessorClient.MakeMerchantDeposit(token, estateId, merchantId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> CreateContractProduct(String accessToken,
                                                    Guid actionId,
                                                    Guid estateId,
                                                    Guid contractId,
                                                    CreateContractProductModel createContractProductModel,
                                                    CancellationToken cancellationToken) {
        async Task<Result> ClientMethod(String token, CancellationToken ct)
        {
            AddProductToContractRequest apiRequest = ModelFactory.ConvertFrom(createContractProductModel);

            return await this.TransactionProcessorClient.AddProductToContract(token, estateId, contractId, apiRequest, ct);
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
        async Task<Result> ClientMethod(String token, CancellationToken ct) {
            AddTransactionFeeForProductToContractRequest apiRequest = ModelFactory.ConvertFrom(createContractProductTransactionFeeModel);

            return await this.TransactionProcessorClient.AddTransactionFeeForProductToContract(token, estateId, contractId, productId, apiRequest, ct);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    private TokenResponse TokenResponse;

    private async Task<Result<TokenResponse>> GetToken(CancellationToken cancellationToken)
    {
        // Get a token to talk to the estate service
        String clientId = ConfigurationReader.GetValue("AppSettings", "BackEndClientId");
        String clientSecret = ConfigurationReader.GetValue("AppSettings", "BackEndClientSecret");
        Logger.LogDebug($"Client Id is {clientId}");
        Logger.LogDebug($"Client Secret is {clientSecret}");

        if (this.TokenResponse == null)
        {
            Result<TokenResponse> tokenResult = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
            if (tokenResult.IsFailed)
                return ResultHelpers.CreateFailure(tokenResult);
            TokenResponse token = tokenResult.Data;
            Logger.LogDebug($"Token is {token.AccessToken}");
            this.TokenResponse = token;
        }

        if (this.TokenResponse.Expires.UtcDateTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(2))
        {
            Logger.LogDebug($"Token is about to expire at {this.TokenResponse.Expires.DateTime:O}");
            Result<TokenResponse> tokenResult = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
            if (tokenResult.IsFailed)
                return ResultHelpers.CreateFailure(tokenResult);
            TokenResponse token = tokenResult.Data;
            Logger.LogDebug($"Token is {token.AccessToken}");
            this.TokenResponse = token;
        }

        return Result.Success(this.TokenResponse);
    }
}