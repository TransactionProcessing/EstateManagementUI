using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using Xunit;
using MediatR;

namespace EstateManagementUI.BlazorServer.Tests.UIServices
{
    public class MerchantUIServiceTests
    {
        private readonly IMediatorImposter _mockMediator;
        private readonly MerchantUIService _service;

        public MerchantUIServiceTests()
        {
            _mockMediator = new IMediatorImposter();
            _service = new MerchantUIService(_mockMediator.Instance());
        }

        [Fact]
        public async Task GetMerchant_ReturnsMappedModel_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var biz = new BusinessLogic.Models.MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "M1",
                MerchantReference = "REF1",
                Balance = 10m,
                AvailableBalance = 8m,
                SettlementSchedule = "Daily",
                Town = "T",
                Region = "R",
                PostalCode = "P",
                Country = "C",
                ContactName = "Contact",
                ContactEmailAddress = "c@x",
                ContactPhoneNumber = "123",
                AddressId = Guid.NewGuid(),
                ContactId = Guid.NewGuid()
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(biz));

            // Act
            var result = await _service.GetMerchant(CorrelationIdHelper.New(), estateId, merchantId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            var model = result.Data!;
            model.MerchantId.ShouldBe(merchantId);
            model.MerchantName.ShouldBe("M1");
            model.MerchantReference.ShouldBe("REF1");
            model.Balance.ShouldBe(10m);
            model.AvailableBalance.ShouldBe(8m);
            model.SettlementSchedule.ShouldBe("Daily");
            model.ContactName.ShouldBe("Contact");
        }

