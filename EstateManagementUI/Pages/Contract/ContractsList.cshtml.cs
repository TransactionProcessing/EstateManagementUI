using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Merchant;

namespace EstateManagementUI.Pages.Contract
{
    public class ContractsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public ContractsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Contract, ContractFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Contracts = new List<ViewModels.Contract>();
        }

        public List<ViewModels.Contract> Contracts { get; set; }

        public override async Task MountAsync() {
            await this.GetContracts();
        }

        private async Task GetContracts() {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetContractsQuery query = new Queries.GetContractsQuery(accessToken, estateId);

            List<ContractModel> response = await this.Mediator.Send(query, CancellationToken.None);

            List<ViewModels.Contract> resultList = new();
            foreach (ContractModel contractModel in response)
            {
                resultList.Add(new ViewModels.Contract
                {
                    ContractId = contractModel.ContractId,
                    OperatorName = contractModel.OperatorName,
                    Description = contractModel.Description,
                    NumberOfProducts = contractModel.NumberOfProducts
                });
            }

            IEnumerable<ViewModels.Contract> sortQuery = this.Sorting switch
            {
                (ContractSorting.Description, Ascending: false) => resultList.OrderBy(p => p.Description),
                (ContractSorting.Description, Ascending: true) => resultList.OrderByDescending(p => p.Description),
                (ContractSorting.Operator, Ascending: false) => resultList.OrderBy(p => p.OperatorName),
                (ContractSorting.Operator, Ascending: true) => resultList.OrderByDescending(p => p.OperatorName),
                (ContractSorting.NumberOfProducts, Ascending: false) => resultList.OrderBy(p => p.NumberOfProducts),
                (ContractSorting.NumberOfProducts, Ascending: true) => resultList.OrderByDescending(p => p.NumberOfProducts),
                
                _ => resultList.AsEnumerable()
            };

            this.Contracts = sortQuery.ToList();

        }

        public async Task Sort(ContractSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await this.GetContracts();
        }

        public (ContractSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum ContractSorting {
        Description,
        Operator,
        NumberOfProducts
    }
}
