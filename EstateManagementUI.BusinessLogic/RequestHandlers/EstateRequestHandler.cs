using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers
{
    public class EstateRequestHandler : IRequestHandler<Queries.GetEstateQuery, Result<EstateModel>>
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
    }
}