        [Fact]
        public async Task GetMerchant_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            // Act
            var result = await _service.GetMerchant(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchants_ReturnsMappedList_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.MerchantListModel>
            {
                new() { MerchantId = Guid.NewGuid(), MerchantName = "M1", MerchantReference = "R1", Balance = 1m, AvailableBalance = 1m, SettlementSchedule = "S", Region = "Reg", PostalCode = "PC", CreatedDateTime = DateTime.UtcNow }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantListModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantListModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            // Act
            var result = await _service.GetMerchants(CorrelationIdHelper.New(), estateId, "n", "r", null, "reg", "pc");

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Data!.Count.ShouldBe(1);
            result.Data![0].MerchantName.ShouldBe("M1");
            result.Data![0].MerchantReference.ShouldBe("R1");
        }

        [Fact]
        public async Task GetMerchants_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantListModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantListModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchants(CorrelationIdHelper.New(), Guid.NewGuid(), "", "", null, "", "");

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantOperators_ReturnsMappedList_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.MerchantOperatorModel>
            {
                new() { MerchantId = merchantId, OperatorId = Guid.NewGuid(), OperatorName = "Op1", MerchantNumber = "MN", TerminalNumber = "TN", IsDeleted = false }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantOperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantOperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            var result = await _service.GetMerchantOperators(CorrelationIdHelper.New(), estateId, merchantId);

            result.IsSuccess.ShouldBeTrue();
            result.Data!.Count.ShouldBe(1);
            result.Data![0].OperatorName.ShouldBe("Op1");
        }

        [Fact]
        public async Task GetMerchantOperators_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantOperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantOperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantOperators(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantContracts_ReturnsMappedList_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.MerchantContractModel>
            {
                new() { MerchantId = merchantId, ContractId = Guid.NewGuid(), ContractName = "C1", OperatorName = "Op" , IsDeleted=false,
                    ContractProducts = new List<BusinessLogic.Models.MerchantModels.MerchantContractProductModel>() }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            var result = await _service.GetMerchantContracts(CorrelationIdHelper.New(), estateId, merchantId);

            result.IsSuccess.ShouldBeTrue();
            result.Data!.Count.ShouldBe(1);
            result.Data![0].ContractName.ShouldBe("C1");
        }

        [Fact]
        public async Task GetMerchantContracts_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantContracts(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantDevices_ReturnsMappedList_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.MerchantDeviceModel>
            {
                new() { MerchantId = merchantId, DeviceId = Guid.NewGuid(), DeviceIdentifier = "dev1", IsDeleted = false }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDeviceModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDeviceModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            var result = await _service.GetMerchantDevices(CorrelationIdHelper.New(), estateId, merchantId);

            result.IsSuccess.ShouldBeTrue();
            result.Data!.Count.ShouldBe(1);
            result.Data![0].DeviceIdentifier.ShouldBe("dev1");
        }

        [Fact]
        public async Task GetMerchantDevices_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDeviceModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDeviceModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantDevices(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateMerchant_SendsCreateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var createModel = new BlazorServer.Models.MerchantModels.CreateMerchantModel
            {
                MerchantName = "NewM",
                SettlementSchedule = "S",
                AddressLine1 = "A1",
                Town = "T",
                Region = "R",
                PostCode = "P",
                Country = "C",
                ContactName = "CN",
                EmailAddress = "e@x",
                PhoneNumber = "ph"
            };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            var result = await _service.CreateMerchant(CorrelationIdHelper.New(), estateId, Guid.NewGuid(), createModel);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId &&
                ((dynamic)c).Name == createModel.MerchantName &&
                ((dynamic)c).SettlementSchedule == createModel.SettlementSchedule &&
                ((dynamic)c).MerchantAddress.AddressLine1 == createModel.AddressLine1 &&
                ((dynamic)c).MerchantAddress.Town == createModel.Town &&
                ((dynamic)c).MerchantAddress.Region == createModel.Region &&
                ((dynamic)c).MerchantAddress.PostalCode == createModel.PostCode &&
                ((dynamic)c).MerchantAddress.Country == createModel.Country &&
                ((dynamic)c).MerchantContact.ContactName == createModel.ContactName &&
                ((dynamic)c).MerchantContact.ContactEmail == createModel.EmailAddress &&
                ((dynamic)c).MerchantContact.ContactPhone == createModel.PhoneNumber
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task CreateMerchant_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            var result = await _service.CreateMerchant(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.CreateMerchantModel { MerchantName = "x", SettlementSchedule = "s" });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateMerchant_SendsUpdateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            var addressId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var editModel = new BlazorServer.Models.MerchantModels.MerchantEditModel
            {
                MerchantName = "Upd",
                SettlementSchedule = "S",
                AddressId = addressId,
                AddressLine1 = "A1",
                Town = "T",
                Region = "R",
                PostalCode = "P",
                Country = "United Kingdom",
                ContactId = contactId,
                ContactName = "CN",
                ContactEmailAddress = "e@x",
                ContactPhoneNumber = "ph"
            };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            var result = await _service.UpdateMerchant(CorrelationIdHelper.New(), estateId, merchantId, editModel);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId &&
                ((dynamic)c).MerchantId == merchantId &&
                ((dynamic)c).Name == editModel.MerchantName &&
                ((dynamic)c).SettlementSchedule == editModel.SettlementSchedule &&
                ((dynamic)c).MerchantAddress.AddressId == addressId &&
                ((dynamic)c).MerchantAddress.AddressLine1 == editModel.AddressLine1 &&
                ((dynamic)c).MerchantContact.ContactId == contactId &&
                ((dynamic)c).MerchantContact.ContactName == editModel.ContactName
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task UpdateMerchant_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            var result = await _service.UpdateMerchant(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.MerchantEditModel { MerchantName = "x", SettlementSchedule = "s" });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateMerchantOpeningHours_SendsUpdateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var openingHours = new BlazorServer.Models.MerchantModels.MerchantOpeningHoursModel
            {
                Sunday = new() { Opening = "0800", Closing = "1800" },
                Monday = new() { Opening = "0800", Closing = "1700" },
                Tuesday = new() { Opening = "0800", Closing = "1700" },
                Wednesday = new() { Opening = "0800", Closing = "1700" },
                Thursday = new() { Opening = "0800", Closing = "1700" },
                Friday = new() { Opening = "0800", Closing = "1700" },
                Saturday = new() { Opening = "0900", Closing = "1600" }
            };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            var result = await _service.UpdateMerchantOpeningHours(CorrelationIdHelper.New(), estateId, merchantId, openingHours);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId &&
                ((dynamic)c).MerchantId == merchantId &&
                ((dynamic)c).OpeningHours.Sunday.Opening == "0800" &&
                ((dynamic)c).OpeningHours.Sunday.Closing == "1800" &&
                ((dynamic)c).OpeningHours.Saturday.Opening == "0900" &&
                ((dynamic)c).OpeningHours.Saturday.Closing == "1600"
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task UpdateMerchantOpeningHours_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            var result = await _service.UpdateMerchantOpeningHours(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.MerchantOpeningHoursModel());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddAndRemoveOperatorToMerchant_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());

            var addResult = await _service.AddOperatorToMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId, "MN", "TN");
            var removeResult = await _service.RemoveOperatorFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId);

            addResult.IsSuccess.ShouldBeTrue();
            removeResult.IsSuccess.ShouldBeTrue();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.AddOperatorToMerchantCommand addOperatorCommand &&
                addOperatorCommand.EstateId == estateId &&
                addOperatorCommand.MerchantId == merchantId &&
                addOperatorCommand.OperatorId == operatorId &&
                addOperatorCommand.MerchantNumber == "MN" &&
                addOperatorCommand.TerminalNumber == "TN"
            ), Arg<CancellationToken>.Any()).Called(Count.Once());

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.RemoveOperatorFromMerchantCommand removeOperatorCommand &&
                removeOperatorCommand.EstateId == estateId &&
                removeOperatorCommand.MerchantId == merchantId &&
                removeOperatorCommand.OperatorId == operatorId
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task AssignAndRemoveContractFromMerchant_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());

            var assign = await _service.AssignContractToMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);
            var remove = await _service.RemoveContractFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            assign.IsSuccess.ShouldBeTrue();
            remove.IsSuccess.ShouldBeTrue();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.AssignContractToMerchantCommand assignCommand &&
                assignCommand.EstateId == estateId && assignCommand.MerchantId == merchantId && assignCommand.ContractId == contractId
            ), Arg<CancellationToken>.Any()).Called(Count.Once());

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.RemoveContractFromMerchantCommand removeCommand &&
                removeCommand.EstateId == estateId && removeCommand.MerchantId == merchantId && removeCommand.ContractId == contractId
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task AddSwapDeviceAndMakeDeposit_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());

            var addDevice = await _service.AddMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "dev1");
            var swapDevice = await _service.SwapMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "old", "new");
            var depositModel = new BlazorServer.Models.MerchantModels.DepositModel { Amount = 12, Date = DateTime.UtcNow, Reference = "note" };
            var deposit = await _service.MakeMerchantDeposit(CorrelationIdHelper.New(), estateId, merchantId, depositModel);

            addDevice.IsSuccess.ShouldBeTrue();
            swapDevice.IsSuccess.ShouldBeTrue();
            deposit.IsSuccess.ShouldBeTrue();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.AddMerchantDeviceCommand addDeviceCommand &&
                addDeviceCommand.EstateId == estateId && addDeviceCommand.MerchantId == merchantId && addDeviceCommand.DeviceIdentifier == "dev1"
            ), Arg<CancellationToken>.Any()).Called(Count.Once());

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.SwapMerchantDeviceCommand swapDeviceCommand &&
                swapDeviceCommand.EstateId == estateId && swapDeviceCommand.MerchantId == merchantId && swapDeviceCommand.OldDevice == "old" && swapDeviceCommand.NewDevice == "new"
            ), Arg<CancellationToken>.Any()).Called(Count.Once());

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                c is MerchantCommands.MakeMerchantDepositCommand depositCommand &&
                depositCommand.EstateId == estateId && depositCommand.MerchantId == merchantId && depositCommand.Amount == depositModel.Amount && depositCommand.Reference == depositModel.Reference
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task GetRecentMerchants_ReturnsMappedList_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.RecentMerchantsModel>
            {
                new() { MerchantId = Guid.NewGuid(), Name = "RecentM", Reference = "RM", CreatedDateTime = DateTime.UtcNow }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            var result = await _service.GetRecentMerchants(CorrelationIdHelper.New(), estateId);

            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(1);
            result.Data[0].Name.ShouldBe("RecentM");
            result.Data[0].Reference.ShouldBe("RM");
        }

        [Fact]
        public async Task GetRecentMerchants_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetRecentMerchants(CorrelationIdHelper.New(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantKpis_ReturnsMappedModel_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var bizKpi = new BusinessLogic.Models.MerchantModels.MerchantKpiModel
            {
                MerchantsWithNoSaleInLast7Days = 5,
                MerchantsWithNoSaleToday = 2,
                MerchantsWithSaleInLastHour = 1
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantKpiModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantKpiModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizKpi));

            var result = await _service.GetMerchantKpis(CorrelationIdHelper.New(), estateId);

            result.IsSuccess.ShouldBeTrue();
            result.Data!.MerchantsWithNoSaleInLast7Days.ShouldBe(5);
            result.Data.MerchantsWithNoSaleToday.ShouldBe(2);
            result.Data.MerchantsWithSaleInLastHour.ShouldBe(1);
        }

        [Fact]
        public async Task GetMerchantKpis_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantKpiModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantKpiModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantKpis(CorrelationIdHelper.New(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantsForDropDown_ReturnsMappedList_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.MerchantModels.MerchantDropDownModel>
            {
                new() { MerchantId = Guid.NewGuid(), MerchantName = "MDrop" }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            var result = await _service.GetMerchantsForDropDown(CorrelationIdHelper.New(), estateId);

            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(1);
            result.Data[0].MerchantName.ShouldBe("MDrop");
        }

        [Fact]
        public async Task GetMerchantsForDropDown_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantsForDropDown(CorrelationIdHelper.New(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetMerchantSchedule_ReturnsMappedModel_WhenMediatorSucceeds()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var year = DateTime.Today.Year;
            var businessLogicSchedule = new BusinessLogic.Models.MerchantModels.MerchantScheduleModel
            {
                Year = year,
                Months = new List<BusinessLogic.Models.MerchantModels.MerchantScheduleMonthModel>
                {
                    new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } }
                }
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(businessLogicSchedule));

            var result = await _service.GetMerchantSchedule(CorrelationIdHelper.New(), estateId, merchantId, year);

            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Year.ShouldBe(year);
            result.Data.Months.Count.ShouldBe(1);
            result.Data.Months[0].Month.ShouldBe(1);
            result.Data.Months[0].ClosedDays.ShouldBe(new List<int> { 1, 2, 15 });
        }

        [Fact]
        public async Task GetMerchantSchedule_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantSchedule(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.Year);

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task SaveMerchantSchedule_NewSchedule_SendsCreateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var schedule = new BlazorServer.Models.MerchantModels.MerchantScheduleModel
            {
                Year = DateTime.Today.Year + 1,
                Months = new List<BlazorServer.Models.MerchantModels.MerchantScheduleMonthModel>
                {
                    new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } },
                    new() { Month = 2, ClosedDays = new List<int>() }
                }
            };
            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.NotFound());
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            var result = await _service.SaveMerchantSchedule(CorrelationIdHelper.New(), estateId, merchantId, schedule);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task SaveMerchantSchedule_ExistingSchedule_SendsUpdateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var schedule = new BlazorServer.Models.MerchantModels.MerchantScheduleModel
            {
                Year = DateTime.Today.Year + 1,
                Months = new List<BlazorServer.Models.MerchantModels.MerchantScheduleMonthModel>
                {
                    new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } },
                    new() { Month = 2, ClosedDays = new List<int>() }
                }
            };

            var year = DateTime.Today.Year;
            var businessLogicSchedule = new BusinessLogic.Models.MerchantModels.MerchantScheduleModel
            {
                Year = year,
                Months = new List<BusinessLogic.Models.MerchantModels.MerchantScheduleMonthModel>
                {
                    new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } }
                }
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(businessLogicSchedule));
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            var result = await _service.SaveMerchantSchedule(CorrelationIdHelper.New(), estateId, merchantId, schedule);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task SaveMerchantSchedule_NewSchedule_ReturnsFailure_WhenMediatorFails()
        {

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.NotFound());
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.SaveMerchantSchedule(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.MerchantScheduleModel
            {
                Year = DateTime.Today.Year,
                Months = new List<BlazorServer.Models.MerchantModels.MerchantScheduleMonthModel>()
            });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task SaveMerchantSchedule_ExistingSchedule_ReturnsFailure_WhenMediatorFails()
        {
            var year = DateTime.Today.Year;
            var businessLogicSchedule = new BusinessLogic.Models.MerchantModels.MerchantScheduleModel
            {
                Year = year,
                Months = new List<BusinessLogic.Models.MerchantModels.MerchantScheduleMonthModel>
                {
                    new() { Month = 1, ClosedDays = new List<int> { 1, 2, 15 } }
                }
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.MerchantModels.MerchantScheduleModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(businessLogicSchedule));
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.SaveMerchantSchedule(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.MerchantScheduleModel
            {
                Year = DateTime.Today.Year,
                Months = new List<BlazorServer.Models.MerchantModels.MerchantScheduleMonthModel>()
            });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddOperatorToMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AddOperatorToMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId, "MN", "TN");

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task RemoveOperatorFromMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.RemoveOperatorFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task AssignContractToMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AssignContractToMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task RemoveContractFromMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.RemoveContractFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task AddMerchantDevice_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var deviceId = "device-123";

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AddMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, deviceId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task SwapMerchantDevice_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.SwapMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "old", "new");

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task MakeMerchantDeposit_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var deposit = new BlazorServer.Models.MerchantModels.DepositModel { Amount = 10, Date = DateTime.UtcNow, Reference = "ref" };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.MakeMerchantDeposit(CorrelationIdHelper.New(), estateId, merchantId, deposit);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }
    }
}
