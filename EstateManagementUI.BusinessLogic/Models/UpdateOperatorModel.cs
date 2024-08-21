using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class UpdateOperatorModel
{
    #region Properties

    public Guid OperatorId { get; set; }
    public String OperatorName { get; set; }

    public Boolean RequireCustomMerchantNumber { get; set; }

    public Boolean RequireCustomTerminalNumber { get; set; }

    #endregion
}