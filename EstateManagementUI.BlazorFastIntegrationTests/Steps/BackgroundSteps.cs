using EstateManagementUI.IntegrationTests.Common;
using Reqnroll;

namespace EstateManagementUI.BlazorFastIntegrationTests.Steps;

[Binding]
[Scope(Tag = "background")]
public sealed class BackgroundSteps
{
    private readonly TestingContext _testingContext;

    public BackgroundSteps(TestingContext testingContext)
    {
        _testingContext = testingContext;
    }

    [Given("I create the following roles")]
    [Given("I create the following api scopes")]
    [Given("I create the following api resources")]
    [Given("I create the following identity resources")]
    [Given("I create the following clients")]
    [Given("I create the following users")]
    [Given("I have created the following file profiles")]
    [Given("I have created the following merchants")]
    [Then("I have created the following file profiles")]
    [Then("I have created the following merchants")]
    [Given("I have created the following reporting operators")]
    [Given("I have created the following reporting merchant setups")]
    [Given("I have made the following reporting merchant deposits")]
    [Given("I have seeded the following reporting sales")]
    [Given("I have processed the following reporting settlements")]
    [When("I create the following roles")]
    [When("I create the following api scopes")]
    [When("I create the following api resources")]
    [When("I create the following identity resources")]
    [When("I create the following clients")]
    [When("I create the following users")]
    [When("I have created the following file profiles")]
    [When("I have created the following merchants")]
    [Then("I have created the following reporting operators")]
    [Then("I have created the following reporting merchant setups")]
    [Then("I have made the following reporting merchant deposits")]
    [Then("I have seeded the following reporting sales")]
    [Then("I have processed the following reporting settlements")]
    [When("I have created the following reporting operators")]
    [When("I have created the following reporting merchant setups")]
    [When("I have made the following reporting merchant deposits")]
    [When("I have seeded the following reporting sales")]
    [When("I have processed the following reporting settlements")]
    public Task NoOpDataSetup(DataTable table) => Task.CompletedTask;

    [Given("I have a token to access the transaction Processor resource")]
    [When("I have a token to access the transaction Processor resource")]
    public Task SetToken(DataTable table)
    {
        _testingContext.AccessToken = "test-token";
        return Task.CompletedTask;
    }

    [Given("I have created the following estates")]
    [When("I have created the following estates")]
    public Task NoOpEstates(DataTable table) => Task.CompletedTask;

    [Given("I have created the following operators")]
    [When("I have created the following operators")]
    public Task NoOpOperators(DataTable table) => Task.CompletedTask;

    [Given("I have assigned the following operators to the estates")]
    [When("I have assigned the following operators to the estates")]
    public Task NoOpAssignments(DataTable table) => Task.CompletedTask;

    [Given("I have created the following security users")]
    [When("I have created the following security users")]
    public Task NoOpSecurityUsers(DataTable table) => Task.CompletedTask;

    [When("I run the todays summary stored procedures for {string}")]
    [Given("I run the todays summary stored procedures for {string}")]
    public Task NoOpStoredProcedures(string dateValue) => Task.CompletedTask;

    [When("I run the historic summary stored procedures for {string}")]
    [Given("I run the historic summary stored procedures for {string}")]
    public Task NoOpHistoricStoredProcedures(string dateValue) => Task.CompletedTask;
}
