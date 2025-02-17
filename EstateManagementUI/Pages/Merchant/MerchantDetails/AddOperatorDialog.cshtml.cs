using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using SimpleResults;
using static EstateManagementUI.Pages.Merchant.MerchantDetails.MerchantPageEvents;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class AddOperatorDialog : OperatorDialog
    {
        [Display(Name = "Operator")]
        public OperatorListModel Operator { get; set; }

        [Display(Name = "Merchant Number")]
        public String MerchantNumber { get; set; }
        [Display(Name = "Terminal Number")]
        public String TerminalNumber { get; set; }

        public AddOperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.AddOperator) {
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.Operator = await DataHelperFunctions.GetOperators(this.AccessToken, this.EstateId, this.Mediator);
        }

        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            AssignOperatorToMerchantModel assignOperatorToMerchantModel = new(){
                OperatorId = Guid.Parse(this.Operator.OperatorId),
                MerchantNumber = this.MerchantNumber,
                TerminalNumber = this.TerminalNumber
            };
            Commands.AssignOperatorToMerchantCommand assignOperatorToMerchantCommand =
                new(this.AccessToken, this.EstateId, this.MerchantId, assignOperatorToMerchantModel);
            Result result = await this.Mediator.Send(assignOperatorToMerchantCommand, CancellationToken.None);

            if (result.IsSuccess) {
                this.Dispatch(new OperatorAssignedToMerchantEvent(), Scope.Global);
            }
            else {
                this.Dispatch(new ShowMessage("Error assigning operator to Merchant", ToastType.Error), Scope.Global);
            }

            await this.Close();
        }

        public async Task Close() => this.Dispatch(new MerchantPageEvents.HideAddOperatorDialog(), Scope.Global);
    }
}
