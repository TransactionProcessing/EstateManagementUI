using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SecurityService.DataTransferObjects.Responses;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.MerchantSchedule;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<MerchantModels.MerchantKpiModel>> GetMerchantKpi(MerchantQueries.GetMerchantKpiQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.RecentMerchantsModel>>> GetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.MerchantListModel>>> GetMerchants(MerchantQueries.GetMerchantsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.MerchantDropDownModel>>> GetMerchants(MerchantQueries.GetMerchantsForDropDownQuery request, CancellationToken cancellationToken);
        Task<Result<MerchantModels.MerchantModel>> GetMerchant(MerchantQueries.GetMerchantQuery request, CancellationToken cancellationToken);
        Task<Result<MerchantModels.MerchantScheduleModel>> GetMerchantSchedule(MerchantQueries.GetMerchantScheduleQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.MerchantOperatorModel>>> GetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.MerchantContractModel>>> GetMerchantContracts(MerchantQueries.GetMerchantContractsQuery request, CancellationToken cancellationToken);
        Task<Result<List<MerchantModels.MerchantDeviceModel>>> GetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> CreateMerchantSchedule(MerchantCommands.CreateMerchantScheduleCommand request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantOpeningHours(MerchantCommands.UpdateMerchantOpeningHoursCommand request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> AddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> AddContractToMerchant(MerchantCommands.AssignContractToMerchantCommand request, CancellationToken cancellationToken);
        Task<Result> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand request, CancellationToken cancellationToken);
        Task<Result> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand request, CancellationToken cancellationToken);
        Task<Result> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand request, CancellationToken cancellationToken);
        Task<Result> CreateMerchant(MerchantCommands.CreateMerchantCommand request, CancellationToken cancellationToken);
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

        public async Task<Result> AddContractToMerchant(MerchantCommands.AssignContractToMerchantCommand request,
                                                          CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            AddMerchantContractRequest apiRequest = new() { ContractId = request.ContractId };

            var apiResult = await this.TransactionProcessorClient.AddContractToMerchant(token.Data, request.EstateId, request.MerchantId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand request,
                                                      CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            AddMerchantDeviceRequest apiRequest = new() { DeviceIdentifier = request.DeviceIdentifier};

            var apiResult = await this.TransactionProcessorClient.AddDeviceToMerchant(token.Data, request.EstateId, request.MerchantId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand request,
                                                     CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            SwapMerchantDeviceRequest apiRequest = new() { NewDeviceIdentifier  = request.NewDevice};

            var apiResult = await this.TransactionProcessorClient.SwapDeviceForMerchant(token.Data, request.EstateId, request.MerchantId, request.OldDevice,apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand request,
                                                      CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            MakeMerchantDepositRequest apiRequest = new() { Amount = request.Amount, DepositDateTime = request.Date, Reference = request.Reference};

            var apiResult = await this.TransactionProcessorClient.MakeMerchantDeposit(token.Data, request.EstateId, request.MerchantId,apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> CreateMerchant(MerchantCommands.CreateMerchantCommand request,
                                                 CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            SettlementSchedule settlementSchedule = Enum.Parse<SettlementSchedule>(request.SettlementSchedule);
            CreateMerchantRequest apiRequest = new() { MerchantId = request.MerchantId, Name = request.Name,SettlementSchedule = settlementSchedule, Address = new Address {
                AddressLine1 = request.MerchantAddress.AddressLine1,
                Town = request.MerchantAddress.Town,
                Country = request.MerchantAddress.Country,
                PostalCode = request.MerchantAddress.PostalCode,
                Region = request.MerchantAddress.Region
            },
                Contact = new Contact {
                    ContactName = request.MerchantContact.ContactName,
                    EmailAddress = request.MerchantContact.ContactEmail,
                    PhoneNumber = request.MerchantContact.ContactPhone
                }
            };

            var apiResult = await this.TransactionProcessorClient.CreateMerchant(token.Data, request.EstateId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> CreateMerchantSchedule(MerchantCommands.CreateMerchantScheduleCommand request,
                                                         CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            CreateMerchantScheduleRequest apiRequest = new() {
                Year = request.Schedule.Year,
                Months = request.Schedule.Months
                    .OrderBy(month => month.Month)
                    .Select(month => new MerchantScheduleMonthRequest {
                        Month = month.Month,
                        ClosedDays = month.ClosedDays.OrderBy(day => day).ToList()
                    }).ToList()
            };

            Result apiResult = await this.TransactionProcessorClient.CreateMerchantSchedule(token.Data, request.EstateId, request.MerchantId, apiRequest, cancellationToken);
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

        public async Task<Result<MerchantModels.MerchantKpiModel>> GetMerchantKpi(MerchantQueries.GetMerchantKpiQuery request,
                                                                                  CancellationToken cancellationToken) {

            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantKpi(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantModels.MerchantKpiModel merchantKpiModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(merchantKpiModel);
        }

        public async Task<Result<List<MerchantModels.RecentMerchantsModel>>> GetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery request,
                                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetRecentMerchants(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<MerchantModels.RecentMerchantsModel> recentMerchantsModels = apiResult.Data.ToRecentMerchant();

            return Result.Success(recentMerchantsModels);
        }
        
        public async Task<Result<List<MerchantModels.MerchantListModel>>> GetMerchants(MerchantQueries.GetMerchantsQuery request,
                                                                                       CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetMerchants(token.Data, request.EstateId, 
                request.Name, request.Reference, request.SettlementSchedule, request.Region, request.PostCode,cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<MerchantModels.MerchantListModel> merchantList = apiResult.Data.ToMerchantList();

            return Result.Success(merchantList);
        }

        public async Task<Result<MerchantModels.MerchantModel>> GetMerchant(MerchantQueries.GetMerchantQuery request,
                                                                            CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<Merchant> apiResult = await this.EstateReportingApiClient.GetMerchant(token.Data, request.EstateId, request.MerchantId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantModels.MerchantModel merchant = apiResult.Data.ToMerchant();

            return Result.Success(merchant);
        }

        public async Task<Result<MerchantModels.MerchantScheduleModel>> GetMerchantSchedule(MerchantQueries.GetMerchantScheduleQuery request,
                                                                                            CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<MerchantScheduleResponse> apiResult = await this.TransactionProcessorClient.GetMerchantSchedule(token.Data, request.EstateId, request.MerchantId, request.Year, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantModels.MerchantScheduleModel merchantSchedule = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(merchantSchedule);
        }

        public async Task<Result<List<MerchantModels.MerchantOperatorModel>>> GetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery request,
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

        public async Task<Result<List<MerchantModels.MerchantContractModel>>> GetMerchantContracts(MerchantQueries.GetMerchantContractsQuery request,
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

        public async Task<Result<List<MerchantModels.MerchantDeviceModel>>> GetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery request,
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

        public async Task<Result> UpdateMerchantOpeningHours(MerchantCommands.UpdateMerchantOpeningHoursCommand request,
                                                             CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            MerchantOpeningRequest apiRequest = new()
            {
                Sunday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Sunday.Opening, Closing = request.OpeningHours.Sunday.Closing },
                Monday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Monday.Opening, Closing = request.OpeningHours.Monday.Closing },
                Tuesday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Tuesday.Opening, Closing = request.OpeningHours.Tuesday.Closing },
                Wednesday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Wednesday.Opening, Closing = request.OpeningHours.Wednesday.Closing },
                Thursday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Thursday.Opening, Closing = request.OpeningHours.Thursday.Closing },
                Friday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Friday.Opening, Closing = request.OpeningHours.Friday.Closing },
                Saturday = new TransactionProcessor.DataTransferObjects.Requests.Merchant.OpeningHours { Opening = request.OpeningHours.Saturday.Opening, Closing = request.OpeningHours.Saturday.Closing }
            };

            Result apiResult = await this.TransactionProcessorClient.UpdateMerchantOpeningHours(token.Data, request.EstateId, request.MerchantId, apiRequest, cancellationToken);
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

        public async Task<Result<List<MerchantModels.MerchantDropDownModel>>> GetMerchants(MerchantQueries.GetMerchantsForDropDownQuery request,
                                                                                           CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetMerchants(token.Data, request.EstateId,null,null,null,null,
                null, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<MerchantModels.MerchantDropDownModel> merchantDropDownModels = apiResult.Data.ToMerchantDropDown();

            return Result.Success(merchantDropDownModels);
        }
    }
}
