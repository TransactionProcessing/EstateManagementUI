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
using EstateManagementUI.Pages.Shared.Components;
using SimpleResults;

namespace EstateManagementUI.Pages.Operator
{
    public class OperatorsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public bool ShowAddDialog { get; set; }

        public OperatorsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Operator, OperatorFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Operators = new List<ViewModels.Operator>();

            Subscribe<OperatorEvents.OperatorCreatedEvent>(async _ => {
                // Sleep for a second
                await Task.Delay(1000); // TODO: might be a better way of handling this
                await this.GetOperators();
            });
            Subscribe<OperatorPageEvents.ShowNewOperatorDialog>(_ => ShowAddDialog = true);
            Subscribe<OperatorPageEvents.HideNewOperatorDialog>(_ => ShowAddDialog = false);
        }

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync() {
            await this.GetOperators();
        }

        public void Add() =>
            Dispatch(new OperatorPageEvents.ShowNewOperatorDialog(), Scope.Global);

        private async Task GetOperators() {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(accessToken, estateId);

            Result<List<OperatorModel>> response = await this.Mediator.Send(query, CancellationToken.None);

            if (response.IsFailed) {
                Dispatch(new ShowMessage("Error getting Operator List", ToastType.Error), Scope.Global);
                return;
            }

            List<ViewModels.Operator> resultList = new();
            foreach (OperatorModel operatorModel in response.Data)
            {
                resultList.Add(new ViewModels.Operator
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
