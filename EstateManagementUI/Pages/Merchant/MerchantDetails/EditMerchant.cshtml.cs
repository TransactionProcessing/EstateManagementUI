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
using System.Diagnostics.CodeAnalysis;
using Hydro.Utils;
using System.Reflection.Metadata;
using EstateManagementUI.Pages.Components;
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
            Subscribe<MerchantPageEvents.HideAddOperatorDialog>(Handle);

            Subscribe<MerchantPageEvents.ShowAddContractDialog>(Handle);
            Subscribe<MerchantPageEvents.HideAddContractDialog>(Handle);

            Subscribe<MerchantPageEvents.ShowAddDeviceDialog>(Handle);
            Subscribe<MerchantPageEvents.HideAddDeviceDialog>(Handle);

            Subscribe<MerchantPageEvents.OperatorAssignedToMerchantEvent>(Handle);
            Subscribe<MerchantPageEvents.OperatorRemovedFromMerchantEvent>(Handle);

            this.Subscribe<MerchantPageEvents.ContractAssignedToMerchantEvent>(Handle);
            this.Subscribe<MerchantPageEvents.ContractRemovedFromMerchantEvent>(Handle);

            this.Subscribe<MerchantPageEvents.DeviceAssignedToMerchantEvent>(Handle);
        }

        public void SetActiveTab(String activeTab) {
            this.ActiveTab = activeTab;
        }

        [ExcludeFromCodeCoverage]
        public Boolean ShowOperatorDialog { get; set; }
        [ExcludeFromCodeCoverage]
        public Boolean ShowContractDialog { get; set; }
        [ExcludeFromCodeCoverage]
        public Boolean ShowDeviceDialog { get; set; }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.OperatorAssignedToMerchantEvent obj) {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Assigned to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ContractAssignedToMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Contract Assigned to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.DeviceAssignedToMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Device Added to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.OperatorRemovedFromMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Removed from Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ContractRemovedFromMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Contract Removed from Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ShowAddOperatorDialog obj) {
            this.ShowOperatorDialog = true;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ShowAddContractDialog obj)
        {
            this.ShowContractDialog = true;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ShowAddDeviceDialog obj)
        {
            this.ShowDeviceDialog= true;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.HideAddOperatorDialog obj) {
            this.ShowOperatorDialog = false;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.HideAddContractDialog obj)
        {
            this.ShowContractDialog= false;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.HideAddDeviceDialog obj)
        {
            this.ShowDeviceDialog = false;
        }

        public void AddOperator() => this.Dispatch(new MerchantPageEvents.ShowAddOperatorDialog(), Scope.Global);

        public void AddContract() => this.Dispatch(new MerchantPageEvents.ShowAddContractDialog(), Scope.Global);

        public void AddDevice() => this.Dispatch(new MerchantPageEvents.ShowAddDeviceDialog(), Scope.Global);

        public List<OptionItem> GetSettlementSchedules() => DataHelperFunctions.GetSettlementSchedules();
    }
}
