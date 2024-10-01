using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record MerchantOperatorModel {
    public String Name { get; set; }

    public Guid OperatorId { get; set; }
    public string MerchantNumber { get; set; }

    public string TerminalNumber { get; set; }
    public bool IsDeleted { get; set; }
}