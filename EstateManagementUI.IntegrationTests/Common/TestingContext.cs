using EstateManagement.IntegrationTesting.Helpers;
using Reqnroll;
using SecurityService.DataTransferObjects.Responses;
using Shared.IntegrationTesting;
using Shared.Logger;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common
{
    public class TestingContext
    {
        public EstateDetails GetEstateDetails(DataTableRow tableRow, Guid? testId = null)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName").Replace("[id]", testId.Value.ToString("N"));

            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public List<Guid> GetAllEstateIds()
        {
            return this.Estates.Select(e => e.EstateId).ToList();
        }

        public EstateDetails GetEstateDetails(String estateName)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public EstateDetails GetEstateDetails(Guid estateId)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateId == estateId);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public  List<EstateDetails> Estates;
        public void AddEstateDetails(Guid estateId, String estateName, String estateReference)
        {
            this.Estates.Add(EstateDetails.Create(estateId, estateName, estateReference));
        }

        public String AccessToken { get; set; }

        public DockerHelper DockerHelper { get; set; }

        public NlogLogger Logger { get; set; }

        public Dictionary<String, Guid> Users;
        public Dictionary<String, Guid> Roles;

        public List<ClientDetails> Clients;

        public List<String> ApiResources;
        public List<String> IdentityResources;

        public TokenResponse TokenResponse;

        public TestingContext()
        {
            this.Users = new Dictionary<String, Guid>();
            this.Roles = new Dictionary<String, Guid>();
            this.Clients = new List<ClientDetails>();
            this.ApiResources = new List<String>();
            this.Estates = new List<EstateDetails>();
            this.IdentityResources = new List<String>();
        }

        public ClientDetails GetClientDetails(String clientId)
        {
            ClientDetails clientDetails = this.Clients.SingleOrDefault(c => c.ClientId == clientId);

            clientDetails.ShouldNotBeNull();

            return clientDetails;
        }

        public void AddClientDetails(String clientId,
                                     String clientSecret,
                                     List<String> grantTypes)
        {
            this.Clients.Add(ClientDetails.Create(clientId, clientSecret, grantTypes));
        }
    }
}