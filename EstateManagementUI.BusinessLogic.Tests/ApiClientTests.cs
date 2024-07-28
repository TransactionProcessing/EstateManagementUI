using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses.Merchant;
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

        [Fact]
        public async Task ApiClient_GetMerchants_MerchantsReturned()
        {
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            estateClient.Setup(e => e.GetMerchants(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.MerchantResponses);

            IApiClient apiClient = new ApiClient(estateClient.Object);

            var merchantList = await apiClient.GetMerchants(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.Count.ShouldBe(3);

            foreach (MerchantResponse merchantResponse in TestData.MerchantResponses) {
                var merchant = merchantList.SingleOrDefault(m => m.MerchantId == merchantResponse.MerchantId);
                merchant.ShouldNotBeNull();
            }
        }
    }
}
