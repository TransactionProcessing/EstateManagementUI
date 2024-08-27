using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Estate
{
    public class UsersList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public UsersList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.ViewEstateUsers, permissionsService)
        {
            this.Mediator = mediator;
            this.Users = new List<User>();
        }

        public List<User> Users { get; set; }

        public override async Task MountAsync()
        {
            await this.PopulateTokenAndEstateId();

            Queries.GetEstateQuery query = new Queries.GetEstateQuery(this.AccessToken, this.EstateId);

            EstateModel response = await this.Mediator.Send(query, CancellationToken.None);

            foreach (SecurityUserModel securityUserModel in response.SecurityUsers) {
                this.Users.Add(new User() {
                    Id = securityUserModel.SecurityUserId,
                    EmailAddress = securityUserModel.EmailAddress
                });
            }
        }
    }

    
}
