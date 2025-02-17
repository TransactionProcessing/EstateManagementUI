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
using static EstateManagementUI.Pages.Merchant.MerchantDetails.MerchantPageEvents;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class AddContractDialog : ContractDialog
    {
        [Display(Name = "Contract")]
        public ContractListModel Contract { get; set; }

        public AddContractDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.AddContract) {
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.Contract = await DataHelperFunctions.GetContracts(this.AccessToken, this.EstateId, this.Mediator);
        }

        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            AssignContractToMerchantModel assignContractToMerchantModel = new(){
                ContractId= Guid.Parse(this.Contract.ContractId)
            };
            Commands.AssignContractToMerchantCommand assignContractToMerchantCommand =
                new(this.AccessToken, this.EstateId, this.MerchantId, assignContractToMerchantModel);
            Result result = await this.Mediator.Send(assignContractToMerchantCommand, CancellationToken.None);

            if (result.IsSuccess) {
                this.Dispatch(new ContractAssignedToMerchantEvent(), Scope.Global);
            }
            else {
                this.Dispatch(new ShowMessage("Error assigning contract to Merchant", ToastType.Error), Scope.Global);
            }

            await this.Close();
        }

        public async Task Close() => this.Dispatch(new MerchantPageEvents.HideAddContractDialog(), Scope.Global);
    }

    [ExcludeFromCodeCoverage]
    public class ContractDialog : SecureHydroComponent {
        protected readonly IMediator Mediator;

        public Guid MerchantId { get; set; }

        public ContractDialog(IMediator mediator,
                              IPermissionsService permissionsService,
                              String merchantFunction) : base(ApplicationSections.Merchant, merchantFunction, permissionsService) {
            this.Mediator = mediator;
        }
    }
}
