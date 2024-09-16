using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TopBottomOperatorDataModel
{
    public string OperatorName { get; set; }

    public Decimal SalesValue { get; set; }
}