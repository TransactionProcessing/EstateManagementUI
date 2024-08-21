using MediatR;
using SimpleResults;

namespace EstateManagmentUI.BusinessLogic.Requests;

public record Commands {
    public record AddNewOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
    public record UpdateOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
}