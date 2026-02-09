using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagementUI.BusinessLogic.Services;

/// <summary>
/// Interface for managing test data in memory during integration testing
/// Provides CRUD operations with data isolation between test scenarios
/// </summary>
public interface ITestDataStore
{
    // Estate Management
    EstateModels.EstateModel GetEstate(Guid estateId);
    void SetEstate(EstateModels.EstateModel estate);
    
    // Merchant Management
    List<MerchantModels.MerchantModel> GetMerchants(Guid estateId);
    List<MerchantModels.RecentMerchantsModel> GetRecentMerchants(Guid estateId);
    MerchantModels.MerchantModel? GetMerchant(Guid estateId, Guid merchantId);
    void AddMerchant(Guid estateId, MerchantModels.MerchantModel merchant);
    void UpdateMerchant(Guid estateId, MerchantModels.MerchantModel merchant);
    void RemoveMerchant(Guid estateId, Guid merchantId);
    
    // Operator Management
    List<OperatorModels.OperatorModel> GetOperators(Guid estateId);
    OperatorModels.OperatorModel? GetOperator(Guid estateId, Guid operatorId);
    void AddOperator(Guid estateId, OperatorModels.OperatorModel operatorModel);
    void UpdateOperator(Guid estateId, OperatorModels.OperatorModel operatorModel);
    void RemoveOperator(Guid estateId, Guid operatorId);
    
    // Contract Management
    List<ContractModels.ContractModel> GetContracts(Guid estateId);
    ContractModels.ContractModel? GetContract(Guid estateId, Guid contractId);
    void AddContract(Guid estateId, ContractModels.ContractModel contract);
    void UpdateContract(Guid estateId, ContractModels.ContractModel contract);
    void RemoveContract(Guid estateId, Guid contractId);
    
    // Reset all test data (for test isolation)
    void Reset();
}
