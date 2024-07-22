using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagementUI.Testing
{
    public static class TestData {
        public static String AccessToken = "token1";
        public static Guid EstateId = Guid.Parse("BD6F1ED7-6290-4285-A200-E4F8D25F4CBE");
        public static Queries.GetEstateQuery GetEstateQuery => new(AccessToken, EstateId);

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
    }
}