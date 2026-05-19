using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.FileProcessing
{
    public partial class Details
    {
        [Parameter]
        public Guid ImportLogId { get; set; }

        private const int PageSize = 5;

        private FileImportLogDetailsModel? importLog;
        private int currentPage = 1;
        private string fileNameFilter = string.Empty;

        private string FileNameFilter
        {
            get => fileNameFilter;
            set
            {
                if (fileNameFilter == value)
                {
                    return;
                }

                fileNameFilter = value;
                currentPage = 1;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.FileProcessing, PermissionFunction.List, this.GetImportLog);
            if (result.IsFailed)
            {
                return;
            }

            this.StateHasChanged();
        }

        protected async Task<Result> GetImportLog()
        {
            await RequirePermission(PermissionSection.FileProcessing, PermissionFunction.View);
            currentPage = 1;
            CorrelationId correlationId = new(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();

            var result = await FileProcessingUIService.GetImportLog(correlationId, estateId,null, ImportLogId);
            if (result.IsSuccess)
            {
                importLog = result.Data;
            }

            return Result.Success();
        }

        private IReadOnlyList<FileProcessingFileModel> FilteredFiles =>
            importLog?.Files
                .Where(file => string.IsNullOrWhiteSpace(FileNameFilter) || file.FileName.Contains(FileNameFilter, StringComparison.OrdinalIgnoreCase))
                .ToList() ?? [];

        private int FilteredFileCount => FilteredFiles.Count;

        private IReadOnlyList<FileProcessingFileModel> PagedFiles =>
            FilteredFiles.Skip((currentPage - 1) * PageSize).Take(PageSize).ToList();

        private int TotalPages => FilteredFileCount == 0
            ? 1
            : (int)Math.Ceiling(FilteredFileCount / (double)PageSize);

        private int FirstFileOnPage => FilteredFileCount == 0 ? 0 : ((currentPage - 1) * PageSize) + 1;

        private int LastFileOnPage => FilteredFileCount == 0
            ? 0
            : Math.Min(currentPage * PageSize, FilteredFileCount);

        private bool IsPreviousPageDisabled => currentPage <= 1;

        private bool IsNextPageDisabled => currentPage >= TotalPages;

        private void ClearFileNameFilter()
        {
            FileNameFilter = string.Empty;
        }

        private void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
        }

        private void NextPage()
        {
            if (currentPage < TotalPages)
            {
                currentPage++;
            }
        }
    }
}
