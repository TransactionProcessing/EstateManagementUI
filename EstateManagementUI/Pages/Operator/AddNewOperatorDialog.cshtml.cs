using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EstateManagement.DataTransferObjects.Requests.Merchant;
using EstateManagementUI.Pages.Shared.Components;
using static EstateManagementUI.Pages.Operator.OperatorEvents;
using static EstateManagementUI.Pages.Operator.OperatorPageEvents;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using MediatR;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Commands;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EstateManagementUI.Pages.Operator
{
    public class AddNewOperatorDialog : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public AddNewOperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Operator, OperatorFunctions.New, permissionsService) {
            this.Mediator = mediator;
        }

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

            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            AddNewOperatorCommand command = new AddNewOperatorCommand(accessToken,estateId, Guid.NewGuid(), this.Name, this.RequireCustomMerchantNumber,
                this.RequireCustomTerminalNumber);

            Result result = await this.Mediator.Send(command, CancellationToken.None);

            if (result.IsFailed) {
                Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                return;
            }

            
            Dispatch(new OperatorCreatedEvent(), Scope.Global);
            Close();
        }
    }

    public class OperatorPageEvents {
        public record ShowNewOperatorDialog;
        public record HideNewOperatorDialog;
        //public record ShowOrderValidationMessage(string Message);
    }

    public class OperatorEvents {
        public record OperatorCreatedEvent;
    }
}
