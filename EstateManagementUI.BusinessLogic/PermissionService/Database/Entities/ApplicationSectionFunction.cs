using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class ApplicationSectionFunction
{
    [PrimaryKey, AutoIncrement]
    public Int32 ApplicationSectionFunctionId { get; set; }

    [Indexed(Name = "IX_ApplicationSectionId_FunctionId", Unique = true)]
    public Int32 ApplicationSectionId { get; set; }

    [Indexed(Name = "IX_ApplicationSectionId_FunctionId", Unique = true)]
    public Int32 FunctionId { get; set; }
}