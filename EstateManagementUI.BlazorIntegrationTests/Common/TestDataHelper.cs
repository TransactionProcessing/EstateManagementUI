using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Services;

namespace EstateManagementUI.BlazorIntegrationTests.Common;

/// <summary>
/// Helper class for accessing and manipulating test data during integration tests
/// Provides easy access to the test data store for test setup and verification
/// </summary>
public class TestDataHelper
{
    private readonly ITestDataStore _testDataStore;

    public TestDataHelper(ITestDataStore testDataStore)
    {
        _testDataStore = testDataStore;
    }

    // Estate Management
    public EstateModel GetEstate(Guid estateId) => _testDataStore.GetEstate(estateId);
    public void SetEstate(EstateModel estate) => _testDataStore.SetEstate(estate);

    // Merchant Management
    public List<MerchantModel> GetMerchants(Guid estateId) => _testDataStore.GetMerchants(estateId);
    public MerchantModel? GetMerchant(Guid estateId, Guid merchantId) => _testDataStore.GetMerchant(estateId, merchantId);
    public void AddMerchant(Guid estateId, MerchantModel merchant) => _testDataStore.AddMerchant(estateId, merchant);
    public void UpdateMerchant(Guid estateId, MerchantModel merchant) => _testDataStore.UpdateMerchant(estateId, merchant);
    public void RemoveMerchant(Guid estateId, Guid merchantId) => _testDataStore.RemoveMerchant(estateId, merchantId);

    // Operator Management
    public List<OperatorModel> GetOperators(Guid estateId) => _testDataStore.GetOperators(estateId);
    public OperatorModel? GetOperator(Guid estateId, Guid operatorId) => _testDataStore.GetOperator(estateId, operatorId);
    public void AddOperator(Guid estateId, OperatorModel operatorModel) => _testDataStore.AddOperator(estateId, operatorModel);
    public void UpdateOperator(Guid estateId, OperatorModel operatorModel) => _testDataStore.UpdateOperator(estateId, operatorModel);
    public void RemoveOperator(Guid estateId, Guid operatorId) => _testDataStore.RemoveOperator(estateId, operatorId);

    // Contract Management
    public List<ContractModel> GetContracts(Guid estateId) => _testDataStore.GetContracts(estateId);
    public ContractModel? GetContract(Guid estateId, Guid contractId) => _testDataStore.GetContract(estateId, contractId);
    public void AddContract(Guid estateId, ContractModel contract) => _testDataStore.AddContract(estateId, contract);
    public void UpdateContract(Guid estateId, ContractModel contract) => _testDataStore.UpdateContract(estateId, contract);
    public void RemoveContract(Guid estateId, Guid contractId) => _testDataStore.RemoveContract(estateId, contractId);

    // Reset all test data (for test isolation)
    public void Reset() => _testDataStore.Reset();

    // Helper methods for common test scenarios
    public Guid DefaultEstateId => Guid.Parse("11111111-1111-1111-1111-111111111111");

    public void ResetToDefaultState()
    {
        _testDataStore.Reset();
    }

    public MerchantModel CreateTestMerchant(Guid estateId, string name, string reference)
    {
        var merchant = new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = name,
            MerchantReference = reference,
            Balance = 0,
            AvailableBalance = 0,
            SettlementSchedule = "Immediate"
        };
        AddMerchant(estateId, merchant);
        return merchant;
    }

    public OperatorModel CreateTestOperator(Guid estateId, string name, bool requireCustomMerchantNumber = false, bool requireCustomTerminalNumber = false)
    {
        var operatorModel = new OperatorModel
        {
            OperatorId = Guid.NewGuid(),
            Name = name,
            RequireCustomMerchantNumber = requireCustomMerchantNumber,
            RequireCustomTerminalNumber = requireCustomTerminalNumber
        };
        AddOperator(estateId, operatorModel);
        return operatorModel;
    }

    public ContractModel CreateTestContract(Guid estateId, string description, Guid operatorId, string operatorName)
    {
        var contract = new ContractModel
        {
            ContractId = Guid.NewGuid(),
            Description = description,
            OperatorId = operatorId,
            OperatorName = operatorName,
            Products = new List<ContractProductModel>()
        };
        AddContract(estateId, contract);
        return contract;
    }
}
