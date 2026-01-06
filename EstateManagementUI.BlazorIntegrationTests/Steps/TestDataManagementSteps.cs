using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Services;
using Reqnroll;
using Shouldly;

namespace EstateManagementUI.BlazorIntegrationTests.Steps;

/// <summary>
/// Example step definitions showing how to use the test data store
/// These examples demonstrate CRUD operations on test data during integration tests
/// </summary>
[Binding]
public class TestDataManagementSteps
{
    private readonly ITestDataStore _testDataStore;
    private readonly Guid _defaultEstateId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public TestDataManagementSteps(ITestDataStore testDataStore)
    {
        _testDataStore = testDataStore;
    }

    // Example: Adding a merchant dynamically during test
    [Given(@"I have added a merchant with name ""(.*)"" and reference ""(.*)""")]
    public void GivenIHaveAddedMerchant(string merchantName, string merchantReference)
    {
        var merchant = new MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = merchantName,
            MerchantReference = merchantReference,
            Balance = 0,
            AvailableBalance = 0,
            SettlementSchedule = "Immediate"
        };

        _testDataStore.AddMerchant(_defaultEstateId, merchant);
    }

    // Example: Verifying merchant exists
    [Then(@"the merchant ""(.*)"" should exist in the system")]
    public void ThenMerchantShouldExist(string merchantReference)
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        var merchant = merchants.FirstOrDefault(m => m.MerchantReference == merchantReference);
        
        merchant.ShouldNotBeNull();
    }

    // Example: Updating merchant balance
    [When(@"I update the balance of merchant ""(.*)"" to (.*)")]
    public void WhenIUpdateMerchantBalance(string merchantReference, decimal newBalance)
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        var merchant = merchants.FirstOrDefault(m => m.MerchantReference == merchantReference);
        
        merchant.ShouldNotBeNull();
        merchant.Balance = newBalance;
        
        _testDataStore.UpdateMerchant(_defaultEstateId, merchant);
    }

    // Example: Verifying merchant balance
    [Then(@"the merchant ""(.*)"" should have a balance of (.*)")]
    public void ThenMerchantShouldHaveBalance(string merchantReference, decimal expectedBalance)
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        var merchant = merchants.FirstOrDefault(m => m.MerchantReference == merchantReference);
        
        merchant.ShouldNotBeNull();
        merchant.Balance.ShouldBe(expectedBalance);
    }

    // Example: Removing a merchant
    [When(@"I remove the merchant ""(.*)""")]
    public void WhenIRemoveMerchant(string merchantReference)
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        var merchant = merchants.FirstOrDefault(m => m.MerchantReference == merchantReference);
        
        merchant.ShouldNotBeNull();
        _testDataStore.RemoveMerchant(_defaultEstateId, merchant.MerchantId);
    }

    // Example: Verifying merchant doesn't exist
    [Then(@"the merchant ""(.*)"" should not exist in the system")]
    public void ThenMerchantShouldNotExist(string merchantReference)
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        var merchant = merchants.FirstOrDefault(m => m.MerchantReference == merchantReference);
        
        merchant.ShouldBeNull();
    }

    // Example: Adding an operator
    [Given(@"I have added an operator with name ""(.*)""")]
    public void GivenIHaveAddedOperator(string operatorName)
    {
        var operatorModel = new OperatorModel
        {
            OperatorId = Guid.NewGuid(),
            Name = operatorName,
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        };

        _testDataStore.AddOperator(_defaultEstateId, operatorModel);
    }

    // Example: Verifying operator count
    [Then(@"there should be (.*) operators in the system")]
    public void ThenThereShouldBeOperators(int expectedCount)
    {
        var operators = _testDataStore.GetOperators(_defaultEstateId);
        operators.Count.ShouldBe(expectedCount);
    }

    // Example: Adding a contract
    [Given(@"I have added a contract with description ""(.*)"" for operator ""(.*)""")]
    public void GivenIHaveAddedContract(string description, string operatorName)
    {
        var operators = _testDataStore.GetOperators(_defaultEstateId);
        var operatorModel = operators.FirstOrDefault(o => o.Name == operatorName);
        
        operatorModel.ShouldNotBeNull();

        var contract = new ContractModel
        {
            ContractId = Guid.NewGuid(),
            Description = description,
            OperatorId = operatorModel.OperatorId,
            OperatorName = operatorModel.Name,
            Products = new List<ContractProductModel>()
        };

        _testDataStore.AddContract(_defaultEstateId, contract);
    }

    // Example: Verifying contract exists
    [Then(@"the contract ""(.*)"" should exist in the system")]
    public void ThenContractShouldExist(string description)
    {
        var contracts = _testDataStore.GetContracts(_defaultEstateId);
        var contract = contracts.FirstOrDefault(c => c.Description == description);
        
        contract.ShouldNotBeNull();
    }

    // Example: Resetting test data (useful in BeforeScenario hook)
    [Given(@"the system is reset to default state")]
    public void GivenSystemIsReset()
    {
        _testDataStore.Reset();
    }

    // Example: Verifying default merchants exist
    [Then(@"the default test merchants should exist")]
    public void ThenDefaultMerchantsShouldExist()
    {
        var merchants = _testDataStore.GetMerchants(_defaultEstateId);
        
        // Default setup includes 3 merchants
        merchants.Count.ShouldBeGreaterThanOrEqualTo(3);
        
        // Verify specific default merchants
        merchants.ShouldContain(m => m.MerchantReference == "MERCH001");
        merchants.ShouldContain(m => m.MerchantReference == "MERCH002");
        merchants.ShouldContain(m => m.MerchantReference == "MERCH003");
    }
}
