using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ContractRequestHandler : IRequestHandler<Queries.GetContractsQuery, List<ContractModel>>
{
    private readonly IApiClient ApiClient;

    public ContractRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<List<ContractModel>> Handle(Queries.GetContractsQuery request,
                                                  CancellationToken cancellationToken) {
        List<ContractModel> models = await this.ApiClient.GetContracts(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return models;
    }
}