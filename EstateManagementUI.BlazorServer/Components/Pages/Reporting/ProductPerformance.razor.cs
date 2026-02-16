using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Components.Pages.Reporting
{
    public partial class ProductPerformance
    {
        private bool isLoading = true;
        private bool showChart = false;

        // Filter states
        //private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
        private DateOnly _startDate = DateOnly.FromDateTime(new DateTime(2025, 12, 10));
        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Now);

        // Data
        private TransactionModels.ProductPerformanceResponse? performanceData;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Reporting, PermissionFunction.ProductPerformanceReport, this.LoadData);
            if (result.IsFailed)
            {
                return;
            }
        }

        private async Task<Result> LoadData()
        {
            try
            {
                isLoading = true;
                errorMessage = null;
                StateHasChanged();

                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var startDate = _startDate.ToDateTime(TimeOnly.MinValue);
                var endDate = _endDate.ToDateTime(TimeOnly.MaxValue);

                var result = await Mediator.Send(new TransactionQueries.GetProductPerformanceQuery(
                    correlationId,
                    estateId,
                    startDate,
                    endDate
                ));

                if (result.IsSuccess && result.Data != null)
                {
                    performanceData = ModelFactory.ConvertFrom(result.Data);
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to load product performance data";
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load product performance data: {ex.Message}";
                return Result.Failure(errorMessage);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
