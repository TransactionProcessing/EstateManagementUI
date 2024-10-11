using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Logger;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class MerchantRequestHandler : IRequestHandler<Queries.GetMerchantsQuery, Result<List<MerchantModel>>>,
                                      IRequestHandler<Queries.GetMerchantQuery, Result<MerchantModel>>,
                                      IRequestHandler<Commands.AddMerchantCommand, Result>,
                                      IRequestHandler<Commands.UpdateMerchantCommand, Result>,
                                      IRequestHandler<Commands.UpdateMerchantAddressCommand, Result>,
                                      IRequestHandler<Commands.UpdateMerchantContactCommand, Result>,
                                      IRequestHandler<Commands.AssignOperatorToMerchantCommand, Result>,
                                      IRequestHandler<Commands.RemoveOperatorFromMerchantCommand, Result>,
IRequestHandler<Commands.AssignContractToMerchantCommand, Result>,
IRequestHandler<Commands.RemoveContractFromMerchantCommand, Result>
{
    private readonly IApiClient ApiClient;

    public MerchantRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<MerchantModel>>> Handle(Queries.GetMerchantsQuery request,
                                                  CancellationToken cancellationToken) {
        try {
            Result<List<MerchantModel>> result = await this.ApiClient.GetMerchants(request.AccessToken, Guid.Empty,
                request.EstateId, cancellationToken);
            return result;
        }
        catch (Exception ex) {
            Logger.LogError(ex);
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> Handle(Commands.AddMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.CreateMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.CreateMerchantModel, cancellationToken);
        return result;
    }

    public async Task<Result<MerchantModel>> Handle(Queries.GetMerchantQuery request,
                                                    CancellationToken cancellationToken) {
        Result<MerchantModel> result = await this.ApiClient.GetMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.UpdateMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.UpdateMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.UpdateMerchantModel, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.UpdateMerchantAddressCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.UpdateMerchantAddress(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.UpdatedAddressModel, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.UpdateMerchantContactCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.UpdateMerchantContact(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.UpdatedContactModel, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.AssignOperatorToMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.AssignOperatorToMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.AssignOperatorRequestModel, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.RemoveOperatorFromMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.RemoveOperatorFromMerchant(request.AccessToken, Guid.Empty,
            request.EstateId, request.MerchantId, request.OperatorId, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.AssignContractToMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.AssignContractToMerchant(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.AssignContractToMerchantModel, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.RemoveContractFromMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.RemoveContractFromMerchant(request.AccessToken, Guid.Empty,
            request.EstateId, request.MerchantId, request.ContractId, cancellationToken);
        return result; ;
    }
}