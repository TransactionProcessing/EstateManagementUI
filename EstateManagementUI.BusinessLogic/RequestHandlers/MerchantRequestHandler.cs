using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class MerchantRequestHandler : IRequestHandler<Queries.GetMerchantsQuery, List<MerchantModel>>,
                                      IRequestHandler<Commands.AddNewMerchantCommand, Result>
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

    public async Task<Result> Handle(Commands.AddNewMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.CreateMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.CreateMerchantModel, cancellationToken);
        return result;
    }
}