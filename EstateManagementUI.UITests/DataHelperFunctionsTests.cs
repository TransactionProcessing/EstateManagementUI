using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;

namespace EstateManagementUI.UITests;

public class DataHelperFunctionsTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task GetComparisonDates_ValidInputs_ReturnsComparisonDateListModel()
    {
        // Arrange
        string accessToken = "testAccessToken";
        Guid estateId = Guid.NewGuid();
        var comparisonDates = new List<ComparisonDateModel>
        {
            new ComparisonDateModel { Date = DateTime.Now, Description = "2023-01-01", OrderValue = 1 },
            new ComparisonDateModel { Date = DateTime.Now.AddDays(1), Description = "2023-01-02", OrderValue = 2 }
        };
        var result = Result.Success(comparisonDates);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetComparisonDatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var response = await DataHelperFunctions.GetComparisonDates(accessToken, estateId, this._mediatorMock.Object);

        // Assert
        response.ShouldNotBeNull();
        response.Dates.ShouldNotBeEmpty();
        response.SelectedDate.ShouldBe(comparisonDates.First().Date);
    }

    [Fact]
    public async Task GetOperators_ValidInputs_ReturnsOperatorListModel()
    {
        // Arrange
        string accessToken = "testAccessToken";
        Guid estateId = Guid.NewGuid();
        var operators = new List<OperatorModel>
        {
            new OperatorModel { OperatorId = Guid.NewGuid(), Name = "Operator1" },
            new OperatorModel { OperatorId = Guid.NewGuid(), Name = "Operator2" }
        };
        var result = Result.Success(operators);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var response = await DataHelperFunctions.GetOperators(accessToken, estateId, this._mediatorMock.Object);

        // Assert
        response.ShouldNotBeNull();
        response.Operators.ShouldNotBeEmpty();
        response.Operators.First().Text.ShouldBe("- Select an Operator -");
    }

    [Fact]
    public async Task GetMerchants_ValidInputs_ReturnsMerchantListModel()
    {
        // Arrange
        string accessToken = "testAccessToken";
        Guid estateId = Guid.NewGuid();
        var merchants = new List<MerchantModel>
        {
            new MerchantModel { MerchantId = Guid.NewGuid(), MerchantName = "Merchant1" },
            new MerchantModel { MerchantId = Guid.NewGuid(), MerchantName = "Merchant2" }
        };
        var result = Result.Success(merchants);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var response = await DataHelperFunctions.GetMerchants(accessToken, estateId, this._mediatorMock.Object);

        // Assert
        response.ShouldNotBeNull();
        response.Merchants.ShouldNotBeEmpty();
        response.Merchants.First().Text.ShouldBe("- Select a Merchant -");
    }

    [Fact]
    public async Task GetContracts_ValidInputs_ReturnsContractListModel()
    {
        // Arrange
        string accessToken = "testAccessToken";
        Guid estateId = Guid.NewGuid();
        var contracts = new List<ContractModel>
        {
            new ContractModel { ContractId = Guid.NewGuid(), Description = "Contract1" },
            new ContractModel { ContractId = Guid.NewGuid(), Description = "Contract2" }
        };
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);

        // Act
        var response = await DataHelperFunctions.GetContracts(accessToken, estateId, this._mediatorMock.Object);

        // Assert
        response.ShouldNotBeNull();
        response.Contracts.ShouldNotBeEmpty();
        response.Contracts.First().Text.ShouldBe("- Select a Contract -");
    }
}