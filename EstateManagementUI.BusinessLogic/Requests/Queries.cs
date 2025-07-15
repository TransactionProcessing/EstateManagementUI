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
        public record GetMerchantsQuery(String AccessToken, Guid EstateId) : IRequest<Result<List<MerchantModel>>>;
        public record GetOperatorsQuery(String AccessToken, Guid EstateId) : IRequest<Result<List<OperatorModel>>>;
        public record GetContractsQuery(String AccessToken, Guid EstateId) : IRequest<Result<List<ContractModel>>>;
        public record GetOperatorQuery(String AccessToken, Guid EstateId, Guid OperatorId) : IRequest<Result<OperatorModel>>;
        public record GetContractQuery(String AccessToken, Guid EstateId, Guid ContractId) : IRequest<Result<ContractModel>>;
        public record GetFileImportLogsListQuery(String AccessToken, Guid EstateId, Guid MerchantId, DateTime StartDate, DateTime EndDate)
            : IRequest<Result<List<FileImportLogModel>>>;

        public record GetFileImportLogQuery(String AccessToken, Guid EstateId, Guid MerchantId, Guid FileImportLogId)
            : IRequest<Result<FileImportLogModel>>;

        public record GetFileDetailsQuery(String AccessToken, Guid EstateId, Guid FileId) : IRequest<Result<FileDetailsModel>>;

        public record GetComparisonDatesQuery(String AccessToken, Guid EstateId) : IRequest<Result<List<ComparisonDateModel>>>;

        public record GetTodaysSalesQuery(String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
        public record GetTodaysSettlementQuery(String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSettlementModel>>;

        public record GetTodaysSalesCountByHourQuery(String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesCountByHourModel>>>;
        public record GetTodaysSalesValueByHourQuery(String AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesValueByHourModel>>>;

        public record GetMerchantKpiQuery(String AccessToken, Guid EstateId) : IRequest<Result<MerchantKpiModel>>;

        public record GetTodaysFailedSalesQuery(String AccessToken, Guid EstateId, String ResponseCode, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;

        public record GetTopProductDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
        public record GetBottomProductDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;

        public record GetTopMerchantDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
        public record GetBottomMerchantDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;

        public record GetTopOperatorDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
        public record GetBottomOperatorDataQuery(String AccessToken, Guid EstateId, Int32 ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;

        public record GetLastSettlementQuery(String AccessToken, Guid EstateId) : IRequest<Result<LastSettlementModel>>;

        public record GetMerchantQuery(String AccessToken, Guid EstateId, Guid MerchantId) : IRequest<Result<MerchantModel>>;
    }
}
