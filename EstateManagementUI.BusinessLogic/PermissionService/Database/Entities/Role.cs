using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

[ExcludeFromCodeCoverage]
public class Role
{
    [PrimaryKey, AutoIncrement]
    public Int32 RoleId { get; set; }
    [Indexed(Name = "IX_Name", Unique = true)]
    public string Name { get; set; }
}