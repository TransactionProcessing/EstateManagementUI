using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Pages.Operator.OperatorDialogs;
using System.Reflection.Metadata;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using Microsoft.AspNetCore.Mvc;
using Shared.Logger;

namespace EstateManagementUI.Pages.Merchant.MerchantsList
{
    public class MerchantsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public Guid MerchantId { get; set; }
        public bool ShowDialog { get; set; }

        public MerchantsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Merchant, MerchantFunctions.ViewList, permissionsService)
        {
            Mediator = mediator;
            Merchants = new List<ViewModels.Merchant>();
        }

        public void Add() => this.Location("/Merchant/NewMerchant");

        public async Task View(Guid merchantId) =>
            this.Location("/Merchant/ViewMerchant", new { MerchantId = merchantId });

        public async Task Edit(Guid merchantId) =>
            this.Location("/Merchant/EditMerchant", new { MerchantId = merchantId });

        public async Task MakeDeposit(Guid merchantId) =>
            this.Location("/Merchant/MakeDeposit", new { MerchantId = merchantId });

        public List<ViewModels.Merchant> Merchants { get; set; }

        public override async Task MountAsync()
        {
            await GetMerchants();
        }

        private async Task GetMerchants()
        {
                await PopulateTokenAndEstateId();

                Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(AccessToken, EstateId);

                List<MerchantModel> response = await Mediator.Send(query, CancellationToken.None);

                List<ViewModels.Merchant> resultList = new();
                foreach (MerchantModel merchantModel in response) {
                    resultList.Add(new ViewModels.Merchant {
                        Id = merchantModel.MerchantId,
                        Name = merchantModel.MerchantName,
                        SettlementSchedule = merchantModel.SettlementSchedule,
                        Reference = merchantModel.MerchantReference,
                        AddressLine1 = merchantModel.Address.AddressLine1,
                        ContactName = merchantModel.Contact.ContactName,
                        Town = merchantModel.Address.Town
                    });
                }

                IEnumerable<ViewModels.Merchant> sortQuery = Sorting switch {
                    (MerchantSorting.Name, Ascending: false) => resultList.OrderBy(p => p.Name),
                    (MerchantSorting.Name, Ascending: true) => resultList.OrderByDescending(p => p.Name),
                    (MerchantSorting.Reference, Ascending: false) => resultList.OrderBy(p => p.Reference),
                    (MerchantSorting.Reference, Ascending: true) => resultList.OrderByDescending(p => p.Reference),
                    (MerchantSorting.SettlementSchedule, Ascending: false) => resultList.OrderBy(p =>
                        p.SettlementSchedule),
                    (MerchantSorting.SettlementSchedule, Ascending: true) => resultList.OrderByDescending(p =>
                        p.SettlementSchedule),
                    (MerchantSorting.Contact, Ascending: false) => resultList.OrderBy(p => p.ContactName),
                    (MerchantSorting.Contact, Ascending: true) => resultList.OrderByDescending(p => p.ContactName),
                    (MerchantSorting.AddressLine1, Ascending: false) => resultList.OrderBy(p => p.AddressLine1),
                    (MerchantSorting.AddressLine1, Ascending: true) =>
                        resultList.OrderByDescending(p => p.AddressLine1),
                    (MerchantSorting.Town, Ascending: false) => resultList.OrderBy(p => p.Town),
                    (MerchantSorting.Town, Ascending: true) => resultList.OrderByDescending(p => p.Town),
                    _ => resultList.AsEnumerable()
                };

                Merchants = sortQuery.ToList();
        }

        public async Task Sort(MerchantSorting value)
        {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await GetMerchants();
        }

        public (MerchantSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum MerchantSorting
    {
        Name,
        Reference,
        SettlementSchedule,
        Contact,
        AddressLine1,
        Town
    }
}
