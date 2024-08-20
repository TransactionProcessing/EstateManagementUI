using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class OperatorRequestHandler : IRequestHandler<Queries.GetOperatorsQuery, Result<List<OperatorModel>>>,
                                      IRequestHandler<Commands.AddNewOperatorCommand, Result> {
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
}