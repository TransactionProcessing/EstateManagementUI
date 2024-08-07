using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;

public class ApplicationSection {
    [PrimaryKey, AutoIncrement]
    public Int32 ApplicationSectionId { get; set; }
    [Indexed(Name = "IX_ApplicationSection_Name", Unique = true)]
    public String Name { get; set; }
}