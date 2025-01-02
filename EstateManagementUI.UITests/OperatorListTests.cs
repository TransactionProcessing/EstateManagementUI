using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Estate.OperatorList;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class OperatorListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly OperatorList _operatorList;

    public OperatorListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._operatorList = new OperatorList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_PopulatesOperators()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);

        // Act
        await this._operatorList.MountAsync();

        // Assert
        this._operatorList.Operators.ShouldNotBeNull();
        this._operatorList.Operators.Count.ShouldBe(2);
        this._operatorList.Operators[0].Name.ShouldBe("Operator1");
        this._operatorList.Operators[1].Name.ShouldBe("Operator2");
    }
}