using EstateManagementUI.IntegrationTests.Common;
using Reqnroll;
using SecurityService.DataTransferObjects;
using SecurityService.IntegrationTesting.Helpers;
using Shared.IntegrationTesting;
using Shouldly;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionProcessor.DataTransferObjects.Requests.Estate;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.IntegrationTesting.Helpers;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

namespace EstateManagementUI.IntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "background")]
    public class BackgroundSteps
    {
        private readonly TestingContext TestingContext;
        private readonly SecurityServiceSteps SecurityServiceSteps;
        private readonly TransactionProcessorSteps TransactionProcessorSteps;

        public BackgroundSteps(TestingContext testingContext) {
            this.TestingContext = testingContext;
            this.SecurityServiceSteps = new SecurityServiceSteps(testingContext.DockerHelper.SecurityServiceClient);
            this.TransactionProcessorSteps = new TransactionProcessorSteps(testingContext.DockerHelper.TransactionProcessorClient,
                testingContext.DockerHelper.TestHostHttpClient, testingContext.DockerHelper.ProjectionManagementClient);
        }

        [Given(@"I create the following roles")]
        public async Task GivenICreateTheFollowingRoles(DataTable table)
        {
            List<CreateRoleRequest> requests = table.Rows.ToCreateRoleRequests();
            List<(String, String)> responses = await this.SecurityServiceSteps.GivenICreateTheFollowingRoles(requests, CancellationToken.None);

            foreach ((String, String) response in responses)
            {
                this.TestingContext.Roles.Add(response.Item1, response.Item2);
            }
        }

        [Given(@"I create the following api resources")]
        public async Task GivenICreateTheFollowingApiResources(DataTable table)
        {
            List<CreateApiResourceRequest> requests = table.Rows.ToCreateApiResourceRequests();
            await this.SecurityServiceSteps.GivenTheFollowingApiResourcesExist(requests);

            foreach (CreateApiResourceRequest createApiResourceRequest in requests)
            {
                this.TestingContext.ApiResources.Add(createApiResourceRequest.Name);
            }
        }

        [Given(@"I create the following api scopes")]
        public async Task GivenICreateTheFollowingApiScopes(DataTable table)
        {
            List<CreateApiScopeRequest> requests = table.Rows.ToCreateApiScopeRequests();
            await this.SecurityServiceSteps.GivenICreateTheFollowingApiScopes(requests);
        }

        [Given(@"I create the following clients")]
        public async Task GivenICreateTheFollowingClients(DataTable table) {
            var estateManagementUiPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
            List <CreateClientRequest> requests = table.Rows.ToCreateClientRequests(this.TestingContext.DockerHelper.TestId, estateManagementUiPort);
            List<(String clientId, String secret, List<String> allowedGrantTypes)> clients = await this.SecurityServiceSteps.GivenTheFollowingClientsExist(requests);
            foreach ((String clientId, String secret, List<String> allowedGrantTypes) client in clients)
            {
                this.TestingContext.AddClientDetails(client.clientId, client.secret, client.allowedGrantTypes);
            }
        }

        [Given(@"I create the following identity resources")]
        public async Task GivenICreateTheFollowingIdentityResources(DataTable table)
        {
            foreach (DataTableRow tableRow in table.Rows)
            {
                // Get the scopes
                String userClaims = ReqnrollTableHelper.GetStringRowValue(tableRow, "UserClaims");

                CreateIdentityResourceRequest createIdentityResourceRequest = new CreateIdentityResourceRequest
                {
                    Name = ReqnrollTableHelper.GetStringRowValue(tableRow, "Name"),
                    Claims = String.IsNullOrEmpty(userClaims) ? null : userClaims.Split(",").ToList(),
                    Description = ReqnrollTableHelper.GetStringRowValue(tableRow, "Description"),
                    DisplayName = ReqnrollTableHelper.GetStringRowValue(tableRow, "DisplayName")
                };

                await this.CreateIdentityResource(createIdentityResourceRequest, CancellationToken.None).ConfigureAwait(false);
            }
        }

        [Given(@"I create the following users")]
        public async Task GivenICreateTheFollowingUsers(DataTable table)
        {
            List<CreateUserRequest> requests = table.Rows.ToCreateUserRequests();

            List<(String, String)> results = await this.SecurityServiceSteps.GivenICreateTheFollowingUsers(requests, CancellationToken.None);

            foreach ((String, String) response in results)
            {
                this.TestingContext.Users.Add(response.Item1, response.Item2);
            }
        }

        private async Task CreateIdentityResource(CreateIdentityResourceRequest createIdentityResourceRequest,
                                                  CancellationToken cancellationToken)
        {
            Result<List<IdentityResourceResponse>>? identityResourceList = await this.TestingContext.DockerHelper.SecurityServiceClient.GetIdentityResources(cancellationToken);
            if (identityResourceList.IsFailed)
            {
                // TODO: Handle error properly, e.g., show a message to the user
            }
            if (identityResourceList.Data == null || identityResourceList.Data.Any() == false)
            {
                Result result = await this
                    .TestingContext.DockerHelper.SecurityServiceClient
                    .CreateIdentityResource(createIdentityResourceRequest, cancellationToken)
                    .ConfigureAwait(false);
                result.IsSuccess.ShouldBeTrue();

                this.TestingContext.IdentityResources.Add(createIdentityResourceRequest.Name);
            }
            else
            {
                if (identityResourceList.Data.Any(i => i.Name == createIdentityResourceRequest.Name))
                {
                    return;
                }

                Result result = await this
                    .TestingContext.DockerHelper.SecurityServiceClient
                    .CreateIdentityResource(createIdentityResourceRequest, cancellationToken)
                    .ConfigureAwait(false);
                result.IsSuccess.ShouldBeTrue();

                result.IsSuccess.ShouldBeTrue();

                this.TestingContext.IdentityResources.Add(createIdentityResourceRequest.Name);
            }
        }

        [Given(@"I have a token to access the transaction Processor resource")]
        public async Task GivenIHaveATokenToAccessTheTransactionProcessorResource(DataTable table)
        {
            DataTableRow firstRow = table.Rows.First();
            String clientId = ReqnrollTableHelper.GetStringRowValue(firstRow, "ClientId").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            this.TestingContext.AccessToken = await this.SecurityServiceSteps.GetClientToken(clientDetails.ClientId, clientDetails.ClientSecret, CancellationToken.None);
        }

        [Given(@"I have created the following estates")]
        public async Task GivenIHaveCreatedTheFollowingEstates(DataTable table)
        {
            List<CreateEstateRequest> requests = table.Rows.ToCreateEstateRequests();

            List<EstateResponse> verifiedEstates = await this.TransactionProcessorSteps.WhenICreateTheFollowingEstatesX(this.TestingContext.AccessToken, requests);

            foreach (EstateResponse verifiedEstate in verifiedEstates)
            {
                //await Retry.For(async () =>
                //{
                //    String databaseName = $"EstateReportingReadModel{verifiedEstate.EstateId}";
                //    var connString = Setup.GetLocalConnectionString(databaseName);
                //    connString = $"{connString};Encrypt=false";
                //    var ctx = new EstateManagementContext(connString);

                //    var estates = ctx.Estates.ToList();
                //    estates.Count.ShouldBe(1);

                this.TestingContext.AddEstateDetails(verifiedEstate.EstateId, verifiedEstate.EstateName, verifiedEstate.EstateReference);
                this.TestingContext.Logger.LogInformation($"Estate {verifiedEstate.EstateName} created with Id {verifiedEstate.EstateId}");
                //});
            }
        }

        [Given(@"I have created the following operators")]
        public async Task GivenIHaveCreatedTheFollowingOperators(DataTable table)
        {
            List<(EstateDetails estate, CreateOperatorRequest request)> requests = table.Rows.ToCreateOperatorRequests(this.TestingContext.Estates);

            List<(Guid, EstateOperatorResponse)> results = await this.TransactionProcessorSteps.WhenICreateTheFollowingOperators(this.TestingContext.AccessToken, requests);

            foreach ((Guid, EstateOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} created with Id {result.Item2.OperatorId} for Estate {result.Item1}");
            }
        }

        [Given("I have assigned the following operators to the estates")]
        public async Task GivenIHaveAssignedTheFollowingOperatorsToTheEstates(DataTable dataTable)
        {
            List<(EstateDetails estate, AssignOperatorRequest request)> requests = dataTable.Rows.ToAssignOperatorToEstateRequests(this.TestingContext.Estates);

            await this.TransactionProcessorSteps.GivenIHaveAssignedTheFollowingOperatorsToTheEstates(this.TestingContext.AccessToken, requests);

            // TODO Verify
        }

        [When(@"I create the following security users")]
        [Given("I have created the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(DataTable table)
        {
            List<CreateNewUserRequest> createUserRequests = table.Rows.ToCreateNewUserRequests(this.TestingContext.Estates);
            await this.TransactionProcessorSteps.WhenICreateTheFollowingSecurityUsers(this.TestingContext.AccessToken, createUserRequests, this.TestingContext.Estates);
        }

    }
}
