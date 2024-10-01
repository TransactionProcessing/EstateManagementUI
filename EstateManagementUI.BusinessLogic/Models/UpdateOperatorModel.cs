using EstateManagement.DataTransferObjects.Responses.Merchant;
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

[ExcludeFromCodeCoverage]
public class CreateMerchantModel
{
    #region Properties
    public AddressModel Address { get; set; }

    public ContactModel Contact { get; set; }

    public String MerchantName { get; set; }

    public SettlementSchedule SettlementSchedule { get; set; }


    #endregion
}

[ExcludeFromCodeCoverage]
public class UpdateMerchantModel
{
    #region Properties

    public String MerchantName { get; set; }

    public SettlementSchedule SettlementSchedule { get; set; }


    #endregion
}

[ExcludeFromCodeCoverage]
public class ContactModel
{
    #region Properties

    public String ContactEmailAddress { get; set; }

    public Guid ContactId { get; set; }
    public String ContactName { get; set; }

    public String ContactPhoneNumber { get; set; }

    #endregion
}

[ExcludeFromCodeCoverage]
public class AddressModel
{
    #region Properties

    public Guid AddressId { get; set; }

    public String AddressLine1 { get; set; }

    public String AddressLine2 { get; set; }

    public String AddressLine3 { get; set; }

    public String AddressLine4 { get; set; }

    public String Country { get; set; }
    public String PostalCode { get; set; }
public String Region { get; set; }
    public String Town { get; set; }

    #endregion
}

public enum SettlementSchedule
{
    Immediate,
    Weekly,
    Monthly
}