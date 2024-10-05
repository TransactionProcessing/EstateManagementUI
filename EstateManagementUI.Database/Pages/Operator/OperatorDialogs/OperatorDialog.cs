using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs;

public class OperatorDialog : SecureHydroComponent {
    private readonly IMediator Mediator;

    public OperatorDialog(IMediator mediator,
                          IPermissionsService permissionsService,
                          String operatorFunction) : base(ApplicationSections.Operator, operatorFunction,
        permissionsService) {
        this.Mediator = mediator;
    }

    public override async Task MountAsync() {
        if (this.OperatorId != Guid.Empty) {
            await this.LoadOperator(CancellationToken.None);
        }
    }

    private async Task LoadOperator(CancellationToken cancellationToken) {

        await this.PopulateTokenAndEstateId();

        Queries.GetOperatorQuery query = new Queries.GetOperatorQuery(this.AccessToken, this.EstateId, this.OperatorId);
        Result<OperatorModel> result = await this.Mediator.Send(query, cancellationToken);
        if (result.IsFailed) {
            // handle this
        }

        this.Name = result.Data.Name;
        this.RequireCustomTerminalNumber = result.Data.RequireCustomTerminalNumber;
        this.RequireCustomMerchantNumber = result.Data.RequireCustomMerchantNumber;
    }

    public Guid OperatorId { get; set; }

    [Required(ErrorMessage = "A name is required to create an Operator")]
    public string Name { get; set; }

    public bool RequireCustomMerchantNumber { get; set; }

    public bool RequireCustomTerminalNumber { get; set; }

    public void Close() => this.Dispatch(new OperatorPageEvents.HideNewOperatorDialog(), Scope.Global);

    public async Task Save() {
        if (!this.ModelState.IsValid) {
            return;
        }

        await this.PopulateTokenAndEstateId();

        if (this.OperatorId == Guid.Empty) {
            Commands.AddNewOperatorCommand command = new Commands.AddNewOperatorCommand(this.AccessToken, this.EstateId,
                Guid.NewGuid(), this.Name, this.RequireCustomMerchantNumber, this.RequireCustomTerminalNumber);

            Result result = await this.Mediator.Send(command, CancellationToken.None);

            if (result.IsFailed) {
                this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                return;
            }

            this.Dispatch(new OperatorPageEvents.OperatorCreatedEvent(), Scope.Global);
        }
        else {
            Commands.UpdateOperatorCommand command = new Commands.UpdateOperatorCommand(this.AccessToken, this.EstateId,
                this.OperatorId, this.Name, this.RequireCustomMerchantNumber, this.RequireCustomTerminalNumber);

            Result result = await this.Mediator.Send(command, CancellationToken.None);

            if (result.IsFailed) {
                this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                return;
            }

            this.Dispatch(new OperatorPageEvents.OperatorUpdatedEvent(), Scope.Global);
        }

        this.Close();
    }
}