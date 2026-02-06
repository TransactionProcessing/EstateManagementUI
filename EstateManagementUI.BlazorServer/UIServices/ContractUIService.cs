using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;
using static FastExpressionCompiler.ExpressionCompiler;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Result = SimpleResults.Result;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface IContractUIService {
    Task<Result<List<ContractModels.ContractModel>>> GetContracts(CorrelationId correlationId,
                                                                  Guid estateId);

    Task<Result<ContractModels.ContractModel>> GetContract(CorrelationId correlationId,
                                                           Guid estateId,
                                                           Guid contractId);

    Task<Result> AddProductToContract(CorrelationId correlationId,
                                      Guid estateId,
                                      Guid contractId,
                                      ContractModels.AddProductModel productModel);

    Task<Result> AddTransactionFeeToProduct(CorrelationId correlationId,
                                            Guid estateId,
                                            Guid contractId,
                                            Guid contractProductId,
                                            ContractModels.AddTransactionFeeModel feeModel);

    Task<Result> RemoveTransactionFeeFromProduct(CorrelationId correlationId,
                                            Guid estateId,
                                            Guid contractId,
                                            Guid contractProductId,
                                            Guid transactionFeeId);

    Task<Result> CreateContract(CorrelationId correlationId, Guid estateId, ContractModels.CreateContractFormModel createContractFormModel);
}

public class ContractUIService : IContractUIService {
    private readonly IMediator Mediator;

    public ContractUIService(IMediator mediator) {
        this.Mediator = mediator;
    }

    public async Task<Result<List<ContractModels.ContractModel>>> GetContracts(CorrelationId correlationId,
                                                                               Guid estateId) {
        var result = await this.Mediator.Send(new ContractQueries.GetContractsQuery(correlationId, estateId));
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var contracts = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(contracts);
    }

    public async Task<Result<ContractModels.ContractModel>> GetContract(CorrelationId correlationId,
                                                                        Guid estateId,
                                                                        Guid contractId) {
        var result = await this.Mediator.Send(new ContractQueries.GetContractQuery(correlationId, estateId, contractId));
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var contract = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(contract);
    }

    public async Task<Result> AddProductToContract(CorrelationId correlationId,
                                                   Guid estateId,
                                                   Guid contractId,
                                                   ContractModels.AddProductModel productModel) {
        ContractCommands.AddProductToContractCommand command = new(CorrelationIdHelper.New(), estateId, contractId, productModel.ProductName!, productModel.DisplayText!, productModel.IsVariableValue ? null : productModel.Value);

        Result result = await this.Mediator.Send(command);

        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> AddTransactionFeeToProduct(CorrelationId correlationId,
                                                         Guid estateId,
                                                         Guid contractId,
                                                         Guid contractProductId,
                                                         ContractModels.AddTransactionFeeModel feeModel) {
        var command = new ContractCommands.AddTransactionFeeToProductCommand(CorrelationIdHelper.New(), estateId, contractId, contractProductId, feeModel.Description!, feeModel.FeeValue!.Value, feeModel.CalculationType, feeModel.FeeType);

        Result result = await this.Mediator.Send(command);

        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> RemoveTransactionFeeFromProduct(CorrelationId correlationId,
                                                              Guid estateId,
                                                              Guid contractId,
                                                              Guid contractProductId,
                                                              Guid transactionFeeId) {
        var command = new ContractCommands.RemoveTransactionFeeFromProductCommand(CorrelationIdHelper.New(), estateId, contractId, contractProductId, transactionFeeId);

        Result result = await this.Mediator.Send(command);

        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }

    public async Task<Result> CreateContract(CorrelationId correlationId,
                                             Guid estateId,
                                             ContractModels.CreateContractFormModel createContractFormModel) {
        var command = new ContractCommands.CreateContractCommand(CorrelationIdHelper.New(), estateId, createContractFormModel.Description!, Guid.Parse(createContractFormModel.OperatorId!));

        Result result = await this.Mediator.Send(command);

        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        return Result.Success();
    }
}