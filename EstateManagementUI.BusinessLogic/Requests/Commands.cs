using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagmentUI.BusinessLogic.Requests;

[ExcludeFromCodeCoverage]
public record Commands {
    public record AddNewOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
    public record UpdateOperatorCommand(String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;

    public record AddMerchantCommand(String AccessToken, Guid EstateId, CreateMerchantModel CreateMerchantModel) : IRequest<Result>;

    public record UpdateMerchantCommand(String AccessToken, Guid EstateId,Guid MerchantId, UpdateMerchantModel UpdateMerchantModel) : IRequest<Result>;

    public record UpdateMerchantAddressCommand(String AccessToken, Guid EstateId, Guid MerchantId, AddressModel UpdatedAddressModel) : IRequest<Result>;

    public record UpdateMerchantContactCommand(String AccessToken, Guid EstateId, Guid MerchantId, ContactModel UpdatedContactModel) : IRequest<Result>;
}