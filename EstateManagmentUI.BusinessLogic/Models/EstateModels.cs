namespace EstateManagementUI.BusinessLogic.Models;

public class EstateModels {
    public class EstateModel {
        public Guid EstateId { get; set; }
        public string? EstateName { get; set; }
        public string? Reference { get; set; }
        public List<EstateOperatorModel>? Operators { get; set; }
        public List<EstateMerchantModel>? Merchants { get; set; }
        public List<EstateContractModel>? Contracts { get; set; }
        public List<EstateUserModel>? Users { get; set; }
    }

    public class EstateUserModel {
        public Guid UserId { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class EstateOperatorModel {
        public Guid OperatorId { get; set; }
        public string? Name { get; set; }
        public bool RequireCustomMerchantNumber { get; set; }
        public bool RequireCustomTerminalNumber { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class EstateContractModel {
        public Guid OperatorId { get; set; }
        public Guid ContractId { get; set; }
        public string? Name { get; set; }
        public string? OperatorName { get; set; }
    }


    public class EstateMerchantModel {
        public Guid MerchantId { get; set; }
        public string? Name { get; set; }
        public string? Reference { get; set; }
    }
}