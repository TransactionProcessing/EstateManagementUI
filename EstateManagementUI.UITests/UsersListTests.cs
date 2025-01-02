using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Estate.UsersList;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class UsersListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly UsersList _usersList;

    public UsersListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        this._usersList = new UsersList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
    }

    [Fact]
    public async Task MountAsync_PopulatesUsers()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.EstateResult);

        // Act
        await this._usersList.MountAsync();

        // Assert
        this._usersList.Users.ShouldNotBeNull();
        this._usersList.Users.Count.ShouldBe(2);
        this._usersList.Users[0].EmailAddress.ShouldBe("user1@example.com");
        this._usersList.Users[1].EmailAddress.ShouldBe("user2@example.com");
    }
}