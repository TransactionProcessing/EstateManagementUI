using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

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
}

public  static class FactoryExtensions{

    public static OperatorModel ToOperator(this Operator apiResultData) {
        OperatorModel model = new() {
            Name = apiResultData.Name,
            OperatorId = apiResultData.OperatorId,
            RequireCustomMerchantNumber = apiResultData.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = apiResultData.RequireCustomTerminalNumber
        };
        return model;
    }

    public static List<OperatorModel> ToOperator(this List<Operator> apiResultData)
    {
        List<OperatorModel> operators = new();
        foreach (Operator op in apiResultData) {
            operators.Add(op.ToOperator());
        }

        return operators;
    }

    public static List<OperatorModel> ToOperator(this List<EstateOperator> apiResultData)
    {
        List<OperatorModel> operators = new();
        foreach (EstateOperator estateOperator in apiResultData)
        {
            operators.Add(new OperatorModel()
            {
                Name = estateOperator.Name,
                OperatorId = estateOperator.OperatorId,
                RequireCustomMerchantNumber = estateOperator.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperator.RequireCustomTerminalNumber
            });
        }
        return operators;
    }

    public static List<RecentMerchantsModel> ToRecentMerchant(this List<Merchant> apiResultData)
    {
        List<RecentMerchantsModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new RecentMerchantsModel
            {
                CreatedDateTime = merchant.CreatedDateTime,
                MerchantId = merchant.MerchantId,
                Name = merchant.Name,
                Reference = merchant.Reference,
            });
        }

        return merchants;
    }

    public static List<MerchantListModel> ToMerchantList(this List<Merchant> apiResultData)
    {
        List<MerchantListModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new MerchantListModel
            {
                CreatedDateTime = merchant.CreatedDateTime,
                MerchantId = merchant.MerchantId,
                Balance = merchant.Balance,
                AvailableBalance = 0, // TODO: remove this
                MerchantName = merchant.Name,
                PostalCode = merchant.PostCode,
                MerchantReference = merchant.Reference,
                Region = merchant.Region,
                SettlementSchedule = ((SettlementSchedule)merchant.SettlementSchedule).ToString(),
            });
        }

        return merchants;
    }

    public static List<MerchantDropDownModel> ToMerchantDropDown(this List<Merchant> apiResultData)
    {
        List<MerchantDropDownModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new MerchantDropDownModel
            {
                MerchantId = merchant.MerchantId,
                MerchantName = merchant.Name,
            });
        }

        return merchants;
    }

    public static MerchantModel ToMerchant(this Merchant apiResultData) {
        MerchantModel model = new MerchantModel {
            CreatedDateTime = apiResultData.CreatedDateTime,
            MerchantReference = apiResultData.Reference,
            AddressId = apiResultData.AddressId,
            AddressLine1 = apiResultData.AddressLine1,
            AddressLine2 = apiResultData.AddressLine2,
            AvailableBalance = 0,
            Balance = apiResultData.Balance,
            ContactId = apiResultData.ContactId,
            ContactEmailAddress = apiResultData.ContactEmail,
            ContactName = apiResultData.ContactName,
            ContactPhoneNumber = apiResultData.ContactPhone,
            Country = apiResultData.Country,
            MerchantId = apiResultData.MerchantId,
            MerchantName = apiResultData.Name,
            Region = apiResultData.Region,
            PostalCode = apiResultData.PostCode,
            SettlementSchedule = ((SettlementSchedule)apiResultData.SettlementSchedule).ToString(),
            Town = apiResultData.Town,
        };
        return model;
    }

    public static List<MerchantOperatorModel> ToMerchantOperators(this List<MerchantOperator> apiResultData)
    {
        List<MerchantOperatorModel> merchantOperators = new();
        
        foreach (MerchantOperator merchantOperator in apiResultData)
        {
            merchantOperators.Add(new MerchantOperatorModel
            {
                MerchantId = merchantOperator.MerchantId,
                OperatorId = merchantOperator.OperatorId,
                OperatorName = merchantOperator.OperatorName,
                MerchantNumber = merchantOperator.MerchantNumber,
                TerminalNumber = merchantOperator.TerminalNumber,
                IsDeleted = merchantOperator.IsDeleted
            });
        }

        return merchantOperators;
    }

    public static List<MerchantContractModel> ToMerchantContracts(this List<MerchantContract> apiResultData)
    {
        List<MerchantContractModel> merchantContracts = new();

        foreach (MerchantContract merchantContract in apiResultData) {
            MerchantContractModel c = new() {
                ContractId = merchantContract.ContractId,
                ContractName = merchantContract.ContractName,
                OperatorName = merchantContract.OperatorName,
                ContractProducts = new List<MerchantContractProductModel>(),
                IsDeleted = merchantContract.IsDeleted,
                MerchantId = merchantContract.MerchantId
            };

            foreach (MerchantContractProduct merchantContractContractProduct in merchantContract.ContractProducts) {
                c.ContractProducts.Add(new MerchantContractProductModel
                {
                    MerchantId = merchantContractContractProduct.MerchantId,
                    ContractId = merchantContractContractProduct.ContractId,
                    DisplayText = merchantContractContractProduct.DisplayText,
                    ProductId = merchantContractContractProduct.ProductId,
                    ProductName = merchantContractContractProduct.ProductName,
                    ProductType = ((ProductType)merchantContractContractProduct.ProductType).ToString(),
                    Value = merchantContractContractProduct.Value
                });
            }
            merchantContracts.Add(c);
        }

        return merchantContracts;
    }

    public static List<MerchantDeviceModel> ToMerchantDevices(this List<MerchantDevice> apiResultData)
    {
        List<MerchantDeviceModel> merchantDevices= new();

        foreach (MerchantDevice merchantDevice in apiResultData)
        {
            merchantDevices.Add(new MerchantDeviceModel
            {
                MerchantId = merchantDevice.MerchantId,
                DeviceId = merchantDevice.DeviceId,
                DeviceIdentifier = merchantDevice.DeviceIdentifier,
                IsDeleted = merchantDevice.IsDeleted
            });
        }

        return merchantDevices;
    }

    public static List<RecentContractModel> ToRecentContracts(this List<Contract> apiResultData)
    {
        List<RecentContractModel> contracts = new();

        foreach (Contract contract in apiResultData)
        {
            contracts.Add(new RecentContractModel
            {
                ContractId = contract.ContractId,
                Description = contract.Description,
                OperatorName = contract.OperatorName
            });
        }

        return contracts;
    }
    public static List<ContractDropDownModel> ToContractDropDown(this List<Contract> apiResultData)
    {
        List<ContractDropDownModel> contracts = new();

        foreach (Contract contract in apiResultData)
        {
            var c = new ContractDropDownModel
            {
                ContractId = contract.ContractId,
                Description = contract.Description,
                OperatorName = contract.OperatorName
            };
            contracts.Add(c);
        }

        return contracts;
    }
}