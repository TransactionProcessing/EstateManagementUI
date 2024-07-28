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

    public class MerchantRequestHandler : IRequestHandler<Queries.GetMerchantsQuery, List<MerchantModel>>
    {
        private readonly IApiClient ApiClient;

        public MerchantRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<List<MerchantModel>> Handle(Queries.GetMerchantsQuery request,
                                                      CancellationToken cancellationToken)
        {
            List<MerchantModel> models = await this.ApiClient.GetMerchants(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
            return models;
        }
    }
}
