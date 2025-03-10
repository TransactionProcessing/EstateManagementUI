using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.Pages.Estate.OperatorList;
using EstateManagementUI.ViewModels;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using System.Reflection.Metadata;
using EstateManagementUI.Pages.Components;
using Hydro;

namespace EstateManagementUI.Pages.Contract.Contract
{
    [ExcludeFromCodeCoverage]
    public class NewContract : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public NewContract(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Contract, ContractFunctions.New, permissionsService)
        {
            this.Mediator = mediator;

            Subscribe<ContractPageEvents.ContractCreatedEvent>(Handle);
            Subscribe<ContractPageEvents.ContractUpdatedEvent>(Handle);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(ContractPageEvents.ContractCreatedEvent obj)
        {
            this.Dispatch(new ShowMessage("Contract Created Successfully", ToastType.Success), Scope.Global);
            await Task.Delay(1000); // TODO: might be a better way of handling this
            this.Close();
        }

        public void Close() => this.Location("/Contract/Index");

        [ExcludeFromCodeCoverage]
        private async Task Handle(ContractPageEvents.ContractUpdatedEvent obj)
        {
            this.Dispatch(new ShowMessage("Contract Updated Successfully", ToastType.Success), Scope.Global);
            await Task.Delay(1000); // TODO: might be a better way of handling this
            this.Close();
        }

        public String OperatorId { get; set; } = "";


        public String Name { get; set; }
        public override async Task MountAsync()
        {

            await this.PopulateTokenAndEstateId();

            if (this.ContractId != Guid.Empty)
            {
                //await this.LoadContract(CancellationToken.None);
            }
        }

        public async Task Save()
        {
            if (!this.ModelState.IsValid)
            {
                return;
            }
            await this.PopulateTokenAndEstateId();

            Task t = this.ContractId switch
            {
                _ when this.ContractId == Guid.Empty => this.CreateNeContract(),
                _ => this.UpdateExitingContract(),

            };

            await t;
        }

        private async Task UpdateExitingContract()
        {
        }

        public List<OptionItem> GetOperators()
        {
            // Might have async issues :|
            this.PopulateTokenAndEstateId().Wait();
            return DataHelperFunctions.GetOperators(this.AccessToken, this.EstateId, this.Mediator).Result;
        }

        private async Task CreateNeContract()
        {

            Commands.CreateContractCommand createContractCommand = new(this.AccessToken, this.EstateId, new CreateContractModel
            {
                OperatorId = Guid.Parse(this.OperatorId),
                Description = this.Name
            });

            Result result = await this.Mediator.Send(createContractCommand, CancellationToken.None);
            if (result.IsSuccess)
            {
                this.Dispatch(new ContractPageEvents.ContractCreatedEvent(), Scope.Global);
            }
            // TODO: handle the failure case
        }

        public Guid ContractId { get; set; }
    }
}


public class ContractPageEvents {
    public record ContractCreatedEvent;
    public record ContractUpdatedEvent;
    public record ContractProductCreatedEvent;
    public record ContractProductUpdatedEvent;
    public record ContractProductTransactionFeeCreatedEvent;

}