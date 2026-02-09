using EstateManagementUI.BusinessLogic.Models;
using System.Collections.Concurrent;

namespace EstateManagementUI.BusinessLogic.Services;

/// <summary>
/// In-memory test data store implementation
/// Thread-safe storage for test data with CRUD operations
/// </summary>
public class TestDataStore : ITestDataStore
{
    private readonly ConcurrentDictionary<Guid, EstateModels.EstateModel> _estates = new();
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, MerchantModels.MerchantModel>> _merchants = new();
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, OperatorModels.OperatorModel>> _operators = new();
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, ContractModels.ContractModel>> _contracts = new();

    public TestDataStore()
    {
        // Initialize with default test data
        this.InitializeDefaultData();
    }

    public EstateModels.EstateModel GetEstate(Guid estateId)
    {
        if (this._estates.TryGetValue(estateId, out var estate))
        {
            // Populate operators list
            if (this._operators.TryGetValue(estateId, out var operatorDict))
            {
                estate.Operators = operatorDict.Values.Select(o => new EstateModels.EstateOperatorModel
                {
                    OperatorId = o.OperatorId,
                    Name = o.Name,
                    RequireCustomMerchantNumber = o.RequireCustomMerchantNumber,
                    RequireCustomTerminalNumber = o.RequireCustomTerminalNumber
                }).ToList();
            }
            if (this._contracts.TryGetValue(estateId, out var contractDict))
            {
                estate.Contracts = contractDict.Values.Select(o => new EstateModels.EstateContractModel()
                {
                    OperatorId = o.OperatorId,
                    Name = o.Description,
                    OperatorName = o.OperatorName,
                    ContractId = o.ContractId
                }).ToList();
            }
            if (this._merchants.TryGetValue(estateId, out var merchantDict))
            {
                estate.Merchants = merchantDict.Values.Select(o => new EstateModels.EstateMerchantModel()
                {
                    Name = o.MerchantName,
                    Reference = o.MerchantReference,
                    MerchantId = o.MerchantId
                }).ToList();
            }

            estate.Users = [new EstateModels.EstateUserModel { CreatedDateTime = DateTime.Now, EmailAddress = "estatedevuser1@estate.co.uk", UserId = Guid.Parse("61BFC7DC-FDB4-408A-9CAA-B9F0CF690130") }];

            return estate;
        }
        
        // Return a default estate if not found for consistency
        return new EstateModels.EstateModel
        {
            EstateId = estateId,
            EstateName = "Unknown Estate",
            Reference = "Unknown"
        };
    }

    public void SetEstate(EstateModels.EstateModel estate)
    {
        this._estates[estate.EstateId] = estate;
        
        // Ensure collections exist for this estate
        this._merchants.TryAdd(estate.EstateId, new ConcurrentDictionary<Guid, MerchantModels.MerchantModel>());
        this._operators.TryAdd(estate.EstateId, new ConcurrentDictionary<Guid, OperatorModels.OperatorModel>());
        this._contracts.TryAdd(estate.EstateId, new ConcurrentDictionary<Guid, ContractModels.ContractModel>());
    }

    public List<MerchantModels.MerchantModel> GetMerchants(Guid estateId)
    {
        if (this._merchants.TryGetValue(estateId, out var merchantDict))
        {
            return merchantDict.Values.ToList();
        }
        return new List<MerchantModels.MerchantModel>();
    }

    public List<MerchantModels.RecentMerchantsModel> GetRecentMerchants(Guid estateId)
    {
        if (this._merchants.TryGetValue(estateId, out var merchantDict))
        {
            return merchantDict.Values.Select(m => new MerchantModels.RecentMerchantsModel {
                CreatedDateTime = m.CreatedDateTime,
                Reference = m.MerchantReference,
                Name = m.MerchantName,
                MerchantId = m.MerchantId
            }).ToList();
        }
        return new List<MerchantModels.RecentMerchantsModel>();
    }

    public MerchantModels.MerchantModel? GetMerchant(Guid estateId, Guid merchantId)
    {
        if (this._merchants.TryGetValue(estateId, out var merchantDict))
        {
            merchantDict.TryGetValue(merchantId, out var merchant);
            return merchant;
        }
        return null;
    }

