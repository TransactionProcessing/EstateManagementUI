using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleResults;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class NewMerchant : Merchant
    {
        public NewMerchant(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.New)
        {

        }
    }
}