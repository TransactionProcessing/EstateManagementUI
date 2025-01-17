using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagement.DataTransferObjects.Requests.Merchant;
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

    public record AssignOperatorToMerchantCommand(String AccessToken,
                                                  Guid EstateId,
                                                  Guid MerchantId,
                                                  AssignOperatorToMerchantModel AssignOperatorRequestModel) : IRequest<Result>;

    public record RemoveOperatorFromMerchantCommand(String AccessToken,
                                                      Guid EstateId,
                                                      Guid MerchantId,
                                                      Guid OperatorId) : IRequest<Result>;

    public record AssignContractToMerchantCommand(String AccessToken,
                                                  Guid EstateId,
                                                  Guid MerchantId,
                                                  AssignContractToMerchantModel AssignContractToMerchantModel) : IRequest<Result>;

    public record RemoveContractFromMerchantCommand(String AccessToken,
                                                    Guid EstateId,
                                                    Guid MerchantId,
                                                    Guid ContractId) : IRequest<Result>;

    public record CreateContractCommand(String AccessToken,
                                        Guid EstateId,
                                        CreateContractModel CreateContractModel) : IRequest<Result>;
}