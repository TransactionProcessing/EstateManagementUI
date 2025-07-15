using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Components;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using Hydro.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails
{
    public class MakeDeposit : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public MakeDeposit(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Merchant, MerchantFunctions.MakeDeposit, permissionsService) {
            this.Mediator = mediator;

            Subscribe<MerchantPageEvents.DepositMadeEvent>(Handle);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.DepositMadeEvent obj)
        {
            this.Dispatch(new ShowMessage("Merchant Deposit Made Successfully", ToastType.Success), Scope.Global);
            await Task.Delay(1000); // TODO: might be a better way of handling this
            this.Close();
        }

        public void Close() => this.Location("/Merchant/Index");
        /*
        public MakeMerchantDeposit(IMediator mediator, IPermissionsService permissionsService) : base(mediator, permissionsService, MerchantFunctions.Edit)
        {
            if (String.IsNullOrEmpty(this.ActiveTab) == true) {
                this.ActiveTab = "merchantdetails";
            }

            Subscribe<MerchantPageEvents.ShowAddOperatorDialog>(Handle);
            Subscribe<MerchantPageEvents.HideAddOperatorDialog>(Handle);

            Subscribe<MerchantPageEvents.ShowAddContractDialog>(Handle);
            Subscribe<MerchantPageEvents.HideAddContractDialog>(Handle);

            Subscribe<MerchantPageEvents.OperatorAssignedToMerchantEvent>(Handle);
            Subscribe<MerchantPageEvents.OperatorRemovedFromMerchantEvent>(Handle);

            this.Subscribe<MerchantPageEvents.ContractAssignedToMerchantEvent>(Handle);
            this.Subscribe<MerchantPageEvents.ContractRemovedFromMerchantEvent>(Handle);
        }

        public void SetActiveTab(String activeTab) {
            this.ActiveTab = activeTab;
        }

        public Boolean ShowOperatorDialog { get; set; }
        public Boolean ShowContractDialog { get; set; }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.OperatorAssignedToMerchantEvent obj) {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Assigned to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ContractAssignedToMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Contract Assigned to Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.OperatorRemovedFromMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Operator Removed from Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ContractRemovedFromMerchantEvent obj)
        {
            await Task.Delay(1000);
            this.Dispatch(new ShowMessage("Contract Removed from Merchant Successfully", ToastType.Success), Scope.Global);
            await this.LoadMerchant(CancellationToken.None);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ShowAddOperatorDialog obj) {
            this.ShowOperatorDialog = true;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.ShowAddContractDialog obj)
        {
            this.ShowContractDialog = true;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.HideAddOperatorDialog obj) {
            this.ShowOperatorDialog = false;
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(MerchantPageEvents.HideAddContractDialog obj)
        {
            this.ShowContractDialog= false;
        }

        public void AddOperator() => this.Dispatch(new MerchantPageEvents.ShowAddOperatorDialog(), Scope.Global);

        public void AddContract() => this.Dispatch(new MerchantPageEvents.ShowAddContractDialog(), Scope.Global);

        public List<OptionItem> GetSettlementSchedules() => DataHelperFunctions.GetSettlementSchedules();
        */
        public Guid MerchantId { get; set; }
        public override async Task MountAsync()
        {
            if (this.MerchantId != Guid.Empty)
            {
                await this.LoadMerchant(CancellationToken.None);
            }
        }
        public string Name { get; set; }

        protected async Task LoadMerchant(CancellationToken cancellationToken) {
            await this.PopulateTokenAndEstateId();

            Queries.GetMerchantQuery query = new(this.CorrelationId, this.AccessToken, this.EstateId, this.MerchantId);
            Result<MerchantModel> result = await this.Mediator.Send(query, cancellationToken);
            //if (result.IsFailed) {
            //    // handle this
            //}

            this.Name = result.Data.MerchantName;
        }
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public String Reference { get; set; }
        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            Commands.MakeDepositCommand command = new(this.CorrelationId, this.AccessToken, this.EstateId, this.MerchantId, new BusinessLogic.Models.MakeDepositModel
            {
                Amount = this.Amount,
                Date = this.Date,
                Reference = this.Reference
            });

            Result result = await this.Mediator.Send(command, CancellationToken.None);

            if (result.IsFailed)
            {
                this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                return;
            }

            this.Dispatch(new MerchantPageEvents.DepositMadeEvent(), Scope.Global);
        }
    }
}
