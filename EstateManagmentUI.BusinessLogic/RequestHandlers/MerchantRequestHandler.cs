using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class MerchantRequestHandler : IRequestHandler<MerchantQueries.GetMerchantsQuery, Result<List<MerchantModels.MerchantListModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantQuery, Result<MerchantModels.MerchantModel>>,
    IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Result>,
    IRequestHandler<MerchantCommands.AddOperatorToMerchantCommand, Result>,
    IRequestHandler<MerchantCommands.CreateMerchantCommand, Result>,
    IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Result>,
    IRequestHandler<MerchantCommands.RemoveContractFromMerchantCommand, Result>,
    IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand, Result>,
    IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand, Result>,
    IRequestHandler<MerchantCommands.UpdateMerchantCommand, Result>,
    IRequestHandler<MerchantCommands.AssignContractToMerchantCommand, Result>,
    IRequestHandler<MerchantQueries.GetRecentMerchantsQuery, Result<List<MerchantModels.RecentMerchantsModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantsForDropDownQuery, Result<List<MerchantModels.MerchantDropDownModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantContractsQuery, Result<List<MerchantModels.MerchantContractModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantOperatorsQuery, Result<List<MerchantModels.MerchantOperatorModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantDevicesQuery, Result<List<MerchantModels.MerchantDeviceModel>>>,
    IRequestHandler<MerchantQueries.GetMerchantKpiQuery, Result<MerchantModels.MerchantKpiModel>>
{

    private readonly IApiClient ApiClient;

    public MerchantRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<MerchantModels.MerchantListModel>>> Handle(MerchantQueries.GetMerchantsQuery request,
                                                                             CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchants(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.AddMerchantDeviceCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddDeviceToMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.AddOperatorToMerchantCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddOperatorToMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.CreateMerchantCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.CreateMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.MakeMerchantDepositCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.MakeMerchantDeposit(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.RemoveContractFromMerchantCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.RemoveContractFromMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.RemoveOperatorFromMerchantCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.RemoveOperatorFromMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.SwapMerchantDeviceCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.SwapMerchantDevice(request, cancellationToken);
    }


    public async Task<Result> Handle(MerchantCommands.UpdateMerchantCommand request,
                                     CancellationToken cancellationToken) {
        Result updateMerchantResult = await this.ApiClient.UpdateMerchant(request, cancellationToken);
        if (updateMerchantResult.IsFailed)
            return ResultHelpers.CreateFailure(updateMerchantResult);

        Result updateMerchantAddressResult = await this.ApiClient.UpdateMerchantAddress(request, cancellationToken);
        if (updateMerchantAddressResult.IsFailed)
            return ResultHelpers.CreateFailure(updateMerchantAddressResult);
        Result updateMerchantContactResult = await this.ApiClient.UpdateMerchantContact(request, cancellationToken);
        if (updateMerchantContactResult.IsFailed)
            return ResultHelpers.CreateFailure(updateMerchantContactResult);

        return Result.Success();
    }

    public async Task<Result<MerchantModels.MerchantModel>> Handle(MerchantQueries.GetMerchantQuery request,
                                                                   CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchant(request, cancellationToken);
    }

    public async Task<Result> Handle(MerchantCommands.AssignContractToMerchantCommand request,
                                     CancellationToken cancellationToken) {
        return await this.ApiClient.AddContractToMerchant(request, cancellationToken);
    }

    public async Task<Result<List<MerchantModels.RecentMerchantsModel>>> Handle(MerchantQueries.GetRecentMerchantsQuery request,
                                                                                CancellationToken cancellationToken) {
        return await this.ApiClient.GetRecentMerchants(request, cancellationToken);
    }

    public async Task<Result<List<MerchantModels.MerchantDropDownModel>>> Handle(MerchantQueries.GetMerchantsForDropDownQuery request,
                                                                                 CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchants(request, cancellationToken);
    }

    public async Task<Result<List<MerchantModels.MerchantContractModel>>> Handle(MerchantQueries.GetMerchantContractsQuery request,
                                                                                 CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantContracts(request, cancellationToken);
    }

    public async Task<Result<List<MerchantModels.MerchantOperatorModel>>> Handle(MerchantQueries.GetMerchantOperatorsQuery request,
                                                                                 CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantOperators(request, cancellationToken);
    }

    public async Task<Result<List<MerchantModels.MerchantDeviceModel>>> Handle(MerchantQueries.GetMerchantDevicesQuery request,
                                                                               CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantDevices(request, cancellationToken);
    }

    public async Task<Result<MerchantModels.MerchantKpiModel>> Handle(MerchantQueries.GetMerchantKpiQuery request,
                                                                      CancellationToken cancellationToken) {
        return await this.ApiClient.GetMerchantKpi(request, cancellationToken);
    }
}