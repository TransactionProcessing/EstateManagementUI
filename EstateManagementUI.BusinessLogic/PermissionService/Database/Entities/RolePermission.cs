using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class RolePermission
{
    [PrimaryKey, AutoIncrement]
    public Int32 RolePermissionId { get; set; }

    [Indexed(Name= "IX_RoleId_PermissionId", Unique = true)]
    public Int32 RoleId { get; set; }
    [Indexed(Name = "IX_RoleId_PermissionId", Unique = true)]
    public Int32 PermissionId { get; set; }
}