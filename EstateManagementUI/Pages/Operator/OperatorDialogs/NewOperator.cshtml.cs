using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    public class NewOperator : Operator {
        public NewOperator(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.New){
            
        }
    }
}
