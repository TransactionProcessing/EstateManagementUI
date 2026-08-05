using EstateManagementUI.BlazorServer.Models;
using TransactionProcessor.DataTransferObjects.Responses.Estate;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class TestingContext
{
    public TestingContext(LocalAppHost appHost)
    {
        DockerHelper = new DockerHelper(appHost);
        Estates =
        [
            EstateDetails.Create(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Test Estate", "Test Estate")
        ];

        Clients = [];
        ApiResources = [];
        IdentityResources = [];
        Users = [];
        Roles = [];
        AccessToken = "test-token";
    }

    public DockerHelper DockerHelper { get; set; }
    public List<EstateDetails> Estates { get; }
    public string AccessToken { get; set; }
    public Dictionary<string, string> Users { get; }
    public Dictionary<string, string> Roles { get; }
    public List<ClientDetails> Clients { get; }
    public List<string> ApiResources { get; }
    public List<string> IdentityResources { get; }
    public object? TokenResponse { get; set; }

    public EstateDetails GetEstateDetails(Guid estateId) => Estates.Single(estate => estate.EstateId == estateId);
    public List<Guid> GetAllEstateIds() => Estates.Select(estate => estate.EstateId).ToList();
    public ClientDetails GetClientDetails(string clientId) => Clients.Single(client => client.ClientId == clientId);

    public void AddEstateDetails(Guid estateId, string estateName, string estateReference) => Estates.Add(EstateDetails.Create(estateId, estateName, estateReference));
    public void AddClientDetails(string clientId, string clientSecret, List<string> grantTypes) => Clients.Add(ClientDetails.Create(clientId, clientSecret, grantTypes));
}

public sealed class EstateDetails
{
    private EstateDetails(Guid estateId, string estateName, string estateReference)
    {
        EstateId = estateId;
        EstateName = estateName;
        EstateReference = estateReference;
    }

    public Guid EstateId { get; }
    public string EstateName { get; }
    public string EstateReference { get; }

    public static EstateDetails Create(Guid estateId, string estateName, string estateReference) => new(estateId, estateName, estateReference);
}

public sealed class ClientDetails
{
    private ClientDetails(string clientId, string clientSecret, List<string> grantTypes)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        GrantTypes = grantTypes;
    }

    public string ClientId { get; }
    public string ClientSecret { get; }
    public List<string> GrantTypes { get; }

    public static ClientDetails Create(string clientId, string clientSecret, List<string> grantTypes) => new(clientId, clientSecret, grantTypes);
}
