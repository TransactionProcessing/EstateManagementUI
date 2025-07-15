using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using FileProcessor.DataTransferObjects.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;
using File = EstateManagementUI.ViewModels.File;


namespace EstateManagementUI.Pages.FileProcessing.FileImportLog
{
    public class FileImportLog : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public Guid FileImportLogId { get; set; }
        
        public DateTime ImportLogDate { get; set; }
        public Guid MerchantId{ get; set; }

        public List<ViewModels.File> Files { get; set; }

        public FileImportLog(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.FileProcessing, FileProcessingFunctions.ViewImportLog, permissionsService)
        {
            this.Mediator = mediator;
            this.Files = new List<File>();
        }
       

        public override async Task MountAsync() {
            await this.GetFileImportLog();
        }

        private async Task GetFileImportLog() {
            await this.PopulateTokenAndEstateId();
            CorrelationId c = CorrelationIdHelper.New();
            Queries.GetEstateQuery estateQuery = new(c, AccessToken, EstateId);
            Result<EstateModel> estate = await this.Mediator.Send(estateQuery);
            if (estate.IsFailed)
            {
                // TODO: Handle error properly, e.g., show a message to the user
            }
            Queries.GetFileImportLogQuery query = new Queries.GetFileImportLogQuery(this.AccessToken, this.EstateId,
                this.MerchantId, this.FileImportLogId);

            Result<BusinessLogic.Models.FileImportLogModel> response =
                await this.Mediator.Send(query, CancellationToken.None);

            this.ImportLogDate = response.Data.ImportLogDate;
            this.FileImportLogId = response.Data.FileImportLogId;
            List<File> resultList = new List<File>();
            foreach (FileImportLogFileModel fileImportLogFileModel in response.Data.Files) {
                SecurityUserModel user = estate.Data.SecurityUsers.SingleOrDefault(u => u.SecurityUserId == fileImportLogFileModel.UserId);
                String fileProfile = fileImportLogFileModel.FileProfileId.ToString().ToUpper() switch {
                    "B2A59ABF-293D-4A6B-B81B-7007503C3476" => "Safaricom Topup",
                    "8806EDBC-3ED6-406B-9E5F-A9078356BE99" => "Voucher Issue",
                    _ => "Unknown File Type"
                };
                resultList.Add(new File {
                    MerchantName = fileImportLogFileModel.MerchantId.ToString(),
                    MerchantId = fileImportLogFileModel.MerchantId,
                    UserId = fileImportLogFileModel.UserId,
                    UserName = user == null? "N/A" :user.EmailAddress,
                    UploadDateTime = fileImportLogFileModel.FileUploadedDateTime,
                    OriginalFileName = fileImportLogFileModel.OriginalFileName,
                    FilePath = fileImportLogFileModel.FilePath,
                    FileId = fileImportLogFileModel.FileId,
                    FileProfileId = fileImportLogFileModel.FileProfileId,
                    FileProfileName = fileProfile
                });
            }

            IEnumerable<ViewModels.File> sortQuery = this.Sorting switch {
                (FileImportLogSorting.OriginalFileName, Ascending: false) => resultList.OrderBy(p => p.OriginalFileName),
                (FileImportLogSorting.OriginalFileName, Ascending: true) => resultList.OrderByDescending(p =>
                    p.OriginalFileName),
                (FileImportLogSorting.DateTimeUploaded, Ascending: false) => resultList.OrderBy(p => p.UploadDateTime),
                (FileImportLogSorting.DateTimeUploaded, Ascending: true) => resultList.OrderByDescending(p =>
                    p.UploadDateTime),
                (FileImportLogSorting.FileName, Ascending: false) => resultList.OrderBy(p => p.FilePath),
                (FileImportLogSorting.FileName, Ascending: true) => resultList.OrderByDescending(p =>
                    p.FilePath),
                (FileImportLogSorting.FileProfile, Ascending: false) => resultList.OrderBy(p => p.FileProfileName),
                (FileImportLogSorting.FileProfile, Ascending: true) => resultList.OrderByDescending(p =>
                    p.FileProfileName),
                (FileImportLogSorting.UserName, Ascending: false) => resultList.OrderBy(p => p.UserName),
                (FileImportLogSorting.UserName, Ascending: true) => resultList.OrderByDescending(p =>
                    p.UserName),
                _ => resultList.AsEnumerable()
            };

            this.Files= sortQuery.ToList();
        }
        
        public async Task Sort(FileImportLogSorting value)
        {
            this.Sorting = (Column: value, Ascending: this.Sorting.Column == value && !this.Sorting.Ascending);

            await this.GetFileImportLog();
        }

        public (FileImportLogSorting Column, bool Ascending) Sorting { get; set; }

        public async Task ViewFileDetails(Guid fileId)
        {
            this.Location("/FileProcessing/FileDetails", new { FileId = fileId});
        }
    }

    public enum FileImportLogSorting
    {
        FileName,
        FileProfile,
        UserName,
        OriginalFileName,
        DateTimeUploaded
    }
}
