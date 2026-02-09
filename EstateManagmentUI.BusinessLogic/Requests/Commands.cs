using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class Commands
{
    public record CreateMerchantUserCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string EmailAddress, string Password) : IRequest<Result>;
    
    
    
}