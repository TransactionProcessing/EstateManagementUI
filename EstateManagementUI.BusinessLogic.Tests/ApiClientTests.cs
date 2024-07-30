using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Merchant;
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

            EstateModel estate = await apiClient.GetEstate(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

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

            List<MerchantModel> merchantList = await apiClient.GetMerchants(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            merchantList.ShouldNotBeNull();
            merchantList.ShouldNotBeEmpty();
            merchantList.Count.ShouldBe(3);

            foreach (MerchantResponse merchantResponse in TestData.MerchantResponses) {
                MerchantModel? merchant = merchantList.SingleOrDefault(m => m.MerchantId == merchantResponse.MerchantId);
                merchant.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ApiClient_GetOperators_OperatorsReturned()
        {
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            estateClient.Setup(e => e.GetOperators(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestData.OperatorResponses);

            IApiClient apiClient = new ApiClient(estateClient.Object);

            List<OperatorModel> operatorsList = await apiClient.GetOperators(TestData.AccessToken, Guid.NewGuid(), TestData.EstateId, CancellationToken.None);

            operatorsList.ShouldNotBeNull();
            operatorsList.ShouldNotBeEmpty();
            operatorsList.Count.ShouldBe(2);

            foreach (OperatorResponse operatorResponse in TestData.OperatorResponses)
            {
                OperatorModel? @operatorModel = operatorsList.SingleOrDefault(m => m.OperatorId == operatorResponse.OperatorId);
                operatorModel.ShouldNotBeNull();
            }
        }
    }
}
