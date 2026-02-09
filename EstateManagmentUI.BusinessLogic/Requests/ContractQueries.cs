using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class ContractQueries {
    public record GetRecentContractsQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<ContractModels.RecentContractModel>>>;
    public record GetContractsForDropDownQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<ContractModels.ContractDropDownModel>>>;
    public record GetContractsQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<ContractModels.ContractModel>>>;
    public record GetContractQuery(CorrelationId CorrelationId, Guid EstateId, Guid ContractId) : IRequest<Result<ContractModels.ContractModel>>;
}