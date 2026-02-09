    using EstateManagementUI.BlazorServer.UIServices;
    using EstateManagementUI.BusinessLogic.Requests;
    using MediatR;
    using Moq;
    using Shouldly;
    using SimpleResults;
    
    namespace EstateManagementUI.BlazorServer.Tests.UIServices;

    public class OperatorUIServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly OperatorUIService _service;

        public OperatorUIServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _service = new OperatorUIService(_mockMediator.Object);
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
                .Setup(m => m.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<OperatorQueries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<OperatorQueries.GetOperatorQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<OperatorQueries.GetOperatorQuery>(), It.IsAny<CancellationToken>()))
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
                .Setup(m => m.Send(It.IsAny<OperatorCommands.UpdateOperatorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.UpdateOperator(correlationId, estateId, operatorId, editModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<OperatorCommands.UpdateOperatorCommand>(c =>
                c.EstateId == estateId &&
                c.OperatorId == operatorId &&
                c.Name == editModel.OperatorName &&
                c.RequireCustomMerchantNumber == editModel.RequireCustomMerchantNumber &&
                c.RequireCustomTerminalNumber == editModel.RequireCustomTerminalNumber
            ), It.IsAny<CancellationToken>()), Times.Once);
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
                .Setup(m => m.Send(It.IsAny<OperatorCommands.CreateOperatorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.CreateOperator(correlationId, estateId, createModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<OperatorCommands.CreateOperatorCommand>(c =>
                c.EstateId == estateId &&
                c.Name == createModel.OperatorName &&
                c.RequireCustomMerchantNumber == createModel.RequireCustomMerchantNumber &&
                c.RequireCustomTerminalNumber == createModel.RequireCustomTerminalNumber
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateOperator_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<OperatorCommands.CreateOperatorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            // Act
            var result = await _service.CreateOperator(CorrelationIdHelper.New(), Guid.NewGuid(), new EstateManagementUI.BlazorServer.Models.OperatorModels.CreateOperatorModel { OperatorName = "x" });

            // Assert
            result.IsFailed.ShouldBeTrue();
        }
    }