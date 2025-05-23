﻿using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

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

[ExcludeFromCodeCoverage]
public class CreateContractModel
{
    public Guid OperatorId { get; set; }

    public string Description { get; set; }
}

[ExcludeFromCodeCoverage]
public class CreateContractProductModel
{
    public Decimal? Value { get; set; }
    public Boolean IsVariable { get; set; }
    public String DisplayText { get; set; }
    public String Name { get; set; }
    public Int32 Type { get; set; }
}

[ExcludeFromCodeCoverage]
public class CreateContractProductTransactionFeeModel
{
    public CalculationType CalculationType { get; set; }

    public string Description { get; set; }

    public FeeType FeeType { get; set; }

    public Decimal Value { get; set; }

}

public enum SettlementSchedule
{
    Immediate,
    Weekly,
    Monthly
}

[ExcludeFromCodeCoverage]
public class MakeDepositModel {
    public Decimal Amount { get; set; }
    public String Reference { get; set; }
    public DateTime Date { get; set; }
}