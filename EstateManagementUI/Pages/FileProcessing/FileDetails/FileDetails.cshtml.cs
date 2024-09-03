using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.FileProcessing.FileImportLog;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;
using FileLineProcessingResult = EstateManagementUI.BusinessLogic.Models.FileLineProcessingResult;


namespace EstateManagementUI.Pages.FileProcessing.FileDetails
{
    public class FileDetails : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public Guid FileId { get; set; }
        public String FileName { get; set; }
        public String MerchantName { get; set; }
        public String FileProfile { get; set; }
        public String UploadedBy { get; set; }
        public List<FileLine> FileLines { get; set; }

        public FileDetails(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.FileProcessing, ContractFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.FileLines = new();
        }
       
        public override async Task MountAsync() {
            await this.GetFileDetails();
        }

        private async Task GetFileDetails() {
            await this.PopulateTokenAndEstateId();

            Queries.GetFileDetails fileDetailsQuery = new Queries.GetFileDetails(this.AccessToken, this.EstateId, this.FileId);

            Result<BusinessLogic.Models.FileDetailsModel> response = await this.Mediator.Send(fileDetailsQuery, CancellationToken.None);

            this.FileName = response.Data.FileLocation;
            this.MerchantName = response.Data.MerchantName;
            this.FileProfile = response.Data.FileProfileName;
            this.UploadedBy = response.Data.UserEmailAddress;

            List<FileLine> resultList = new List<FileLine>();
            foreach (FileLineModel fileLineModel in response.Data.FileLines) {
                resultList.Add(new FileLine {
                    Data = fileLineModel.LineData,
                    ProcessingResult = fileLineModel.ProcessingResult switch {
                        FileLineProcessingResult.NotProcessed => ViewModels.FileLineProcessingResult.NotProcessed,
                        FileLineProcessingResult.Ignored => ViewModels.FileLineProcessingResult.Ignored,
                        FileLineProcessingResult.Failed => ViewModels.FileLineProcessingResult.Failed,
                        FileLineProcessingResult.Successful => ViewModels.FileLineProcessingResult.Successful,
                        FileLineProcessingResult.Rejected => ViewModels.FileLineProcessingResult.Rejected,
                        _ => ViewModels.FileLineProcessingResult.Unknown
                    },
                    LineNumber = fileLineModel.LineNumber,
                    RejectionReason = String.IsNullOrEmpty(fileLineModel.RejectionReason) ? "Rejected" : fileLineModel.RejectionReason,
                    TransactionId = fileLineModel.TransactionId
                });
            }
            
            IEnumerable<ViewModels.FileLine> sortQuery = this.Sorting switch
            {
                (FileDetailsSorting.LineNumber, Ascending: false) => resultList.OrderBy(p => p.LineNumber),
                (FileDetailsSorting.LineNumber, Ascending: true) => resultList.OrderByDescending(p =>
                    p.LineNumber),
                (FileDetailsSorting.Result, Ascending: false) => resultList.OrderBy(p => p.ProcessingResult),
                (FileDetailsSorting.Result, Ascending: true) => resultList.OrderByDescending(p =>
                    p.ProcessingResult),
                _ => resultList.AsEnumerable()
            };

            this.FileLines= sortQuery.ToList();
        }
        
        public async Task Sort(FileDetailsSorting value)
        {
            this.Sorting = (Column: value, Ascending: this.Sorting.Column == value && !this.Sorting.Ascending);

            await this.GetFileDetails();
        }

        public (FileDetailsSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum FileDetailsSorting
    {
        LineNumber,
        Result,
    }
}
