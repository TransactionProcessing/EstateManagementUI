using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class OperatorRequestHandler : IRequestHandler<Queries.GetOperatorsQuery, Result<List<OperatorModel>>>,
                                      IRequestHandler<Queries.GetOperatorQuery, Result<OperatorModel>>,
                                      IRequestHandler<Commands.AddNewOperatorCommand, Result>,
                                      IRequestHandler<Commands.UpdateOperatorCommand, Result>
{
    private readonly IApiClient ApiClient;

    public OperatorRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<OperatorModel>>> Handle(Queries.GetOperatorsQuery request,
                                                          CancellationToken cancellationToken) {
        Result<List<OperatorModel>> result =
            await this.ApiClient.GetOperators(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return result;
    }


    public async Task<Result> Handle(Commands.AddNewOperatorCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.CreateOperator(request.AccessToken, Guid.Empty, request.EstateId,
            new CreateOperatorModel {
                RequireCustomMerchantNumber = request.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = request.RequireCustomTerminalNumber,
                OperatorName = request.OperatorName,
                OperatorId = request.OperatorId
            }, cancellationToken);
        return result;
    }

    public async Task<Result> Handle(Commands.UpdateOperatorCommand request,
                                     CancellationToken cancellationToken) {
        Result result = await this.ApiClient.UpdateOperator(request.AccessToken, Guid.Empty, request.EstateId,
            new UpdateOperatorModel
            {
                RequireCustomMerchantNumber = request.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = request.RequireCustomTerminalNumber,
                OperatorName = request.OperatorName,
                OperatorId = request.OperatorId
            }, cancellationToken);
        return result;
    }

    public async Task<Result<OperatorModel>> Handle(Queries.GetOperatorQuery request,
                                                    CancellationToken cancellationToken) {
        Result<OperatorModel> result =
            await this.ApiClient.GetOperator(request.AccessToken, Guid.Empty, request.EstateId, request.OperatorId, cancellationToken);
        return result;
    }
}