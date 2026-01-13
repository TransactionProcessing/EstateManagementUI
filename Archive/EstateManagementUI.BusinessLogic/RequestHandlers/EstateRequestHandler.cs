using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers
{
    public class EstateRequestHandler : IRequestHandler<Queries.GetEstateQuery, Result<EstateModel>>,
                                        IRequestHandler<Commands.AssignOperatorToEstateCommand, Result>,
                                        IRequestHandler<Commands.RemoveOperatorFromEstateCommand, Result>
    {
        private readonly IApiClient ApiClient;

        public EstateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }

        public async Task<Result<EstateModel>> Handle(Queries.GetEstateQuery request,
                                        CancellationToken cancellationToken) {
            Result<EstateModel> model = await this.ApiClient.GetEstate(request.AccessToken, request.CorrelationId.Value, request.EstateId, cancellationToken);
            return model;
        }

        public async Task<Result> Handle(Commands.AssignOperatorToEstateCommand request,
                                        CancellationToken cancellationToken) {
            Result result = await this.ApiClient.AssignOperatorToEstate(request.AccessToken, request.CorrelationId.Value, request.EstateId, request.AssignOperatorRequestModel, cancellationToken);
            return result;
        }

        public async Task<Result> Handle(Commands.RemoveOperatorFromEstateCommand request,
                                        CancellationToken cancellationToken) {
            Result result = await this.ApiClient.RemoveOperatorFromEstate(request.AccessToken, request.CorrelationId.Value, request.EstateId, request.OperatorId, cancellationToken);
            return result;
        }
    }
}
