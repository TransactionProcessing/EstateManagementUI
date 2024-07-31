using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagementUI.BusinessLogic.Clients
{
    public interface IApiClient
    {
        Task<EstateModel> GetEstate(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CancellationToken cancellationToken);

        Task<List<MerchantModel>> GetMerchants(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);

        Task<List<OperatorModel>> GetOperators(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);

        Task<List<ContractModel>> GetContracts(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);
    }
}
