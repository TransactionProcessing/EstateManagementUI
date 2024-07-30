using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.RequestHandlers;
using EstateManagmentUI.BusinessLogic.Requests;
using Lamar;
using MediatR;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class MediatorRegistry : ServiceRegistry {
    public MediatorRegistry() {
        this.AddTransient<IMediator, Mediator>();

        this.AddSingleton<IRequestHandler<Queries.GetEstateQuery, EstateModel>, EstateRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetMerchantsQuery, List<MerchantModel>>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetOperatorsQuery, List<OperatorModel>>, OperatorRequestHandler>();
    }
}