using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class AssignOperatorToMerchantModel
{
    #region Properties

    public String MerchantNumber { get; set; }

    public Guid OperatorId { get; set; }

    public String TerminalNumber { get; set; }

    #endregion
}