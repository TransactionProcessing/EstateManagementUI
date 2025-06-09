using System.Runtime.CompilerServices;
using System.Security.Claims;
using Azure;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;
using static System.Collections.Specialized.BitVector32;

namespace EstateManagementUI.Pages.Estate.ViewEstate
{
    public class ViewEstate : SecureHydroComponent
    {
        public ViewModels.Estate Estate { get; set; }

        private readonly IMediator Mediator;

        public ViewEstate(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.View, permissionsService)
        {
            Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            await PopulateTokenAndEstateId();

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(AccessToken, EstateId);

            Result<EstateModel> response = await Mediator.Send(query, CancellationToken.None);
            if (response.IsFailed)
            {
                // TODO: Handle error properly, e.g., show a message to the user
            }
            Estate = new ViewModels.Estate
            {
                Name = response.Data.EstateName,
                Id = response.Data.EstateId,
                Reference = response.Data.Reference
            };
        }
    }
}