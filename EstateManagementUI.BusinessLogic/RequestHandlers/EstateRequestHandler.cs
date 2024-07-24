using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagmentUI.BusinessLogic.RequestHandlers
{
    public class EstateRequestHandler : IRequestHandler<Queries.GetEstateQuery, EstateModel>
    {
        private readonly IApiClient ApiClient;

        public EstateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }

        public async Task<EstateModel> Handle(Queries.GetEstateQuery request,
                                        CancellationToken cancellationToken) {
            EstateModel model = await this.ApiClient.GetEstate(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
            return model;
        }
    }
}
