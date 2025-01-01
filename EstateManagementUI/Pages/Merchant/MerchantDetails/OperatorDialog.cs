using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using MediatR;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

[ExcludeFromCodeCoverage]
public class OperatorDialog : SecureHydroComponent {
    protected readonly IMediator Mediator;

    public Guid MerchantId { get; set; }

    public OperatorDialog(IMediator mediator,
                          IPermissionsService permissionsService,
                          String merchantFunction) : base(ApplicationSections.Merchant, merchantFunction, permissionsService) {
        this.Mediator = mediator;
    }
}