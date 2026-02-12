namespace EstateManagementUI.BusinessLogic.Models;

public class ContractModels {
    public class ContractModel {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
        public Guid OperatorId { get; set; }
        public List<ContractProductModel>? Products { get; set; }
    }

    public class ContractDropDownModel {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
    }

    public class ContractProductModel {
        public Guid ContractProductId { get; set; }
        public Int32 ContractProductReportingId { get; set; }
        public string? ProductName { get; set; }
        public string? DisplayText { get; set; }

        public string? ProductType { get; set; }

        // Changed from decimal? to string? to support displaying "Variable" for variable-value products
        // This aligns with how the backend represents variable vs fixed value products
        public string? Value { get; set; }
        public int NumberOfFees { get; set; }
        public List<ContractProductTransactionFeeModel>? TransactionFees { get; set; }
    }

    public class ContractProductTransactionFeeModel {
        public Guid TransactionFeeId { get; set; }
        public string? Description { get; set; }
        public Int32 CalculationType { get; set; }
        public Int32 FeeType { get; set; }
        public decimal Value { get; set; }
    }

    public class RecentContractModel
    {
        public Guid ContractId { get; set; }
        public string? Description { get; set; }
        public string? OperatorName { get; set; }
    }
}