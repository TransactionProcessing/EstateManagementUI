using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers
{
    public class DateRequestHandler : IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>
    {
        private readonly IApiClient ApiClient;
        
        public DateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }
        public async Task<Result<List<ComparisonDateModel>>> Handle(Queries.GetComparisonDatesQuery request,
                                                                    CancellationToken cancellationToken) {
            return await this.ApiClient.GetComparisonDates(request, cancellationToken);
        }
    }
}
