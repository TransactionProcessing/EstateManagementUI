using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

[ExcludeFromCodeCoverage]
public class UserRole {
    [PrimaryKey, AutoIncrement]
    public Int32 UserRoleId { get; set; }
    public Int32 RoleId { get; set; }
    public String UserName { get; set; }
}