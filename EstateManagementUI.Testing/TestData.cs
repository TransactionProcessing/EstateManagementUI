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

        public static String EstateName = "Test Estate 1";

        public static String OperatorName = "Test Operator 1";

        public static Guid OperatorId = Guid.Parse("DECA8293-F045-41C5-A2F7-30F2792FD273");

        public static Boolean RequireCustomMerchantNumber = true;

        public static Boolean RequireCustomTerminalNumber = true;

        public static String EmailAddress = "estateuser1@testestate1.co.uk";

        public static Guid SecurityUserId = Guid.Parse("6A3C1B74-F01E-4017-85D5-6E6082038AB8");

        public static EstateResponse EstateResponse =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                Operators = new List<EstateOperatorResponse>
                {
                    new EstateOperatorResponse
                    {
                        OperatorId = TestData.OperatorId,
                        Name = TestData.OperatorName,
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
                    Name = "Operator 1",
                    OperatorId = Guid.Parse("257AA9F3-FBA8-4147-B1E5-CB759C3DD9DE"),
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                },
                new OperatorResponse {
                    Name = "Operator 2",
                    OperatorId = Guid.Parse("4412915E-02C5-442B-981C-1710F8770FBC"),
                    RequireCustomMerchantNumber = false,
                    RequireCustomTerminalNumber = false
                }
            };

        public static List<MerchantResponse> MerchantResponses =>
            new List<MerchantResponse> {
                new MerchantResponse {
                    SettlementSchedule = SettlementSchedule.Immediate,
                    MerchantReference = "Reference1",
                    MerchantName = "Test Merchant 1",
                    MerchantId = Guid.Parse("2F8431D9-8D04-4AE5-B66C-DB40DFADE581"),
                    Addresses = new List<AddressResponse> {
                        new AddressResponse {
                            AddressLine1 = "Address Line 1",
                            Town = "Test Town 1"
                        }
                    },
                    Contacts = new List<ContactResponse> {
                        new ContactResponse {
                            ContactName = "Contact 1"
                        }
                    }
                },
                new MerchantResponse {
                    SettlementSchedule = SettlementSchedule.Weekly,
                    MerchantReference = "Reference2",
                    MerchantName = "Test Merchant 2",
                    MerchantId = Guid.Parse("8959608C-2448-48EA-AFB4-9D10FFFB6140"),
                    Addresses = new List<AddressResponse> {
                        new AddressResponse {
                            AddressLine1 = "Address Line 2",
                            Town = "Test Town 2"
                        }
                    },
                    Contacts = new List<ContactResponse> {
                        new ContactResponse {
                            ContactName = "Contact 2"
                        }
                    }
                },
                new MerchantResponse {
                    SettlementSchedule = SettlementSchedule.Monthly,
                    MerchantReference = "Reference3",
                    MerchantName = "Test Merchant 3",
                    MerchantId = Guid.Parse("877D7384-9A72-4A73-A275-9DB62BF32EDB"),
                    Addresses = new List<AddressResponse> {
                        new AddressResponse {
                            AddressLine1 = "Address Line 3",
                            Town = "Test Town 3"
                        }
                    },
                    Contacts = new List<ContactResponse> {
                        new ContactResponse {
                            ContactName = "Contact 3"
                        }
                    }
                }
            };
    }
}