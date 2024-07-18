using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using EstateManagementUI.Common;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Newtonsoft.Json;

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

    public record User {
        public string EmailAddress { get; set; }

        public Guid Id { get; set; }
    }
}
