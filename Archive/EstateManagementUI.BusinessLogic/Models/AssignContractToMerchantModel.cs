using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class AssignContractToMerchantModel
{
    #region Properties

    public Guid ContractId { get; set; }

    #endregion
}

[ExcludeFromCodeCoverage]
public class AssignDeviceToMerchantModel
{
    #region Properties

    public String DeviceIdentifier { get; set; }

    #endregion
}