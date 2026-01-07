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
using Shared.Logger;
using SimpleResults;
using Hydro;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Estate.ViewEstate
{
    [ExcludeFromCodeCoverage]
    public class ViewEstate : SecureHydroComponent
    {
        public ViewModels.Estate Estate { get; set; }
        public List<EstateOperatorViewModel> Operators { get; set; }
        public string ActiveTab { get; set; }
        public bool ShowOperatorDialog { get; set; }

        private readonly IMediator Mediator;

        public ViewEstate(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.View, permissionsService)
        {
            Mediator = mediator;
            Operators = new List<EstateOperatorViewModel>();
            
            Subscribe<EstatePageEvents.ShowAddOperatorDialog>(Handle);
            Subscribe<EstatePageEvents.HideAddOperatorDialog>(Handle);
            Subscribe<EstatePageEvents.OperatorAssignedToEstateEvent>(Handle);
            Subscribe<EstatePageEvents.OperatorRemovedFromEstateEvent>(Handle);
            
            if (String.IsNullOrEmpty(this.ActiveTab) == true) {
                this.ActiveTab = "estatedetails";
            }
        }

        public override async Task MountAsync()
        {
            await PopulateTokenAndEstateId();
            await LoadEstate();
        }

        private async Task LoadEstate()
        {
            Queries.GetEstateQuery query = new(this.CorrelationId, AccessToken, EstateId);

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

            this.Operators = new List<EstateOperatorViewModel>();
            if (response.Data.Operators != null)
            {
                foreach (EstateOperatorModel estateOperatorModel in response.Data.Operators)
                {
                    this.Operators.Add(new EstateOperatorViewModel
                    {
                        Name = estateOperatorModel.Name,
                        OperatorId = estateOperatorModel.OperatorId,
                        RequireCustomMerchantNumber = estateOperatorModel.RequireCustomMerchantNumber,
                        RequireCustomTerminalNumber = estateOperatorModel.RequireCustomTerminalNumber
                    });
                }
            }
        }

        public void SetActiveTab(String activeTab)
        {
            this.ActiveTab = activeTab;
        }

        private async Task Handle(EstatePageEvents.ShowAddOperatorDialog obj)
        {
            this.ShowOperatorDialog = true;
        }

        private async Task Handle(EstatePageEvents.HideAddOperatorDialog obj)
        {
            this.ShowOperatorDialog = false;
        }

        private async Task Handle(EstatePageEvents.OperatorAssignedToEstateEvent obj)
        {
            this.Dispatch(new ShowMessage("Operator Assigned to Estate Successfully", ToastType.Success), Scope.Global);
            await LoadEstate();
        }

        private async Task Handle(EstatePageEvents.OperatorRemovedFromEstateEvent obj)
        {
            this.Dispatch(new ShowMessage("Operator Removed from Estate Successfully", ToastType.Success), Scope.Global);
            await LoadEstate();
        }

        public void AddOperator() => this.Dispatch(new EstatePageEvents.ShowAddOperatorDialog(), Scope.Global);

        public async Task RemoveOperator(Guid operatorId)
        {
            await this.PopulateTokenAndEstateId();

            Commands.RemoveOperatorFromEstateCommand removeOperatorFromEstateCommand =
                new(this.CorrelationId, this.AccessToken, this.EstateId, operatorId);
            Result result = await this.Mediator.Send(removeOperatorFromEstateCommand, CancellationToken.None);

            if (result.IsSuccess)
            {
                this.Dispatch(new EstatePageEvents.OperatorRemovedFromEstateEvent(), Scope.Global);
            }
            else
            {
                this.Dispatch(new ShowMessage("Error removing operator from Estate", ToastType.Error), Scope.Global);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class EstateOperatorViewModel
    {
        public String Name { get; set; }
        public Guid OperatorId { get; set; }
        public Boolean RequireCustomMerchantNumber { get; set; }
        public Boolean RequireCustomTerminalNumber { get; set; }
    }
}
