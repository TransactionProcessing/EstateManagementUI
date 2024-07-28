using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Merchant
{
    public class MerchantsList : HydroComponent
    {
        private readonly IMediator Mediator;

        public MerchantsList(IMediator mediator)
        {
            this.Mediator = mediator;
            this.Merchants = new List<Merchant>();
        }

        public List<Merchant> Merchants { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
            
            Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(accessToken, estateId);

            List<MerchantModel> response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (MerchantModel merchantModel in response) {
                this.Merchants.Add(new Merchant() {
                    Id = merchantModel.MerchantId,
                    Name = merchantModel.MerchantName,
                    SettlementSchedule = merchantModel.SettlementSchedule,
                    Reference = merchantModel.MerchantReference,
                    AddressLine1 = merchantModel.AddressLine1,
                    ContactName = merchantModel.ContactName,
                    Town = merchantModel.Town
                });
            }
        }
    }

    public record Merchant {
        public string Name { get; set; }
        public string Reference { get; set; }
        public string SettlementSchedule { get; set; }
        public string ContactName { get; set; }
        public string AddressLine1 { get; set; }
        public string Town { get; set; }

        public Guid Id { get; set; }
    }
}
