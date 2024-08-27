using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class FileImportLogList
    {
        public Guid FileImportLogId { get; set; }

        public DateTime ImportLogDateTime { get; set; }

        public DateTime ImportLogDate { get; set; }

        public TimeSpan ImportLogTime { get; set; }

        public Int32 FileCount { get; set; }
    }

    //public class FileImportLog
    //{
    //    public Guid FileImportLogId { get; set; }

    //    public DateTime ImportLogDateTime { get; set; }

    //    public DateTime ImportLogDate { get; set; }

    //    public TimeSpan ImportLogTime { get; set; }

    //    public List<File> Files { get; set; }
    //}

    //public class File {
    //    public Guid FileId { get; set; }
    //    public String FilePath { get; set; }
    //    public Guid FileProfileId { get; set; } // TODO: will need to map to a string this somehow
    //    public DateTime UploadDateTime { get; set; }
    //    public Guid MerchantId { get; set; }
    //    public String  MerchantName { get; set; }
    //    public String OriginalFileName { get; set; }
    //    public Guid UserId { get; set; }
    //    public String UserName { get; set; }
    //}
}
