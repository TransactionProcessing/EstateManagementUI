using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs;

[ExcludeFromCodeCoverage]
public class OperatorPageEvents
{
    public record ShowNewOperatorDialog;
    public record HideNewOperatorDialog;
    public record ShowEditOperatorDialog(Guid OperatorId);
    public record HideEditOperatorDialog;
    public record OperatorCreatedEvent;
    public record OperatorUpdatedEvent;
}