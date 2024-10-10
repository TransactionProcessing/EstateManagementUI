using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using Hydro.Utils;
using System.Reflection.Metadata;
using Hydro;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class EditMerchant : Merchant {
        
        public EditMerchant(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.Edit)
        {
            if (String.IsNullOrEmpty(this.ActiveTab) == true) {
                this.ActiveTab = "merchantdetails";
            }

            Subscribe<MerchantPageEvents.ShowAddOperatorDialog>(Handle);
            Subscribe<MerchantPageEvents.ShowEditOperatorDialog>(Handle);

            Subscribe<MerchantPageEvents.HideAddOperatorDialog>(Handle);
            Subscribe<MerchantPageEvents.HideEditOperatorDialog>(Handle);

            Subscribe<MerchantPageEvents.OperatorAssignedToMerchantEvent>(Handle);
            Subscribe<MerchantPageEvents.OperatorRemovedFromMerchantEvent>(Handle);
        }

        public void SetActiveTab(String activeTab) {
            this.ActiveTab = activeTab;
        }

        public Boolean ShowOperatorDialog { get; set; }
        public Guid OperatorId { get; set; }

        private async Task Handle(MerchantPageEvents.OperatorAssignedToMerchantEvent obj) {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Assigned to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        private async Task Handle(MerchantPageEvents.OperatorRemovedFromMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Removed from Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }


        private async Task Handle(MerchantPageEvents.ShowAddOperatorDialog obj) {
            this.ShowOperatorDialog = true;
            this.OperatorId = Guid.Empty;
        }

        private async Task Handle(MerchantPageEvents.ShowEditOperatorDialog obj) {
            this.ShowOperatorDialog = true;
            this.OperatorId = obj.OperatorId;
        }

        private async Task Handle(MerchantPageEvents.HideAddOperatorDialog obj) {
            this.ShowOperatorDialog = false;
            this.OperatorId = Guid.Empty;
        }

        private async Task Handle(MerchantPageEvents.HideEditOperatorDialog obj) {
            this.ShowOperatorDialog = false;
            this.OperatorId = Guid.Empty;
        }

        public async Task AddOperator() => this.Dispatch(new MerchantPageEvents.ShowAddOperatorDialog(), Scope.Global);
    }
}
