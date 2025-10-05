using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Testing;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.General;
using Shouldly;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Xunit;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class PermissionsServiceTests
    {
        private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private readonly Mock<IConfigurationService> _configurationServiceMock;
        private readonly PermissionsService _permissionsService;
        
        public PermissionsServiceTests()
        {
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _configurationServiceMock = new Mock<IConfigurationService>();
            _permissionsService = new PermissionsService(_permissionsRepositoryMock.Object, this._configurationServiceMock.Object);
            
        }
        
        [Fact]
        public async Task DoIHavePermissions_BypassPermissions_ReturnsSuccess()
        {
            // Arrange
            this._configurationServiceMock.Setup(service => service.GetPermissionsBypass()).Returns(true);

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", "function");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoIHavePermissions_UserNameValidation_ReturnsForbidden(String userName)
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions(userName, "section", "function");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoIHavePermissions_SectionNameValidation_ReturnsForbidden(String sectionName)
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", sectionName, "function");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoIHavePermissions_FunctionValidation_ReturnsForbidden(String function)
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", function);

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_NoRolesAssigned_ReturnsForbidden()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", "function");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_NoFunctionsAssigned_ReturnsForbidden()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", "function");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_NoPermissions_ReturnsSuccess()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)> { ("role", "section", "function") });
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", "function1");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_PermissionGranted_ReturnsSuccess()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)> { ("role", "section", "function") });
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section", "function");

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task DoIHavePermissions_SectionOnly_BypassPermissions_ReturnsSuccess()
        {
            // Arrange
            this._configurationServiceMock.Setup(service => service.GetPermissionsBypass()).Returns(true);

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section");

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoIHavePermissions_SectionOnly_UserNameValidation_ReturnsForbidden(String userName)
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions(userName, "section");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoIHavePermissions_SectionOnly_SectionNameValidation_ReturnsForbidden(String sectionName)
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", sectionName);

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_SectionOnly_NoRolesAssigned_ReturnsForbidden()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)>());

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_SectionOnly_NoFunctionsAssigned_ReturnsForbidden()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>());
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_SectionOnly_NoPermission_ReturnsForbidden()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)>() {
                ("role", "section1", "function")
            });
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section");

            // Assert
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }

        [Fact]
        public async Task DoIHavePermissions_SectionOnly_PermissionGranted_ReturnsSuccess()
        {
            // Arrange
            _permissionsRepositoryMock.Setup(repo => repo.GetRolesFunctions()).ReturnsAsync(new List<(string, string, string)> { ("role", "section", "function") });
            _permissionsRepositoryMock.Setup(repo => repo.GetUsers(It.IsAny<CancellationToken>())).ReturnsAsync(new List<(string, string)> { ("user", "role") });

            // Act
            var result = await _permissionsService.DoIHavePermissions("user", "section");

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }
    }
}
