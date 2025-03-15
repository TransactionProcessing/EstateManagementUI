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

namespace EstateManagementUI.Pages.Contract.ContractProduct
{
    [ExcludeFromCodeCoverage]
    public class NewContractProduct : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public NewContractProduct(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Contract, ContractFunctions.AddProduct, permissionsService) {
            this.Mediator = mediator;
            
            Subscribe<ContractPageEvents.ContractProductCreatedEvent>(Handle);
        }

        [ExcludeFromCodeCoverage]
        private async Task Handle(ContractPageEvents.ContractProductCreatedEvent obj)
        {
            this.Dispatch(new ShowMessage("Contract Product Created Successfully", ToastType.Success), Scope.Global);
            await Task.Delay(1000); // TODO: might be a better way of handling this
            this.Close();
        }

        public void Close() => this.Location("/Contract/ContractProducts",new { ContractId = this.ContractId });

        public String ProductTypeId { get; set; }
        public List<OptionItem> ProductTypes { get; set; }

        public String Name { get; set; }
        public String DisplayText { get; set; }
        public String Value { get; set; }
        public Boolean IsVariableValue { get; set; }
        public override async Task MountAsync() {

            await this.PopulateTokenAndEstateId();
        }

        public async Task Save()
        {
            if (!this.ModelState.IsValid)
            {
                return;
            }
            await this.PopulateTokenAndEstateId();
            
            await this.CreateNeContractProduct();
        }

        public List<OptionItem> GetProductTypes()
        {
            // Might have async issues :|
            this.PopulateTokenAndEstateId().Wait();
            return DataHelperFunctions.GetProductTypes(this.AccessToken, this.EstateId).Result;
        }

        private async Task CreateNeContractProduct() {
            Commands.CreateContractProductCommand command = new(this.AccessToken, this.EstateId, this.ContractId, new CreateContractProductModel
            {
                DisplayText = this.DisplayText,
                IsVariable= this.IsVariableValue,
                Type = Int32.Parse(this.ProductTypeId),
                Name = this.Name,
                Value = this.IsVariableValue switch {
                    false => decimal.Parse(this.Value),
                    _ => null
                }
            });
            
            Result result = await this.Mediator.Send(command, CancellationToken.None);
            if (result.IsSuccess) {
                this.Dispatch(new ContractPageEvents.ContractProductCreatedEvent(), Scope.Global);
            }
            // TODO: handle the failure case
        }

        public Guid ContractId { get; set; }
    }
}
