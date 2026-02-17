using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

namespace EstateManagementUI.BusinessLogic.BackendAPI
{
    public interface IEstateReportingApiClient
    {
        Task<Result<Estate>> GetEstate(String accessToken, Guid estateId, CancellationToken cancellationToken);

        Task<Result<List<EstateOperator>>> GetEstateAssignedOperators(String accessToken,
                                                                      Guid estateId,
                                                                      CancellationToken cancellationToken);

        Task<Result<List<MerchantOperator>>> GetMerchantAssignedOperators(String accessToken,
                                                                      Guid estateId, Guid merchantId,
                                                                      CancellationToken cancellationToken);
        Task<Result<List<Operator>>> GetOperators(String accessToken,
                                                                      Guid estateId,
                                                                      CancellationToken cancellationToken);

        Task<Result<Operator>> GetOperator(String accessToken,
                                                  Guid estateId,
                                                  Guid operatorId,
                                                  CancellationToken cancellationToken);

        Task<Result<List<ComparisonDate>>> GetComparisonDates(String accessToken, Guid estateId, CancellationToken cancellationToken);
        Task<Result<MerchantKpi>> GetMerchantKpi(String accessToken, Guid estateId, CancellationToken cancellationToken);

        Task<Result<List<Merchant>>> GetRecentMerchants(String accessToken,
                                                        Guid estateId,
                                                        CancellationToken cancellationToken);
        Task<Result<List<Merchant>>> GetMerchants(String accessToken,
                                                        Guid estateId,
                                                        String? name,
                                                        String? reference,
                                                        Int32? settlementSchedule,
                                                        String? region,
                                                        String? postCode,
                                                        CancellationToken cancellationToken);

        Task<Result<Merchant>> GetMerchant(String accessToken,
                                                  Guid estateId,
                                                  Guid merchantId,
                                                  CancellationToken cancellationToken);

        Task<Result<List<Contract>>> GetRecentContracts(String accessToken,
                                                        Guid estateId, CancellationToken cancellationToken);

        Task<Result<List<Contract>>> GetContracts(String accessToken,
                                                        Guid estateId, CancellationToken cancellationToken);

        Task<Result<Contract>> GetContract(String accessToken,
                                                  Guid estateId, 
                                                  Guid contractId,
                                                  CancellationToken cancellationToken);
        Task<Result<TodaysSales>> GetTodaysSales(String accessToken,
                                                 Guid estateId,
                                                 Int32 merchantReportingId,
                                                 Int32 operatorReportingId,
                                                 DateTime comparisonDate,
                                                 CancellationToken cancellationToken);

        Task<Result<TodaysSales>> GetTodaysFailedSales(String accessToken, Guid estateId, Int32 merchantReportingId, Int32 operatorReportingId, String responseCode, DateTime comparisonDate, CancellationToken cancellationToken);

        Task<Result<List<MerchantContract>>> GetMerchantContracts(String accessToken,
                                                                  Guid estateId,
                                                                  Guid merchantId,
                                                                  CancellationToken cancellationToken);

        Task<Result<List<MerchantDevice>>> GetMerchantDevices(String accessToken,
                                                              Guid estateId,
                                                              Guid merchantId,
                                                              CancellationToken cancellationToken);

        Task<Result<List<MerchantOperator>>> GetMerchantOperators(String accessToken,
                                                                  Guid estateId,
                                                                  Guid merchantId,
                                                                  CancellationToken cancellationToken);

        Task<Result<TransactionDetailReportResponse>> GetTransactionDetailReport(String accessToken,
                                               Guid estateId,
                                               TransactionDetailReportRequest request,
                                               CancellationToken cancellationToken);

        Task<Result<TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(String accessToken, Guid estateId, TransactionSummaryByMerchantRequest request, CancellationToken cancellationToken);
        Task<Result<TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(String accessToken, Guid estateId, TransactionSummaryByOperatorRequest request, CancellationToken cancellationToken);
        Task<Result<ProductPerformanceResponse>> GetProductPerformance(String accessToken, Guid estateId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        Task<Result<List<TodaysSalesByHour>>> GetTodaysSalesByHour(String accessToken,
                                                 Guid estateId,
                                                 DateTime comparisonDate,
                                                 CancellationToken cancellationToken);

        Task<Result<TodaysSettlement>> GetTodaysSettlement(String accessToken,
                                                                   Guid estateId,
                                                                   DateTime comparisonDate,
                                                                   CancellationToken cancellationToken);
    }

