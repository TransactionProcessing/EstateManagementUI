using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Requests.Operator;
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
}