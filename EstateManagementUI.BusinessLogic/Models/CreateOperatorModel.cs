using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagementUI.BusinessLogic.Models
{
    [ExcludeFromCodeCoverage]
    public class CreateOperatorModel
    {
        #region Properties

        public Guid OperatorId { get; set; }
        public String OperatorName { get; set; }

        public Boolean RequireCustomMerchantNumber { get; set; }

        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }

    [ExcludeFromCodeCoverage]
    public class UpdateOperatorModel
    {
        #region Properties

        public Guid OperatorId { get; set; }
        public String OperatorName { get; set; }

        public Boolean RequireCustomMerchantNumber { get; set; }

        public Boolean RequireCustomTerminalNumber { get; set; }

        #endregion
    }
}
