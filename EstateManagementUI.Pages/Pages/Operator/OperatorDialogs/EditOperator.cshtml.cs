using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    public class EditOperator : Operator
    {
        public EditOperator(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.Edit)
        {

        }
    }
}
