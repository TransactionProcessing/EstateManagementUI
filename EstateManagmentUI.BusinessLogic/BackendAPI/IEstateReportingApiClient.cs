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

        Task<Result<TodaysSales>> GetTodaysSales(String accessToken,
                                                 Guid estateId,
                                                 Int32 merchantReportingId,
                                                 Int32 operatorReportingId,
                                                 DateTime comparisonDate,
                                                 CancellationToken cancellationToken);

        Task<Result<TodaysSales>> GetTodaysFailedSales(String accessToken, Guid estateId, Int32 merchantReportingId, Int32 operatorReportingId, String responseCode, DateTime comparisonDate, CancellationToken cancellationToken);
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

        public async Task<Result<TodaysSales>> GetTodaysSales(String accessToken, Guid estateId, Int32 merchantReportingId, Int32 operatorReportingId, DateTime comparisonDate, CancellationToken cancellationToken)
        {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("comparisonDate", $"{comparisonDate.Date:yyyy-MM-dd}");
            builder.AddParameter("merchantReportingId", merchantReportingId);
            builder.AddParameter("operatorReportingId", operatorReportingId);

            String requestUri = this.BuildRequestUrl($"/api/transactions/todayssales?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<TodaysSales>? result = await this.SendHttpGetRequest<TodaysSales>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting todays sales for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<TodaysSales>> GetTodaysFailedSales(String accessToken, Guid estateId, Int32 merchantReportingId, Int32 operatorReportingId, String responseCode, DateTime comparisonDate, CancellationToken cancellationToken)
        {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("comparisonDate", $"{comparisonDate.Date:yyyy-MM-dd}");
            builder.AddParameter("merchantReportingId", merchantReportingId);
            builder.AddParameter("operatorReportingId", operatorReportingId);
            builder.AddParameter("responseCode", responseCode);

            String requestUri = this.BuildRequestUrl($"/api/transactions/todaysfailedsales?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<TodaysSales>? result = await this.SendHttpGetRequest<TodaysSales>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting todays failed sales for estate {estateId} and response code {responseCode}.", ex);

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


public class QueryStringBuilder
{
    private Dictionary<string, (object value, Boolean alwaysInclude)> parameters = new Dictionary<String, (Object value, Boolean alwaysInclude)>();

    public QueryStringBuilder AddParameter(string key, object value, Boolean alwaysInclude = false)
    {
        this.parameters.Add(key, (value, alwaysInclude));
        return this;
    }

    static Dictionary<string, object> FilterDictionary(Dictionary<string, (object value, Boolean alwaysInclude)> inputDictionary)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        foreach (KeyValuePair<String, (object value, Boolean alwaysInclude)> entry in inputDictionary)
        {
            if (entry.Value.value != null && !IsDefaultValue(entry.Value.value, entry.Value.alwaysInclude))
            {
                result.Add(entry.Key, entry.Value.value);
            }
        }

        return result;
    }

    static bool IsDefaultValue<T>(T value, Boolean alwaysInclude)
    {
        if (alwaysInclude)
            return false;

        Object? defaultValue = GetDefault(value.GetType());

        if (defaultValue == null && value.GetType() == typeof(String))
        {
            defaultValue = String.Empty;
        }
        return defaultValue.Equals(value);
    }

    public static object GetDefault(Type t)
    {
        Func<object> f = GetDefault<object>;
        return f.Method.GetGenericMethodDefinition().MakeGenericMethod(t).Invoke(null, null);
    }

    private static T GetDefault<T>()
    {
        return default(T);
    }

    public string BuildQueryString()
    {
        Dictionary<String, Object> filtered = FilterDictionary(this.parameters);

        if (filtered.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder queryString = new StringBuilder();

        foreach (KeyValuePair<String, Object> kvp in filtered)
        {
            if (queryString.Length > 0)
            {
                queryString.Append("&");
            }

            queryString.Append($"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value.ToString())}");
        }

        return queryString.ToString();
    }
}