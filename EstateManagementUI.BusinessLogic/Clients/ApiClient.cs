using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses.Estate;
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