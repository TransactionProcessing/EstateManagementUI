using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using MediatR;

namespace EstateManagementUI.Pages.Estate.EstateDetails;

[ExcludeFromCodeCoverage]
public class OperatorDialog : SecureHydroComponent {
    protected readonly IMediator Mediator;

    public OperatorDialog(IMediator mediator,
                          IPermissionsService permissionsService,
                          String estateFunction) : base(ApplicationSections.Estate, estateFunction, permissionsService) {
        this.Mediator = mediator;
    }
}
