using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using Xunit;
using MediatR;

namespace EstateManagementUI.BlazorServer.Tests.UIServices
{
    public class MerchantUIServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly MerchantUIService _service;

        public MerchantUIServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _service = new MerchantUIService(_mockMediator.Object);
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantCommands.CreateMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            var result = await _service.CreateMerchant(CorrelationIdHelper.New(), estateId, Guid.NewGuid(), createModel);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.CreateMerchantCommand>(c =>
                c.EstateId == estateId &&
                c.Name == createModel.MerchantName &&
                c.SettlementSchedule == createModel.SettlementSchedule &&
                c.MerchantAddress.AddressLine1 == createModel.AddressLine1 &&
                c.MerchantAddress.Town == createModel.Town &&
                c.MerchantAddress.Region == createModel.Region &&
                c.MerchantAddress.PostalCode == createModel.PostCode &&
                c.MerchantAddress.Country == createModel.Country &&
                c.MerchantContact.ContactName == createModel.ContactName &&
                c.MerchantContact.ContactEmail == createModel.EmailAddress &&
                c.MerchantContact.ContactPhone == createModel.PhoneNumber
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateMerchant_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.CreateMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            var result = await _service.CreateMerchant(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.CreateMerchantModel { MerchantName = "x", SettlementSchedule = "s" });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateMerchant_SendsUpdateCommand_AndReturnsSuccess()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            var editModel = new BlazorServer.Models.MerchantModels.MerchantEditModel
            {
                MerchantName = "Upd",
                SettlementSchedule = "S",
                AddressLine1 = "A1",
                Town = "T",
                Region = "R",
                PostalCode = "P",
                Country = "C",
                ContactName = "CN",
                ContactEmailAddress = "e@x",
                ContactPhoneNumber = "ph"
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            var result = await _service.UpdateMerchant(CorrelationIdHelper.New(), estateId, merchantId, editModel);

            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.UpdateMerchantCommand>(c =>
                c.EstateId == estateId &&
                c.MerchantId == merchantId &&
                c.Name == editModel.MerchantName &&
                c.SettlementSchedule == editModel.SettlementSchedule &&
                c.MerchantAddress.AddressLine1 == editModel.AddressLine1 &&
                c.MerchantContact.ContactName == editModel.ContactName
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMerchant_ReturnsFailure_WhenMediatorFails()
        {
            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            var result = await _service.UpdateMerchant(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new BlazorServer.Models.MerchantModels.MerchantEditModel { MerchantName = "x", SettlementSchedule = "s" });

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddAndRemoveOperatorToMerchant_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.AddOperatorToMerchantCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.RemoveOperatorFromMerchantCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            var addResult = await _service.AddOperatorToMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId, "MN", "TN");
            var removeResult = await _service.RemoveOperatorFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId);

            addResult.IsSuccess.ShouldBeTrue();
            removeResult.IsSuccess.ShouldBeTrue();

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.AddOperatorToMerchantCommand>(c =>
                c.EstateId == estateId &&
                c.MerchantId == merchantId &&
                c.OperatorId == operatorId &&
                c.MerchantNumber == "MN" &&
                c.TerminalNumber == "TN"
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.RemoveOperatorFromMerchantCommand>(c =>
                c.EstateId == estateId &&
                c.MerchantId == merchantId &&
                c.OperatorId == operatorId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AssignAndRemoveContractFromMerchant_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.RemoveContractFromMerchantCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            var assign = await _service.AssignContractToMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);
            var remove = await _service.RemoveContractFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            assign.IsSuccess.ShouldBeTrue();
            remove.IsSuccess.ShouldBeTrue();

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.AssignContractToMerchantCommand>(c =>
                c.EstateId == estateId && c.MerchantId == merchantId && c.ContractId == contractId
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.RemoveContractFromMerchantCommand>(c =>
                c.EstateId == estateId && c.MerchantId == merchantId && c.ContractId == contractId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddSwapDeviceAndMakeDeposit_SendCorrectCommands()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.AddMerchantDeviceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.SwapMerchantDeviceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);
            _mockMediator.Setup(m => m.Send(It.IsAny<MerchantCommands.MakeMerchantDepositCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success);

            var addDevice = await _service.AddMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "dev1");
            var swapDevice = await _service.SwapMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "old", "new");
            var depositModel = new BlazorServer.Models.MerchantModels.DepositModel { Amount = 12, Date = DateTime.UtcNow, Reference = "note" };
            var deposit = await _service.MakeMerchantDeposit(CorrelationIdHelper.New(), estateId, merchantId, depositModel);

            addDevice.IsSuccess.ShouldBeTrue();
            swapDevice.IsSuccess.ShouldBeTrue();
            deposit.IsSuccess.ShouldBeTrue();

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.AddMerchantDeviceCommand>(c =>
                c.EstateId == estateId && c.MerchantId == merchantId && c.DeviceIdentifier == "dev1"
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.SwapMerchantDeviceCommand>(c =>
                c.EstateId == estateId && c.MerchantId == merchantId && c.OldDevice == "old" && c.NewDevice == "new"
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mockMediator.Verify(m => m.Send(It.Is<MerchantCommands.MakeMerchantDepositCommand>(c =>
                c.EstateId == estateId && c.MerchantId == merchantId && c.Amount == depositModel.Amount && c.Reference == depositModel.Reference
            ), It.IsAny<CancellationToken>()), Times.Once);
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantKpiQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantsForDropDownQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantsForDropDownQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.GetMerchantsForDropDown(CorrelationIdHelper.New(), Guid.NewGuid());

            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddOperatorToMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.AddOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AddOperatorToMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId, "MN", "TN");

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.AddOperatorToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveOperatorFromMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.RemoveOperatorFromMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.RemoveOperatorFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, operatorId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.RemoveOperatorFromMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AssignContractToMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AssignContractToMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.AssignContractToMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveContractFromMerchant_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.RemoveContractFromMerchantCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.RemoveContractFromMerchant(CorrelationIdHelper.New(), estateId, merchantId, contractId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.RemoveContractFromMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddMerchantDevice_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var deviceId = "device-123";

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.AddMerchantDeviceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.AddMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, deviceId);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.AddMerchantDeviceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SwapMerchantDevice_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.SwapMerchantDeviceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.SwapMerchantDevice(CorrelationIdHelper.New(), estateId, merchantId, "old", "new");

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.SwapMerchantDeviceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MakeMerchantDeposit_ReturnsFailure_WhenMediatorFails()
        {
            var estateId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var deposit = new BlazorServer.Models.MerchantModels.DepositModel { Amount = 10, Date = DateTime.UtcNow, Reference = "ref" };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<MerchantCommands.MakeMerchantDepositCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            var result = await _service.MakeMerchantDeposit(CorrelationIdHelper.New(), estateId, merchantId, deposit);

            result.IsFailed.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.IsAny<MerchantCommands.MakeMerchantDepositCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}