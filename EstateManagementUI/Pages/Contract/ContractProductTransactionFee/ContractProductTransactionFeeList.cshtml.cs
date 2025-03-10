using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Merchant;

namespace EstateManagementUI.Pages.Contract.ContractProductTransactionFee
{
    public class ContractProductTransactionFeeList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public ContractProductTransactionFeeList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Contract, ContractFunctions.ViewProductFeesList, permissionsService)
        {
            Mediator = mediator;
            ContractProductTransactionFees = new List<ViewModels.ContractProductTransactionFee>();
        }

        public Guid ContractId { get; set; }
        public Guid ContractProductId { get; set; }
        public string ContractProductName { get; set; }
        public List<ViewModels.ContractProductTransactionFee> ContractProductTransactionFees { get; set; }

        public override async Task MountAsync()
        {
            await GetContract();
        }

        private async Task GetContract()
        {
            await this.PopulateTokenAndEstateId();

            Queries.GetContractQuery query = new Queries.GetContractQuery(this.AccessToken, this.EstateId, ContractId);

            ContractModel response = await Mediator.Send(query, CancellationToken.None);

            ContractProductModel contractProduct =
                response.ContractProducts.SingleOrDefault(cp => cp.ContractProductId == this.ContractProductId);
            this.ContractProductName = contractProduct.ProductName;

            List<ViewModels.ContractProductTransactionFee> resultList = new();
            foreach (ContractProductTransactionFeeModel responseContractProductTransactionFee in contractProduct.ContractProductTransactionFees)
            {
                resultList.Add(new ViewModels.ContractProductTransactionFee()
                {
                    ContractProductTransactionFeeId = responseContractProductTransactionFee.ContractProductTransactionFeeId,
                    CalculationType = responseContractProductTransactionFee.CalculationType,
                    Description = responseContractProductTransactionFee.Description,
                    Value = responseContractProductTransactionFee.Value,
                    FeeType = responseContractProductTransactionFee.FeeType
                });
            }

            IEnumerable<ViewModels.ContractProductTransactionFee> sortQuery = this.Sorting switch
            {
                (ContractProductTransactionFeeSorting.CalculationType, Ascending: false) => resultList.OrderBy(p => p.CalculationType),
                (ContractProductTransactionFeeSorting.CalculationType, Ascending: true) => resultList.OrderByDescending(p => p.CalculationType),
                (ContractProductTransactionFeeSorting.Description, Ascending: false) => resultList.OrderBy(p => p.Description),
                (ContractProductTransactionFeeSorting.Description, Ascending: true) => resultList.OrderByDescending(p => p.Description),
                (ContractProductTransactionFeeSorting.Value, Ascending: false) => resultList.OrderBy(p => p.Value),
                (ContractProductTransactionFeeSorting.Value, Ascending: true) => resultList.OrderByDescending(p => p.Value),
                (ContractProductTransactionFeeSorting.FeeType, Ascending: false) => resultList.OrderBy(p => p.FeeType),
                (ContractProductTransactionFeeSorting.FeeType, Ascending: true) => resultList.OrderByDescending(p => p.FeeType),
                _ => resultList.AsEnumerable()
            };
            this.ContractProductTransactionFees = sortQuery.ToList();

        }

        public async Task Sort(ContractProductTransactionFeeSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await GetContract();
        }

        public (ContractProductTransactionFeeSorting Column, bool Ascending) Sorting { get; set; }

        public async Task NewContractProductTransactionFee()
        {
            this.Location("/Contract/NewContractProductTransactionFee", new { ContractId = this.ContractId.ToString(), ProductId = this.ContractProductId });
        }
    }

    public enum ContractProductTransactionFeeSorting
    {
        CalculationType,
        Description,
        Value,
        FeeType
    }
}
