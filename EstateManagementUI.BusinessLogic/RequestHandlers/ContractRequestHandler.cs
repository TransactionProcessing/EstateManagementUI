﻿using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ContractRequestHandler : IRequestHandler<Queries.GetContractsQuery, Result<List<ContractModel>>>,
                                      IRequestHandler<Queries.GetContractQuery, Result<ContractModel>>,
IRequestHandler<Commands.CreateContractCommand, Result>,
IRequestHandler<Commands.CreateContractProductCommand, Result>,
                                      IRequestHandler<Commands.CreateContractProductTransactionFeeCommand, Result> {
    private readonly IApiClient ApiClient;

    public ContractRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<ContractModel>>> Handle(Queries.GetContractsQuery request,
                                                          CancellationToken cancellationToken) {
        Result<List<ContractModel>> models = await this.ApiClient.GetContracts(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return models;
    }

    public async Task<Result<ContractModel>> Handle(Queries.GetContractQuery request,
                                                    CancellationToken cancellationToken) {
        Result<ContractModel> model = await this.ApiClient.GetContract(request.AccessToken, Guid.Empty, request.EstateId, request.ContractId, cancellationToken);
        return model;
    }

    public async Task<Result> Handle(Commands.CreateContractCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateContract(request.AccessToken, Guid.Empty, request.EstateId, request.CreateContractModel, cancellationToken);
    }

    public async Task<Result> Handle(Commands.CreateContractProductCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateContractProduct(request.AccessToken, Guid.Empty, request.EstateId, request.ContractId, request.CreateContractProductModel, cancellationToken);
    }

    public async Task<Result> Handle(Commands.CreateContractProductTransactionFeeCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateContractProductTransactionFee(request.AccessToken, Guid.Empty, request.EstateId, request.ContractId, request.ProductId, request.CreateContractProductTransactionFeeModel, cancellationToken);
    }
}