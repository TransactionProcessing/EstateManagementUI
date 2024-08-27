using FileProcessor.DataTransferObjects.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagementUI.BusinessLogic.Models
{
    [ExcludeFromCodeCoverage]
    public class FileImportLogModel
    {
        public Guid FileImportLogId { get; set; }

        public DateTime ImportLogDateTime { get; set; }

        public DateTime ImportLogDate { get; set; }

        public TimeSpan ImportLogTime { get; set; }

        public int FileCount { get; set; }
        public List<FileImportLogFileModel> Files { get; set; }
    }
}
