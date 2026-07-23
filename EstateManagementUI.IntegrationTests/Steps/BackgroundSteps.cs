using EstateManagementUI.IntegrationTests.Common;
using FileProcessor.DataTransferObjects.Requests;
using Microsoft.Playwright;
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
        private readonly IPage Page;

        public BackgroundSteps(TestingContext testingContext, IPage page)
        {
            this.TestingContext = testingContext;
            this.Page = page;
            this.SecurityServiceSteps = new SecurityServiceSteps(testingContext.DockerHelper.SecurityServiceClient);
            this.TransactionProcessorSteps = new TransactionProcessorSteps(testingContext.DockerHelper.TransactionProcessorClient,
                testingContext.DockerHelper.TestHostHttpClient, testingContext.DockerHelper.ProjectionManagementClient);
        }

        private DashboardPageHelper GetHelper() => new(this.Page, this.TestingContext);

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
        public async Task GivenICreateTheFollowingClients(DataTable table)
        {
            var estateManagementUiPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
            List<CreateClientRequest> requests = table.Rows.ToCreateClientRequests(this.TestingContext.DockerHelper.TestId, estateManagementUiPort);
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

        [Given(@"I have created the following file profiles")]
        [Then(@"I have created the following file profiles")]
        [Given(@"I create the following file profiles")]
        public async Task GivenIHaveCreatedTheFollowingFileProfiles(DataTable table)
        {
            foreach (DataTableRow tableRow in table.Rows)
            {
                string lineTerminatorValue = ReqnrollTableHelper.GetStringRowValue(tableRow, "LineTerminator");

                CreateFileProfileRequest request = new()
                {
                    FileProfileId = Guid.Parse(ReqnrollTableHelper.GetStringRowValue(tableRow, "FileProfileId")),
                    Name = ReqnrollTableHelper.GetStringRowValue(tableRow, "Name"),
                    ListeningDirectory = ReqnrollTableHelper.GetStringRowValue(tableRow, "ListeningDirectory"),
                    RequestType = ReqnrollTableHelper.GetStringRowValue(tableRow, "RequestType"),
                    OperatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                    LineTerminator = Enum.TryParse(lineTerminatorValue, true, out LineTerminatorType lineTerminator)
                        ? lineTerminator
                        : null,
                    FileFormatHandler = ReqnrollTableHelper.GetStringRowValue(tableRow, "FileFormatHandler")
                };

                Result<FileProcessor.Models.FileProfile> result = await this.TestingContext.DockerHelper.FileProcessorClient.CreateFileProfile(this.TestingContext.AccessToken, request, CancellationToken.None);
                result.IsSuccess.ShouldBeTrue();
            }
        }

        [Given(@"I have created the following merchants")]
        [Then(@"I have created the following merchants")]
        public async Task GivenIHaveCreatedTheFollowingMerchants(DataTable table)
        {
            DashboardPageHelper helper = this.GetHelper();

            foreach (DataTableRow tableRow in table.Rows)
            {
                string? addressLine2 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine2");

                string merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
                string settlementSchedule = ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");
                string addressLine1 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine1");
                string town = ReqnrollTableHelper.GetStringRowValue(tableRow, "Town");
                string region = ReqnrollTableHelper.GetStringRowValue(tableRow, "Region");
                string postCode = ReqnrollTableHelper.GetStringRowValue(tableRow, "PostCode");
                string country = ReqnrollTableHelper.GetStringRowValue(tableRow, "Country");
                string contactName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContactName");
                string emailAddress = ReqnrollTableHelper.GetStringRowValue(tableRow, "EmailAddress");
                string phoneNumber = ReqnrollTableHelper.GetStringRowValue(tableRow, "PhoneNumber");

                await helper.CreateMerchantAsync(merchantName, settlementSchedule, addressLine1, addressLine2, town, region, postCode, country, contactName, emailAddress, phoneNumber);
                await helper.AssertMerchantListContainsAsync(merchantName);
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
