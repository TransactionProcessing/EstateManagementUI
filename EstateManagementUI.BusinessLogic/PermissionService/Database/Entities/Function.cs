using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

[ExcludeFromCodeCoverage]
public class Function
{
    [PrimaryKey, AutoIncrement]
    public Int32 FunctionId { get; set; }
    public Int32 ApplicationSectionId { get; set; }
    public string Name { get; set; }
}