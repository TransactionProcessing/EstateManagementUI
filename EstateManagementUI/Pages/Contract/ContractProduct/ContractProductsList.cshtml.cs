using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Merchant;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using EstateManagementUI.BusinessLogic.PermissionService;

namespace EstateManagementUI.Pages.Contract.ContractProduct
{
    public class ContractProductsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public ContractProductsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Contract, ContractFunctions.ViewList, permissionsService)
        {
            Mediator = mediator;
            ContractProducts = new List<ViewModels.ContractProduct>();
        }

        public Guid ContractId { get; set; }
        public string ContractName { get; set; }
        public List<ViewModels.ContractProduct> ContractProducts { get; set; }

        public override async Task MountAsync()
        {
            await GetContract();
        }

        private async Task GetContract()
        {
            await this.PopulateTokenAndEstateId();

            Queries.GetContractQuery query = new Queries.GetContractQuery(this.AccessToken, this.EstateId, ContractId);

            ContractModel response = await Mediator.Send(query, CancellationToken.None);

            ContractName = response.Description;

            List<ViewModels.ContractProduct> resultList = new();
            foreach (ContractProductModel responseContractProduct in response.ContractProducts)
            {
                resultList.Add(new ViewModels.ContractProduct
                {
                    ContractProductId = responseContractProduct.ContractProductId,
                    DisplayText = responseContractProduct.DisplayText,
                    NumberOfFees = responseContractProduct.NumberOfFees,
                    ProductName = responseContractProduct.ProductName,
                    ProductType = responseContractProduct.ProductType,
                    Value = responseContractProduct.Value
                });
            }

            IEnumerable<ViewModels.ContractProduct> sortQuery = this.Sorting switch
            {
                (ContractProductSorting.DisplayText, Ascending: false) => resultList.OrderBy(p => p.DisplayText),
                (ContractProductSorting.DisplayText, Ascending: true) => resultList.OrderByDescending(p => p.DisplayText),
                (ContractProductSorting.NumberOfFees, Ascending: false) => resultList.OrderBy(p => p.NumberOfFees),
                (ContractProductSorting.NumberOfFees, Ascending: true) => resultList.OrderByDescending(p => p.NumberOfFees),
                (ContractProductSorting.ProductName, Ascending: false) => resultList.OrderBy(p => p.ProductName),
                (ContractProductSorting.ProductName, Ascending: true) => resultList.OrderByDescending(p => p.ProductName),
                (ContractProductSorting.ProductType, Ascending: false) => resultList.OrderBy(p => p.ProductType),
                (ContractProductSorting.ProductType, Ascending: true) => resultList.OrderByDescending(p => p.ProductType),
                (ContractProductSorting.Value, Ascending: false) => resultList.OrderBy(p => p.Value),
                (ContractProductSorting.Value, Ascending: true) => resultList.OrderByDescending(p => p.Value),

                _ => resultList.AsEnumerable()
            };
            ContractProducts = sortQuery.ToList();

        }

        public async Task Sort(ContractProductSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await GetContract();
        }

        public (ContractProductSorting Column, bool Ascending) Sorting { get; set; }

        public async Task Edit(Guid contractProductId)
        {

        }

        public async Task ViewProductFees(Guid contractProductId)
        {
            this.Location(this.Url.Page("/Contract/ContractProductTransactionFees", new { ContractId = this.ContractId, ContractProductId = contractProductId }));
        }
    }

    public enum ContractProductSorting
    {
        DisplayText,
        NumberOfFees,
        ProductName,
        ProductType,
        Value
    }
}
