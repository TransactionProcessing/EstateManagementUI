using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagmentUI.BusinessLogic.Requests;

[ExcludeFromCodeCoverage]
public record Commands {
    public record AddNewOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
    public record UpdateOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;

    public record AddNewMerchantCommand(String AccessToken, Guid EstateId, CreateMerchantModel CreateMerchantModel) : IRequest<Result>;
}