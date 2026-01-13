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
using SimpleResults;

namespace EstateManagementUI.Pages.Estate.UsersList
{
    public class UsersList : SecureHydroComponent
    {
        private readonly IMediator Mediator;

        public UsersList(IMediator mediator, IPermissionsService permissionsService) : base(ApplicationSections.Estate, EstateFunctions.ViewUsersList, permissionsService)
        {
            Mediator = mediator;
            Users = new List<User>();
        }

        public List<User> Users { get; set; }

        public override async Task MountAsync()
        {
            await PopulateTokenAndEstateId();

            CorrelationId c = CorrelationIdHelper.New();
            Queries.GetEstateQuery query = new(c, AccessToken, EstateId);

            Result<EstateModel> response = await Mediator.Send(query, CancellationToken.None);
            if (response.IsFailed)
            {
                // TODO: Handle error properly, e.g., show a message to the user
            }
            foreach (SecurityUserModel securityUserModel in response.Data.SecurityUsers)
            {
                Users.Add(new User()
                {
                    Id = securityUserModel.SecurityUserId,
                    EmailAddress = securityUserModel.EmailAddress
                });
            }
        }
    }


}
