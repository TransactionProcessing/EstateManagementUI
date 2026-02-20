using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BlazorServer.Models;

[ExcludeFromCodeCoverage]
public record OperatorModels {
    public record EditOperatorModel {
        [Required(ErrorMessage = "Operator name is required")]
        public string? OperatorName { get; set; }

        public bool RequireCustomMerchantNumber { get; set; }

        public bool RequireCustomTerminalNumber { get; set; }
    }

    public record OperatorModel
    {
        public Guid OperatorId { get; set; }
        public string? Name { get; set; }
        public bool RequireCustomMerchantNumber { get; set; }
        public bool RequireCustomTerminalNumber { get; set; }
    }

    public class OperatorDropDownModel
    {
        public Guid OperatorId { get; set; }
        public Int32 OperatorReportingId { get; set; }
        public string? OperatorName { get; set; }
    }

    public class CreateOperatorModel
    {
        [Required(ErrorMessage = "Operator name is required")]
        public string? OperatorName { get; set; }

        public bool RequireCustomMerchantNumber { get; set; }

        public bool RequireCustomTerminalNumber { get; set; }
    }
}