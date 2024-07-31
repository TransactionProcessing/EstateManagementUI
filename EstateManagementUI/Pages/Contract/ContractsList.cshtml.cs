using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Contract
{
    public class ContractsList : HydroComponent
    {
        private readonly IMediator Mediator;

        public ContractsList(IMediator mediator)
        {
            this.Mediator = mediator;
            this.Contracts = new List<ViewModels.Contract>();
        }

        public List<ViewModels.Contract> Contracts { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
            
            Queries.GetContractsQuery query = new Queries.GetContractsQuery(accessToken, estateId);

            List<ContractModel> response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (ContractModel contractModel in response) {
                this.Contracts.Add(new ViewModels.Contract {
                    ContractId = contractModel.ContractId,
                    OperatorName = contractModel.OperatorName,
                    Description = contractModel.Description,
                    NumberOfProducts = contractModel.NumberOfProducts
                });
            }
        }
    }

    
}
