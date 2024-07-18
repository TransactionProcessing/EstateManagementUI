using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Newtonsoft.Json;

namespace EstateManagementUI.Pages.Estate
{
    public class OperatorList : HydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorList(IMediator mediator)
        {
            this.Mediator = mediator;
            this.Operators = new List<Operator>();
        }

        public List<Operator> Operators { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(accessToken, estateId);

            EstateModel response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (EstateOperatorModel estateOperatorModel in response.Operators) {
                this.Operators.Add(new Operator {
                    Id = estateOperatorModel.OperatorId,
                    Name = estateOperatorModel.Name,
                    RequireCustomTerminalNumber = estateOperatorModel.RequireCustomTerminalNumber ? "Yes" : "No",
                    RequireCustomMerchantNumber = estateOperatorModel.RequireCustomMerchantNumber ? "Yes" : "No"
                });
            }
        }
    }

    public record Operator {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public String RequireCustomMerchantNumber { get; set; }

        public String RequireCustomTerminalNumber { get; set; }

        //public bool IsDeleted { get; set; }
    }
}
