using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using TransactionProcessor.DataTransferObjects.Responses.Estate;

namespace EstateManagementUI.BusinessLogic.Client;

public static class APIModelFactory {
    public static List<ComparisonDateModel> ConvertFrom(List<ComparisonDate> apiResult)
    {
        List<ComparisonDateModel> comparisonDates = new();

        foreach (ComparisonDate comparisonDate in apiResult)
        {
            comparisonDates.Add(new ComparisonDateModel
            {
                Date = comparisonDate.Date,
                Description = comparisonDate.Description,
                OrderValue = comparisonDate.OrderValue
            });
        }

        return comparisonDates;
    }

    public static MerchantKpiModel ConvertFrom(MerchantKpi apiResult) {
        MerchantKpiModel model = new MerchantKpiModel {
            MerchantsWithNoSaleInLast7Days = apiResult.MerchantsWithNoSaleInLast7Days, 
            MerchantsWithNoSaleToday = apiResult.MerchantsWithNoSaleToday, 
            MerchantsWithSaleInLastHour = apiResult.MerchantsWithSaleInLastHour
        };

        return model;
    }

    public static TodaysSalesModel ConvertFrom(TodaysSales apiResultData) {
        TodaysSalesModel model = new TodaysSalesModel {
            ComparisonAverageValue = apiResultData.ComparisonAverageSalesValue,
            ComparisonSalesCount = apiResultData.ComparisonSalesCount,
            ComparisonSalesValue = apiResultData.ComparisonSalesValue,
            TodaysAverageValue = apiResultData.TodaysAverageSalesValue,
            TodaysSalesCount = apiResultData.TodaysSalesCount,
            TodaysSalesValue = apiResultData.TodaysSalesValue
        };
        return model;
    }

    public static List<RecentMerchantsModel> ConvertFrom(List<Merchant> apiResultData) {
        List<RecentMerchantsModel> merchants = new();

        foreach (Merchant merchant in apiResultData) {
            merchants.Add(new RecentMerchantsModel
            {
                CreatedDateTime = merchant.CreatedDateTime,
                EstateReportingId = merchant.EstateReportingId,
                LastSale = merchant.LastSale,
                LastSaleDateTime = merchant.LastSaleDateTime,
                LastStatement = merchant.LastStatement,
                MerchantId = merchant.MerchantId,
                MerchantReportingId = merchant.MerchantReportingId,
                Name = merchant.Name,
                PostCode = merchant.PostCode,
                Reference = merchant.Reference,
                Region = merchant.Region,
                Town = merchant.Town
            });
        }

        return merchants;
    }

    public static EstateModel ConvertFrom(EstateResponse apiResultData) {
        EstateModel model = new EstateModel {
            Reference = apiResultData.EstateReference,
            EstateId = apiResultData.EstateId,
            EstateName = apiResultData.EstateName,
            Operators = new List<EstateOperatorModel>()
        };

        foreach (EstateOperatorResponse estateOperatorResponse in apiResultData.Operators) {
            model.Operators.Add(new EstateOperatorModel {
                Name = estateOperatorResponse.Name,
                OperatorId = estateOperatorResponse.OperatorId,
                RequireCustomMerchantNumber = estateOperatorResponse.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperatorResponse.RequireCustomTerminalNumber,
            });
        }

        return model;
    }
}