using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagmentUI.BusinessLogic.Requests;
using Lamar;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Queries;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class MediatorRegistry : ServiceRegistry {
    public MediatorRegistry() {
        this.AddTransient<IMediator, Mediator>();

        this.RegisterEstateRequestHandler();
        this.RegisterMerchantRequestHandler();
        this.RegisterOperatorRequestHandler();
        this.RegisterContractRequestHandler();
        this.RegisterFileRequestHandler();
        this.RegisterReportingRequestHandler();
    }

    private void RegisterEstateRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetEstateQuery, EstateModel>, EstateRequestHandler>();
    }

    private void RegisterMerchantRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetMerchantsQuery, Result<List<MerchantModel>>>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetMerchantQuery, Result<MerchantModel>>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.AddMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.UpdateMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.UpdateMerchantAddressCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.UpdateMerchantContactCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.AssignOperatorToMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.RemoveOperatorFromMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.AssignContractToMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.RemoveContractFromMerchantCommand, Result>, MerchantRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.MakeDepositCommand, Result>, MerchantRequestHandler>();
    }

    private void RegisterOperatorRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetOperatorsQuery, Result<List<OperatorModel>>>, OperatorRequestHandler>();

        this.AddSingleton<IRequestHandler<Commands.AddNewOperatorCommand, Result>, OperatorRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.UpdateOperatorCommand, Result>, OperatorRequestHandler>();
    }

    private void RegisterContractRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetContractsQuery, List<ContractModel>>, ContractRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetContractQuery, Result<ContractModel>>, ContractRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetOperatorQuery, Result<OperatorModel>>, OperatorRequestHandler>();

        this.AddSingleton<IRequestHandler<Commands.CreateContractCommand, Result>, ContractRequestHandler>();
        this.AddSingleton<IRequestHandler<Commands.CreateContractProductCommand, Result>, ContractRequestHandler>();
    }

    private void RegisterFileRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetFileImportLogsListQuery, Result<List<FileImportLogModel>>>, FileRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetFileImportLogQuery, Result<FileImportLogModel>>, FileRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetFileDetailsQuery, Result<FileDetailsModel>>, FileRequestHandler>();
    }

    private void RegisterReportingRequestHandler() {
        this.AddSingleton<IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTodaysSalesQuery, Result<TodaysSalesModel>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTodaysSettlementQuery, Result<TodaysSettlementModel>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>, ReportingRequestHandler>();

        this.AddSingleton<IRequestHandler<Queries.GetMerchantKpiQuery, Result<MerchantKpiModel>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>, ReportingRequestHandler>();
        this.AddSingleton<IRequestHandler<Queries.GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>, ReportingRequestHandler>();

        this.AddSingleton<IRequestHandler<Queries.GetLastSettlementQuery, Result<LastSettlementModel>>, ReportingRequestHandler>();

    }
}