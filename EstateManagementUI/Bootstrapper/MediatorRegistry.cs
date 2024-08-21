using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagmentUI.BusinessLogic.Requests;
using Lamar;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class MediatorRegistry : ServiceRegistry {
    public MediatorRegistry() {
        this.AddTransient<IMediator, Mediator>();

        // Queries
        this.AddSingleton<IRequestHandler<Queries.GetEstateQuery, EstateModel>, EstateRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetMerchantsQuery, List<MerchantModel>>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetOperatorsQuery, Result<List<OperatorModel>>>, OperatorRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetContractsQuery, List<ContractModel>>, ContractRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetContractQuery, Result<ContractModel>>, ContractRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetOperatorQuery, Result<OperatorModel>>, OperatorRequestHandler>();
        
        // Commands
        this.AddSingleton<IRequestHandler<Commands.AddNewOperatorCommand, Result>, OperatorRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.UpdateOperatorCommand, Result>, OperatorRequestHandler>();
    }
}