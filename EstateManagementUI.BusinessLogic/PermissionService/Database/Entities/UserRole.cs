using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class UserRole {
    [PrimaryKey, AutoIncrement]
    public Int32 UserRoleId { get; set; }
    public Int32 RoleId { get; set; }
    public String UserName { get; set; }
}