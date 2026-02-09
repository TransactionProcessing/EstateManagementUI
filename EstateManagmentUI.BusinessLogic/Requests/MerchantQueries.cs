using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class MerchantQueries {
    public record GetMerchantsQuery(CorrelationId CorrelationId, Guid EstateId, String? Name, String? Reference, Int32? SettlementSchedule, String? Region, String? PostCode) : IRequest<Result<List<MerchantModels.MerchantListModel>>>;
    public record GetMerchantsForDropDownQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<MerchantModels.MerchantDropDownModel>>>;
    public record GetRecentMerchantsQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<MerchantModels.RecentMerchantsModel>>>;
    public record GetMerchantKpiQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<MerchantModels.MerchantKpiModel>>;
    public record GetMerchantQuery(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId) : IRequest<Result<MerchantModels.MerchantModel>>;
    public record GetMerchantOperatorsQuery(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId) : IRequest<Result<List<MerchantModels.MerchantOperatorModel>>>;
    public record GetMerchantContractsQuery(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId) : IRequest<Result<List<MerchantModels.MerchantContractModel>>>;
    public record GetMerchantDevicesQuery(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId) : IRequest<Result<List<MerchantModels.MerchantDeviceModel>>>;
}