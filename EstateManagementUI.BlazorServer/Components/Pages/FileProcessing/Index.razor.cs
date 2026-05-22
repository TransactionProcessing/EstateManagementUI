using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.FileProcessing
{
    public partial class Index
    {
        private bool isLoading = true;
        private DateTime startDate;
        private DateTime endDate;
        private List<FileImportLogDetailsModel> filteredLogs = [];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            startDate = DateTime.Today.AddDays(-30);
            endDate = DateTime.Today;

            Result result = await OnAfterRender(PermissionSection.FileProcessing, PermissionFunction.List, this.ApplyFilter);
            if (result.IsFailed)
            {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private int TotalFiles => filteredLogs.Sum(log => log.NumberOfFilesProcessed);
        private int TotalLines => filteredLogs.Sum(log => log.TotalFileLines);
        private int SuccessfulLines => filteredLogs.Sum(log => log.SuccessfulLines);
        private string SuccessRateText => TotalLines == 0 ? "0%" : $"{Math.Round(SuccessfulLines * 100m / TotalLines, 1):0.#}%";

        private async Task ApplyDateFilter()
        {
            await ApplyFilter();
        }

        private async Task ResetDateFilter()
        {
            startDate = DateTime.Today.AddDays(-30);
            endDate = DateTime.Today;
            await ApplyFilter();
        }

        private async Task<Result> ApplyFilter()
        {
            CorrelationId correlationId = new(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();
            var result = await this.FileProcessingUIService.GetImportLogList(correlationId, estateId, null, startDate, endDate);
            if (result.IsSuccess)
            {
                filteredLogs = result.Data;
            }
            return Result.Success();
        }

        private void NavigateToImportLog(Guid fileImportLogId) =>
            NavigationManager.NavigateTo($"/file-processing/import-log/{fileImportLogId}");

    }
}
