using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Merchant;
using System.Collections.Generic;

namespace EstateManagementUI.Pages.Operator
{
    public class OperatorsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Operator, OperatorFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Operators = new List<ViewModels.Operator>();
        }

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync() {
            await this.GetOperators();
        }

        private async Task GetOperators() {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(accessToken, estateId);

            List<OperatorModel> response = await this.Mediator.Send(query, CancellationToken.None);

            List<ViewModels.Operator> resultList = new();
            foreach (OperatorModel operatorModel in response)
            {
                resultList.Add(new ViewModels.Operator()
                {
                    Id = operatorModel.OperatorId,
                    Name = operatorModel.Name,
                    RequireCustomTerminalNumber = @operatorModel.RequireCustomTerminalNumber ? "Yes" : "No",
                    RequireCustomMerchantNumber = @operatorModel.RequireCustomMerchantNumber ? "Yes" : "No"
                });
            }

            IEnumerable<ViewModels.Operator> sortQuery = this.Sorting switch
            {
                (OperatorSorting.Name, Ascending: false) => resultList.OrderBy(p => p.Name),
                (OperatorSorting.Name, Ascending: true) => resultList.OrderByDescending(p => p.Name),
                (OperatorSorting.RequireCustomTerminalNumber, Ascending: false) => resultList.OrderBy(p => p.RequireCustomTerminalNumber),
                (OperatorSorting.RequireCustomTerminalNumber, Ascending: true) => resultList.OrderByDescending(p => p.RequireCustomTerminalNumber),
                (OperatorSorting.RequireCustomMerchantNumber, Ascending: false) => resultList.OrderBy(p => p.RequireCustomMerchantNumber),
                (OperatorSorting.RequireCustomMerchantNumber, Ascending: true) => resultList.OrderByDescending(p => p.RequireCustomMerchantNumber),
                _ => resultList.AsEnumerable()
            };

            this.Operators = sortQuery.ToList();
        }

        public async Task Sort(OperatorSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await this.GetOperators();
        }

        public (OperatorSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum OperatorSorting {
        Name,
        RequireCustomTerminalNumber,
        RequireCustomMerchantNumber
    }
}
