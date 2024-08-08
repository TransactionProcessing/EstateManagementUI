using System.Runtime.CompilerServices;
using System.Security.Claims;
using Azure;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace EstateManagementUI.Pages.Estate
{
    public class ViewEstate : SecureHydroComponent
    {
        public ViewModels.Estate Estate { get; set; }

        private readonly IMediator Mediator;

        public ViewEstate(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.View, permissionsService) {
            Mediator = mediator;
        }
        
        public override async Task MountAsync()
        {
            String accessToken = await HttpContext.GetTokenAsync("access_token");

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