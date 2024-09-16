using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Requests.Operator;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.Models;
using EstateReportingAPI.Client;
using EstateReportingAPI.DataTransferObjects;
using EstateReportingAPI.DataTrasferObjects;
using FileProcessor.Client;
using FileProcessor.DataTransferObjects.Responses;
using Shared.Logger;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients;

public class ApiClient : IApiClient {
    private readonly IEstateClient EstateClient;
    private readonly IFileProcessorClient FileProcessorClient;
    private readonly IEstateReportingApiClient EstateReportingApiClient;

    public ApiClient(IEstateClient estateClient, IFileProcessorClient fileProcessorClient,
                     IEstateReportingApiClient estateReportingApiClient) {
        this.EstateClient = estateClient;
        this.FileProcessorClient = fileProcessorClient;
        this.EstateReportingApiClient = estateReportingApiClient;
    }

    public async Task<Result<List<FileImportLogModel>>> GetFileImportLogList(String accessToken,
                                                                            Guid actionId,
                                                                            Guid estateId,
                                                                            Guid merchantId,
                                                                            DateTime startDate,
                                                                            DateTime endDate,
                                                                            CancellationToken cancellationToken) {
        async Task<Result<List<FileImportLogModel>>> ClientMethod() {
            FileImportLogList? fileImportLogs = await this.FileProcessorClient.GetFileImportLogs(accessToken, estateId,
                startDate, endDate, merchantId, cancellationToken);

            return ModelFactory.ConvertFrom(fileImportLogs);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<EstateModel> GetEstate(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CancellationToken cancellationToken) {
        async Task<Result<EstateModel>> ClientMethod()
        {
            EstateResponse? estate = await this.EstateClient.GetEstate(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(estate);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<List<MerchantModel>> GetMerchants(String accessToken, Guid actionId, Guid estateId, CancellationToken cancellationToken)
    {
        async Task<Result<List<MerchantModel>>> ClientMethod()
        {
            List<MerchantResponse> merchants = await this.EstateClient.GetMerchants(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(merchants);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<OperatorModel>>> GetOperators(String accessToken,
                                                  Guid actionId,
                                                  Guid estateId,
                                                  CancellationToken cancellationToken) {
        async Task<Result<List<OperatorModel>>> ClientMethod()
        {
            List<OperatorResponse>? operators = await this.EstateClient.GetOperators(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(operators);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<OperatorModel>> GetOperator(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid operatorId,
                                                         CancellationToken cancellationToken) {
        async Task<Result<OperatorModel>> ClientMethod()
        {
            OperatorResponse @operator = await this.EstateClient.GetOperator(accessToken, estateId, operatorId, cancellationToken);

            return ModelFactory.ConvertFrom(@operator);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<List<ContractModel>> GetContracts(String accessToken,
                                                        Guid actionId,
                                                        Guid estateId,
                                                        CancellationToken cancellationToken) {
        async Task<Result<List<ContractModel>>> ClientMethod()
        {
            List<ContractResponse>? contracts = await this.EstateClient.GetContracts(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(contracts);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<ContractModel>> GetContract(String accessToken,
                                                                Guid actionId,
                                                                Guid estateId,
                                                                Guid contractId,
                                                                CancellationToken cancellationToken) {
        async Task<Result<ContractModel>> ClientMethod()
        {
            ContractResponse? contract = await this.EstateClient.GetContract(accessToken, estateId, contractId, cancellationToken);
            
            return ModelFactory.ConvertFrom(contract);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);

    }

    public async Task<Result> CreateOperator(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             CreateOperatorModel createOperatorModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod() {
            CreateOperatorRequest request = new CreateOperatorRequest {
                RequireCustomMerchantNumber = createOperatorModel.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = createOperatorModel.RequireCustomTerminalNumber,
                Name = createOperatorModel.OperatorName,
                OperatorId = createOperatorModel.OperatorId
            };

            await this.EstateClient.CreateOperator(accessToken, estateId, request, cancellationToken);

            return Result.Success();
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result> UpdateOperator(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             UpdateOperatorModel updateOperatorModel,
                                             CancellationToken cancellationToken) {
        async Task<Result> ClientMethod()
        {
            UpdateOperatorRequest request = new UpdateOperatorRequest
            {
                RequireCustomMerchantNumber = updateOperatorModel.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = updateOperatorModel.RequireCustomTerminalNumber,
                Name = updateOperatorModel.OperatorName
            };

            await this.EstateClient.UpdateOperator(accessToken, estateId, updateOperatorModel.OperatorId, request, cancellationToken);

            return Result.Success();
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<FileImportLogModel>> GetFileImportLog(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   Guid merchantId,
                                                                   Guid fileImportLogId,
                                                                   CancellationToken cancellationToken) {
        async Task<Result<FileImportLogModel>> ClientMethod()
        {
            FileImportLog? response = await this.FileProcessorClient.GetFileImportLog(accessToken, fileImportLogId, estateId, merchantId, cancellationToken);

            return ModelFactory.ConvertFrom(response);


        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<FileDetailsModel>> GetFileDetails(String accessToken,
                                             Guid actionId,
                                             Guid estateId,
                                             Guid fileId,
                                             CancellationToken cancellationToken) {
        
        async Task<Result<FileDetailsModel>> ClientMethod()
        {
            FileDetails? response = await this.FileProcessorClient.GetFile(accessToken, estateId, fileId, cancellationToken);

             return ModelFactory.ConvertFrom(response);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<ComparisonDateModel>>> GetComparisonDates(String accessToken, Guid actionId, Guid estateId, CancellationToken cancellationToken)
    {
        async Task<Result<List<ComparisonDateModel>>> ClientMethod()
        {
            List<ComparisonDate> apiResponse = await this.EstateReportingApiClient.GetComparisonDates(accessToken, estateId, cancellationToken);

            return Result.Success(ModelFactory.ConvertFrom(apiResponse));
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<TodaysSalesModel>> GetTodaysSales(String accessToken,
                                                               Guid actionId,
                                                               Guid estateId,
                                                               Int32? merchantReportingId,
                                                               Int32? operatorReportingId,
                                                               DateTime comparisonDate,
                                                               CancellationToken cancellationToken)
    {
        async Task<Result<TodaysSalesModel>> ClientMethod()
        {
            TodaysSales apiResponse = await this.EstateReportingApiClient.GetTodaysSales(accessToken, estateId, merchantReportingId.GetValueOrDefault(0), operatorReportingId.GetValueOrDefault(0), comparisonDate, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<TodaysSettlementModel>> GetTodaysSettlement(String accessToken,
                                                                         Guid actionId,
                                                                         Guid estateId,
                                                                         Int32? merchantReportingId,
                                                                         Int32? operatorReportingId,
                                                                         DateTime comparisonDate,
                                                                         CancellationToken cancellationToken)
    {
        async Task<Result<TodaysSettlementModel>> ClientMethod()
        {
            TodaysSettlement apiResponse = await this.EstateReportingApiClient.GetTodaysSettlement(accessToken, estateId, merchantReportingId.GetValueOrDefault(0), operatorReportingId.GetValueOrDefault(0), comparisonDate, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> GetTodaysSalesCountByHour(string accessToken, Guid actionId, Guid estateId, Guid? merchantId, Guid? operatorId, DateTime comparisonDate,
        CancellationToken cancellationToken)
    {
        async Task<Result<List<TodaysSalesCountByHourModel>>> ClientMethod()
        {
            List<TodaysSalesCountByHour> apiResponse =
                await this.EstateReportingApiClient.GetTodaysSalesCountByHour(accessToken, estateId, 0, 0,
                    comparisonDate, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> GetTodaysSalesValueByHour(String accessToken, Guid actionId, Guid estateId, Guid? merchantId, Guid? operatorId, DateTime comparisonDate, CancellationToken cancellationToken)
    {
        async Task<Result<List<TodaysSalesValueByHourModel>>> ClientMethod()
        {
            List<TodaysSalesValueByHour> apiResponse = await this.EstateReportingApiClient.GetTodaysSalesValueByHour(accessToken, estateId, 0, 0, comparisonDate, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    private async Task<Result<T>> CallClientMethod<T>(Func<Task<Result<T>>> clientMethod, CancellationToken cancellationToken)
    {
        try
        {
            Result<T> clientResult = await clientMethod();
            return Result.Success(clientResult);
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            return Result.Failure(e.Message);

        }
    }

    private async Task<Result> CallClientMethod(Func<Task<Result>> clientMethod, CancellationToken cancellationToken)
    {
        try
        {
            Result clientResult = await clientMethod();
            return Result.Success(clientResult);
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            return Result.Failure(e.Message);

        }
    }

    public async Task<Result<TodaysSalesModel>> GetTodaysFailedSales(String accessToken, Guid estateId, String responseCode, DateTime comparisonDate, CancellationToken cancellationToken)
    {
        async Task<Result<TodaysSalesModel>> ClientMethod()
        {
            TodaysSales apiResponse = await this.EstateReportingApiClient.GetTodaysFailedSales(accessToken, estateId, 0, 0, responseCode, comparisonDate, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> GetTopBottomOperatorData(String accessToken, Guid estateId, TopBottom topBottom, Int32 resultCount, CancellationToken cancellationToken)
    {
        async Task<Result<List<TopBottomOperatorDataModel>>> ClientMethod()
        {
            List<TopBottomOperatorData> apiResponse = await this.EstateReportingApiClient.GetTopBottomOperatorData(accessToken, estateId, this.Convert(topBottom), resultCount, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> GetTopBottomMerchantData(string accessToken,
                                                                                         Guid estateId,
                                                                                         TopBottom topBottom,
                                                                                         int resultCount,
                                                                                         CancellationToken cancellationToken)
    {
        async Task<Result<List<TopBottomMerchantDataModel>>> ClientMethod()
        {
            List<TopBottomMerchantData> apiResponse = await this.EstateReportingApiClient.GetTopBottomMerchantData(accessToken, estateId, this.Convert(topBottom), resultCount, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<List<TopBottomProductDataModel>>> GetTopBottomProductData(string accessToken,
                                                                                       Guid estateId,
                                                                                       TopBottom topBottom,
                                                                                       int resultCount,
                                                                                       CancellationToken cancellationToken)
    {
        async Task<Result<List<TopBottomProductDataModel>>> ClientMethod()
        {
            List<TopBottomProductData> apiResponse = await this.EstateReportingApiClient.GetTopBottomProductData(accessToken, estateId, this.Convert(topBottom), resultCount, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    public async Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken, Guid estateId, CancellationToken cancellationToken)
    {
        async Task<Result<MerchantKpiModel>> ClientMethod()
        {
            MerchantKpi apiResponse = await this.EstateReportingApiClient.GetMerchantKpi(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }

    private EstateReportingAPI.DataTransferObjects.TopBottom Convert(TopBottom model)
    {
        return model switch
        {
            TopBottom.Bottom => EstateReportingAPI.DataTransferObjects.TopBottom.Bottom,
            _ => EstateReportingAPI.DataTransferObjects.TopBottom.Top
        };
    }

    public async Task<Result<LastSettlementModel>> GetLastSettlement(String accessToken, Guid estateId, Int32? merchantReportingId, Int32? operatorReportingId, CancellationToken cancellationToken)
    {
        async Task<Result<LastSettlementModel>> ClientMethod()
        {
            LastSettlement apiResponse = await this.EstateReportingApiClient.GetLastSettlement(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(apiResponse);
        }

        return await this.CallClientMethod(ClientMethod, cancellationToken);
    }
}