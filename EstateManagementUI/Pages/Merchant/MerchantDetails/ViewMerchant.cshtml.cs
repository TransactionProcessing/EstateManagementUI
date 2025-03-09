using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using Hydro.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    [ExcludeFromCodeCoverage]
    public class ViewMerchant : Merchant {
        
        public ViewMerchant(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.View)
        {
            if (String.IsNullOrEmpty(this.ActiveTab) == true) {
                this.ActiveTab = "merchantdetails";
            }
        }

        public void SetActiveTab(String activeTab) {
            this.ActiveTab = activeTab;
        }
    }
}


