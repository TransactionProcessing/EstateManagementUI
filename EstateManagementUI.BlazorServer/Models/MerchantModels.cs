using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Models;

public class MerchantModels
{
    public class MerchantModel
    {
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
        public Guid ContactId { get; set; }
        public string? Country { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmailAddress { get; set; }
        public string? ContactPhoneNumber { get; set; }
    }

    public class MerchantOperatorModel
    {
        public Guid MerchantId { get; set; }
        public Guid OperatorId { get; set; }
        public String OperatorName { get; set; }
        public String MerchantNumber { get; set; }
        public String TerminalNumber { get; set; }
        public Boolean IsDeleted { get; set; }
    }

    public class MerchantContractModel
    {
        public Guid MerchantId { get; set; }
        public Guid ContractId { get; set; }
        public String OperatorName { get; set; }
        public String ContractName { get; set; }
        public Boolean IsDeleted { get; set; }
        public List<MerchantContractProductModel> ContractProducts { get; set; }
    }

    public class MerchantContractProductModel
    {
        public Guid MerchantId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ProductId { get; set; }
        public String ProductName { get; set; }
        public String DisplayText { get; set; }
        public String ProductType { get; set; }
        public Decimal? Value { get; set; }
    }

    public class MerchantDeviceModel
    {
        public Guid MerchantId { get; set; }
        public Guid DeviceId { get; set; }
        public String DeviceIdentifier { get; set; }
        public Boolean IsDeleted { get; set; }
    }

    public class MerchantDropDownModel
    {
        public Guid MerchantId { get; set; }
        public string? MerchantName { get; set; }
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

    public class CreateMerchantModel
    {
        [Required(ErrorMessage = "Merchant name is required")]
        public string? MerchantName { get; set; }

        [Required(ErrorMessage = "Settlement schedule is required")]
        public string? SettlementSchedule { get; set; }

        [Required(ErrorMessage = "Address line 1 is required")]
        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "Town is required")]
        public string? Town { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string? Region { get; set; }

        [Required(ErrorMessage = "PostCode is required")]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Contact name is required")]
        public string? ContactName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
    }

    public class MerchantEditModel
    {
        [Required(ErrorMessage = "Merchant name is required")]
        public string? MerchantName { get; set; }

        public string? SettlementSchedule { get; set; }

        public Guid AddressId { get; set; }

        [Required(ErrorMessage = "Address line 1 is required")]
        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "Town is required")]
        public string? Town { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string? Region { get; set; }

        [Required(ErrorMessage = "PostCode is required")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        public Guid ContactId { get; set; }
        [Required(ErrorMessage = "Contact name is required")]
        public string? ContactName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? ContactEmailAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? ContactPhoneNumber { get; set; }
    }

    public class DepositModel
    {
        [Required(ErrorMessage = "Deposit amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Deposit amount must be greater than 0")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Date of deposit is required")]
        [DateNotInFuture(ErrorMessage = "Date cannot be in the future")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Reference is required")]
        public string? Reference { get; set; }
    }

    private class DateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date.Date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the future");
                }
            }
            return ValidationResult.Success;
        }
    }
}