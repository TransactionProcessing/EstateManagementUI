using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

namespace EstateManagementUI.BlazorServer.Models;

[ExcludeFromCodeCoverage]
public class ContractModels {
    public class RecentContractModel
    {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
    }
    public class ContractDropDownModel
    {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
    }

    // Contract Models
    public class ContractModel
    {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
        public Guid OperatorId { get; set; }
        public List<ContractProductModel>? Products { get; set; }
    }

    public class ContractProductModel
    {
        public Guid ContractProductId { get; set; }
        public Int32 ContractProductReportingId { get; set; }
        public string? ProductName { get; set; }
        public string? DisplayText { get; set; }
        public ProductType ProductType { get; set; }
        // Changed from decimal? to string? to support displaying "Variable" for variable-value products
        // This aligns with how the backend represents variable vs fixed value products
        public string? Value { get; set; }
        public int NumberOfFees { get; set; }
        public List<ContractProductTransactionFeeModel>? TransactionFees { get; set; }
    }

    public class ContractProductTransactionFeeModel
    {
        public Guid TransactionFeeId { get; set; }
        public string? Description { get; set; }
        public CalculationType CalculationType { get; set; }
        public FeeType FeeType { get; set; }
        public decimal Value { get; set; }
    }

    public class EditContractModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
    }

    public class AddProductModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Display text is required")]
        public string? DisplayText { get; set; }

        public bool IsVariableValue { get; set; }

        public decimal? Value { get; set; }
    }

    public class AddTransactionFeeModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Calculation type is required")]
        public string? CalculationType { get; set; }

        [Required(ErrorMessage = "Fee type is required")]
        public string? FeeType { get; set; }

        [Required(ErrorMessage = "Fee value is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fee value must be greater than 0")]
        public decimal? FeeValue { get; set; }
    }

    public class CreateContractFormModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Operator is required")]
        public string? OperatorId { get; set; }
    }
}