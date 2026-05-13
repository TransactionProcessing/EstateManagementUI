namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class ProductPerformanceResponse
{
    public List<ProductPerformanceDetail> ProductDetails { get; set; }
    public ProductPerformanceSummary Summary { get; set; }
}