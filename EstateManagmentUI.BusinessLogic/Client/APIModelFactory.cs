using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using ContractProductTransactionFee = EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects.ContractProductTransactionFee;

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

    public static MerchantModels.MerchantKpiModel ConvertFrom(MerchantKpi apiResult) {
        MerchantModels.MerchantKpiModel model = new MerchantModels.MerchantKpiModel {
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

    

    public static EstateModels.EstateModel ConvertFrom(Estate apiResultData) {
        EstateModels.EstateModel model = new() {
            Reference = apiResultData.Reference,
            EstateId = apiResultData.EstateId,
            EstateName = apiResultData.EstateName,
            Operators = new(),
            Users = new(),
            Contracts = new(),
            Merchants = new ()
        };

        foreach (var estateOperator in apiResultData.Operators) {
            model.Operators.Add(new EstateModels.EstateOperatorModel {
                Name = estateOperator.Name,
                OperatorId = estateOperator.OperatorId,
                RequireCustomMerchantNumber = estateOperator.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperator.RequireCustomTerminalNumber,
            });
        }
        foreach (var estateMerchant in apiResultData.Merchants)
        {
            model.Merchants.Add(new EstateModels.EstateMerchantModel()
            {
                Name = estateMerchant.Name,
                Reference = estateMerchant.Reference,
                MerchantId = estateMerchant.MerchantId
            });
        }
        foreach (var estateContract in apiResultData.Contracts)
        {
            model.Contracts.Add(new EstateModels.EstateContractModel()
            {
                Name = estateContract.Name,
                OperatorId = estateContract.OperatorId,
                ContractId = estateContract.ContractId,
                OperatorName = estateContract.OperatorName
            });
        }
        foreach (var estateUser in apiResultData.Users)
        {
            model.Users.Add(new EstateModels.EstateUserModel()
            {
                CreatedDateTime = estateUser.CreatedDateTime,
                EmailAddress = estateUser.EmailAddress,
                UserId = estateUser.UserId
            });
        }
        

        return model;
    }

    public static TransactionModels.TransactionDetailReportResponse ConvertFrom(TransactionDetailReportResponse apiResultData) {
        TransactionModels.TransactionDetailReportResponse model = new();

        model.Summary = new TransactionDetailSummary { TotalFees = apiResultData.Summary.TotalFees, TotalValue = apiResultData.Summary.TotalValue, TransactionCount = apiResultData.Summary.TransactionCount };
        model.Transactions = new();

        foreach (TransactionDetail transaction in apiResultData.Transactions) {
            model.Transactions.Add(new TransactionModels.TransactionDetail {
                Id = transaction.Id,
                DateTime = transaction.DateTime,
                Merchant = transaction.Merchant,
                MerchantId = transaction.MerchantId,
                MerchantReportingId = transaction.MerchantReportingId,
                Operator = transaction.Operator,
                OperatorId = transaction.OperatorId,
                OperatorReportingId = transaction.OperatorReportingId,
                Product = transaction.Product,
                ProductId = transaction.ProductId,
                ProductReportingId = transaction.ProductReportingId,
                Type = transaction.Type,
                Status = transaction.Status,
                Value = transaction.Value,
                TotalFees = transaction.TotalFees,
                SettlementReference = transaction.SettlementReference
            });
        }
        return model;
    }
}

public  static class FactoryExtensions{

    public static OperatorModels.OperatorModel ToOperator(this Operator apiResultData) {
        OperatorModels.OperatorModel model = new() {
            Name = apiResultData.Name,
            OperatorId = apiResultData.OperatorId,
            RequireCustomMerchantNumber = apiResultData.RequireCustomMerchantNumber,
            RequireCustomTerminalNumber = apiResultData.RequireCustomTerminalNumber
        };
        return model;
    }

    public static OperatorModels.OperatorDropDownModel ToOperatorDropDown(this Operator apiResultData)
    {
        OperatorModels.OperatorDropDownModel model = new()
        {
            OperatorName= apiResultData.Name,
            OperatorId = apiResultData.OperatorId
        };
        return model;
    }

    public static List<OperatorModels.OperatorDropDownModel> ToOperatorDropDown(this List<Operator> apiResultData)
    {
        List<OperatorModels.OperatorDropDownModel> operators = new();
        foreach (Operator op in apiResultData) {
            operators.Add(op.ToOperatorDropDown());
        }

        return operators;
    }

    public static List<OperatorModels.OperatorModel> ToOperator(this List<Operator> apiResultData)
    {
        List<OperatorModels.OperatorModel> operators = new();
        foreach (Operator op in apiResultData)
        {
            operators.Add(op.ToOperator());
        }

        return operators;
    }

    public static List<OperatorModels.OperatorModel> ToOperator(this List<EstateOperator> apiResultData)
    {
        List<OperatorModels.OperatorModel> operators = new();
        foreach (EstateOperator estateOperator in apiResultData)
        {
            operators.Add(new OperatorModels.OperatorModel()
            {
                Name = estateOperator.Name,
                OperatorId = estateOperator.OperatorId,
                RequireCustomMerchantNumber = estateOperator.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperator.RequireCustomTerminalNumber
            });
        }
        return operators;
    }

    public static List<MerchantModels.RecentMerchantsModel> ToRecentMerchant(this List<Merchant> apiResultData)
    {
        List<MerchantModels.RecentMerchantsModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new MerchantModels.RecentMerchantsModel
            {
                CreatedDateTime = merchant.CreatedDateTime,
                MerchantId = merchant.MerchantId,
                Name = merchant.Name,
                Reference = merchant.Reference,
            });
        }

        return merchants;
    }

    public static List<MerchantModels.MerchantListModel> ToMerchantList(this List<Merchant> apiResultData)
    {
        List<MerchantModels.MerchantListModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new MerchantModels.MerchantListModel
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

    public static List<MerchantModels.MerchantDropDownModel> ToMerchantDropDown(this List<Merchant> apiResultData)
    {
        List<MerchantModels.MerchantDropDownModel> merchants = new();

        foreach (Merchant merchant in apiResultData)
        {
            merchants.Add(new MerchantModels.MerchantDropDownModel
            {
                MerchantId = merchant.MerchantId,
                MerchantName = merchant.Name,
            });
        }

        return merchants;
    }

    public static MerchantModels.MerchantModel ToMerchant(this Merchant apiResultData) {
        MerchantModels.MerchantModel model = new MerchantModels.MerchantModel {
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

    public static List<MerchantModels.MerchantOperatorModel> ToMerchantOperators(this List<MerchantOperator> apiResultData)
    {
        List<MerchantModels.MerchantOperatorModel> merchantOperators = new();
        
        foreach (MerchantOperator merchantOperator in apiResultData)
        {
            merchantOperators.Add(new MerchantModels.MerchantOperatorModel
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

    public static List<MerchantModels.MerchantContractModel> ToMerchantContracts(this List<MerchantContract> apiResultData)
    {
        List<MerchantModels.MerchantContractModel> merchantContracts = new();

        foreach (MerchantContract merchantContract in apiResultData) {
            MerchantModels.MerchantContractModel c = new() {
                ContractId = merchantContract.ContractId,
                ContractName = merchantContract.ContractName,
                OperatorName = merchantContract.OperatorName,
                ContractProducts = new List<MerchantModels.MerchantContractProductModel>(),
                IsDeleted = merchantContract.IsDeleted,
                MerchantId = merchantContract.MerchantId
            };

            foreach (MerchantContractProduct merchantContractContractProduct in merchantContract.ContractProducts) {
                c.ContractProducts.Add(new MerchantModels.MerchantContractProductModel
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

    public static List<MerchantModels.MerchantDeviceModel> ToMerchantDevices(this List<MerchantDevice> apiResultData)
    {
        List<MerchantModels.MerchantDeviceModel> merchantDevices= new();

        foreach (MerchantDevice merchantDevice in apiResultData)
        {
            merchantDevices.Add(new MerchantModels.MerchantDeviceModel
            {
                MerchantId = merchantDevice.MerchantId,
                DeviceId = merchantDevice.DeviceId,
                DeviceIdentifier = merchantDevice.DeviceIdentifier,
                IsDeleted = merchantDevice.IsDeleted
            });
        }

        return merchantDevices;
    }

    public static List<ContractModels.RecentContractModel> ToRecentContracts(this List<Contract> apiResultData)
    {
        List<ContractModels.RecentContractModel> contracts = new();

        foreach (Contract contract in apiResultData)
        {
            contracts.Add(new ContractModels.RecentContractModel
            {
                ContractId = contract.ContractId,
                Description = contract.Description,
                OperatorName = contract.OperatorName
            });
        }

        return contracts;
    }
    public static List<ContractModels.ContractDropDownModel> ToContractDropDown(this List<Contract> apiResultData)
    {
        List<ContractModels.ContractDropDownModel> contracts = new();

        foreach (Contract contract in apiResultData)
        {
            var c = new ContractModels.ContractDropDownModel
            {
                ContractId = contract.ContractId,
                Description = contract.Description,
                OperatorName = contract.OperatorName
            };
            contracts.Add(c);
        }

        return contracts;
    }

    public static List<ContractModels.ContractModel> ToContract(this List<Contract> apiResultData)
    {
        List<ContractModels.ContractModel> contracts = new();

        foreach (Contract contract in apiResultData) {
            contracts.Add(contract.ToContract());
        }

        return contracts;
    }

    public static ContractModels.ContractModel ToContract(this Contract apiResultData) {
        var contract = new ContractModels.ContractModel {
            ContractId = apiResultData.ContractId,
            Description = apiResultData.Description,
            OperatorName = apiResultData.OperatorName,
            OperatorId = apiResultData.OperatorId,
            Products = new List<ContractModels.ContractProductModel>()
        };

        foreach (var contractProduct in apiResultData.Products) {
            var cp = new ContractModels.ContractProductModel {
                ProductType = ((ProductType)contractProduct.ProductType).ToString(),
                Value = contractProduct.Value.HasValue ? contractProduct.Value.Value.ToString("F2") : "Variable",
                DisplayText = contractProduct.DisplayText,
                ProductName = contractProduct.ProductName,
                ContractProductId = contractProduct.ProductId,
                NumberOfFees = contractProduct.TransactionFees.Count,
                TransactionFees = new List<ContractModels.ContractProductTransactionFeeModel>()
            };

            foreach (ContractProductTransactionFee contractProductTransactionFee in contractProduct.TransactionFees) {
                cp.TransactionFees.Add(new ContractModels.ContractProductTransactionFeeModel {
                    Value = contractProductTransactionFee.Value,
                    Description = contractProductTransactionFee.Description,
                    CalculationType = contractProductTransactionFee.CalculationType,
                    FeeType = contractProductTransactionFee.FeeType,
                    TransactionFeeId = contractProductTransactionFee.TransactionFeeId
                });
            }

            contract.Products.Add(cp);
        }

        return contract;
    }
}