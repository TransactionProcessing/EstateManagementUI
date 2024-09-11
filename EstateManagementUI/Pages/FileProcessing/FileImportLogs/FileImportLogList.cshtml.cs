using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.ViewModels;
using SimpleResults;

namespace EstateManagementUI.Pages.FileProcessing.FileImportLogs
{
    public class FileImportLogList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        [Display(Name = "Merchant")]
        public MerchantListModel Merchant { get; set; }

        [Display(Name = "Start Date")]
        public DateModel StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateModel EndDate { get; set; }

        public FileImportLogList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.FileProcessing, ContractFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.FileImportLogs= new List<ViewModels.FileImportLogList>();
        }

        public List<ViewModels.FileImportLogList> FileImportLogs { get; set; }
        

        public override async Task MountAsync() {
            await this.GetMerchants();
            this.StartDate = new DateModel { SelectedDate = DateTime.Now };
            this.EndDate = new DateModel { SelectedDate = DateTime.Now };
        }

        public async Task Query() {
            if (String.IsNullOrEmpty(this.Merchant.MerchantId) == false)
            {
                await this.GetFiles();
            }
        }

        private async Task GetMerchants()
        {
            await this.PopulateTokenAndEstateId();

            Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(this.AccessToken, this.EstateId);

            List<MerchantModel> response = await this.Mediator.Send(query, CancellationToken.None);

            List<SelectListItem> resultList = new();
            foreach (MerchantModel merchantModel in response)
            {
                resultList.Add(new SelectListItem
                {
                    Value = merchantModel.MerchantId.ToString(),
                    Text = merchantModel.MerchantName,
                });
            }

            List<SelectListItem> ordered = resultList.OrderBy(m => m.Text).ToList();
            ordered.Insert(0, new SelectListItem("- Select a Merchant -", "", true));
            this.Merchant = new MerchantListModel { Merchants = ordered };
        }

        private async Task GetFiles()
        {
            await this.PopulateTokenAndEstateId();

            Queries.GetFileImportLogsListQuery query = new Queries.GetFileImportLogsListQuery(this.AccessToken, this.EstateId, Guid.Parse(this.Merchant.MerchantId), this.StartDate.SelectedDate.Value, this.EndDate.SelectedDate.Value);

            Result<List<BusinessLogic.Models.FileImportLogModel>> response = await this.Mediator.Send(query, CancellationToken.None);

            List<ViewModels.FileImportLogList> resultList = new();
            foreach (BusinessLogic.Models.FileImportLogModel fileImportLogModel in response.Data) {
                resultList.Add(new ViewModels.FileImportLogList() {
                    ImportLogDate = fileImportLogModel.ImportLogDate,
                    FileCount = fileImportLogModel.FileCount,
                    FileImportLogId = fileImportLogModel.FileImportLogId,
                    ImportLogDateTime = fileImportLogModel.ImportLogDateTime,
                    ImportLogTime = fileImportLogModel.ImportLogTime
                });
            }

            IEnumerable<ViewModels.FileImportLogList> sortQuery = this.Sorting switch
            {
                (FileImportLogListSorting.ImportLogDate, Ascending: false) => resultList.OrderBy(p => p.ImportLogDate),
                (FileImportLogListSorting.ImportLogDate, Ascending: true) => resultList.OrderByDescending(p => p.ImportLogDate),
                (FileImportLogListSorting.NumberOfFiles, Ascending: false) => resultList.OrderBy(p => p.FileCount),
                (FileImportLogListSorting.NumberOfFiles, Ascending: true) => resultList.OrderByDescending(p => p.FileCount),
                
                _ => resultList.AsEnumerable()
            };

            this.FileImportLogs = sortQuery.ToList();

        }

        public async Task Sort(FileImportLogListSorting value)
        {
            this.Sorting = (Column: value, Ascending: this.Sorting.Column == value && !this.Sorting.Ascending);

            await this.GetFiles();
        }

        public (FileImportLogListSorting Column, bool Ascending) Sorting { get; set; }

        //public async Task Edit(Guid contractId)
        //{

        //}

        public async Task ViewFiles(Guid fileImportLogId)
        {
          this.Location(this.Url.Page("/FileProcessing/FileImportLog", new { FileImportLogId = fileImportLogId, MerchantId = Guid.Parse(this.Merchant.MerchantId)}));
        }
    }

    public enum FileImportLogListSorting
    {
        ImportLogDate,
        NumberOfFiles
    }
}
