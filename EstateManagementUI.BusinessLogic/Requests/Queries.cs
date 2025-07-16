using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using SimpleResults;
using SQLitePCL;
using Shared.Logger.TennantContext;

namespace EstateManagmentUI.BusinessLogic.Requests {
    public record CorrelationId(Guid Value);

    public static class CorrelationIdHelper {
        public static CorrelationId New(HttpContext context = null) {
            CorrelationId c = new(Guid.NewGuid());
            if (TenantContext.CurrentTenant != null) {
                TenantContext.CurrentTenant.SetCorrelationId(c.Value);
                if (context != null)
                    context.Items["correlationId"] = c.Value;
            }
            return new CorrelationId(Guid.NewGuid());
        }
    }

    [ExcludeFromCodeCoverage]
    public record Queries {
        public record GetEstateQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<EstateModel>>;
        public record GetMerchantsQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<List<MerchantModel>>>;
        public record GetOperatorsQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<List<OperatorModel>>>;
        public record GetContractsQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<List<ContractModel>>>;
        public record GetOperatorQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid OperatorId) : IRequest<Result<OperatorModel>>;
        public record GetContractQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid ContractId) : IRequest<Result<ContractModel>>;
        public record GetFileImportLogsListQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid MerchantId, DateTime StartDate, DateTime EndDate)
            : IRequest<Result<List<FileImportLogModel>>>;

        public record GetFileImportLogQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid MerchantId, Guid FileImportLogId)
            : IRequest<Result<FileImportLogModel>>;

        public record GetFileDetailsQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid FileId) : IRequest<Result<FileDetailsModel>>;

        public record GetComparisonDatesQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<List<ComparisonDateModel>>>;

        public record GetTodaysSalesQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
        public record GetTodaysSettlementQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSettlementModel>>;

        public record GetTodaysSalesCountByHourQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesCountByHourModel>>>;
        public record GetTodaysSalesValueByHourQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesValueByHourModel>>>;

        public record GetMerchantKpiQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<MerchantKpiModel>>;

        public record GetTodaysFailedSalesQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, String ResponseCode, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;

        public record GetTopProductDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
        public record GetBottomProductDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;

        public record GetTopMerchantDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
        public record GetBottomMerchantDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;

        public record GetTopOperatorDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
        public record GetBottomOperatorDataQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;

        public record GetLastSettlementQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId) : IRequest<Result<LastSettlementModel>>;

        public record GetMerchantQuery(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid MerchantId) : IRequest<Result<MerchantModel>>;
    }
}
