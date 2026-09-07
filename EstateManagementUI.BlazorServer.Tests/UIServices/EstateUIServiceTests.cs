using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using Xunit;
using MediatR;

namespace EstateManagementUI.BlazorServer.Tests.UIServices
{
    public class EstateUIServiceTests
    {
        private readonly IMediatorImposter _mockMediator;
        private readonly EstateUIService _service;

        public EstateUIServiceTests()
        {
            _mockMediator = new IMediatorImposter();
            _service = new EstateUIService(_mockMediator.Instance());
        }

        [Fact]
        public async Task LoadEstate_ReturnsMappedModel_WhenAllQueriesSucceed()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            var bizEstate = new BusinessLogic.Models.EstateModels.EstateModel
            {
                EstateId = estateId,
                EstateName = "Test Estate",
                Reference = "REF001",
                Merchants = new List<BusinessLogic.Models.EstateModels.EstateMerchantModel> { new() { MerchantId = Guid.NewGuid(), Name = "M1", Reference = "R1" } },
                Contracts = new List<BusinessLogic.Models.EstateModels.EstateContractModel> { new() { ContractId = Guid.NewGuid(), Name = "C1", OperatorName = "Op" } },
                Operators = new List<BusinessLogic.Models.EstateModels.EstateOperatorModel> { new() { OperatorId = Guid.NewGuid(), Name = "OpAssigned" } },
                Users = new List<BusinessLogic.Models.EstateModels.EstateUserModel> { new() { UserId = Guid.NewGuid(), EmailAddress = "u@x" } }
            };

            var recentMerchants = new List<BusinessLogic.Models.MerchantModels.RecentMerchantsModel>
            {
                new() { MerchantId = Guid.NewGuid(), Name = "RecentM", Reference = "RM", CreatedDateTime = DateTime.UtcNow }
            };

            var recentContracts = new List<BusinessLogic.Models.ContractModels.RecentContractModel>
            {
                new() { ContractId = Guid.NewGuid(), Description = "RecentC", OperatorName = "Op" }
            };

            var assignedOperators = new List<BusinessLogic.Models.OperatorModels.OperatorModel>
            {
                new() { OperatorId = bizEstate.Operators!.First().OperatorId, Name = "OpAssigned" }
            };

            var allOperators = new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>
            {
                new() { OperatorId = Guid.NewGuid(), OperatorName = "OpA" },
                new() { OperatorId = assignedOperators[0].OperatorId, OperatorName = "OpAssigned" }
            };

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(bizEstate));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(recentMerchants));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(recentContracts));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(assignedOperators));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(allOperators));

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            var model = result.Data!;
            model.EstateId.ShouldBe(estateId);
            model.EstateName.ShouldBe("Test Estate");
            model.MerchantCount.ShouldBe(1);
            model.ContractCount.ShouldBe(1);
            model.UserCount.ShouldBe(1);
            model.AllOperators.ShouldNotBeNull();
            model.AllOperators.ShouldContain(op => op.OperatorName == "OpA");
            model.AssignedOperators.ShouldNotBeEmpty();
            model.RecentMerchants.ShouldNotBeEmpty();
            model.RecentContracts.ShouldNotBeEmpty();
            model.OperatorCount.ShouldBe(1);
        }

        [Fact]
        public async Task LoadEstate_ReturnsFailure_WhenEstateQueryFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Failure("estate fail"));

            // Provide success for other queries so the service behaviour is isolated
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.MerchantModels.RecentMerchantsModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.ContractModels.RecentContractModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>()));

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task LoadEstate_ReturnsFailure_WhenRecentMerchantQueryFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.ContractModels.RecentContractModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>()));

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task LoadEstate_ReturnsFailure_WhenRecentContractsQueryFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new EstateModels.EstateModel()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<MerchantModels.RecentMerchantsModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>()));

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task LoadEstate_ReturnsFailure_WhenGetAssignedOperatorsQueryFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new EstateModels.EstateModel()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<MerchantModels.RecentMerchantsModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<ContractModels.RecentContractModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>()));

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task LoadEstate_ReturnsFailure_WhenGetOperatorsForDropDownQueryFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            _mockMediator.Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.EstateModels.EstateModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new EstateModels.EstateModel()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.MerchantModels.RecentMerchantsModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<MerchantModels.RecentMerchantsModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.ContractModels.RecentContractModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<ContractModels.RecentContractModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(new List<OperatorModels.OperatorModel>()));
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            // Act
            var result = await _service.LoadEstate(correlationId, estateId);

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddOperatorToEstate_CallsMediatorWithAddCommand_OnSuccess()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();
            var operatorIdString = operatorId.ToString();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success());

            // Act
            var result = await _service.AddOperatorToEstate(correlationId, estateId, operatorIdString);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId && ((dynamic)c).OperatorId == operatorId), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task AddOperatorToEstate_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Failure());

            // Act
            var result = await _service.AddOperatorToEstate(correlationId, estateId, operatorId.ToString());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task RemoveOperatorFromEstate_ShouldCallRemoveCommand()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                         .ReturnsAsync(Result.Success());

            // Act
            var result = await _service.RemoveOperatorFromEstate(correlationId, estateId, operatorId);

            // Assert - the service should send a RemoveOperatorFromEstateCommand
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId && ((dynamic)c).OperatorId == operatorId), Arg<CancellationToken>.Any()).Called(Count.Once());
        }


        [Fact]
        public async Task RemoveOperatorFromEstate_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();

            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            // Act
            var result = await _service.RemoveOperatorFromEstate(correlationId, estateId, operatorId);

            // Assert - the service should send a RemoveOperatorFromEstateCommand
            result.IsFailed.ShouldBeTrue();
        }
    }
}
