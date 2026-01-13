using MediatR;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;

namespace EstateManagmentUI.BusinessLogic.Requests;

[ExcludeFromCodeCoverage]
public record Commands {
    public record AddNewOperatorCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;
    public record UpdateOperatorCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid OperatorId, String OperatorName, Boolean RequireCustomMerchantNumber, Boolean RequireCustomTerminalNumber) : IRequest<Result>;

    public record AddMerchantCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, CreateMerchantModel CreateMerchantModel) : IRequest<Result>;

    public record UpdateMerchantCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId,Guid MerchantId, UpdateMerchantModel UpdateMerchantModel) : IRequest<Result>;

    public record UpdateMerchantAddressCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid MerchantId, AddressModel UpdatedAddressModel) : IRequest<Result>;

    public record UpdateMerchantContactCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid MerchantId, ContactModel UpdatedContactModel) : IRequest<Result>;

    public record AssignOperatorToMerchantCommand(CorrelationId CorrelationId, String AccessToken,
                                                  Guid EstateId,
                                                  Guid MerchantId,
                                                  AssignOperatorToMerchantModel AssignOperatorRequestModel) : IRequest<Result>;

    public record RemoveOperatorFromMerchantCommand(CorrelationId CorrelationId, String AccessToken,
                                                      Guid EstateId,
                                                      Guid MerchantId,
                                                      Guid OperatorId) : IRequest<Result>;

    public record AssignContractToMerchantCommand(CorrelationId CorrelationId, String AccessToken,
                                                  Guid EstateId,
                                                  Guid MerchantId,
                                                  AssignContractToMerchantModel AssignContractToMerchantModel) : IRequest<Result>;

    public record RemoveContractFromMerchantCommand(CorrelationId CorrelationId, String AccessToken,
                                                    Guid EstateId,
                                                    Guid MerchantId,
                                                    Guid ContractId) : IRequest<Result>;

    public record CreateContractCommand(CorrelationId CorrelationId, String AccessToken,
                                        Guid EstateId,
                                        CreateContractModel CreateContractModel) : IRequest<Result>;

    public record MakeDepositCommand(CorrelationId CorrelationId, String AccessToken,
                                     Guid EstateId,
                                     Guid MerchantId,
                                     MakeDepositModel MakeDepositModel) : IRequest<Result>;

    public record CreateContractProductCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid ContractId, CreateContractProductModel CreateContractProductModel) : IRequest<Result>;

    public record CreateContractProductTransactionFeeCommand(CorrelationId CorrelationId, String AccessToken, Guid EstateId, Guid ContractId, Guid ProductId, CreateContractProductTransactionFeeModel CreateContractProductTransactionFeeModel) : IRequest<Result>;

    public record AssignDeviceToMerchantCommand(CorrelationId CorrelationId, String AccessToken,
                                                  Guid EstateId,
                                                  Guid MerchantId,
                                                  AssignDeviceToMerchantModel AssignDeviceToMerchantModel) : IRequest<Result>;

    public record AssignOperatorToEstateCommand(CorrelationId CorrelationId, String AccessToken,
                                                  Guid EstateId,
                                                  AssignOperatorToEstateModel AssignOperatorRequestModel) : IRequest<Result>;

    public record RemoveOperatorFromEstateCommand(CorrelationId CorrelationId, String AccessToken,
                                                       Guid EstateId,
                                                       Guid OperatorId) : IRequest<Result>;
}