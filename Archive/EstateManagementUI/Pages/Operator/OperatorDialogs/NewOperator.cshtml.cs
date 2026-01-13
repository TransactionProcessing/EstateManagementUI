using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Operator.OperatorDialogs
{
    [ExcludeFromCodeCoverage]
    public class NewOperator : Operator {
        public NewOperator(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, OperatorFunctions.New){
            
        }
    }
}
