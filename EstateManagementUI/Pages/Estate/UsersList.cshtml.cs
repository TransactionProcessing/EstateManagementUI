using Hydro;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;

namespace EstateManagementUI.Pages.Estate
{
    public class UsersList : HydroComponent
    {
        private readonly IMediator Mediator;

        public UsersList(IMediator mediator)
        {
            this.Mediator = mediator;
            this.Users = new List<User>();
        }

        public List<User> Users { get; set; }

        public override async Task MountAsync()
        {
            String accessToken = await this.HttpContext.GetTokenAsync("access_token");

            Guid estateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
            
            Queries.GetEstateQuery query = new Queries.GetEstateQuery(accessToken, estateId);

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
