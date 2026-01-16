using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using Shared.Results;

namespace EstateManagementUI.BusinessLogic.BackendAPI
{
    public interface IEstateReportingApiClient
    {
        Task<Result<List<ComparisonDate>>> GetComparisonDates(String accessToken, Guid estateId, CancellationToken cancellationToken);
        Task<Result<MerchantKpi>> GetMerchantKpi(String accessToken, Guid estateId, CancellationToken cancellationToken);
    }

    public class EstateReportingApiClient : ClientProxyBase.ClientProxyBase, IEstateReportingApiClient {

        private readonly Func<String, String> BaseAddressResolver;
        private const String EstateIdHeaderName = "EstateId";
        public EstateReportingApiClient(Func<String, String> baseAddressResolver,
                                        HttpClient httpClient) : base(httpClient) {
            this.BaseAddressResolver = baseAddressResolver;
        }

        public async Task<Result<List<ComparisonDate>>> GetComparisonDates(String accessToken,
                                                                           Guid estateId,
                                                                           CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl("/api/calendars/comparisondates");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<List<ComparisonDate>> result = await this.SendHttpGetRequest<List<ComparisonDate>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting comparison dates for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<MerchantKpi>> GetMerchantKpi(String accessToken, Guid estateId, CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl("/api/merchants/kpis");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<MerchantKpi>? result = await this.SendHttpGetRequest<MerchantKpi>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant kpis for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        private String BuildRequestUrl(String route)
        {
            String baseAddress = this.BaseAddressResolver("EstateReportingApi");

            String requestUri = $"{baseAddress}{route}";

            return requestUri;
        }
    }
}
