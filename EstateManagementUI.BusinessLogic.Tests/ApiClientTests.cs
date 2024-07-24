using EstateManagement.Client;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Testing;
using Moq;
using Shouldly;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class ApiClientTests
    {
        [Fact]
        public async Task ApiClient_GetEstate_EstateReturned() {
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.EstateResponse);

            IApiClient apiClient = new ApiClient(estateClient.Object);

            var estate = await apiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            estate.ShouldNotBeNull();
            estate.EstateName.ShouldBe(TestData.EstateName);
        }
    }
}
