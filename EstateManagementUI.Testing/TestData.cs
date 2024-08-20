using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagementUI.Testing
{
    public static class TestData {
        public static String AccessToken = "token1";
        public static Guid EstateId = Guid.Parse("BD6F1ED7-6290-4285-A200-E4F8D25F4CBE");
        public static Queries.GetEstateQuery GetEstateQuery => new(AccessToken, EstateId);
        public static Queries.GetMerchantsQuery GetMerchantsQuery => new(AccessToken, EstateId);
        public static Queries.GetOperatorsQuery GetOperatorsQuery => new(AccessToken, EstateId);
        public static Queries.GetContractsQuery GetContractsQuery => new(AccessToken, EstateId);

        public static Commands.AddNewOperatorCommand AddNewOperatorCommand => new(AccessToken, EstateId, Operator1Id, Operator1Name, RequireCustomMerchantNumber, RequireCustomTerminalNumber);

        public static String EstateName = "Test Estate 1";

        public static String Operator1Name = "Operator 1";
        public static Guid Operator1Id = Guid.Parse("DECA8293-F045-41C5-A2F7-30F2792FD273");
        public static String Operator2Name = "Operator 2";
        public static Guid Operator2Id = Guid.Parse("4412915E-02C5-442B-981C-1710F8770FBC");

        public static Boolean RequireCustomMerchantNumber = true;

        public static Boolean RequireCustomTerminalNumber = true;

        public static String EmailAddress = "estateuser1@testestate1.co.uk";

        public static Guid SecurityUserId = Guid.Parse("6A3C1B74-F01E-4017-85D5-6E6082038AB8");

        public static String Merchant1Reference = "Reference1";
        public static String Merchant1Name = "Test Merchant 1";
        public static Guid Merchant1Id = Guid.Parse("2F8431D9-8D04-4AE5-B66C-DB40DFADE581");
        public static SettlementSchedule Merchant1SettlementSchedule = SettlementSchedule.Immediate;
        public static AddressResponse Merchant1Address = new() {
            AddressLine1 = "Address Line 1", Town = "Test Town 1"
        };
        public static ContactResponse Merchant1Contact = new() { ContactName = "Contact 1" };
        
        public static String Merchant2Reference = "Reference2";
        public static String Merchant2Name = "Test Merchant 2";
        public static Guid Merchant2Id = Guid.Parse("8959608C-2448-48EA-AFB4-9D10FFFB6140");
        public static SettlementSchedule Merchant2SettlementSchedule = SettlementSchedule.Weekly;
        public static AddressResponse Merchant2Address = new()
        {
            AddressLine1 = "Address Line 2",
            Town = "Test Town 2"
        };
        public static ContactResponse Merchant2Contact = new() { ContactName = "Contact 2" };

        public static String Merchant3Reference = "Reference3";
        public static String Merchant3Name = "Test Merchant 3";
        public static Guid Merchant3Id = Guid.Parse("877D7384-9A72-4A73-A275-9DB62BF32EDB");
        public static SettlementSchedule Merchant3SettlementSchedule = SettlementSchedule.Monthly;
        public static AddressResponse Merchant3Address = new()
        {
            AddressLine1 = "Address Line 3",
            Town = "Test Town 3"
        };

        public static ContactResponse Merchant3Contact = new() { ContactName = "Contact 3" };

        public static EstateResponse EstateResponse =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                Operators = new List<EstateOperatorResponse>
                {
                    new EstateOperatorResponse
                    {
                        OperatorId = Operator1Id,
                        Name = Operator1Name,
                        RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                        RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber
                    }
                },
                EstateId = TestData.EstateId,
                SecurityUsers = new List<SecurityUserResponse>
                {
                    new SecurityUserResponse
                    {
                        EmailAddress = TestData.EmailAddress,
                        SecurityUserId = TestData.SecurityUserId

                    }
                }
            };

        public static List<OperatorResponse> OperatorResponses =>
            new List<OperatorResponse> {
                new OperatorResponse {
                    Name = Operator1Name,
                    OperatorId = Operator1Id,
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                },
                new OperatorResponse {
                    Name = Operator2Name,
                    OperatorId = Operator2Id,
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                }
            };

        public static List<MerchantResponse> MerchantResponses =>
            new List<MerchantResponse> {
                new MerchantResponse {
                    SettlementSchedule = Merchant1SettlementSchedule,
                    MerchantReference = Merchant1Reference,
                    MerchantName = Merchant1Name,
                    MerchantId = Merchant1Id,
                    Addresses = [Merchant1Address],
                    Contacts = [Merchant1Contact]
                },
                new MerchantResponse {
                    SettlementSchedule = Merchant2SettlementSchedule,
                    MerchantReference = Merchant2Reference,
                    MerchantName = Merchant2Name,
                    MerchantId = Merchant2Id,
                    Addresses = [Merchant2Address],
                    Contacts = [Merchant2Contact]
                },
                new MerchantResponse {
                    SettlementSchedule = Merchant3SettlementSchedule,
                    MerchantReference = Merchant3Reference,
                    MerchantName = Merchant3Name,
                    MerchantId = Merchant3Id,
                    Addresses = [Merchant3Address],
                    Contacts = [Merchant3Contact]
                }
            };

        public static Guid Contract1Id = Guid.Parse("82ED1302-491D-48F4-8FC7-58A22EB1CF4C");
        public static String Contract1Description = "Test Contract 1";

        public static String Contract1Product1Name = "Contract 1 Product 1";
        public static String Contract1Product1DisplayText = "Contract 1 Prod 1";
        public static Guid Contract1Product1Id = Guid.Parse("49E996AC-7068-42E5-950F-5E73E8909730");
        public static ProductType Contract1Product1ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product1Value = 100.00m;

        public static String Contract1Product2Name = "Contract 1 Product 2";
        public static String Contract1Product2DisplayText = "Contract 1 Prod 2";
        public static Guid Contract1Product2Id = Guid.Parse("71159369-A719-46E3-9D86-E681307EEFAF");
        public static ProductType Contract1Product2ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product2Value = 200.00m;

        public static String Contract1Product3Name = "Contract 1 Product 3";
        public static String Contract1Product3DisplayText = "Contract 1 Prod 3";
        public static Guid Contract1Product3Id = Guid.Parse("29A7B01D-825C-4CD2-A6CE-F18D1DE9087B");
        public static ProductType Contract1Product3ProductType = ProductType.MobileTopup;
        public static Decimal? Contract1Product3Value = null;

        public static List<ContractResponse> ContractResponses =>
            new List<ContractResponse>() {
                new ContractResponse {
                    OperatorId = Operator1Id,
                    OperatorName = Operator1Name,
                    ContractId = Contract1Id,
                    Description = Contract1Description,
                    Products = new List<ContractProduct> {
                        new ContractProduct {
                            Name = Contract1Product1Name,
                            DisplayText = Contract1Product1DisplayText,
                            ProductId = Contract1Product1Id,
                            ProductType = Contract1Product1ProductType,
                            Value = Contract1Product1Value
                        },
                        new ContractProduct {
                            Name = Contract1Product2Name,
                            DisplayText = Contract1Product2DisplayText,
                            ProductId = Contract1Product2Id,
                            ProductType = Contract1Product2ProductType,
                            Value = Contract1Product2Value
                        },
                        new ContractProduct {
                            Name = Contract1Product3Name,
                            DisplayText = Contract1Product3DisplayText,
                            ProductId = Contract1Product3Id,
                            ProductType = Contract1Product3ProductType,
                            Value = Contract1Product3Value
                        }
                    }
                }
            };
    }
}