using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Client;

public interface IUserAccessTokenProvider
{
    Task<Result<string>> GetAccessTokenAsync(CancellationToken cancellationToken);
}
