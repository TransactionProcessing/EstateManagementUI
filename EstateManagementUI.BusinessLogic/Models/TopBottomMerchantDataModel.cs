using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TopBottomMerchantDataModel
{
    public string MerchantName { get; set; }

    public Decimal SalesValue { get; set; }
}