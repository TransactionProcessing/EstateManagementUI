using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    [ExcludeFromCodeCoverage]
    public class EditOperator : Operator
    {
        public EditOperator(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.Edit)
        {

        }
    }
}
