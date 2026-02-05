using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface IOperatorUIService
{
    Task<Result<List<OperatorModels.OperatorModel>>> GetOperators(CorrelationId correlationId, Guid estateId);
    Task<Result<OperatorModels.OperatorModel>> GetOperator(CorrelationId correlationId, Guid estateId, Guid operatorId);

    Task<Result> UpdateOperator(CorrelationId correlationId, Guid estateId, Guid operatorId, OperatorModels.EditOperatorModel editOperatorModel);
    Task<Result> CreateOperator(CorrelationId correlationId, Guid estateId, OperatorModels.CreateOperatorModel createOperatorModel);
}
public class OperatorUIService : IOperatorUIService
{
    private readonly IMediator Mediator;
    public OperatorUIService(IMediator mediator)
    {
        this.Mediator = mediator;
    }
    public async Task<Result<List<OperatorModels.OperatorModel>>> GetOperators(CorrelationId correlationId, Guid estateId)
    {
        var result = await this.Mediator.Send(new OperatorQueries.GetOperatorsQuery(correlationId, estateId));
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var operators = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(operators);
    }

    public async Task<Result<OperatorModels.OperatorModel>> GetOperator(CorrelationId correlationId,
                                                         Guid estateId,
                                                         Guid operatorId) {
        var result = await this.Mediator.Send(new OperatorQueries.GetOperatorQuery(correlationId, estateId, operatorId));
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var @operator = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(@operator);
    }

    public async Task<Result> UpdateOperator(CorrelationId correlationId,
                                             Guid estateId,
                                             Guid operatorId,
                                             OperatorModels.EditOperatorModel editOperatorModel) {
        var command = new OperatorCommands.UpdateOperatorCommand(correlationId, estateId, operatorId, editOperatorModel.OperatorName, editOperatorModel.RequireCustomMerchantNumber, editOperatorModel.RequireCustomTerminalNumber);

        var result = await Mediator.Send(command);
        return result;
    }

    public async Task<Result> CreateOperator(CorrelationId correlationId,
                                             Guid estateId,
                                             OperatorModels.CreateOperatorModel createOperatorModel) {
        // Create operator
        var command = new OperatorCommands.CreateOperatorCommand(correlationId,
            estateId,
            createOperatorModel.OperatorName!,
            createOperatorModel.RequireCustomMerchantNumber,
            createOperatorModel.RequireCustomTerminalNumber
        );

        var result = await Mediator.Send(command);
        return result;
    }
}