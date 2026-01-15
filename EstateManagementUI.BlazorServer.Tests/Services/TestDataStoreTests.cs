using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Services;
using Shouldly;
using EstateModel = EstateManagementUI.BusinessLogic.Models.EstateModel;
using MerchantModel = EstateManagementUI.BusinessLogic.Models.MerchantModel;
using OperatorModel = EstateManagementUI.BusinessLogic.Models.OperatorModel;

namespace EstateManagementUI.BlazorServer.Tests.Services;

public class TestDataStoreTests
{
    [Fact]
    public void Constructor_InitializesWithDefaultData()
    {
        // Act
        var dataStore = new TestDataStore();
        var testEstateId = new Guid("11111111-1111-1111-1111-111111111111");
        var estate = dataStore.GetEstate(testEstateId);
        
        // Assert
        estate.ShouldNotBeNull();
        estate.EstateName.ShouldBe("Test Estate");
    }
    
    [Fact]
    public void GetEstate_WithNonExistentId_ReturnsUnknownEstate()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var estate = dataStore.GetEstate(nonExistentId);
        
        // Assert
        estate.ShouldNotBeNull();
        estate.EstateName.ShouldBe("Unknown Estate");
        estate.Reference.ShouldBe("Unknown");
    }
    
    [Fact]
    public void SetEstate_AddsNewEstate()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var estate = new EstateModel
        {
            EstateId = estateId,
            EstateName = "New Estate",
            Reference = "NEW001"
        };
        
        // Act
        dataStore.SetEstate(estate);
        var result = dataStore.GetEstate(estateId);
        
        // Assert
        result.ShouldNotBeNull();
        result.EstateName.ShouldBe("New Estate");
        result.Reference.ShouldBe("NEW001");
    }
    
    [Fact]
    public void GetMerchants_WithExistingEstate_ReturnsMerchantList()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var testEstateId = new Guid("11111111-1111-1111-1111-111111111111");
        
        // Act
        var merchants = dataStore.GetMerchants(testEstateId);
        
        // Assert
        merchants.ShouldNotBeNull();
        merchants.ShouldNotBeEmpty();
    }
    
    [Fact]
    public void GetMerchants_WithNonExistentEstate_ReturnsEmptyList()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var merchants = dataStore.GetMerchants(nonExistentId);
        
        // Assert
        merchants.ShouldNotBeNull();
        merchants.ShouldBeEmpty();
    }
    
    [Fact]
    public void AddMerchant_AddsNewMerchant()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "MERCH001"
        };
        
        // Act
        dataStore.AddMerchant(estateId, merchant);
        var result = dataStore.GetMerchant(estateId, merchantId);
        
        // Assert
        result.ShouldNotBeNull();
        result.MerchantName.ShouldBe("Test Merchant");
        result.MerchantReference.ShouldBe("MERCH001");
    }
    
    [Fact]
    public void UpdateMerchant_UpdatesExistingMerchant()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Original Name",
            MerchantReference = "ORIG001"
        };
        dataStore.AddMerchant(estateId, merchant);
        
        // Act
        merchant.MerchantName = "Updated Name";
        dataStore.UpdateMerchant(estateId, merchant);
        var result = dataStore.GetMerchant(estateId, merchantId);
        
        // Assert
        result.ShouldNotBeNull();
        result.MerchantName.ShouldBe("Updated Name");
    }
    
    [Fact]
    public void RemoveMerchant_RemovesExistingMerchant()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "MERCH001"
        };
        dataStore.AddMerchant(estateId, merchant);
        
        // Act
        dataStore.RemoveMerchant(estateId, merchantId);
        var result = dataStore.GetMerchant(estateId, merchantId);
        
        // Assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetOperators_WithExistingEstate_ReturnsOperatorList()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var testEstateId = new Guid("11111111-1111-1111-1111-111111111111");
        
        // Act
        var operators = dataStore.GetOperators(testEstateId);
        
        // Assert
        operators.ShouldNotBeNull();
        operators.ShouldNotBeEmpty();
    }
    
    [Fact]
    public void AddOperator_AddsNewOperator()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        // Act
        dataStore.AddOperator(estateId, operatorModel);
        var result = dataStore.GetOperator(estateId, operatorId);
        
        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Test Operator");
        result.RequireCustomMerchantNumber.ShouldBeTrue();
    }
    
    [Fact]
    public void UpdateOperator_UpdatesExistingOperator()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Original Operator",
            RequireCustomMerchantNumber = false
        };
        dataStore.AddOperator(estateId, operatorModel);
        
        // Act
        operatorModel.Name = "Updated Operator";
        operatorModel.RequireCustomMerchantNumber = true;
        dataStore.UpdateOperator(estateId, operatorModel);
        var result = dataStore.GetOperator(estateId, operatorId);
        
        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Updated Operator");
        result.RequireCustomMerchantNumber.ShouldBeTrue();
    }
    
    [Fact]
    public void RemoveOperator_RemovesExistingOperator()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var operatorModel = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator"
        };
        dataStore.AddOperator(estateId, operatorModel);
        
        // Act
        dataStore.RemoveOperator(estateId, operatorId);
        var result = dataStore.GetOperator(estateId, operatorId);
        
        // Assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetContracts_WithExistingEstate_ReturnsContractList()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var testEstateId = new Guid("11111111-1111-1111-1111-111111111111");
        
        // Act
        var contracts = dataStore.GetContracts(testEstateId);
        
        // Assert
        contracts.ShouldNotBeNull();
        contracts.ShouldNotBeEmpty();
    }
    
    [Fact]
    public void Reset_ClearsAllDataAndReinitializes()
    {
        // Arrange
        var dataStore = new TestDataStore();
        var estateId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant"
        };
        dataStore.AddMerchant(estateId, merchant);
        
        // Act
        dataStore.Reset();
        var merchants = dataStore.GetMerchants(estateId);
        var testEstateId = new Guid("11111111-1111-1111-1111-111111111111");
        var defaultEstate = dataStore.GetEstate(testEstateId);
        
        // Assert
        merchants.ShouldBeEmpty();
        defaultEstate.ShouldNotBeNull();
        defaultEstate.EstateName.ShouldBe("Test Estate");
    }
}
