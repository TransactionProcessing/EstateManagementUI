using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.Models {
    public class MerchantModels {
        public class MerchantModel {
            public Guid MerchantId { get; set; }
            public string? MerchantName { get; set; }
            public string? MerchantReference { get; set; }
            public decimal? Balance { get; set; }
            public decimal? AvailableBalance { get; set; }
            public string? SettlementSchedule { get; set; }

            public Guid AddressId { get; set; }
            public string? AddressLine1 { get; set; }
            public string? AddressLine2 { get; set; }
            public string? Town { get; set; }
            public string? Region { get; set; }
            public string? PostalCode { get; set; }
            public string? Country { get; set; }
            public Guid ContactId { get; set; }
            public string? ContactName { get; set; }
            public string? ContactEmailAddress { get; set; }
            public string? ContactPhoneNumber { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }

        public class MerchantListModel {
            public Guid MerchantId { get; set; }
            public string? MerchantName { get; set; }
            public string? MerchantReference { get; set; }
            public decimal? Balance { get; set; }
            public decimal? AvailableBalance { get; set; }
            public string? SettlementSchedule { get; set; }
            public string? Region { get; set; }
            public string? PostalCode { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }

        public class MerchantDropDownModel {
            public Guid MerchantId { get; set; }
            public Int32 MerchantReportingId { get; set; }
            public string? MerchantName { get; set; }
        }

        public class RecentMerchantsModel {
            public DateTime CreatedDateTime { get; set; }
            public Guid MerchantId { get; set; }
            public String Name { get; set; }
            public String Reference { get; set; }
        }

        public class MerchantOperatorModel {
            public Guid MerchantId { get; set; }
            public Guid OperatorId { get; set; }
            public String OperatorName { get; set; }
            public String MerchantNumber { get; set; }
            public String TerminalNumber { get; set; }
            public Boolean IsDeleted { get; set; }
        }

        public class MerchantContractModel {
            public Guid MerchantId { get; set; }
            public Guid ContractId { get; set; }
            public String OperatorName { get; set; }
            public String ContractName { get; set; }
            public Boolean IsDeleted { get; set; }
            public List<MerchantContractProductModel> ContractProducts { get; set; }
        }

        public class MerchantContractProductModel {
            public Guid MerchantId { get; set; }
            public Guid ContractId { get; set; }
            public Guid ProductId { get; set; }
            public String ProductName { get; set; }
            public String DisplayText { get; set; }
            public String ProductType { get; set; }
            public Decimal? Value { get; set; }
        }

        public class MerchantDeviceModel {
            public Guid MerchantId { get; set; }
            public Guid DeviceId { get; set; }
            public String DeviceIdentifier { get; set; }
            public Boolean IsDeleted { get; set; }
        }

        public class MerchantKpiModel
        {
            public int MerchantsWithNoSaleInLast7Days { get; set; }
            public int MerchantsWithNoSaleToday { get; set; }
            public int MerchantsWithSaleInLastHour { get; set; }
        }
    }
}