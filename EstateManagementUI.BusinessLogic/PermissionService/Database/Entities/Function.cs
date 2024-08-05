using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class Function
{
    [PrimaryKey, AutoIncrement]
    public Int32 FunctionId { get; set; }
    [Indexed(Name = "IX_Name", Unique = true)]
    public string Name { get; set; }
}