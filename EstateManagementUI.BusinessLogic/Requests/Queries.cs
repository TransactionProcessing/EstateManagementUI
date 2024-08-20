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
    }

    public record Commands {
        public record AddNewOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
    }
}
