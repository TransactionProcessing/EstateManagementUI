using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Components;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

namespace EstateManagementUI.Pages.Contract.ContractProductTransactionFee
{
    [ExcludeFromCodeCoverage]
    public class NewContractProductTransactionFee : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public NewContractProductTransactionFee(IMediator mediator, IPermissionsService permissionsService) : base( ApplicationSections.Contract, ContractFunctions.AddProduct, permissionsService) {
            this.Mediator = mediator;

            this.Subscribe<ContractPageEvents.ContractProductTransactionFeeCreatedEvent>(this.Handle);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(ContractPageEvents.ContractProductTransactionFeeCreatedEvent arg) {
            this.Dispatch(new ShowMessage("Transaction Fee Created Successfully", ToastType.Success), Scope.Global);
            await Task.Delay(1000); // TODO: might be a better way of handling this
            this.Close();
        }

        public void Close() => this.Location("/Contract/ContractProductTransactionFees", new { ContractId = this.ContractId, ContractProductId = this.ProductId });
 
        public Guid ContractId { get; set; }
        public Guid ProductId { get; set; }
        public String Description { get; set; }
        public Decimal Value { get; set; }
        public String CalculationTypeId { get; set; }
        public List<OptionItem> FeeTypes { get; set; }
        public String FeeTypeId { get; set; }

        public List<OptionItem> GetFeeCalculationTypes() => DataHelperFunctions.GetCalculationTypes();

        public List<OptionItem> GetFeeTypes() => DataHelperFunctions.GetFeeTypes();

        public async Task Save() {
            await this.PopulateTokenAndEstateId();

            Commands.CreateContractProductTransactionFeeCommand command = new(this.AccessToken, this.EstateId, this.ContractId, this.ProductId, new CreateContractProductTransactionFeeModel
            {
                Description = this.Description,
                Value = this.Value,
                CalculationType = (CalculationType)Int32.Parse(this.CalculationTypeId),
                FeeType = (FeeType)Int32.Parse(this.FeeTypeId)
            });

            Result result = await this.Mediator.Send(command, CancellationToken.None);
            if (result.IsSuccess)
            {
                this.Dispatch(new ContractPageEvents.ContractProductTransactionFeeCreatedEvent(), Scope.Global);
            }
            // TODO: handle the failure case
        }
    }
}
