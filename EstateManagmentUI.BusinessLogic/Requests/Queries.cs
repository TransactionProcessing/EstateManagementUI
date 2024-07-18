using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagmentUI.BusinessLogic.Requests {
    public record Queries {
        public record GetEstateQuery(String AccessToken, Guid EstateId) : IRequest<EstateModel>;
    }

    public record EstateModel {
        #region Properties

        public Guid EstateId { get; set; }

        public String EstateName { get; set; }

        public List<EstateOperatorModel> Operators { get; set; }

        public List<SecurityUserModel> SecurityUsers { get; set; }

        #endregion
    }

    public record EstateOperatorModel{
        #region Properties

        public String Name { get; set; }

        public Guid OperatorId { get; set; }

        public Boolean RequireCustomMerchantNumber { get; set; }

        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }

    [ExcludeFromCodeCoverage]
    public record SecurityUserModel
    {
        #region Properties
        
        public String EmailAddress { get; set; }

        public Guid SecurityUserId { get; set; }

        #endregion
    }
}
