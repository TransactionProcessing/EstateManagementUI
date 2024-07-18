using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Estate
{
    public class ViewEstate : HydroComponent
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Reference { get; set; }

        private readonly IMediator Mediator;

        public ViewEstate(IMediator mediator) {
            this.Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(accessToken, estateId);

            EstateModel response = await this.Mediator.Send(query, CancellationToken.None);

            this.Id = response.EstateId;
            this.Name = response.EstateName;
            this.Reference = response.EstateName; 
        }
    }
}
