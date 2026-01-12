using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Services;

/// <summary>
/// Interface for managing test data in memory during integration testing
/// Provides CRUD operations with data isolation between test scenarios
/// </summary>
public interface ITestDataStore
{
    // Estate Management
    EstateModel GetEstate(Guid estateId);
    void SetEstate(EstateModel estate);
    
    // Merchant Management
    List<MerchantModel> GetMerchants(Guid estateId);
    MerchantModel? GetMerchant(Guid estateId, Guid merchantId);
    void AddMerchant(Guid estateId, MerchantModel merchant);
    void UpdateMerchant(Guid estateId, MerchantModel merchant);
    void RemoveMerchant(Guid estateId, Guid merchantId);
    
    // Operator Management
    List<OperatorModel> GetOperators(Guid estateId);
    OperatorModel? GetOperator(Guid estateId, Guid operatorId);
    void AddOperator(Guid estateId, OperatorModel operatorModel);
    void UpdateOperator(Guid estateId, OperatorModel operatorModel);
    void RemoveOperator(Guid estateId, Guid operatorId);
    
    // Contract Management
    List<ContractModel> GetContracts(Guid estateId);
    ContractModel? GetContract(Guid estateId, Guid contractId);
    void AddContract(Guid estateId, ContractModel contract);
    void UpdateContract(Guid estateId, ContractModel contract);
    void RemoveContract(Guid estateId, Guid contractId);
    
    // Reset all test data (for test isolation)
    void Reset();
}
