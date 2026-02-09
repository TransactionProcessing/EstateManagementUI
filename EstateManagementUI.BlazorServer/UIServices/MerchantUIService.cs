using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;
using Spectre.Console;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using static EstateManagementUI.BlazorServer.Models.MerchantModels;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface IMerchantUIService {

    Task<Result<MerchantModels.MerchantModel>> GetMerchant(CorrelationId correlationId,
                                                           Guid estateId,
                                                           Guid merchantId);

    Task<Result<List<MerchantModels.MerchantListModel>>> GetMerchants(CorrelationId correlationId,
                                                                      Guid estateId,
                                                                      String name,
                                                                      String reference,
                                                                      Int32? settlementSchedule,
                                                                      String region,
                                                                      String postCode);

    Task<Result<List<MerchantModels.MerchantOperatorModel>>> GetMerchantOperators(CorrelationId correlationId,
                                                                                  Guid estateId,
                                                                                  Guid merchantId);

    Task<Result<List<MerchantModels.MerchantContractModel>>> GetMerchantContracts(CorrelationId correlationId,
                                                                                  Guid estateId,
                                                                                  Guid merchantId);

    Task<Result<List<MerchantModels.MerchantDeviceModel>>> GetMerchantDevices(CorrelationId correlationId,
                                                                              Guid estateId,
                                                                              Guid merchantId);

    Task<Result> CreateMerchant(CorrelationId correlationId,
                                Guid estateId,
                                Guid merchantId,
                                MerchantModels.CreateMerchantModel createMerchantModel);

    Task<Result> UpdateMerchant(CorrelationId correlationId,
                                Guid estateId,
                                Guid merchantId,
                                MerchantModels.MerchantEditModel editMerchantModel);

    Task<Result> AddOperatorToMerchant(CorrelationId correlationId,
                                       Guid estateId,
                                       Guid merchantId,
                                       Guid operatorId,
                                       String? merchantNumber,
                                       String? terminalNumber);

    Task<Result> RemoveOperatorFromMerchant(CorrelationId correlationId,
                                            Guid estateId,
                                            Guid merchantId,
                                            Guid operatorId);

    Task<Result> AssignContractToMerchant(CorrelationId correlationId,
                                          Guid estateId,
                                          Guid merchantId,
                                          Guid contractId);

    Task<Result> RemoveContractFromMerchant(CorrelationId correlationId,
                                            Guid estateId,
                                            Guid merchantId,
                                            Guid contractId);

    Task<Result> AddMerchantDevice(CorrelationId correlationId,
                                   Guid estateId,
                                   Guid merchantId,
                                   String deviceIdentifier);

    Task<Result> SwapMerchantDevice(CorrelationId correlationId,
                                    Guid estateId,
                                    Guid merchantId,
                                    String originalDeviceIdentifier,
                                    String newDeviceIdentifier);

    Task<Result> MakeMerchantDeposit(CorrelationId correlationId,
                                     Guid estateId,
                                     Guid merchantId,
                                     MerchantModels.DepositModel depositModel);

    //Contract UI Service
    //    GetContractsForDropDownQuery


}

public class MerchantUIService : IMerchantUIService {
    private readonly IMediator Mediator;

    public MerchantUIService(IMediator mediator) {
        this.Mediator = mediator;
    }

