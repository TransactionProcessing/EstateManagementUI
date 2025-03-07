using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstateManagementUI.Pages.Contract.Contract
{
    [ExcludeFromCodeCoverage]
    public class ViewContract : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public ViewContract(IMediator mediator, IPermissionsService permissionsService) : base( ApplicationSections.Contract, ContractFunctions.View, permissionsService) {
            this.Mediator = mediator;
        }
        
        public void Close() => this.Location("/Contract/Index");

        public String OperatorName { get; set; }
        public String Name { get; set; }
        public override async Task MountAsync() {

            await this.PopulateTokenAndEstateId();

            if (this.ContractId != Guid.Empty)
            {
                await this.LoadContract(CancellationToken.None);
            }
        }

        private async Task LoadContract(CancellationToken cancellationToken) {
            await this.PopulateTokenAndEstateId();

            Queries.GetContractQuery query = new(this.AccessToken, this.EstateId, this.ContractId);
            Result<ContractModel> result = await this.Mediator.Send(query, cancellationToken);
            if (result.IsFailed)
            {
                // handle this
            }
            this.Name = result.Data.Description;
            //SelectListItem @operator = operatorList.Operators.Single(x => x.Value == result.Data.OperatorId.ToString());
            //this.OperatorName = @operator.Text;
        }

        public Guid ContractId { get; set; }
    }
}