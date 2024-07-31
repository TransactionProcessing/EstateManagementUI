using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class MerchantRequestHandler : IRequestHandler<Queries.GetMerchantsQuery, List<MerchantModel>>
{
    private readonly IApiClient ApiClient;

    public MerchantRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<List<MerchantModel>> Handle(Queries.GetMerchantsQuery request,
                                                  CancellationToken cancellationToken)
    {
        List<MerchantModel> models = await this.ApiClient.GetMerchants(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return models;
    }
}