    public async Task<Result<MerchantModels.MerchantModel>> GetMerchant(CorrelationId correlationId,
                                                                        Guid estateId,
                                                                        Guid merchantId) {
        MerchantQueries.GetMerchantQuery query = new(correlationId, estateId, merchantId);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchant = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchant);
    }

    public async Task<Result<List<MerchantModels.MerchantListModel>>> GetMerchants(CorrelationId correlationId,
                                                                                   Guid estateId,
                                                                                   String name,
                                                                                   String reference,
                                                                                   Int32? settlementSchedule,
                                                                                   String region,
                                                                                   String postCode) {
        MerchantQueries.GetMerchantsQuery query = new(correlationId, estateId, name, reference, settlementSchedule, region, postCode);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchantList = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchantList);
    }

    public async Task<Result<List<MerchantModels.MerchantOperatorModel>>> GetMerchantOperators(CorrelationId correlationId,
                                                                                               Guid estateId,
                                                                                               Guid merchantId) {
        MerchantQueries.GetMerchantOperatorsQuery query = new(correlationId, estateId, merchantId);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchantOperatorList = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchantOperatorList);
    }

    public async Task<Result<List<MerchantModels.MerchantContractModel>>> GetMerchantContracts(CorrelationId correlationId,
                                                                                               Guid estateId,
                                                                                               Guid merchantId) {
        MerchantQueries.GetMerchantContractsQuery query = new(correlationId, estateId, merchantId);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchantContractList = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchantContractList);
    }

    public async Task<Result<List<MerchantModels.MerchantDeviceModel>>> GetMerchantDevices(CorrelationId correlationId,
                                                                                           Guid estateId,
                                                                                           Guid merchantId) {
        MerchantQueries.GetMerchantDevicesQuery query = new(correlationId, estateId, merchantId);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchantDeviceList = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchantDeviceList);
    }

    public async Task<Result> CreateMerchant(CorrelationId correlationId,
                                             Guid estateId,
                                             Guid merchantId,
                                             MerchantModels.CreateMerchantModel createMerchantModel) {

        MerchantCommands.MerchantAddress address = new(Guid.NewGuid(), createMerchantModel.AddressLine1, createMerchantModel.Town, createMerchantModel.Region, createMerchantModel.PostCode, createMerchantModel.Country);
        MerchantCommands.MerchantContact contact = new(Guid.NewGuid(), createMerchantModel.ContactName, createMerchantModel.EmailAddress, createMerchantModel.PhoneNumber);
        MerchantCommands.CreateMerchantCommand command = new(correlationId, estateId, merchantId, createMerchantModel.MerchantName, createMerchantModel.SettlementSchedule, 
            address, contact);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> UpdateMerchant(CorrelationId correlationId,
                                             Guid estateId,
                                             Guid merchantId,
                                             MerchantModels.MerchantEditModel editMerchantModel) {
        MerchantCommands.MerchantAddress address = new(Guid.NewGuid(), editMerchantModel.AddressLine1, editMerchantModel.Town, editMerchantModel.Region, editMerchantModel.PostalCode, editMerchantModel.Country);
        MerchantCommands.MerchantContact contact = new(Guid.NewGuid(), editMerchantModel.ContactName, editMerchantModel.ContactEmailAddress, editMerchantModel.ContactPhoneNumber);

        MerchantCommands.UpdateMerchantCommand command = new(correlationId, estateId, merchantId, editMerchantModel.MerchantName, editMerchantModel.SettlementSchedule, 
            address,contact);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> AddOperatorToMerchant(CorrelationId correlationId,
                                                    Guid estateId,
                                                    Guid merchantId,
                                                    Guid operatorId,
                                                    String? merchantNumber,
                                                    String? terminalNumber) {
        MerchantCommands.AddOperatorToMerchantCommand command = new(correlationId, estateId, merchantId, operatorId, merchantNumber, terminalNumber);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> RemoveOperatorFromMerchant(CorrelationId correlationId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         Guid operatorId) {
        MerchantCommands.RemoveOperatorFromMerchantCommand command = new(correlationId, estateId, merchantId, operatorId);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> AssignContractToMerchant(CorrelationId correlationId,
                                                       Guid estateId,
                                                       Guid merchantId,
                                                       Guid contractId) {
        MerchantCommands.AssignContractToMerchantCommand command = new(correlationId, estateId, merchantId, contractId);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> RemoveContractFromMerchant(CorrelationId correlationId,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         Guid contractId) {
        MerchantCommands.RemoveContractFromMerchantCommand command = new(correlationId, estateId, merchantId, contractId);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> AddMerchantDevice(CorrelationId correlationId,
                                                Guid estateId,
                                                Guid merchantId,
                                                String deviceIdentifier) {
        MerchantCommands.AddMerchantDeviceCommand command = new(correlationId, estateId, merchantId, deviceIdentifier);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> SwapMerchantDevice(CorrelationId correlationId,
                                                 Guid estateId,
                                                 Guid merchantId,
                                                 String originalDeviceIdentifier,
                                                 String newDeviceIdentifier) {
        MerchantCommands.SwapMerchantDeviceCommand command = new(correlationId, estateId, merchantId, originalDeviceIdentifier, newDeviceIdentifier);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> MakeMerchantDeposit(CorrelationId correlationId,
                                                  Guid estateId,
                                                  Guid merchantId,
                                                  MerchantModels.DepositModel depositModel) {
        MerchantCommands.MakeMerchantDepositCommand command = new(correlationId, estateId, merchantId, depositModel.Amount, depositModel.Date, depositModel.Reference);
        var result = await this.Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }
}