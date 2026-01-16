using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;

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
}