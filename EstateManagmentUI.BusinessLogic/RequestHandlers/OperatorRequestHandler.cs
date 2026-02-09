using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class OperatorRequestHandler : IRequestHandler<OperatorQueries.GetOperatorsQuery, Result<List<OperatorModels.OperatorModel>>>,
    IRequestHandler<OperatorQueries.GetOperatorQuery, Result<OperatorModels.OperatorModel>>,
    IRequestHandler<OperatorQueries.GetOperatorsForDropDownQuery, Result<List<OperatorModels.OperatorDropDownModel>>>,
    IRequestHandler<OperatorCommands.CreateOperatorCommand, Result>,
    IRequestHandler<OperatorCommands.UpdateOperatorCommand, Result>
{
    private readonly IApiClient ApiClient;

    public OperatorRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<OperatorModels.OperatorModel>>> Handle(OperatorQueries.GetOperatorsQuery request,
                                                                         CancellationToken cancellationToken) {
        return await this.ApiClient.GetOperators(request, cancellationToken);
    }
    public async Task<Result<OperatorModels.OperatorModel>> Handle(OperatorQueries.GetOperatorQuery request,
                                                                   CancellationToken cancellationToken) {
        return await this.ApiClient.GetOperator(request, cancellationToken);
    }
    public async Task<Result> Handle(OperatorCommands.CreateOperatorCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateOperator(request, cancellationToken);
    }
    public async Task<Result> Handle(OperatorCommands.UpdateOperatorCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.UpdateOperator(request, cancellationToken);
    }

    public async Task<Result<List<OperatorModels.OperatorDropDownModel>>> Handle(OperatorQueries.GetOperatorsForDropDownQuery request,
                                                                                 CancellationToken cancellationToken) {
        return await this.ApiClient.GetOperators(request, cancellationToken);
    }
}