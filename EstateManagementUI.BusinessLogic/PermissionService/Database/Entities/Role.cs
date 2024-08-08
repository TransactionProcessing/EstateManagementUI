using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class Role
{
    [PrimaryKey, AutoIncrement]
    public Int32 RoleId { get; set; }
    [Indexed(Name = "IX_Name", Unique = true)]
    public string Name { get; set; }
}