using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;

namespace EstateManagementUI.Pages.Merchant
{
    public class MerchantsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public MerchantsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Merchant, MerchantFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Merchants = new List<ViewModels.Merchant>();
        }

        public List<ViewModels.Merchant> Merchants { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
            
            Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(accessToken, estateId);

            List<MerchantModel> response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (MerchantModel merchantModel in response) {
                this.Merchants.Add(new ViewModels.Merchant() {
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

    
}
