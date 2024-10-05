using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    public class NewOperatorDialog : OperatorDialog {
        public NewOperatorDialog(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.New){
            
        }
    }
}
