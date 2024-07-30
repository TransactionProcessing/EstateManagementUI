using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Operator
{
    public class OperatorsList : HydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorsList(IMediator mediator)
        {
            this.Mediator = mediator;
            this.Operators = new List<ViewModels.Operator>();
        }

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
            
            Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(accessToken, estateId);

            List<OperatorModel> response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (OperatorModel operatorModel in response) {
                this.Operators.Add(new ViewModels.Operator() {
                    Id = operatorModel.OperatorId,
                    Name = operatorModel.Name,
                    RequireCustomTerminalNumber = @operatorModel.RequireCustomTerminalNumber ? "Yes" : "No",
                    RequireCustomMerchantNumber = @operatorModel.RequireCustomMerchantNumber ? "Yes" : "No"
                });
            }
        }
    }
}