    public void AddMerchant(Guid estateId, MerchantModels.MerchantModel merchant)
    {
        var merchantDict = this._merchants.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, MerchantModels.MerchantModel>());
        merchantDict[merchant.MerchantId] = merchant;
    }

    public void UpdateMerchant(Guid estateId, MerchantModels.MerchantModel merchant)
    {
        var merchantDict = this._merchants.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, MerchantModels.MerchantModel>());
        merchantDict[merchant.MerchantId] = merchant;
    }

    public void RemoveMerchant(Guid estateId, Guid merchantId)
    {
        if (this._merchants.TryGetValue(estateId, out var merchantDict))
        {
            merchantDict.TryRemove(merchantId, out _);
        }
    }

    public List<OperatorModels.OperatorModel> GetOperators(Guid estateId)
    {
        if (this._operators.TryGetValue(estateId, out var operatorDict))
        {
            return operatorDict.Values.ToList();
        }
        return new List<OperatorModels.OperatorModel>();
    }

    public OperatorModels.OperatorModel? GetOperator(Guid estateId, Guid operatorId)
    {
        if (this._operators.TryGetValue(estateId, out var operatorDict))
        {
            operatorDict.TryGetValue(operatorId, out var operatorModel);
            return operatorModel;
        }
        return null;
    }

    public void AddOperator(Guid estateId, OperatorModels.OperatorModel operatorModel)
    {
        var operatorDict = this._operators.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, OperatorModels.OperatorModel>());
        operatorDict[operatorModel.OperatorId] = operatorModel;
    }

    public void UpdateOperator(Guid estateId, OperatorModels.OperatorModel operatorModel)
    {
        var operatorDict = this._operators.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, OperatorModels.OperatorModel>());
        operatorDict[operatorModel.OperatorId] = operatorModel;
    }

    public void RemoveOperator(Guid estateId, Guid operatorId)
    {
        if (this._operators.TryGetValue(estateId, out var operatorDict))
        {
            operatorDict.TryRemove(operatorId, out _);
        }
    }

    public List<ContractModels.ContractModel> GetContracts(Guid estateId)
    {
        if (this._contracts.TryGetValue(estateId, out var contractDict))
        {
            return contractDict.Values.ToList();
        }
        return new List<ContractModels.ContractModel>();
    }

    public ContractModels.ContractModel? GetContract(Guid estateId, Guid contractId)
    {
        if (this._contracts.TryGetValue(estateId, out var contractDict))
        {
            contractDict.TryGetValue(contractId, out var contract);
            return contract;
        }
        return null;
    }

    public void AddContract(Guid estateId, ContractModels.ContractModel contract)
    {
        var contractDict = this._contracts.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, ContractModels.ContractModel>());
        contractDict[contract.ContractId] = contract;
    }

    public void UpdateContract(Guid estateId, ContractModels.ContractModel contract)
    {
        var contractDict = this._contracts.GetOrAdd(estateId, _ => new ConcurrentDictionary<Guid, ContractModels.ContractModel>());
        contractDict[contract.ContractId] = contract;
    }

    public void RemoveContract(Guid estateId, Guid contractId)
    {
        if (this._contracts.TryGetValue(estateId, out var contractDict))
        {
            contractDict.TryRemove(contractId, out _);
        }
    }

    public void Reset()
    {
        this._estates.Clear();
        this._merchants.Clear();
        this._operators.Clear();
        this._contracts.Clear();
        
        // Reinitialize with default data
        this.InitializeDefaultData();
    }

    private void InitializeDefaultData()
    {
        // Default test estate
        var estateId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var estate = new EstateModels.EstateModel
        {
            EstateId = estateId,
            EstateName = "Test Estate",
            Reference = "Test Estate",
        };
        this.SetEstate(estate);

        // Default test merchants
        this.AddMerchant(estateId, new MerchantModels.MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MerchantName = "Test Merchant 1",
            MerchantReference = "MERCH001",
            Balance = 10000.00m,
            AvailableBalance = 8500.00m,
            SettlementSchedule = "Immediate",
            AddressLine1 = "123 Main Street",
            Town = "Test Town",
            Region = "Test Region",
            PostalCode = "12345",
            Country = "Test Country",
            ContactName = "John Smith",
            ContactEmailAddress = "john@testmerchant.com",
            ContactPhoneNumber = "555-1234",
            CreatedDateTime = DateTime.Now
        });

        this.AddMerchant(estateId, new MerchantModels.MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222223"),
            MerchantName = "Test Merchant 2",
            MerchantReference = "MERCH002",
            Balance = 5000.00m,
            AvailableBalance = 4200.00m,
            SettlementSchedule = "Weekly",
            CreatedDateTime = DateTime.Now.AddDays(-1)
        });

        this.AddMerchant(estateId, new MerchantModels.MerchantModel
        {
            MerchantId = Guid.Parse("22222222-2222-2222-2222-222222222224"),
            MerchantName = "Test Merchant 3",
            MerchantReference = "MERCH003",
            Balance = 15000.00m,
            AvailableBalance = 12000.00m,
            SettlementSchedule = "Monthly",
            CreatedDateTime = DateTime.Now.AddDays(-5)
        });

        // Default test operators
        this.AddOperator(estateId, new OperatorModels.OperatorModel
        {
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Safaricom",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        });

        this.AddOperator(estateId, new OperatorModels.OperatorModel
        {
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
            Name = "Voucher",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        });

        // Default test contracts
        this.AddContract(estateId, new ContractModels.ContractModel
        {
            ContractId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Description = "Standard Transaction Contract",
            OperatorName = "Safaricom",
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
                {
                    ContractProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ProductName = "Mobile Topup",
                    DisplayText = "Mobile Airtime",
                    ProductType = "Mobile Topup",
                    Value = "Variable",
                    NumberOfFees = 2,
                    TransactionFees = new List<ContractModels.ContractProductTransactionFeeModel>
                    {
                        new ContractModels.ContractProductTransactionFeeModel
                        {
                            TransactionFeeId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                            Description = "Merchant Commission",
                            CalculationType = 0,
                            FeeType = 0,
                            Value = 0.50m
                        },
                        new ContractModels.ContractProductTransactionFeeModel
                        {
                            TransactionFeeId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                            Description = "Service Provider Fee",
                            CalculationType = 1,
                            FeeType = 1,
                            Value = 2.5m
                        }
                    }
                }
            }
        });

        this.AddContract(estateId, new ContractModels.ContractModel
        {
            ContractId = Guid.Parse("44444444-4444-4444-4444-444444444445"),
            Description = "Voucher Sales Contract",
            OperatorName = "Voucher",
            OperatorId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
            Products = new List<ContractModels.ContractProductModel>
            {
                new ContractModels.ContractProductModel
                {
                    ContractProductId = Guid.Parse("55555555-5555-5555-5555-555555555556"),
                    ProductName = "Voucher",
                    DisplayText = "Voucher Purchase"
                }
            }
        });
    }
}
