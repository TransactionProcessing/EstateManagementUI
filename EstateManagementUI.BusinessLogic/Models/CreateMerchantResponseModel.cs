using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class CreateMerchantResponseModel
{
    #region Properties

    public Guid AddressId { get; set; }

    public Guid ContactId { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    #endregion
}