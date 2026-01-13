using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record MerchantModel {
    public MerchantModel() {
        this.Operators = new ();
        this.Devices = new();
    }

    public Guid MerchantId { get; set; }

    public String MerchantName { get; set; }
    public String SettlementSchedule { get; set; }
    public String MerchantReference { get; set; }
    
    public AddressModel Address { get; set; }
    public ContactModel Contact { get; set; }
    public List<MerchantOperatorModel> Operators { get; set; }
    public Dictionary<Guid, string> Devices { get; set; }

    public List<MerchantContractModel> Contracts { get; set; }
}
