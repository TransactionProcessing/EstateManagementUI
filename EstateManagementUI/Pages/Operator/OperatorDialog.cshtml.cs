using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EstateManagement.DataTransferObjects.Requests.Merchant;
using EstateManagementUI.Pages.Shared.Components;
using static EstateManagementUI.Pages.Operator.OperatorPageEvents;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using MediatR;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Commands;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagementUI.Pages.Operator
{
    public class OperatorDialog : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public OperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Operator, OperatorFunctions.New, permissionsService) {
            this.Mediator = mediator;
        }

        public override async Task MountAsync()
        {
            if (this.OperatorId != Guid.Empty) {
                await this.LoadOperator(CancellationToken.None);
            }
        }

        private async Task LoadOperator(CancellationToken cancellationToken) {

            await this.PopulateTokenAndEstateId();

            Queries.GetOperatorQuery query =
                new Queries.GetOperatorQuery(this.AccessToken, this.EstateId, this.OperatorId);
            Result<OperatorModel> result =  await this.Mediator.Send(query, cancellationToken);
            if (result.IsFailed) {
                // handle this
            }

            this.Name = result.Data.Name;
            this.RequireCustomTerminalNumber = result.Data.RequireCustomTerminalNumber;
            this.RequireCustomMerchantNumber = result.Data.RequireCustomMerchantNumber;
        }

        public Guid OperatorId{ get; set; }

        [Required(ErrorMessage = "A name is required to create an Operator")]
        public String Name { get; set; }

        public Boolean RequireCustomMerchantNumber{ get; set; }

        public Boolean RequireCustomTerminalNumber { get; set; }

        public void Close() =>
            Dispatch(new HideNewOperatorDialog(), Scope.Global);

        public async Task Save() {
            if (!ModelState.IsValid)
            {
                return;
            }
            await this.PopulateTokenAndEstateId();

            if (this.OperatorId == Guid.Empty) {
                AddNewOperatorCommand command = new AddNewOperatorCommand(this.AccessToken, this.EstateId, Guid.NewGuid(),
                    this.Name, this.RequireCustomMerchantNumber, this.RequireCustomTerminalNumber);

                Result result = await this.Mediator.Send(command, CancellationToken.None);

                if (result.IsFailed) {
                    Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                    return;
                }
                
                Dispatch(new OperatorCreatedEvent(), Scope.Global);
            }
            else {
                UpdateOperatorCommand command = new UpdateOperatorCommand(this.AccessToken, this.EstateId, this.OperatorId,
                    this.Name, this.RequireCustomMerchantNumber, this.RequireCustomTerminalNumber);

                Result result = await this.Mediator.Send(command, CancellationToken.None);

                if (result.IsFailed)
                {
                    Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                    return;
                }

                Dispatch(new OperatorUpdatedEvent(), Scope.Global);
            }

            Close();
        }
    }

    public class OperatorPageEvents {
        public record ShowNewOperatorDialog;
        public record HideNewOperatorDialog;
        public record ShowEditOperatorDialog(Guid OperatorId);
        public record HideEditOperatorDialog;
        public record OperatorCreatedEvent;
        public record OperatorUpdatedEvent;
    }
}
