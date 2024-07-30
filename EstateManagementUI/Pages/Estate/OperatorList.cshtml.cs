using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace EstateManagementUI.Pages.Estate
{
    public class OperatorList : HydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorList(IMediator mediator)
        {
            Mediator = mediator;
            Operators = new List<ViewModels.Operator>();
        }

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(accessToken, estateId);

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
