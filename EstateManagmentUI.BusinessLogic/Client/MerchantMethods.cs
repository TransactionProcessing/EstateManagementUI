using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SecurityService.DataTransferObjects.Responses;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<MerchantKpiModel>> GetMerchantKpi(MerchantQueries.GetMerchantKpiQuery request, CancellationToken cancellationToken);
        Task<Result<List<RecentMerchantsModel>>> GetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery request, CancellationToken cancellationToken);
        Task<Result<List<RecentContractModel>>> GetRecentContracts(Queries.GetRecentContractsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantListModel>>> GetMerchants(MerchantQueries.GetMerchantsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantDropDownModel>>> GetMerchants(MerchantQueries.GetMerchantsForDropDownQuery request, CancellationToken cancellationToken);
        Task<Result<MerchantModel>> GetMerchant(MerchantQueries.GetMerchantQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantOperatorModel>>> GetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantContractModel>>> GetMerchantContracts(MerchantQueries.GetMerchantContractsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantDeviceModel>>> GetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> AddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand request, CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {

        public async Task<Result> AddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand request,
                                                             CancellationToken cancellationToken)
        {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiRequest = new AssignOperatorRequest { TerminalNumber = request.TerminalNumber, MerchantNumber = request.MerchantNumber, OperatorId = request.OperatorId };
            
            var apiResult = await this.TransactionProcessorClient.AssignOperatorToMerchant(token.Data, request.EstateId, request.MerchantId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand request,
                                                             CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.TransactionProcessorClient.RemoveContractFromMerchant(token.Data, request.EstateId, request.MerchantId, request.ContractId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand request,
                                                             CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.TransactionProcessorClient.RemoveOperatorFromMerchant(token.Data, request.EstateId, request.MerchantId, request.OperatorId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result<MerchantKpiModel>> GetMerchantKpi(MerchantQueries.GetMerchantKpiQuery request,
                                                                   CancellationToken cancellationToken) {

            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantKpi(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantKpiModel merchantKpiModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(merchantKpiModel);
        }

        public async Task<Result<List<RecentMerchantsModel>>> GetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery request,
                                                                             CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetRecentMerchants(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<RecentMerchantsModel> recentMerchantsModels = apiResult.Data.ToRecentMerchant();

            return Result.Success(recentMerchantsModels);
        }

        public async Task<Result<List<RecentContractModel>>> GetRecentContracts(Queries.GetRecentContractsQuery request,
                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Contract>> apiResult = await this.EstateReportingApiClient.GetRecentContracts(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<RecentContractModel> recentContractModels = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(recentContractModels);
        }

        public async Task<Result<List<MerchantListModel>>> GetMerchants(MerchantQueries.GetMerchantsQuery request,
                                                                        CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetMerchants(token.Data, request.EstateId, 
                request.Name, request.Reference, request.SettlementSchedule, request.Region, request.PostCode,cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<MerchantListModel> merchantList = apiResult.Data.ToMerchantList();

            return Result.Success(merchantList);
        }

        public async Task<Result<MerchantModel>> GetMerchant(MerchantQueries.GetMerchantQuery request,
                                                             CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<Merchant> apiResult = await this.EstateReportingApiClient.GetMerchant(token.Data, request.EstateId, request.MerchantId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantModel merchant = apiResult.Data.ToMerchant();

            return Result.Success(merchant);
        }

        public async Task<Result<List<MerchantOperatorModel>>> GetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery request,
                                                                                    CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantOperators(token.Data, request.EstateId, request.MerchantId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            var merchantOperators = apiResult.Data.ToMerchantOperators();

            return Result.Success(merchantOperators);
        }

        public async Task<Result<List<MerchantContractModel>>> GetMerchantContracts(MerchantQueries.GetMerchantContractsQuery request,
                                                                                    CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantContracts(token.Data, request.EstateId, request.MerchantId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            var merchantContracts = apiResult.Data.ToMerchantContracts();

            return Result.Success(merchantContracts);
        }

        public async Task<Result<List<MerchantDeviceModel>>> GetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery request,
                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantDevices(token.Data, request.EstateId, request.MerchantId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            var merchantDevices = apiResult.Data.ToMerchantDevices();

            return Result.Success(merchantDevices);
        }

        public async Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand request,
                                                 CancellationToken cancellationToken) {

            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            SettlementSchedule settlementSchedule = Enum.Parse<SettlementSchedule>(request.SettlementSchedule);

            UpdateMerchantRequest apiRequest = new() { SettlementSchedule = settlementSchedule, Name = request.Name };

            Result? apiResult = await this.TransactionProcessorClient.UpdateMerchant(token.Data, request.EstateId, request.MerchantId,apiRequest,cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantCommand request,
                                                 CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Address apiRequest = new() {
                AddressLine1 = request.MerchantAddress.AddressLine1, Town = request.MerchantAddress.Town,
                Country = request.MerchantAddress.Country, PostalCode = request.MerchantAddress.PostalCode,
                Region = request.MerchantAddress.Region
            };

            Result? apiResult = await this.TransactionProcessorClient.UpdateMerchantAddress(token.Data, request.EstateId, request.MerchantId,request.MerchantAddress.AddressId,
                apiRequest, cancellationToken);
            ;

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantCommand request,
                                                        CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Contact apiRequest = new()
            {
                ContactName = request.MerchantContact.ContactName,
                EmailAddress = request.MerchantContact.ContactEmail,
                PhoneNumber = request.MerchantContact.ContactPhone
            };

            Result? apiResult = await this.TransactionProcessorClient.UpdateMerchantContact(token.Data, request.EstateId, request.MerchantId, request.MerchantContact.ContactId,
                apiRequest, cancellationToken);
            ;

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result<List<MerchantDropDownModel>>> GetMerchants(MerchantQueries.GetMerchantsForDropDownQuery request,
                                                                            CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetMerchants(token.Data, request.EstateId,null,null,null,null,
                null, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<MerchantDropDownModel> merchantDropDownModels = apiResult.Data.ToMerchantDropDown();

            return Result.Success(merchantDropDownModels);
        }
    }
}

