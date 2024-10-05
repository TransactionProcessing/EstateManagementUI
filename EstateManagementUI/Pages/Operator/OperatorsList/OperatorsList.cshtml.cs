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
using System.Reflection.Metadata;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using SimpleResults;
using static EstateManagementUI.Pages.Operator.OperatorDialogs.OperatorPageEvents;
using Microsoft.AspNetCore.Mvc;

namespace EstateManagementUI.Pages.Operator.OperatorsList
{
    public class OperatorsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;
        public Guid OperatorId { get; set; }
        public bool ShowDialog { get; set; }

        public OperatorsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Operator, OperatorFunctions.ViewList, permissionsService)
        {
            Mediator = mediator;
            Operators = new List<ViewModels.Operator>();

            //Subscribe<OperatorPageEvents.OperatorCreatedEvent>(Handle);
            //Subscribe<OperatorPageEvents.OperatorUpdatedEvent>(Handle);
            //Subscribe<OperatorPageEvents.ShowNewOperatorDialog>(Handle);
            //Subscribe<OperatorPageEvents.HideNewOperatorDialog>(Handle);
            //Subscribe<OperatorPageEvents.ShowEditOperatorDialog>(Handle);
            //Subscribe<OperatorPageEvents.HideEditOperatorDialog>(Handle);
        }

        //public async Task Handle(OperatorPageEvents.OperatorCreatedEvent @event)
        //{
        //    // Sleep for a second
        //    await Task.Delay(1000); // TODO: might be a better way of handling this
        //    await GetOperators();
        //}

        //public async Task Handle(OperatorPageEvents.OperatorUpdatedEvent @event)
        //{
        //    // Sleep for a second
        //    await Task.Delay(1000); // TODO: might be a better way of handling this
        //    await GetOperators();
        //}

        //public async Task Handle(OperatorPageEvents.ShowNewOperatorDialog @event)
        //{
        //    OperatorId = Guid.Empty;
        //    ShowDialog = true;
        //}
        //public async Task Handle(OperatorPageEvents.HideNewOperatorDialog @event)
        //{
        //    OperatorId = Guid.Empty;
        //    ShowDialog = false;
        //}
        //public async Task Handle(OperatorPageEvents.ShowEditOperatorDialog @event)
        //{
        //    OperatorId = @event.OperatorId;
        //    ShowDialog = true;
        //}
        //public async Task Handle(OperatorPageEvents.HideEditOperatorDialog @event)
        //{
        //    OperatorId = Guid.Empty;
        //    ShowDialog = false;
        //}

        public List<ViewModels.Operator> Operators { get; set; }

        public override async Task MountAsync()
        {
            await GetOperators();
        }

        public void Add() => this.Location(this.Url.Page("/Operator/NewOperator"));

        public async Task View(Guid operatorId) =>
            this.Location(this.Url.Page("/Operator/ViewOperator", new { OperatorId = operatorId }));

        public async Task Edit(Guid operatorId) =>
            this.Location(this.Url.Page("/Operator/EditOperator", new { OperatorId = operatorId }));

        private async Task GetOperators()
        {
            await PopulateTokenAndEstateId();

            Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(AccessToken, EstateId);

            Result<List<OperatorModel>> response = await Mediator.Send(query, CancellationToken.None);

            if (response.IsFailed)
            {
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

            IEnumerable<ViewModels.Operator> sortQuery = Sorting switch
            {
                (OperatorSorting.Name, Ascending: false) => resultList.OrderBy(p => p.Name),
                (OperatorSorting.Name, Ascending: true) => resultList.OrderByDescending(p => p.Name),
                (OperatorSorting.RequireCustomTerminalNumber, Ascending: false) => resultList.OrderBy(p => p.RequireCustomTerminalNumber),
                (OperatorSorting.RequireCustomTerminalNumber, Ascending: true) => resultList.OrderByDescending(p => p.RequireCustomTerminalNumber),
                (OperatorSorting.RequireCustomMerchantNumber, Ascending: false) => resultList.OrderBy(p => p.RequireCustomMerchantNumber),
                (OperatorSorting.RequireCustomMerchantNumber, Ascending: true) => resultList.OrderByDescending(p => p.RequireCustomMerchantNumber),
                _ => resultList.AsEnumerable()
            };

            Operators = sortQuery.ToList();
        }

        public async Task Sort(OperatorSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await GetOperators();
        }

        public (OperatorSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum OperatorSorting
    {
        Name,
        RequireCustomTerminalNumber,
        RequireCustomMerchantNumber
    }
}
