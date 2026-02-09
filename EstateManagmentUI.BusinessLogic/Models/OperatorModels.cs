namespace EstateManagementUI.BusinessLogic.Models
{
    public class OperatorModels {
        public class OperatorModel {
            public Guid OperatorId { get; set; }
            public string? Name { get; set; }
            public bool RequireCustomMerchantNumber { get; set; }
            public bool RequireCustomTerminalNumber { get; set; }
        }

        public class OperatorDropDownModel {
            public Guid OperatorId { get; set; }
            public string? OperatorName { get; set; }
        }
    }
}
