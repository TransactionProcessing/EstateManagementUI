using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.Models
{
    public class OperatorModel
    {
        public Guid OperatorId { get; set; }
        public string? Name { get; set; }
        public bool RequireCustomMerchantNumber { get; set; }
        public bool RequireCustomTerminalNumber { get; set; }
    }

    public class OperatorDropDownModel
    {
        public Guid OperatorId { get; set; }
        public string? OperatorName { get; set; }
    }
}
