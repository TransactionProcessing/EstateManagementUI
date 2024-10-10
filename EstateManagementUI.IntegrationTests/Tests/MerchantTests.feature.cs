﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EstateManagementUI.IntegrationTests.Tests
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Merchant Tests")]
    [NUnit.Framework.CategoryAttribute("base")]
    [NUnit.Framework.CategoryAttribute("shared")]
    [NUnit.Framework.CategoryAttribute("uigeneral")]
    public partial class MerchantTestsFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared",
                "uigeneral"};
        
#line 1 "MerchantTests.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, NUnit.Framework.TestContext.CurrentContext.WorkerId);
            global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Tests", "Merchant Tests", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 4
#line hidden
            global::Reqnroll.Table table26 = new global::Reqnroll.Table(new string[] {
                        "Role Name"});
            table26.AddRow(new string[] {
                        "Estate"});
            table26.AddRow(new string[] {
                        "Merchant"});
#line 6
 await testRunner.GivenAsync("I create the following roles", ((string)(null)), table26, "Given ");
#line hidden
            global::Reqnroll.Table table27 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table27.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table27.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST Scope",
                        "Scope for Transaction Processor REST"});
            table27.AddRow(new string[] {
                        "fileProcessor",
                        "File Processor REST Scope",
                        "Scope for File Processor REST"});
#line 11
 await testRunner.GivenAsync("I create the following api scopes", ((string)(null)), table27, "Given ");
#line hidden
            global::Reqnroll.Table table28 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table28.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "merchantId,estateId,role"});
            table28.AddRow(new string[] {
                        "transactionProcessor",
                        "Transaction Processor REST",
                        "Secret1",
                        "transactionProcessor",
                        "merchantId,estateId,role"});
            table28.AddRow(new string[] {
                        "fileProcessor",
                        "File Processor REST",
                        "Secret1",
                        "fileProcessor",
                        "merchantId,estateId,role"});
#line 17
 await testRunner.GivenAsync("I create the following api resources", ((string)(null)), table28, "Given ");
#line hidden
            global::Reqnroll.Table table29 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description",
                        "UserClaims"});
            table29.AddRow(new string[] {
                        "openid",
                        "Your user identifier",
                        "",
                        "sub"});
            table29.AddRow(new string[] {
                        "profile",
                        "User profile",
                        "Your user profile information (first name, last name, etc.)",
                        "name,role,email,given_name,middle_name,family_name,estateId,merchantId"});
            table29.AddRow(new string[] {
                        "email",
                        "Email",
                        "Email and Email Verified Flags",
                        "email_verified,email"});
#line 23
 await testRunner.GivenAsync("I create the following identity resources", ((string)(null)), table29, "Given ");
#line hidden
            global::Reqnroll.Table table30 = new global::Reqnroll.Table(new string[] {
                        "ClientId",
                        "Name",
                        "Secret",
                        "Scopes",
                        "GrantTypes",
                        "RedirectUris",
                        "PostLogoutRedirectUris",
                        "RequireConsent",
                        "AllowOfflineAccess",
                        "ClientUri"});
            table30.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,transactionProcessor",
                        "client_credentials",
                        "",
                        "",
                        "",
                        "",
                        ""});
            table30.AddRow(new string[] {
                        "estateUIClient",
                        "Merchant Client",
                        "Secret1",
                        "estateManagement,fileProcessor,transactionProcessor,openid,email,profile",
                        "hybrid",
                        "https://localhost:[port]/signin-oidc",
                        "https://localhost:[port]/signout-oidc",
                        "false",
                        "true",
                        "https://[url]:[port]"});
#line 29
 await testRunner.GivenAsync("I create the following clients", ((string)(null)), table30, "Given ");
#line hidden
            global::Reqnroll.Table table31 = new global::Reqnroll.Table(new string[] {
                        "ClientId"});
            table31.AddRow(new string[] {
                        "serviceClient"});
#line 34
 await testRunner.GivenAsync("I have a token to access the estate management resource", ((string)(null)), table31, "Given ");
#line hidden
            global::Reqnroll.Table table32 = new global::Reqnroll.Table(new string[] {
                        "EstateName"});
            table32.AddRow(new string[] {
                        "Test Estate"});
#line 38
 await testRunner.GivenAsync("I have created the following estates", ((string)(null)), table32, "Given ");