    public class EstateReportingApiClient : ClientProxyBase.ClientProxyBase, IEstateReportingApiClient {

        private readonly Func<String, String> BaseAddressResolver;
        private const String EstateIdHeaderName = "EstateId";
        public EstateReportingApiClient(Func<String, String> baseAddressResolver,
                                        HttpClient httpClient) : base(httpClient) {
            this.BaseAddressResolver = baseAddressResolver;
        }

        public async Task<Result<Operator>> GetOperator(String accessToken,
                                                              Guid estateId,
                                                              Guid operatorId,
                                                              CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/operators/{operatorId}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<Operator> result = await this.SendHttpGetRequest<Operator>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting operators {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<Estate>> GetEstate(String accessToken,
                                                    Guid estateId,
                                                    CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl("/api/estates");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<Estate> result = await this.SendHttpGetRequest<Estate>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<MerchantOperator>>> GetMerchantAssignedOperators(String accessToken,
                                                                                       Guid estateId,
                                                                                       Guid merchantId,
                                                                                       CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/merchants/{merchantId}/operators");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<List<MerchantOperator>> result = await this.SendHttpGetRequest<List<MerchantOperator>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting estate assigned operators {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<EstateOperator>>> GetEstateAssignedOperators(String accessToken,
                                                                                   Guid estateId,
                                                                                   CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl("/api/estates/operators");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<List<EstateOperator>> result = await this.SendHttpGetRequest<List<EstateOperator>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting estate assigned operators {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<Operator>>> GetOperators(String accessToken,
                                                               Guid estateId,
                                                               CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl("/api/operators");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<List<Operator>> result = await this.SendHttpGetRequest<List<Operator>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting operators {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
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

        public async Task<Result<List<Merchant>>> GetRecentMerchants(String accessToken, Guid estateId, CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl("/api/merchants/recent");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<Merchant>> result = await this.SendHttpGetRequest<List<Merchant>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting recent merchants for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<Merchant>>> GetMerchants(String accessToken,
                                                               Guid estateId,
                                                               String? name,
                                                               String? reference,
                                                               Int32? settlementSchedule,
                                                               String? region,
                                                               String? postCode,
                                                               CancellationToken cancellationToken) {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("name",name);
            builder.AddParameter("reference", reference);
            builder.AddParameter("settlementSchedule", settlementSchedule);
            builder.AddParameter("region", region);
            builder.AddParameter("postCode", postCode);

            String requestUri = this.BuildRequestUrl($"/api/merchants?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<Merchant>> result = await this.SendHttpGetRequest<List<Merchant>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchants for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<Merchant>> GetMerchant(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/merchants/{merchantId}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<Merchant> result = await this.SendHttpGetRequest<Merchant>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant id {merchantId} for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<MerchantContract>>> GetMerchantContracts(String accessToken,
                                                        Guid estateId,
                                                        Guid merchantId,
                                                        CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/api/merchants/{merchantId}/contracts");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<MerchantContract>> result = await this.SendHttpGetRequest<List<MerchantContract>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contracts merchant id {merchantId} for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<MerchantOperator>>> GetMerchantOperators(String accessToken,
                                                                               Guid estateId,
                                                                               Guid merchantId,
                                                                               CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/api/merchants/{merchantId}/operators");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<MerchantOperator>> result = await this.SendHttpGetRequest<List<MerchantOperator>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting operators merchant id {merchantId} for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<TransactionDetailReportResponse>> GetTransactionDetailReport(String accessToken,
                                                                                              Guid estateId,
                                                                                              TransactionDetailReportRequest request,
                                                                                              CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/transactions/transactiondetailreport");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<TransactionDetailReportResponse>? result = await this.SendHttpPostRequest<TransactionDetailReportRequest, TransactionDetailReportResponse>(requestUri, request, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting transaction detail report for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(String accessToken,
                                                                                                      Guid estateId,
                                                                                                      TransactionSummaryByMerchantRequest request,
                                                                                                      CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/transactions/transactionsummarybymerchantreport");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<TransactionSummaryByMerchantResponse>? result = await this.SendHttpPostRequest<TransactionSummaryByMerchantRequest, TransactionSummaryByMerchantResponse>(requestUri, request, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting merchant transaction summary report for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(String accessToken,
                                                                                                      Guid estateId,
                                                                                                      TransactionSummaryByOperatorRequest request,
                                                                                                      CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl($"/api/transactions/transactionsummarybyoperatorreport");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<TransactionSummaryByOperatorResponse>? result = await this.SendHttpPostRequest<TransactionSummaryByOperatorRequest, TransactionSummaryByOperatorResponse>(requestUri, request, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting operator transaction summary report for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<ProductPerformanceResponse>> GetProductPerformance(String accessToken,
                                                                                    Guid estateId,
                                                                                    DateTime startDate,
                                                                                    DateTime endDate,
                                                                                    CancellationToken cancellationToken) {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("startDate", $"{startDate:yyyy-MM-dd}");
            builder.AddParameter("endDate", $"{endDate:yyyy-MM-dd}");

            String requestUri = this.BuildRequestUrl($"/api/transactions/productperformancereport?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<ProductPerformanceResponse> result = await this.SendHttpGetRequest<ProductPerformanceResponse>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting product performance report for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<TodaysSalesByHour>>> GetTodaysSalesByHour(String accessToken,
                                                                                Guid estateId,
                                                                                DateTime comparisonDate,
                                                                                CancellationToken cancellationToken) {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("comparisonDate", $"{comparisonDate.Date:yyyy-MM-dd}");

            String requestUri = this.BuildRequestUrl($"/api/transactions/todayssalesbyhour?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                
                Result<List<TodaysSalesByHour>>? result = await this.SendHttpGetRequest<List<TodaysSalesByHour>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting todays sales by hour for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<TodaysSettlement>> GetTodaysSettlement(String accessToken,
                                                                        Guid estateId,
                                                                        DateTime comparisonDate,
                                                                        CancellationToken cancellationToken) {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.AddParameter("comparisonDate", $"{comparisonDate.Date:yyyy-MM-dd}");

            String requestUri = this.BuildRequestUrl($"/api/settlements/todayssettlements?{builder.BuildQueryString()}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];

                Result<TodaysSettlement>? result = await this.SendHttpGetRequest<TodaysSettlement>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting todays settlement for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<MerchantDevice>>> GetMerchantDevices(String accessToken,
                                                                           Guid estateId,
                                                                           Guid merchantId,
                                                                           CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/api/merchants/{merchantId}/devices");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<MerchantDevice>> result = await this.SendHttpGetRequest<List<MerchantDevice>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting devices merchant id {merchantId} for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<Contract>>> GetContracts(String accessToken,
                                                                     Guid estateId,
                                                                     CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl("/api/contracts");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<Contract>> result = await this.SendHttpGetRequest<List<Contract>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contracts for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<Contract>> GetContract(String accessToken,
                                                               Guid estateId,
                                                               Guid contractId,
                                                               CancellationToken cancellationToken)
        {
            String requestUri = this.BuildRequestUrl($"/api/contracts/{contractId}");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<Contract> result = await this.SendHttpGetRequest<Contract>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting contracts for estate {estateId}.", ex);

                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result<List<Contract>>> GetRecentContracts(String accessToken,
                                                                     Guid estateId,
                                                                     CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl("/api/contracts/recent");

            try
            {
                List<(String headerName, String headerValue)> additionalHeaders = [
                    (EstateIdHeaderName, estateId.ToString())
                ];
                Result<List<Contract>> result = await this.SendHttpGetRequest<List<Contract>>(requestUri, accessToken, additionalHeaders, cancellationToken);

                if (result.IsFailed)
                    return ResultHelpers.CreateFailure(result);

                return result;
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting recent contracts for estate {estateId}.", ex);

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