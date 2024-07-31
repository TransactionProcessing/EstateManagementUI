using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class OperatorRequestHandler : IRequestHandler<Queries.GetOperatorsQuery, List<OperatorModel>>
{
    private readonly IApiClient ApiClient;

    public OperatorRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<List<OperatorModel>> Handle(Queries.GetOperatorsQuery request,
                                                  CancellationToken cancellationToken)
    {
        List<OperatorModel> models = await this.ApiClient.GetOperators(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return models;
    }
}