namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects
{
    public class MerchantDevice
    {
        public Guid MerchantId { get; set; }
        public Guid DeviceId { get; set; }
        public String DeviceIdentifier { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
