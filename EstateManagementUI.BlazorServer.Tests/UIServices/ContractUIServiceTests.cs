using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EstateManagementUI.BlazorServer.Tests.UIServices
{
    public class ContractUIServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly ContractUIService _service;

        public ContractUIServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _service = new ContractUIService(_mockMediator.Object);
        }

        [Fact]
        public async Task GetContracts_ReturnsMappedList_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var correlationId = CorrelationIdHelper.New();

            var bizContracts = new List<BusinessLogic.Models.ContractModels.ContractModel>
            {
                new()
                {
                    ContractId = Guid.NewGuid(),
                    Description = "Contract A",
                    OperatorId = Guid.NewGuid(),
                    OperatorName = "OpA",
                    Products = new List<BusinessLogic.Models.ContractModels.ContractProductModel>
                    {
                        new()
                        {
                            ContractProductId = Guid.NewGuid(),
                            ProductName = "Prod1",
                            DisplayText = "Prod 1",
                            ProductType = "NotSet",
                            Value = "10.00",
                            NumberOfFees = 0,
                            TransactionFees = new List<BusinessLogic.Models.ContractModels.ContractProductTransactionFeeModel>()
                        }
                    }
                }
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractQueries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(bizContracts));

            // Act
            var result = await _service.GetContracts(correlationId, estateId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data!.Count.ShouldBe(1);
            var mapped = result.Data.First();
            mapped.ContractId.ShouldBe(bizContracts[0].ContractId);
            mapped.Description.ShouldBe("Contract A");
            mapped.OperatorName.ShouldBe("OpA");
            mapped.Products.ShouldNotBeNull();
            mapped.Products!.Count.ShouldBe(1);
            mapped.Products![0].ProductName.ShouldBe("Prod1");
        }

        [Fact]
        public async Task GetContracts_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractQueries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("err"));

            // Act
            var result = await _service.GetContracts(CorrelationIdHelper.New(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task GetContract_ReturnsMappedModel_WhenMediatorSucceeds()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var contractId = Guid.NewGuid();
            var bizContract = new BusinessLogic.Models.ContractModels.ContractModel
            {
                ContractId = contractId,
                Description = "Contract Detail",
                OperatorId = Guid.NewGuid(),
                OperatorName = "OpDetail",
                Products = new List<BusinessLogic.Models.ContractModels.ContractProductModel>()
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractQueries.GetContractQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(bizContract));

            // Act
            var result = await _service.GetContract(CorrelationIdHelper.New(), estateId, contractId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            var model = result.Data!;
            model.ContractId.ShouldBe(contractId);
            model.Description.ShouldBe("Contract Detail");
            model.OperatorName.ShouldBe("OpDetail");
        }

        [Fact]
        public async Task GetContract_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractQueries.GetContractQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure("nf"));

            // Act
            var result = await _service.GetContract(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddProductToContract_SendsCommand_WithValue_WhenNotVariable()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            var productModel = new ContractModels.AddProductModel
            {
                ProductName = "P1",
                DisplayText = "P One",
                IsVariableValue = false,
                Value = 12.34m
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.AddProductToContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.AddProductToContract(CorrelationIdHelper.New(), estateId, contractId, productModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<ContractCommands.AddProductToContractCommand>(c =>
                c.EstateId == estateId &&
                c.ContractId == contractId &&
                c.ProductName == productModel.ProductName &&
                c.DisplayText == productModel.DisplayText &&
                c.Value.HasValue && c.Value.Value == productModel.Value
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddProductToContract_SendsCommand_NullValue_WhenVariable()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var contractId = Guid.NewGuid();

            var productModel = new ContractModels.AddProductModel
            {
                ProductName = "P2",
                DisplayText = "P Two",
                IsVariableValue = true,
                Value = 99.99m
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.AddProductToContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.AddProductToContract(CorrelationIdHelper.New(), estateId, contractId, productModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<ContractCommands.AddProductToContractCommand>(c =>
                c.EstateId == estateId &&
                c.ContractId == contractId &&
                c.ProductName == productModel.ProductName &&
                c.DisplayText == productModel.DisplayText &&
                c.Value == null
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddProductToContract_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.AddProductToContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            // Act
            var result = await _service.AddProductToContract(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), new ContractModels.AddProductModel { ProductName = "x", DisplayText = "y", IsVariableValue = false, Value = 1m });

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AddTransactionFeeToProduct_SendsCorrectCommand()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var contractId = Guid.NewGuid();
            var contractProductId = Guid.NewGuid();

            var feeModel = new ContractModels.AddTransactionFeeModel
            {
                Description = "Fee A",
                FeeValue = 2.50m,
                CalculationType = "Fixed",
                FeeType = "Charge"
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.AddTransactionFeeToProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.AddTransactionFeeToProduct(CorrelationIdHelper.New(), estateId, contractId, contractProductId, feeModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<ContractCommands.AddTransactionFeeToProductCommand>(c =>
                c.EstateId == estateId &&
                c.ContractId == contractId &&
                c.ProductId == contractProductId &&
                c.Description == feeModel.Description &&
                c.Value == feeModel.FeeValue.Value &&
                c.CalculationType == feeModel.CalculationType &&
                c.FeeType == feeModel.FeeType
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddTransactionFeeToProduct_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.AddTransactionFeeToProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            // Act
            var result = await _service.AddTransactionFeeToProduct(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ContractModels.AddTransactionFeeModel { Description = "d", FeeValue = 1m, CalculationType = "c", FeeType = "f" });

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task RemoveTransactionFeeFromProduct_SendsCorrectCommand()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var contractId = Guid.NewGuid();
            var contractProductId = Guid.NewGuid();
            var transactionFeeId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.RemoveTransactionFeeFromProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);

            // Act
            var result = await _service.RemoveTransactionFeeFromProduct(CorrelationIdHelper.New(), estateId, contractId, contractProductId, transactionFeeId);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task RemoveTransactionFeeFromProduct_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.RemoveTransactionFeeFromProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            // Act
            var result = await _service.RemoveTransactionFeeFromProduct(CorrelationIdHelper.New(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Assert
            result.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateContract_SendsCreateCommand_WithParsedOperatorId()
        {
            // Arrange
            var estateId = Guid.NewGuid();
            var operatorId = Guid.NewGuid();
            var createModel = new ContractModels.CreateContractFormModel
            {
                Description = "New Contract",
                OperatorId = operatorId.ToString()
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.CreateContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success);
            
            // Act
            var result = await _service.CreateContract(CorrelationIdHelper.New(), estateId, createModel);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _mockMediator.Verify(m => m.Send(It.Is<ContractCommands.CreateContractCommand>(c =>
                c.EstateId == estateId &&
                c.Description == createModel.Description &&
                c.OperatorId == operatorId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateContract_ReturnsFailure_WhenMediatorFails()
        {
            // Arrange
            _mockMediator
                .Setup(m => m.Send(It.IsAny<ContractCommands.CreateContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure);

            // Act
            var result = await _service.CreateContract(CorrelationIdHelper.New(), Guid.NewGuid(), new ContractModels.CreateContractFormModel { Description = "x", OperatorId = Guid.NewGuid().ToString() });

            // Assert
            result.IsFailed.ShouldBeTrue();
        }
    }
}