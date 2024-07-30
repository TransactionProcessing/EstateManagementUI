using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace EstateManagementUI.Pages.Estate
{
    public class ViewEstate : HydroComponent
    {
        public ViewModels.Estate Estate { get; set; }

        private readonly IMediator Mediator;

        public ViewEstate(IMediator mediator)
        {
            Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(accessToken, estateId);

            EstateModel response = await Mediator.Send(query, CancellationToken.None);

            Estate = new ViewModels.Estate
            {
                Name = response.EstateName,
                Id = response.EstateId,
                Reference = response.EstateName
            };
        }
    }

    
}
