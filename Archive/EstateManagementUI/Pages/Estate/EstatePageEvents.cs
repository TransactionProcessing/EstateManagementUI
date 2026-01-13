using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Estate;

[ExcludeFromCodeCoverage]
public class EstatePageEvents
{
    public record ShowAddOperatorDialog;
    public record HideAddOperatorDialog;
    public record OperatorAssignedToEstateEvent;
    public record OperatorRemovedFromEstateEvent;
}
