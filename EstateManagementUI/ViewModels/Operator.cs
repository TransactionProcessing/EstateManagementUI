using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record Operator
{
    public string Name { get; set; }

    public Guid Id { get; set; }

    public string RequireCustomMerchantNumber { get; set; }

    public string RequireCustomTerminalNumber { get; set; }
}