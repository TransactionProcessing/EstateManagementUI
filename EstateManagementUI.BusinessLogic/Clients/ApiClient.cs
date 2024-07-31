using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.Models;
using Shared.Logger;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients;

public class ApiClient : IApiClient {
    private readonly IEstateClient EstateClient;

    public ApiClient(IEstateClient estateClient) {
        this.EstateClient = estateClient;
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

    public async Task<List<OperatorModel>> GetOperators(String accessToken,
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

    public async Task<List<ContractModel>> GetContracts(String accessToken,
                                                  Guid actionId,
                                                  Guid estateId,
                                                  CancellationToken cancellationToken) {
        async Task<Result<List<ContractModel>>> ClientMethod()
        {
            List<ContractResponse>? operators = await this.EstateClient.GetContracts(accessToken, estateId, cancellationToken);

            return ModelFactory.ConvertFrom(operators);
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
}