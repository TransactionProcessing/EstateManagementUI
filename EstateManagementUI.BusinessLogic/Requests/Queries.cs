using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Models;
using SimpleResults;

namespace EstateManagmentUI.BusinessLogic.Requests {

    [ExcludeFromCodeCoverage]
    public record Queries {
        public record GetEstateQuery(String AccessToken, Guid EstateId) : IRequest<EstateModel>;
        public record GetMerchantsQuery(String AccessToken, Guid EstateId) : IRequest<List<MerchantModel>>;
        public record GetOperatorsQuery(String AccessToken, Guid EstateId) : IRequest<Result<List<OperatorModel>>>;
        public record GetContractsQuery(String AccessToken, Guid EstateId) : IRequest<List<ContractModel>>;
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
    }
}
