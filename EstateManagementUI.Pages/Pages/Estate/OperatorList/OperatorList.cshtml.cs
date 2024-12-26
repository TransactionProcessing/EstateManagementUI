using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace EstateManagementUI.Pages.Estate.OperatorList
{
    public class OperatorList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.ViewOperatorsList, permissionsService)
        {
            Mediator = mediator;
            Operators = new List<ViewModels.Operator>();
        }

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync()
        {
            await PopulateTokenAndEstateId();

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(AccessToken, EstateId);

            EstateModel response = await Mediator.Send(query, CancellationToken.None);

            foreach (EstateOperatorModel estateOperatorModel in response.Operators)
            {
                Operators.Add(new ViewModels.Operator
                {
                    Id = estateOperatorModel.OperatorId,
                    Name = estateOperatorModel.Name,
                    RequireCustomTerminalNumber = estateOperatorModel.RequireCustomTerminalNumber ? "Yes" : "No",
                    RequireCustomMerchantNumber = estateOperatorModel.RequireCustomMerchantNumber ? "Yes" : "No"
                });
            }
        }
    }


}
