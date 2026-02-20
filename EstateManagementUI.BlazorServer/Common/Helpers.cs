using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.General;
using Shared.Results;
using SimpleResults;
using System.Security.Claims;
using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Common
{
    public static class Helpers
    {
        public static string GetComparisonLabel(List<ComparisonDateModel> comparisonDates, string selectedComparisonDate)
        {
            if (comparisonDates == null) return "Comparison";
            if (!DateTime.TryParse(selectedComparisonDate, out var date))
                return "Comparison";
            ComparisonDateModel? comparisonDate = comparisonDates.FirstOrDefault(d => d.Date.Date == date.Date);
            return comparisonDate?.Description ?? date.ToString("MMM dd");
        }

        public static Result<Guid> GetEstateIdFromClaims(this AuthenticationState authState)
        {
            ClaimsPrincipal user = authState.User;
            Result<Claim>? estateIdClaim = ClaimsHelper.GetUserClaim(user, "estateId");
            if (estateIdClaim.IsFailed)
                return ResultHelpers.CreateFailure(estateIdClaim);
            Guid estateId = Guid.Parse(estateIdClaim.Data.Value);
            return Result.Success(estateId);
        }

        private static decimal GetSalesVariance(TransactionModels.TodaysSalesModel todaysSales)
        {
            if (todaysSales == null) return 0;
            if (todaysSales.ComparisonSalesValue == 0)
            {
                // If comparison is 0 and today is 0, no change
                if (todaysSales.TodaysSalesValue == 0) return 0;
                // If comparison is 0 but today has sales, treat as maximum positive change
                return todaysSales.TodaysSalesValue > 0 ? 999m : 0;
            }

            return (todaysSales.TodaysSalesValue - todaysSales.ComparisonSalesValue) / todaysSales.ComparisonSalesValue;
        }

        public static string GetSalesBackgroundClass(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);

            return variance switch
            {
                _ when variance < 0 => "bg-red-50", // Worse
                _ when variance == 0 => "bg-blue-50", // Same
                _ when variance > 0 && variance < 0.2m => "bg-yellow-50", // Slightly better
                _ => "bg-green-50", // Much better
            };
        }

        public static string GetSalesTextClass(TransactionModels.TodaysSalesModel todaysSales, double opacity = 1.0)
        {
            var variance = GetSalesVariance(todaysSales);

            return opacity switch
            {
                _ when opacity < 1.0 && variance < 0 => "text-red-700",
                _ when opacity < 1.0 && variance == 0 => "text-blue-700",
                _ when opacity < 1.0 && variance > 0 && variance < 0.2m => "text-yellow-700",
                _ when opacity < 1.0 => "text-green-700",
                _ when variance < 0 => "text-red-900",
                _ when variance == 0 => "text-blue-900",
                _ when variance > 0 && variance < 0.2m => "text-yellow-900",
                _ => "text-green-900",
            };
        }

        public static string GetSalesBorderClass(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);
            return variance switch
            {
                _ when variance < 0 => "border-red-200",
                _ when variance == 0 => "border-blue-200",
                _ when variance > 0 && variance < 0.2m => "border-yellow-200",
                _ => "border-green-200",
            };
        }

        public static string GetSalesVarianceDisplay(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);
            // Special case: comparison was 0, now has sales
            if (variance >= 999m) return "NEW";
            Decimal percentageChange = variance * 100;
            String sign = variance > 0 ? "+" : "";
            return $"{sign}{percentageChange:F1}%";
        }

        public static string GetFailedSalesBackgroundClass(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);
            return variance switch
            {
                _ when variance < 0 => "bg-green-50", // Good - fewer failures
                _ when variance == 0 => "bg-blue-50", // Same
                _ when variance > 0 && variance < 0.2m => "bg-yellow-50", // Slightly worse
                _ => "bg-red-50", // Much worse
            };
        }

        public static string GetFailedSalesTextClass(TransactionModels.TodaysSalesModel todaysSales, double opacity = 1.0)
        {
            Decimal variance = GetSalesVariance(todaysSales);

            return opacity switch
            {
                _ when opacity < 1.0 && variance < 0 => "text-green-700",
                _ when opacity < 1.0 && variance == 0 => "text-blue-700",
                _ when opacity < 1.0 && variance > 0 && variance < 0.2m => "text-yellow-700",
                _ when opacity < 1.0 => "text-red-700",
                _ when variance < 0 => "text-green-900",
                _ when variance == 0 => "text-blue-900",
                _ when variance > 0 && variance < 0.2m => "text-yellow-900",
                _ => "text-red-900",
            };
        }

        public static string GetFailedSalesBorderClass(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);
            return variance switch
            {
                _ when variance < 0 => "border-green-200",
                _ when variance == 0 => "border-blue-200",
                _ when variance > 0 && variance < 0.2m => "border-yellow-200",
                _ => "border-red-200",
            };
        }

        public static string GetFailedSalesVarianceDisplay(TransactionModels.TodaysSalesModel todaysSales)
        {
            Decimal variance = GetSalesVariance(todaysSales);
            // Special case: comparison was 0, now has failures
            if (variance >= 999m) return "NEW";
            Decimal percentageChange = variance * 100;
            String sign = variance > 0 ? "+" : "";
            return $"{sign}{percentageChange:F1}%";
        }

        public static void NavigateToErrorPage(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("/error", replace: true);
        }
        
        public static void NavigateToAccessDeniedPage(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("/access-denied", replace: true);
        }
        
        public static void NavigateToEntryPage(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("/entry", replace: true);
        }

        public static string GetTabClass(string activeTab, string tab)
        {
            return activeTab == tab
                ? "border-blue-600 text-blue-600 whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm"
                : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm";
        }
    }
}
