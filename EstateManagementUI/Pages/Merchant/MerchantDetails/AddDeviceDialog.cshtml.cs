using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using SimpleResults;
using static EstateManagementUI.Pages.Merchant.MerchantPageEvents;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class AddDeviceDialog : DeviceDialog
    {
        [Display(Name = "MerchantDevice")]
        public String MerchantDevice { get; set; }

        public AddDeviceDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.AddContract) {
        }

        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            AssignDeviceToMerchantModel assignDeviceToMerchant = new(){
                DeviceIdentifier= this.MerchantDevice
            };
            Commands.AssignDeviceToMerchantCommand assignContractToMerchantCommand =
                new(this.CorrelationId, this.AccessToken, this.EstateId, this.MerchantId, assignDeviceToMerchant);
            Result result = await this.Mediator.Send(assignContractToMerchantCommand, CancellationToken.None);

            if (result.IsSuccess) {
                this.Dispatch(new DeviceAssignedToMerchantEvent(), Scope.Global);
            }
            else {
                this.Dispatch(new ShowMessage("Error assigning device to Merchant", ToastType.Error), Scope.Global);
            }

            await this.Close();
        }

        public async Task Close() => this.Dispatch(new MerchantPageEvents.HideAddDeviceDialog(), Scope.Global);
    }

    [ExcludeFromCodeCoverage]
    public class DeviceDialog : SecureHydroComponent {
        protected readonly IMediator Mediator;

        public Guid MerchantId { get; set; }

        public DeviceDialog(IMediator mediator,
                            IPermissionsService permissionsService,
                            String merchantFunction) : base(ApplicationSections.Merchant, merchantFunction, permissionsService) {
            this.Mediator = mediator;
        }
    }
}
