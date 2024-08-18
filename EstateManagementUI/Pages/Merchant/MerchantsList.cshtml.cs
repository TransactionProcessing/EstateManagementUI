using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;

namespace EstateManagementUI.Pages.Merchant
{
    public class MerchantsList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public MerchantsList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Merchant, MerchantFunctions.ViewList, permissionsService)
        {
            this.Mediator = mediator;
            this.Merchants = new List<ViewModels.Merchant>();
        }

        public List<ViewModels.Merchant> Merchants { get; set; }

        public override async Task MountAsync() {
            await this.GetMerchants();
        }

        private async Task GetMerchants() {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);

            Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(accessToken, estateId);

            List<MerchantModel> response = await this.Mediator.Send(query, CancellationToken.None);

            var resultList = new List<ViewModels.Merchant>();
            foreach (MerchantModel merchantModel in response)
            {
                resultList.Add(new ViewModels.Merchant()
                {
                    Id = merchantModel.MerchantId,
                    Name = merchantModel.MerchantName,
                    SettlementSchedule = merchantModel.SettlementSchedule,
                    Reference = merchantModel.MerchantReference,
                    AddressLine1 = merchantModel.AddressLine1,
                    ContactName = merchantModel.ContactName,
                    Town = merchantModel.Town
                });
            }

            IEnumerable<ViewModels.Merchant> sortQuery = this.Sorting switch {
                (MerchantSorting.Name, Ascending: false) => resultList.OrderBy(p => p.Name),
                (MerchantSorting.Name, Ascending: true) => resultList.OrderByDescending(p => p.Name),
                (MerchantSorting.Reference, Ascending: false) => resultList.OrderBy(p => p.Reference),
                (MerchantSorting.Reference, Ascending: true) => resultList.OrderByDescending(p => p.Reference),
                (MerchantSorting.SettlementSchedule, Ascending: false) => resultList.OrderBy(p => p.SettlementSchedule),
                (MerchantSorting.SettlementSchedule, Ascending: true) => resultList.OrderByDescending(p => p.SettlementSchedule),
                (MerchantSorting.Contact, Ascending: false) => resultList.OrderBy(p => p.ContactName),
                (MerchantSorting.Contact, Ascending: true) => resultList.OrderByDescending(p => p.ContactName),
                (MerchantSorting.AddressLine1, Ascending: false) => resultList.OrderBy(p => p.AddressLine1),
                (MerchantSorting.AddressLine1, Ascending: true) => resultList.OrderByDescending(p => p.AddressLine1),
                (MerchantSorting.Town, Ascending: false) => resultList.OrderBy(p => p.Town),
                (MerchantSorting.Town, Ascending: true) => resultList.OrderByDescending(p => p.Town),
                _ => resultList.AsEnumerable()
            };

            this.Merchants = sortQuery.ToList();
        }

        public async Task Sort(MerchantSorting value) {
            Sorting = (Column: value, Ascending: Sorting.Column == value && !Sorting.Ascending);

            await this.GetMerchants();
        }

        public (MerchantSorting Column, bool Ascending) Sorting { get; set; }
    }

    public enum MerchantSorting {
        Name,
        Reference,
        SettlementSchedule,
        Contact,
        AddressLine1,
        Town
    }
}
