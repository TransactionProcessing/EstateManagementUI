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
}
