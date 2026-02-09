namespace EstateManagementUI.BusinessLogic.Requests;

public record CorrelationId(Guid Value);

public static class CorrelationIdHelper
{
    public static CorrelationId New() => new(Guid.NewGuid());
}