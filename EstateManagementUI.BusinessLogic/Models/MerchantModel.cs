﻿namespace EstateManagementUI.BusinessLogic.Models;

public record MerchantModel {
    public Guid MerchantId { get; set; }

    public String MerchantName { get; set; }
    public String SettlementSchedule { get; set; }
    public String MerchantReference { get; set; }
    public String ContactName { get; set; }
    public String AddressLine1 { get; set; }
    public String Town { get; set; }
}