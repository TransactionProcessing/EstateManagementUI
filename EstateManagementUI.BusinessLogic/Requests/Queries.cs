using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagmentUI.BusinessLogic.Requests {

    [ExcludeFromCodeCoverage]
    public record Queries {
        public record GetEstateQuery(String AccessToken, Guid EstateId) : IRequest<EstateModel>;
        public record GetMerchantsQuery(String AccessToken, Guid EstateId) : IRequest<List<MerchantModel>>;
    }
}
