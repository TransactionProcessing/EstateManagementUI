using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class Function
{
    [PrimaryKey, AutoIncrement]
    public Int32 FunctionId { get; set; }
    public Int32 ApplicationSectionId { get; set; }
    public string Name { get; set; }
}