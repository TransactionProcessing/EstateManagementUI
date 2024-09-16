using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TopBottomProductDataModel
{
    public string ProductName { get; set; }

    public Decimal SalesValue { get; set; }
}