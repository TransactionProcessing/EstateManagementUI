    using EstateManagementUI.BlazorServer.UIServices;
    using EstateManagementUI.BusinessLogic.Requests;
    using MediatR;
    using Imposter.Abstractions;
    using Shouldly;
    using SimpleResults;

    namespace EstateManagementUI.BlazorServer.Tests.UIServices;

    public class OperatorUIServiceTests
    {
        private readonly IMediatorImposter _mockMediator;
        private readonly OperatorUIService _service;

        public OperatorUIServiceTests()
        {
            _mockMediator = new IMediatorImposter();
            _service = new OperatorUIService(_mockMediator.Instance());
        }

        [Fact]
        public async Task GetOperators_ReturnsMappedList_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            var bizOperators = new List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>
            {
                new() { OperatorId = Guid.NewGuid(), Name = "OpA", RequireCustomMerchantNumber = true, RequireCustomTerminalNumber = false }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizOperators));

            // Act
            var result = await _service.GetOperators(correlationId, estateId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(1);
            var mapped = result.Data.First();
            mapped.OperatorId.ShouldBe(bizOperators[0].OperatorId);
            mapped.Name.ShouldBe("OpA");
            mapped.RequireCustomMerchantNumber.ShouldBeTrue();
            mapped.RequireCustomTerminalNumber.ShouldBeFalse();
        }

        [Fact]
        public async Task GetOperators_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("failure"));

            // Act
            var result = await _service.GetOperators(CorrelationIdHelper.New(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetOperator_ReturnsMappedModel_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();

            var bizOperator = new EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel
            {
                OperatorId = operatorId,
                Name = "OpDetail",
                RequireCustomMerchantNumber = false,
                RequireCustomTerminalNumber = true
            };

            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizOperator));

            // Act
            var result = await _service.GetOperator(correlationId, estateId, operatorId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            var model = result.Data!;
            model.OperatorId.ShouldBe(operatorId);
            model.Name.ShouldBe("OpDetail");
            model.RequireCustomMerchantNumber.ShouldBeFalse();
            model.RequireCustomTerminalNumber.ShouldBeTrue();
        }

        [Fact]
        public async Task GetOperator_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Send<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>(Arg<global::MediatR.IRequest<SimpleResults.Result<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorModel>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("not found"));

            // Act
            var result = await _service.GetOperator(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateOperator_CallsMediatorWithCorrectCommand_AndReturnsResult()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();
            var operatorId = Guid.NewGuid();

            var editModel = new EstateManagementUI.BlazorServer.Models.OperatorModels.EditOperatorModel
            {
                OperatorName = "UpdatedName",
                RequireCustomMerchantNumber = true,
                RequireCustomTerminalNumber = false
            };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _service.UpdateOperator(correlationId, estateId, operatorId, editModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId &&
                ((dynamic)c).OperatorId == operatorId &&
                ((dynamic)c).Name == editModel.OperatorName &&
                ((dynamic)c).RequireCustomMerchantNumber == editModel.RequireCustomMerchantNumber &&
                ((dynamic)c).RequireCustomTerminalNumber == editModel.RequireCustomTerminalNumber
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task CreateOperator_CallsMediatorWithCorrectCommand_AndReturnsResult()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            var createModel = new EstateManagementUI.BlazorServer.Models.OperatorModels.CreateOperatorModel
            {
                OperatorName = "NewOperator",
                RequireCustomMerchantNumber = false,
                RequireCustomTerminalNumber = true
            };

            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success());

            // Act
            var result = await _service.CreateOperator(correlationId, estateId, createModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Is(c =>
                ((dynamic)c).EstateId == estateId &&
                ((dynamic)c).Name == createModel.OperatorName &&
                ((dynamic)c).RequireCustomMerchantNumber == createModel.RequireCustomMerchantNumber &&
                ((dynamic)c).RequireCustomTerminalNumber == createModel.RequireCustomTerminalNumber
            ), Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task CreateOperator_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Send<SimpleResults.Result>(Arg<global::MediatR.IRequest<SimpleResults.Result>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure());

            // Act
            var result = await _service.CreateOperator(CorrelationIdHelper.New(), Guid.NewGuid(), new EstateManagementUI.BlazorServer.Models.OperatorModels.CreateOperatorModel { OperatorName = "x" });

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetOperatorsForDropDown_ReturnsMappedList_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var bizList = new List<BusinessLogic.Models.OperatorModels.OperatorDropDownModel>
            {
                new() { OperatorId = Guid.NewGuid(), OperatorName = "Op1" }
            };

            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Success(bizList));

            // Act
            var result = await _service.GetOperatorsForDropDown(CorrelationIdHelper.New(), estateId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(1);
            result.Data[0].OperatorName.ShouldBe("Op1");

            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Is(q => ((dynamic)q).EstateId == estateId),
                    Arg<CancellationToken>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task GetOperatorsForDropDown_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any())
                .ReturnsAsync(Result.Failure("backend error"));

            // Act
            var result = await _service.GetOperatorsForDropDown(CorrelationIdHelper.New(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
            _mockMediator.Send<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>(Arg<global::MediatR.IRequest<SimpleResults.Result<List<EstateManagementUI.BusinessLogic.Models.OperatorModels.OperatorDropDownModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        }
}
