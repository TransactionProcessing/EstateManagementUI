using System;
using System.Collections.Generic;

namespace EstateManagementUI.IntegrationTests.Common
{
    public class ClientDetails
    {
        public String ClientId { get; private set; }
        public String ClientSecret { get; private set; }
        public List<String> GrantTypes { get; private set; }

        private ClientDetails(String clientId,
                              String clientSecret,
                              List<String> grantTypes)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantTypes = grantTypes;
        }

        public static ClientDetails Create(String clientId,
                                           String clientSecret,
                                           List<String> grantTypes)
        {
            return new ClientDetails(clientId, clientSecret, grantTypes);
        }
    }
}