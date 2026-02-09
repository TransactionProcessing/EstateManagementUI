using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface IEstateUIService
{
    Task<Result<EstateModel>> LoadEstate(CorrelationId correlationId, Guid estateId);

    Task<Result> AddOperatorToEstate(CorrelationId correlationId, Guid estateId, String selectedOperatorId);
    Task<Result> RemoveOperatorFromEstate(CorrelationId correlationId, Guid estateId, Guid selectedOperatorId);
}

public class EstateUIService : IEstateUIService
{
    private readonly IMediator Mediator;

    public EstateUIService(IMediator mediator)
    {
        this.Mediator = mediator;
    }

    public async Task<Result<EstateModel>> LoadEstate(CorrelationId correlationId,
                                                      Guid estateId)
    {
        Task<Result<BusinessLogic.Models.EstateModels.EstateModel>> estateTask = Mediator.Send(new EstateQueries.GetEstateQuery(correlationId, estateId));
        Task<Result<List<BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>> merchantTask = Mediator.Send(new MerchantQueries.GetRecentMerchantsQuery(correlationId, estateId));
        Task<Result<List<BusinessLogic.Models.ContractModels.RecentContractModel>>> contractsTask = Mediator.Send(new ContractQueries.GetRecentContractsQuery(correlationId, estateId));
        Task<Result<List<BusinessLogic.Models.OperatorModels.OperatorModel>>> assignedOperatorsTask = Mediator.Send(new EstateQueries.GetAssignedOperatorsQuery(correlationId, estateId));
        Task<Result<List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>> allOperatorsTask = Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(correlationId, estateId));

        await Task.WhenAll(estateTask, merchantTask, contractsTask, assignedOperatorsTask, allOperatorsTask);

        if (estateTask.Result.IsFailed)
            return ResultHelpers.CreateFailure(estateTask.Result);

        if (merchantTask.Result.IsFailed)
            return ResultHelpers.CreateFailure(merchantTask.Result);
        var merchants = ModelFactory.ConvertFrom(merchantTask.Result.Data);

        if (contractsTask.Result.IsFailed)
            return ResultHelpers.CreateFailure(contractsTask.Result);
        var contracts = ModelFactory.ConvertFrom(contractsTask.Result.Data);

        if (assignedOperatorsTask.Result.IsFailed)
            return ResultHelpers.CreateFailure(assignedOperatorsTask.Result);
        var assignedOperators = ModelFactory.ConvertFrom(assignedOperatorsTask.Result.Data);

        if (allOperatorsTask.Result.IsFailed)
            return ResultHelpers.CreateFailure(allOperatorsTask.Result);

        List<OperatorModels.OperatorDropDownModel> unfiltered = ModelFactory.ConvertFrom(allOperatorsTask.Result.Data);
        var availableOperators = unfiltered.Where(u => !assignedOperators.Any(a => a.OperatorId == u.OperatorId)).Select(u => new OperatorModels.OperatorDropDownModel { OperatorId = u.OperatorId, OperatorName = u.OperatorName }).ToList();
        var estateModel = estateTask.Result.Data;
        var resultModel = new EstateModel(estateModel.EstateId, estateModel.EstateName, estateModel.Reference);
        resultModel = resultModel with
        {
            MerchantCount = estateModel.Merchants.Count,
            AllOperators = availableOperators,
            AssignedOperators = assignedOperators,
            ContractCount = estateModel.Contracts.Count,
            RecentContracts = contracts,
            RecentMerchants = merchants,
            UserCount = estateModel.Users.Count,
        };

        return Result.Success(resultModel);
    }

    public async Task<Result> AddOperatorToEstate(CorrelationId correlationId,
                                                  Guid estateId, String selectedOperatorId)
    {
        var operatorId = Guid.Parse(selectedOperatorId);

        var command = new EstateCommands.AddOperatorToEstateCommand(correlationId, estateId, operatorId);

        var result = await Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);

        return Result.Success();
    }

    public async Task<Result> RemoveOperatorFromEstate(CorrelationId correlationId,
                                                       Guid estateId,
                                                       Guid operatorId)
    {
        var command = new EstateCommands.RemoveOperatorFromEstateCommand(correlationId, estateId, operatorId);

        var result = await Mediator.Send(command);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);

        return Result.Success();
    }
}