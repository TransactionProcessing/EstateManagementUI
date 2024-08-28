using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
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

        public FileImportLog(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.FileProcessing, ContractFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Files = new List<File>();
        }
       

        public override async Task MountAsync() {
            await this.GetFileImportLog();
            //this.StartDate = new DateModel { SelectedDate = DateTime.Now };
            //this.EndDate = new DateModel { SelectedDate = DateTime.Now };
        }

        //public async Task Query() {
        //    if (String.IsNullOrEmpty(this.Merchant.MerchantId) == false)
        //    {
        //        await this.GetFiles();
        //    }
        //}

        private async Task GetFileImportLog() {
            await this.PopulateTokenAndEstateId();

            var estateQuery = new Queries.GetEstateQuery(this.AccessToken, this.EstateId);
            var estate = await this.Mediator.Send(estateQuery);

            Queries.GetFileImportLog query = new Queries.GetFileImportLog(this.AccessToken, this.EstateId,
                this.MerchantId, this.FileImportLogId);

            Result<BusinessLogic.Models.FileImportLogModel> response =
                await this.Mediator.Send(query, CancellationToken.None);

            this.ImportLogDate = response.Data.ImportLogDate;
            this.FileImportLogId = response.Data.FileImportLogId;
            List<File> resultList = new List<File>();
            foreach (FileImportLogFileModel fileImportLogFileModel in response.Data.Files) {
                var user = estate.SecurityUsers.SingleOrDefault(u => u.SecurityUserId == fileImportLogFileModel.UserId);
                var fileProfile = fileImportLogFileModel.FileProfileId.ToString().ToUpper() switch {
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

        //public async Task Edit(Guid contractId)
        //{

        //}

        public async Task ViewFiles(Guid fileImportLogId)
        {
        //    this.Location(this.Url.Page("/Contract/ContractProducts", new { ContractId = contractId }));
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
