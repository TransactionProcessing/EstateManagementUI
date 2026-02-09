using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class EstateRequestHandler : IRequestHandler<EstateQueries.GetEstateQuery, Result<EstateModels.EstateModel>>,
    IRequestHandler<EstateCommands.AddOperatorToEstateCommand, Result>,
    IRequestHandler<EstateCommands.RemoveOperatorFromEstateCommand, Result>,
    IRequestHandler<EstateQueries.GetAssignedOperatorsQuery, Result<List<OperatorModels.OperatorModel>>> {
    private readonly IApiClient ApiClient;

    public EstateRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<EstateModels.EstateModel>> Handle(EstateQueries.GetEstateQuery request,
                                                               CancellationToken cancellationToken) {
        return await this.ApiClient.GetEstate(request, cancellationToken);
    }

    public async Task<Result> Handle(EstateCommands.AddOperatorToEstateCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddEstateOperator(request, cancellationToken);
    }

    public async Task<Result> Handle(EstateCommands.RemoveOperatorFromEstateCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.RemoveEstateOperator(request, cancellationToken);
    }

    public async Task<Result<List<OperatorModels.OperatorModel>>> Handle(EstateQueries.GetAssignedOperatorsQuery request,
                                                                         CancellationToken cancellationToken) {
        return await this.ApiClient.GetEstateAssignedOperators(request, cancellationToken);
    }
}