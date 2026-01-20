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

    public static EstateModel ConvertFrom(Estate apiResultData) {
        EstateModel model = new() {
            Reference = apiResultData.Reference,
            EstateId = apiResultData.EstateId,
            EstateName = apiResultData.EstateName,
            Operators = new(),
            Users = new(),
            Contracts = new(),
            Merchants = new ()
        };

        foreach (var estateOperator in apiResultData.Operators) {
            model.Operators.Add(new EstateOperatorModel {
                Name = estateOperator.Name,
                OperatorId = estateOperator.OperatorId,
                RequireCustomMerchantNumber = estateOperator.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperator.RequireCustomTerminalNumber,
            });
        }
        foreach (var estateMerchant in apiResultData.Merchants)
        {
            model.Merchants.Add(new EstateMerchantModel()
            {
                Name = estateMerchant.Name,
                Reference = estateMerchant.Reference,
                MerchantId = estateMerchant.MerchantId
            });
        }
        foreach (var estateContract in apiResultData.Contracts)
        {
            model.Contracts.Add(new EstateContractModel()
            {
                Name = estateContract.Name,
                OperatorId = estateContract.OperatorId,
                ContractId = estateContract.ContractId,
                OperatorName = estateContract.OperatorName
            });
        }
        foreach (var estateUser in apiResultData.Users)
        {
            model.Users.Add(new EstateUserModel()
            {
                CreatedDateTime = estateUser.CreatedDateTime,
                EmailAddress = estateUser.EmailAddress,
                UserId = estateUser.UserId
            });
        }
        

        return model;
    }

    public static List<RecentContractModel> ConvertFrom(List<Contract> apiResultData) {
        List<RecentContractModel> contracts = new();

        foreach (Contract contract in apiResultData) {
            contracts.Add(new RecentContractModel {
                ContractId = contract.ContractId,
                Description = contract.Description,
                OperatorName = contract.OperatorName
            });
        }

        return contracts;
    }

    public static List<OperatorModel> ConvertFrom(List<EstateOperator> apiResultData) {
        List<OperatorModel> operators = new();
        foreach (EstateOperator estateOperator in apiResultData) {
            operators.Add(new OperatorModel() {
                Name = estateOperator.Name,
                OperatorId = estateOperator.OperatorId,
                RequireCustomMerchantNumber = estateOperator.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperator.RequireCustomTerminalNumber
            });
        }
        return operators;
    }

    public static List<OperatorModel> ConvertFrom(List<Operator> apiResultData) {
        List<OperatorModel> operators = new();
        foreach (Operator op in apiResultData)
        {
            operators.Add(new OperatorModel()
            {
                Name = op.Name,
                OperatorId = op.OperatorId
            });
        }
        return operators;
    }
}