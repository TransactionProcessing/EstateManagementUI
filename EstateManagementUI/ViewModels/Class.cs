namespace EstateManagementUI.ViewModels
{
    public record Operator
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public string RequireCustomMerchantNumber { get; set; }

        public string RequireCustomTerminalNumber { get; set; }

        //public bool IsDeleted { get; set; }
    }

    public record Estate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Reference { get; set; }
    }

    public record User
    {
        public string EmailAddress { get; set; }

        public Guid Id { get; set; }
    }

    public record Merchant
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public string SettlementSchedule { get; set; }
        public string ContactName { get; set; }
        public string AddressLine1 { get; set; }
        public string Town { get; set; }

        public Guid Id { get; set; }
    }

    public record Contract {
        public Guid ContractId { get; set; }
        public string Description { get; set; }
        public string OperatorName { get; set; }
        public Int32 NumberOfProducts { get; set; }

    }

    public record ContractProduct {
        public Guid ContractProductId { get; set; }
        public String ProductName { get; set; }
        public String ProductType { get; set; }
        public String DisplayText { get; set; }
        public String Value { get; set; }
        public Int32 NumberOfFees { get; set; }
    }

    public record ContractProductTransactionFee {
        public Guid ContractProductTransactionFeeId { get; set; }
        public String Description { get; set; }
        public String FeeType { get; set; }
        public String CalculationType { get; set; }
        public Decimal Value { get; set; }
    }
}
