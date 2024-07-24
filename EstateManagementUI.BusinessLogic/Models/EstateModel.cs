using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record EstateModel {
    #region Properties

    public Guid EstateId { get; set; }

    public String EstateName { get; set; }

    public List<EstateOperatorModel> Operators { get; set; }

    public List<SecurityUserModel> SecurityUsers { get; set; }

    #endregion
}