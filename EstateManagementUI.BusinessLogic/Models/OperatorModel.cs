using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record OperatorModel
{
    public String Name { get; set; }

    public Guid OperatorId { get; set; }

    public Boolean RequireCustomMerchantNumber { get; set; }

    public Boolean RequireCustomTerminalNumber { get; set; }
}