using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record SecurityUserModel
{
    #region Properties
        
    public String EmailAddress { get; set; }

    public Guid SecurityUserId { get; set; }

    #endregion
}