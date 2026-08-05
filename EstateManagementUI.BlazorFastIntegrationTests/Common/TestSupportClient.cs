using EstateManagementUI.BlazorServer.Models;
using TransactionProcessor.DataTransferObjects.Requests.MerchantSchedule;
using System.Net.Http.Json;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class TestSupportClient
{
    private readonly HttpClient _httpClient;

    public TestSupportClient(Uri baseUri)
    {
        _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        })
        {
            BaseAddress = baseUri
        };
    }

    public async Task ResetAsync()
    {
        await _httpClient.PostAsync("/test-support/reset", new StringContent(string.Empty));
    }

    public async Task<MerchantModels.MerchantScheduleModel?> GetMerchantScheduleAsync(Guid estateId, Guid merchantId, int year)
    {
        return await _httpClient.GetFromJsonAsync<MerchantModels.MerchantScheduleModel>($"/test-support/merchant-schedules/{estateId}/{merchantId}/{year}");
    }

    public async Task SetMerchantScheduleAsync(Guid estateId, Guid merchantId, CreateMerchantScheduleRequest request)
    {
        var schedule = new MerchantModels.MerchantScheduleModel
        {
            Year = request.Year,
            Months = request.Months.Select(month => new MerchantModels.MerchantScheduleMonthModel
            {
                Month = month.Month,
                ClosedDays = month.ClosedDays.ToList()
            }).ToList()
        };

        await _httpClient.PutAsJsonAsync($"/test-support/merchant-schedules/{estateId}/{merchantId}", schedule);
    }
}
