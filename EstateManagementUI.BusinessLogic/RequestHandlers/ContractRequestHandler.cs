using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ContractRequestHandler : IRequestHandler<Queries.GetContractsQuery, List<ContractModel>>,
                                      IRequestHandler<Queries.GetContractQuery, Result<ContractModel>> {
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

    public async Task<Result<ContractModel>> Handle(Queries.GetContractQuery request,
                                                    CancellationToken cancellationToken) {
        ContractModel model = await this.ApiClient.GetContract(request.AccessToken, Guid.Empty, request.EstateId, request.ContractId, cancellationToken);
        return model;
    }
}