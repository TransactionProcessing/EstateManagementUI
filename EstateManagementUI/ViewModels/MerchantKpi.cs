namespace EstateManagementUI.ViewModels;

public class MerchantKpi {
    public int MerchantsWithSaleInLastHour { get; set; }

    public int MerchantsWithNoSaleToday { get; set; }

    public int MerchantsWithNoSaleInLast7Days { get; set; }
}

public class TopBottomMerchant
{
    public string MerchantName { get; set; }

    public Decimal SalesValue { get; set; }
}

public class TopBottomOperator
{
    public string OperatorName { get; set; }

    public Decimal SalesValue { get; set; }
}

public class TopBottomProduct
{
    public string ProductName { get; set; }

    public Decimal SalesValue { get; set; }
}