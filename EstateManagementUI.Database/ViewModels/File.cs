using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public class File
{
    public Guid FileId { get; set; }
    public String FilePath { get; set; }
    public Guid FileProfileId { get; set; } // TODO: will need to map to a string this somehow
        
    public String FileProfileName { get; set; }
    public DateTime UploadDateTime { get; set; }
    public Guid MerchantId { get; set; }
    public String MerchantName { get; set; }
    public String OriginalFileName { get; set; }
    public Guid UserId { get; set; }
    public String UserName { get; set; }
}