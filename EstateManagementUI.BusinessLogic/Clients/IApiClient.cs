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
    }
}
