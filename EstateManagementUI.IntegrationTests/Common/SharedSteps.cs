﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JasperFx.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;
using SecurityService.DataTransferObjects;
using SecurityService.DataTransferObjects.Requests;
using SecurityService.DataTransferObjects.Responses;
using SecurityService.IntegrationTesting.Helpers;
using Shared.IntegrationTesting;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Database.Contexts;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Requests.Estate;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.IntegrationTesting.Helpers;
using AssignOperatorRequest = TransactionProcessor.DataTransferObjects.Requests.Estate.AssignOperatorRequest;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;

namespace EstateManagementUI.IntegrationTests.Common
{
    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        #region Fields

        

        private readonly SecurityServiceSteps SecurityServiceSteps;

        private readonly TestingContext TestingContext;

        private readonly IWebDriver WebDriver;

        private readonly TransactionProcessorSteps TransactionProcessorSteps;

        //private readonly EstateMan EstateAdministrationUiSteps;

        #endregion

        #region Constructors

        public SharedSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            IWebDriver? webDriver = scenarioContext.ScenarioContainer.Resolve<IWebDriver>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));

            this.TestingContext = testingContext;
            this.SecurityServiceSteps = new SecurityServiceSteps(this.TestingContext.DockerHelper.SecurityServiceClient);
            this.TransactionProcessorSteps = new TransactionProcessorSteps(this.TestingContext.DockerHelper.TransactionProcessorClient, this.TestingContext.DockerHelper.TestHostHttpClient, this.TestingContext.DockerHelper.ProjectionManagementClient);
            //this.EstateAdministrationUiSteps = new EstateAdministrationUISteps(webDriver, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        #endregion

        #region Methods

        

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
            List<CreateClientRequest> requests = table.Rows.ToCreateClientRequests(this.TestingContext.DockerHelper.TestId, this.TestingContext.DockerHelper.EstateManagementUiPort);
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

        [Given(@"I create the following roles")]
        public async Task GivenICreateTheFollowingRoles(DataTable table)
        {
            List<CreateRoleRequest> requests = table.Rows.ToCreateRoleRequests();
            List<(String, Guid)> responses = await this.SecurityServiceSteps.GivenICreateTheFollowingRoles(requests, CancellationToken.None);

            foreach ((String, Guid) response in responses)
            {
                this.TestingContext.Roles.Add(response.Item1, response.Item2);
            }
        }

        [Given(@"I create the following users")]
        public async Task GivenICreateTheFollowingUsers(DataTable table)
        {
            List<CreateUserRequest> requests = table.Rows.ToCreateUserRequests();
            foreach (CreateUserRequest createUserRequest in requests)
            {
                createUserRequest.EmailAddress = createUserRequest.EmailAddress.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
                //createUserRequest.Roles.ForEach(r => r.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")));
                List<String> newRoles = new List<String>();
                foreach (String role in createUserRequest.Roles)
                {
                    newRoles.Add(role.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")));
                }
                createUserRequest.Roles = newRoles;
            }

            List<(String, Guid)> results = await this.SecurityServiceSteps.GivenICreateTheFollowingUsers(requests, CancellationToken.None);

            foreach ((String, Guid) response in results)
            {
                this.TestingContext.Users.Add(response.Item1, response.Item2);
            }
        }

        [Given(@"I have a token to access the estate management resource")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementResource(DataTable table)
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
                await Retry.For(async () =>
                {
                    String databaseName = $"EstateReportingReadModel{verifiedEstate.EstateId}";
                    var connString = Setup.GetLocalConnectionString(databaseName);
                    connString = $"{connString};Encrypt=false";
                    var ctx = new EstateManagementContext(connString);

                    var estates = ctx.Estates.ToList();
                    estates.Count.ShouldBe(1);

                    this.TestingContext.AddEstateDetails(verifiedEstate.EstateId, verifiedEstate.EstateName, verifiedEstate.EstateReference);
                    this.TestingContext.Logger.LogInformation($"Estate {verifiedEstate.EstateName} created with Id {verifiedEstate.EstateId}");
                });
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

        

        [When(@"I add the following devices to the merchant")]
        public async Task WhenIAddTheFollowingDevicesToTheMerchant(DataTable table)
        {
            List<(EstateDetails, Guid, AddMerchantDeviceRequest)> requests = table.Rows.ToAddMerchantDeviceRequests(this.TestingContext.Estates);

            List<(EstateDetails, MerchantResponse, String)> results = await this.TransactionProcessorSteps.GivenIHaveAssignedTheFollowingDevicesToTheMerchants(this.TestingContext.AccessToken, requests);
            foreach ((EstateDetails, MerchantResponse, String) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Device {result.Item3} assigned to Merchant {result.Item2.MerchantName} Estate {result.Item1.EstateName}");
            }
        }

        [When(@"I assign the following  operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(DataTable table)
        {
            List<(EstateDetails, Guid, TransactionProcessor.DataTransferObjects.Requests.Merchant.AssignOperatorRequest)> requests = table.Rows.ToAssignOperatorRequests(this.TestingContext.Estates);

            List<(EstateDetails, TransactionProcessor.DataTransferObjects.Responses.Merchant.MerchantOperatorResponse)> results = await this.TransactionProcessorSteps.WhenIAssignTheFollowingOperatorToTheMerchants(this.TestingContext.AccessToken, requests);

            foreach ((EstateDetails, MerchantOperatorResponse) result in results)
            {
                this.TestingContext.Logger.LogInformation($"Operator {result.Item2.Name} assigned to Estate {result.Item1.EstateName}");
            }
        }

        

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(DataTable table)
        {
            List<(EstateDetails estate, CreateMerchantRequest)> requests = table.Rows.ToCreateMerchantRequests(this.TestingContext.Estates);

            List<MerchantResponse> verifiedMerchants = await this.TransactionProcessorSteps.WhenICreateTheFollowingMerchants(this.TestingContext.AccessToken, requests);

            foreach (MerchantResponse verifiedMerchant in verifiedMerchants)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(verifiedMerchant.EstateId);
                estateDetails.AddMerchant(verifiedMerchant);
                this.TestingContext.Logger.LogInformation($"Merchant {verifiedMerchant.MerchantName} created with Id {verifiedMerchant.MerchantId} for Estate {estateDetails.EstateName}");
            }
        }

        [When(@"I create the following security users")]
        [Given("I have created the following security users")]
        public async Task WhenICreateTheFollowingSecurityUsers(DataTable table)
        {
            List<CreateNewUserRequest> createUserRequests = table.Rows.ToCreateNewUserRequests(this.TestingContext.Estates);
            await this.TransactionProcessorSteps.WhenICreateTheFollowingSecurityUsers(this.TestingContext.AccessToken, createUserRequests, this.TestingContext.Estates);
        }

        [Given(@"I have created the following contracts")]
        public async Task GivenIHaveCreatedTheFollowingContracts(DataTable table)
        {
            List<(EstateDetails, CreateContractRequest)> requests = table.Rows.ToCreateContractRequests(this.TestingContext.Estates);

            List<ContractResponse> responses = await this.TransactionProcessorSteps.GivenICreateAContractWithTheFollowingValues(this.TestingContext.AccessToken, requests);
            
            foreach (ContractResponse contractResponse in responses)
            {
                this.TestingContext.Logger.LogInformation($"Contract {contractResponse.Description} created with Id {contractResponse.ContractId} for Estate {contractResponse.EstateId}");
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(contractResponse.EstateId);
                estateDetails.AddContract(contractResponse.ContractId, contractResponse.Description, contractResponse.OperatorId);
            }
        }

        [Given("I have created the following contract products")]
        public async Task GivenIHaveCreatedTheFollowingContractProducts(DataTable table)
        {
            List<(EstateDetails, Contract, AddProductToContractRequest)> requests = table.Rows.ToAddProductToContractRequest(this.TestingContext.Estates);
            await this.TransactionProcessorSteps.WhenICreateTheFollowingProducts(this.TestingContext.AccessToken, requests);
        }


        private async Task CreateIdentityResource(CreateIdentityResourceRequest createIdentityResourceRequest,
                                                  CancellationToken cancellationToken)
        {
            CreateIdentityResourceResponse createIdentityResourceResponse = null;

            Result<List<IdentityResourceDetails>> identityResourceList = await this.TestingContext.DockerHelper.SecurityServiceClient.GetIdentityResources(cancellationToken);
            if (identityResourceList.IsFailed)
            {
                // TODO: Handle error properly, e.g., show a message to the user
            }
            if (identityResourceList.Data == null || identityResourceList.Data.Any() == false)
            {
                Result result= await this
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
        
        #endregion
    }
}