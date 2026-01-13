using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using SimpleResults;
using static EstateManagementUI.Pages.Estate.EstatePageEvents;

namespace EstateManagementUI.Pages.Estate.EstateDetails
{
    public class AddOperatorDialog : OperatorDialog
    {
        [Display(Name = "Operator")]
        public OperatorListModel Operator { get; set; }

        [Display(Name = "Merchant Number")]
        public String MerchantNumber { get; set; }
        [Display(Name = "Terminal Number")]
        public String TerminalNumber { get; set; }

        public AddOperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, EstateFunctions.AddOperator) {
        }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            this.Operator = await DataHelperFunctions.GetOperatorsOld(this.CorrelationId, this.AccessToken, this.EstateId, this.Mediator);
        }

        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            AssignOperatorToEstateModel assignOperatorToEstateModel = new(){
                OperatorId = Guid.Parse(this.Operator.OperatorId),
                MerchantNumber = this.MerchantNumber,
                TerminalNumber = this.TerminalNumber
            };
            Commands.AssignOperatorToEstateCommand assignOperatorToEstateCommand =
                new(this.CorrelationId, this.AccessToken, this.EstateId, assignOperatorToEstateModel);
            Result result = await this.Mediator.Send(assignOperatorToEstateCommand, CancellationToken.None);

            if (result.IsSuccess) {
                this.Dispatch(new OperatorAssignedToEstateEvent(), Scope.Global);
            }
            else {
                this.Dispatch(new ShowMessage("Error assigning operator to Estate", ToastType.Error), Scope.Global);
            }

            await this.Close();
        }

        public async Task Close() => this.Dispatch(new EstatePageEvents.HideAddOperatorDialog(), Scope.Global);
    }
}
