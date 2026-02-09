using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ContractRequestHandler : IRequestHandler<ContractQueries.GetContractsQuery, Result<List<ContractModels.ContractModel>>>,
    IRequestHandler<ContractQueries.GetContractQuery, Result<ContractModels.ContractModel>>,
    IRequestHandler<ContractCommands.CreateContractCommand, Result>,
    IRequestHandler<ContractCommands.AddProductToContractCommand, Result>,
    IRequestHandler<ContractCommands.AddTransactionFeeToProductCommand, Result>,
    IRequestHandler<ContractCommands.RemoveTransactionFeeFromProductCommand, Result>,
    IRequestHandler<ContractQueries.GetRecentContractsQuery, Result<List<ContractModels.RecentContractModel>>>,
    IRequestHandler<ContractQueries.GetContractsForDropDownQuery, Result<List<ContractModels.ContractDropDownModel>>>
{

    private readonly IApiClient ApiClient;

    public ContractRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<ContractModels.ContractModel>>> Handle(ContractQueries.GetContractsQuery request,
                                                                         CancellationToken cancellationToken) {
        return await this.ApiClient.GetContracts(request, cancellationToken);
    }

    public async Task<Result<ContractModels.ContractModel>> Handle(ContractQueries.GetContractQuery request,
                                                                   CancellationToken cancellationToken) {
        return await this.ApiClient.GetContract(request, cancellationToken);
    }

    public async Task<Result> Handle(ContractCommands.CreateContractCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateContract(request, cancellationToken);
    }

    public async Task<Result> Handle(ContractCommands.AddProductToContractCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddProductToContract(request, cancellationToken);
    }

    public async Task<Result> Handle(ContractCommands.AddTransactionFeeToProductCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddTransactionFeeToProduct(request, cancellationToken);
    }

    public async Task<Result> Handle(ContractCommands.RemoveTransactionFeeFromProductCommand request,
                                     CancellationToken cancellationToken)
    {
        return await this.ApiClient.RemoveTransactionFeeFromProduct(request, cancellationToken);
    }

    public async Task<Result<List<ContractModels.RecentContractModel>>> Handle(ContractQueries.GetRecentContractsQuery request,
                                                                               CancellationToken cancellationToken) {
        return await this.ApiClient.GetRecentContracts(request, cancellationToken);
    }

    public async Task<Result<List<ContractModels.ContractDropDownModel>>> Handle(ContractQueries.GetContractsForDropDownQuery request,
                                                                                 CancellationToken cancellationToken) {
        return await this.ApiClient.GetContracts(request, cancellationToken);
    }
}