#line hidden
            global::Reqnroll.Table table33 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table33.AddRow(new string[] {
                        "Test Estate",
                        "Test Operator",
                        "True",
                        "True"});
#line 42
 await testRunner.AndAsync("I have created the following operators", ((string)(null)), table33, "And ");
#line hidden
            global::Reqnroll.Table table34 = new global::Reqnroll.Table(new string[] {
                        "EstateName",
                        "OperatorName"});
            table34.AddRow(new string[] {
                        "Test Estate",
                        "Test Operator"});
#line 46
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table34, "And ");
#line hidden
            global::Reqnroll.Table table35 = new global::Reqnroll.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName"});
            table35.AddRow(new string[] {
                        "estateuser@testestate1.co.uk",
                        "123456",
                        "TestEstate",
                        "User1",
                        "Test Estate"});
#line 50
 await testRunner.AndAsync("I have created the following security users", ((string)(null)), table35, "And ");
#line hidden
            global::Reqnroll.Table table36 = new global::Reqnroll.Table(new string[] {
                        "MerchantName",
                        "SettlementSchedule",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName"});
            table36.AddRow(new string[] {
                        "Test Merchant 1",
                        "Immediate",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate"});
            table36.AddRow(new string[] {
                        "Test Merchant 2",
                        "Weekly",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant2.co.uk",
                        "Test Estate"});
            table36.AddRow(new string[] {
                        "Test Merchant 3",
                        "Monthly",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant3.co.uk",
                        "Test Estate"});
#line 54
 await testRunner.GivenAsync("I create the following merchants", ((string)(null)), table36, "Given ");
#line hidden
            global::Reqnroll.Table table37 = new global::Reqnroll.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table37.AddRow(new string[] {
                        "Test Operator",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate"});
            table37.AddRow(new string[] {
                        "Test Operator",
                        "Test Merchant 2",
                        "00000001",
                        "10000001",
                        "Test Estate"});
            table37.AddRow(new string[] {
                        "Test Operator",
                        "Test Merchant 3",
                        "00000001",
                        "10000001",
                        "Test Estate"});
#line 60
 await testRunner.WhenAsync("I assign the following  operator to the merchants", ((string)(null)), table37, "When ");
#line hidden
            global::Reqnroll.Table table38 = new global::Reqnroll.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "MerchantName",
                        "EstateName"});
            table38.AddRow(new string[] {
                        "merchantuser1@testmerchant1.co.uk",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Merchant 1",
                        "Test Estate"});
            table38.AddRow(new string[] {
                        "merchantuser1@testmerchant2.co.uk",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Merchant 2",
                        "Test Estate"});
            table38.AddRow(new string[] {
                        "merchantuser1@testmerchant3.co.uk",
                        "123456",
                        "TestMerchant",
                        "User1",
                        "Test Merchant 3",
                        "Test Estate"});
#line 66
 await testRunner.WhenAsync("I create the following security users", ((string)(null)), table38, "When ");
#line hidden
            global::Reqnroll.Table table39 = new global::Reqnroll.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table39.AddRow(new string[] {
                        "TestDevice1",
                        "Test Merchant 1",
                        "Test Estate"});
            table39.AddRow(new string[] {
                        "TestDevice2",
                        "Test Merchant 2",
                        "Test Estate"});
            table39.AddRow(new string[] {
                        "TestDevice3",
                        "Test Merchant 3",
                        "Test Estate"});
#line 72
 await testRunner.WhenAsync("I add the following devices to the merchant", ((string)(null)), table39, "When ");
#line hidden
#line 78
 await testRunner.GivenAsync("I am on the application home page", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 80
 await testRunner.AndAsync("I click on the Sign In Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 82
 await testRunner.ThenAsync("I am presented with a login screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 84
 await testRunner.WhenAsync("I login with the username \'estateuser@testestate1.co.uk\' and password \'123456\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 86
 await testRunner.ThenAsync("I am presented with the Estate Administrator Dashboard", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Merchant PR Test")]
        [NUnit.Framework.CategoryAttribute("PRTest")]
        public async System.Threading.Tasks.Task MerchantPRTest()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Merchant PR Test", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 89
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 4
await this.FeatureBackgroundAsync();
#line hidden
#line 91
 await testRunner.GivenAsync("I click on the My Merchants sidebar option", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 92
 await testRunner.ThenAsync("I am presented with the Merchants List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table40 = new global::Reqnroll.Table(new string[] {
                            "MerchantName",
                            "SettlementSchedule",
                            "ContactName",
                            "AddressLine1",
                            "Town"});
                table40.AddRow(new string[] {
                            "Test Merchant 1",
                            "Immediate",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table40.AddRow(new string[] {
                            "Test Merchant 2",
                            "Weekly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table40.AddRow(new string[] {
                            "Test Merchant 3",
                            "Monthly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
#line 93
 await testRunner.AndAsync("the following merchants details are in the list", ((string)(null)), table40, "And ");
#line hidden
#line 98
 await testRunner.WhenAsync("I click on the New Merchant Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 99
 await testRunner.ThenAsync("the Add New Merchant Screen is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table41 = new global::Reqnroll.Table(new string[] {
                            "MerchantName",
                            "SettlementSchedule",
                            "AddressLine1",
                            "Town",
                            "Region",
                            "Country",
                            "ContactName",
                            "EmailAddress"});
                table41.AddRow(new string[] {
                            "Test Merchant 4",
                            "Immediate",
                            "Address Line 1",
                            "TestTown",
                            "Region",
                            "Country",
                            "Test Contact 4",
                            "1@2.com"});
#line 100
 await testRunner.WhenAsync("I enter the following details for the new Merchant", ((string)(null)), table41, "When ");
#line hidden
#line 103
 await testRunner.AndAsync("click the Save Merchant button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 104
 await testRunner.ThenAsync("I am presented with the Merchants List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table42 = new global::Reqnroll.Table(new string[] {
                            "MerchantName",
                            "SettlementSchedule",
                            "ContactName",
                            "AddressLine1",
                            "Town"});
                table42.AddRow(new string[] {
                            "Test Merchant 1",
                            "Immediate",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table42.AddRow(new string[] {
                            "Test Merchant 2",
                            "Weekly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table42.AddRow(new string[] {
                            "Test Merchant 3",
                            "Monthly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table42.AddRow(new string[] {
                            "Test Merchant 4",
                            "Immediate",
                            "Test Contact 4",
                            "Address Line 1",
                            "TestTown"});
#line 105
 await testRunner.AndAsync("the following merchants details are in the list", ((string)(null)), table42, "And ");
#line hidden
#line 111
 await testRunner.WhenAsync("I click on the Edit Merchant Button for \'Test Merchant 1\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 112
 await testRunner.ThenAsync("the Edit Merchant Screen is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table43 = new global::Reqnroll.Table(new string[] {
                            "Tab",
                            "Field",
                            "Value"});
                table43.AddRow(new string[] {
                            "Details",
                            "Name",
                            "Test Merchant 1 Update"});
                table43.AddRow(new string[] {
                            "Address",
                            "AddressLine1",
                            "Address Line 1 Update"});
                table43.AddRow(new string[] {
                            "Contact",
                            "ContactName",
                            "Test Contact 1 Update"});
#line 113
 await testRunner.WhenAsync("I enter the following details for the updated Merchant", ((string)(null)), table43, "When ");
#line hidden
#line 118
 await testRunner.AndAsync("click the Save Merchant button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 119
 await testRunner.ThenAsync("I am presented with the Merchants List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table44 = new global::Reqnroll.Table(new string[] {
                            "MerchantName",
                            "SettlementSchedule",
                            "ContactName",
                            "AddressLine1",
                            "Town"});
                table44.AddRow(new string[] {
                            "Test Merchant 1 Update",
                            "Immediate",
                            "Test Contact 1 Update",
                            "Address Line 1 Update",
                            "TestTown"});
                table44.AddRow(new string[] {
                            "Test Merchant 2",
                            "Weekly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table44.AddRow(new string[] {
                            "Test Merchant 3",
                            "Monthly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table44.AddRow(new string[] {
                            "Test Merchant 4",
                            "Immediate",
                            "Test Contact 4",
                            "Address Line 1",
                            "TestTown"});
#line 120
 await testRunner.AndAsync("the following merchants details are in the list", ((string)(null)), table44, "And ");
#line hidden
#line 126
 await testRunner.WhenAsync("I click on the View Merchant Button for \'Test Merchant 1 Update\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 127
 await testRunner.ThenAsync("the View Merchant Screen is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Merchant Operator Management")]
        public async System.Threading.Tasks.Task MerchantOperatorManagement()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Merchant Operator Management", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 129
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 4
await this.FeatureBackgroundAsync();
#line hidden
                global::Reqnroll.Table table45 = new global::Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table45.AddRow(new string[] {
                            "Test Estate",
                            "Test Operator1",
                            "True",
                            "True"});
#line 130
 await testRunner.GivenAsync("I have created the following operators", ((string)(null)), table45, "Given ");
#line hidden
                global::Reqnroll.Table table46 = new global::Reqnroll.Table(new string[] {
                            "EstateName",
                            "OperatorName"});
                table46.AddRow(new string[] {
                            "Test Estate",
                            "Test Operator1"});
#line 134
 await testRunner.AndAsync("I have assigned the following operators to the estates", ((string)(null)), table46, "And ");
#line hidden
#line 138
 await testRunner.GivenAsync("I click on the My Merchants sidebar option", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 139
 await testRunner.ThenAsync("I am presented with the Merchants List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table47 = new global::Reqnroll.Table(new string[] {
                            "MerchantName",
                            "SettlementSchedule",
                            "ContactName",
                            "AddressLine1",
                            "Town"});
                table47.AddRow(new string[] {
                            "Test Merchant 1",
                            "Immediate",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table47.AddRow(new string[] {
                            "Test Merchant 2",
                            "Weekly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
                table47.AddRow(new string[] {
                            "Test Merchant 3",
                            "Monthly",
                            "Test Contact 1",
                            "Address Line 1",
                            "TestTown"});
#line 140
 await testRunner.AndAsync("the following merchants details are in the list", ((string)(null)), table47, "And ");
#line hidden
#line 146
 await testRunner.WhenAsync("I click on the Edit Merchant Button for \'Test Merchant 1\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 147
 await testRunner.ThenAsync("the Edit Merchant Screen is displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 149
 await testRunner.WhenAsync("I click on the Operators tab", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 150
 await testRunner.ThenAsync("I am presented with the Merchants Operator List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table48 = new global::Reqnroll.Table(new string[] {
                            "OperatorName",
                            "MerchantNumber",
                            "TerminalNumber"});
                table48.AddRow(new string[] {
                            "Test Operator",
                            "00000001",
                            "10000001"});
#line 151
 await testRunner.AndAsync("the following operators are displayed in the list", ((string)(null)), table48, "And ");
#line hidden
#line 154
 await testRunner.WhenAsync("I click on the Add Operator Button", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 155
 await testRunner.ThenAsync("the Assign Operator Dialog will be displayed", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table49 = new global::Reqnroll.Table(new string[] {
                            "OperatorName",
                            "MerchantNumber",
                            "TerminalNumber"});
                table49.AddRow(new string[] {
                            "Test Operator1",
                            "00000111",
                            "10000111"});
#line 156
 await testRunner.WhenAsync("I enter the following details for the Operator", ((string)(null)), table49, "When ");
#line hidden
#line 159
 await testRunner.AndAsync("click the Assign Operator button", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 160
 await testRunner.ThenAsync("I am presented with the Merchants Operator List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table50 = new global::Reqnroll.Table(new string[] {
                            "OperatorName",
                            "MerchantNumber",
                            "TerminalNumber",
                            "IsDeleted"});
                table50.AddRow(new string[] {
                            "Test Operator",
                            "00000001",
                            "10000001",
                            "False"});
                table50.AddRow(new string[] {
                            "Test Operator1",
                            "00000111",
                            "10000111",
                            "False"});
#line 161
 await testRunner.AndAsync("the following operators are displayed in the list", ((string)(null)), table50, "And ");
#line hidden
#line 165
 await testRunner.WhenAsync("I click on the Remove Operator for \'Test Operator1\'", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 166
 await testRunner.ThenAsync("I am presented with the Merchants Operator List Screen", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
                global::Reqnroll.Table table51 = new global::Reqnroll.Table(new string[] {
                            "OperatorName",
                            "MerchantNumber",
                            "TerminalNumber",
                            "IsDeleted"});
                table51.AddRow(new string[] {
                            "Test Operator",
                            "00000001",
                            "10000001",
                            "False"});
                table51.AddRow(new string[] {
                            "Test Operator1",
                            "00000111",
                            "10000111",
                            "True"});
#line 167
 await testRunner.AndAsync("the following operators are displayed in the list", ((string)(null)), table51, "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
