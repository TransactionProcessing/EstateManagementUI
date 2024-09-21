using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    public class EditOperatorDialog : OperatorDialog
    {
        public EditOperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.Edit)
        {

        }
    }
}
