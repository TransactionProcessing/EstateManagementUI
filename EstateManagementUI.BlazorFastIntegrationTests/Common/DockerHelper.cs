using Shared.IntegrationTesting;
using TransactionProcessor.DataTransferObjects.Requests.MerchantSchedule;
using SimpleResults;
using MerchantScheduleModel = EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class DockerHelper
{
    private readonly LocalAppHost _appHost;

    public DockerHelper(LocalAppHost appHost)
    {
        _appHost = appHost;
        TestHostHttpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        })
        {
            BaseAddress = appHost.BaseUri
        };

        TransactionProcessorClient = new LocalTransactionProcessorClient(new TestSupportClient(appHost.BaseUri));
    }

    public Guid TestId { get; } = Guid.NewGuid();
    public HttpClient TestHostHttpClient { get; }
    public LocalTransactionProcessorClient TransactionProcessorClient { get; }

    public int GetHostPort(ContainerType containerType) => 5004;

    public sealed class LocalTransactionProcessorClient
    {
        private readonly TestSupportClient _supportClient;

        public LocalTransactionProcessorClient(TestSupportClient supportClient)
        {
            _supportClient = supportClient;
        }

        public async Task<Result<MerchantScheduleModel>> GetMerchantSchedule(string accessToken, Guid estateId, Guid merchantId, int year, CancellationToken cancellationToken)
        {
            var schedule = await _supportClient.GetMerchantScheduleAsync(estateId, merchantId, year);
            return schedule is null ? Result.NotFound() : Result.Success(new MerchantScheduleModel
            {
                Year = schedule.Year,
                Months = schedule.Months.Select(month => new EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleMonthModel
                {
                    Month = month.Month,
                    ClosedDays = month.ClosedDays.ToList()
                }).ToList()
            });
        }

        public async Task<Result> CreateMerchantSchedule(string accessToken, Guid estateId, Guid merchantId, CreateMerchantScheduleRequest request, CancellationToken cancellationToken)
        {
            await _supportClient.SetMerchantScheduleAsync(estateId, merchantId, request);
            return Result.Success();
        }
    }
}
