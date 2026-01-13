using SimpleResults;

namespace EstateManagementUI.BusinessLogic.PermissionService
{
    public interface IPermissionsService {
        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName,
                                        String function);

        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName);
    }
}
