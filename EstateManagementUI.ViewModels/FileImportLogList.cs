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
}
