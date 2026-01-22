using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.Models
{
    public class MerchantModel
    {
        public Guid MerchantId { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantReference { get; set; }
        public decimal? Balance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public string? SettlementSchedule { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmailAddress { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class MerchantListModel
    {
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

    public class MerchantDropDownModel
    {
        public Guid MerchantId { get; set; }
        public string? MerchantName { get; set; }
    }

    public class RecentMerchantsModel
    {
        public DateTime CreatedDateTime { get; set; }
        public Guid MerchantId { get; set; }
        public String Name { get; set; }
        public String Reference { get; set; }
    }
}
