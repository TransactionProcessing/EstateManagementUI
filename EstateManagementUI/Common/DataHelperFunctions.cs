using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Pages.Components;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleResults;
using System.Globalization;

public static class DataHelperFunctions {
    public static async Task<ComparisonDateListModel> GetComparisonDates(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetComparisonDatesQuery query = new Queries.GetComparisonDatesQuery(accessToken, estateId);

        Result<List<ComparisonDateModel>> response = await mediator.Send(query, CancellationToken.None);

        List<SelectListItem> resultList = new();

        IOrderedEnumerable<ComparisonDateModel> orderedData = response.Data.OrderBy(r => r.OrderValue);

        IEnumerable<DateTime> datesWithStrings = orderedData.Where(o => DateTime.TryParseExact(o.Description, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime dt) == false).Select(d => d.Date);

        foreach (ComparisonDateModel comparisonDateModel in orderedData)
        {
            if (DateTime.TryParseExact(comparisonDateModel.Description, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime dt) && datesWithStrings.Contains(comparisonDateModel.Date))
            {
                continue;
            }

            resultList.Add(new SelectListItem
            {
                Value = comparisonDateModel.Date.ToString(),
                Text = comparisonDateModel.Description,
            });
        }

        

        resultList.First().Selected = true;
        return new ComparisonDateListModel { SelectedDate = orderedData.First().Date, Dates = resultList };
    }

    public static List<OptionItem> GetSettlementSchedules()
    {
        List<OptionItem> schedules = new List<OptionItem>();

        schedules.Add(new OptionItem(-1, "- Select a Settlement Schedule - "));
        schedules.Add(new OptionItem(0, "Immediate"));
        schedules.Add(new OptionItem(1, "Weekly"));
        schedules.Add(new OptionItem(2, "Monthly"));

        return schedules;
    }

    public static async Task<OperatorListModel> GetOperatorsOld(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(accessToken, estateId);

        Result<List<OperatorModel>> response = await mediator.Send(query, CancellationToken.None);

        List<SelectListItem> resultList = new();
        foreach (OperatorModel operatorModel in response.Data)
        {
            resultList.Add(new SelectListItem
            {
                Value = operatorModel.OperatorId.ToString(),
                Text = operatorModel.Name
            });
        }

        List<SelectListItem> ordered = resultList.OrderBy(m => m.Text).ToList();
        ordered.Insert(0, new SelectListItem("- Select an Operator -", "", true));
        return new OperatorListModel() { Operators = ordered };
    }


    public static async Task<List<OptionItem>> GetOperators(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetOperatorsQuery query = new Queries.GetOperatorsQuery(accessToken, estateId);

        Result<List<OperatorModel>> response = await mediator.Send(query, CancellationToken.None);

        List<OptionItem> resultList = new();
        foreach (OperatorModel operatorModel in response.Data)
        {
            resultList.Add(new OptionItem(operatorModel.OperatorId.ToString(),operatorModel.Name));
        }

        List<OptionItem> ordered = resultList.OrderBy(m => m.Text).ToList();
        ordered.Insert(0, new OptionItem("-1","- Select an Operator -"));

        return ordered;
    }

    public static async Task<MerchantListModel> GetMerchants(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(accessToken, estateId);

        Result<List<MerchantModel>> response = await mediator.Send(query, CancellationToken.None);

        List<SelectListItem> resultList = new();
        foreach (MerchantModel merchantModel in response.Data)
        {
            resultList.Add(new SelectListItem
            {
                Value = merchantModel.MerchantId.ToString(),
                Text = merchantModel.MerchantName,
            });
        }

        List<SelectListItem> ordered = resultList.OrderBy(m => m.Text).ToList();
        ordered.Insert(0, new SelectListItem("- Select a Merchant -", "", true));
        return new MerchantListModel { Merchants = ordered };
    }

    public static async Task<ContractListModel> GetContracts(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetContractsQuery query = new(accessToken, estateId);

        List<ContractModel> response = await mediator.Send(query, CancellationToken.None);

        List<SelectListItem> resultList = new();
        foreach (var contractModel in response)
        {
            resultList.Add(new SelectListItem
            {
                Value = contractModel.ContractId.ToString(),
                Text = contractModel.Description
            });
        }

        List<SelectListItem> ordered = resultList.OrderBy(m => m.Text).ToList();
        ordered.Insert(0, new SelectListItem("- Select a Contract -", "", true));
        return new ContractListModel { Contracts = ordered };
    }

    public static async Task<ProductTypeListModel> GetProductTypes(String accessToken, Guid estateId)
    {
        // TODO: this will need to make a query to the Estate Management API to product types pertaining
        // to the contact operator

        var result = new ProductTypeListModel { ProductTypes = new List<SelectListItem>() };
        result.ProductTypes.Add(new SelectListItem("- Select a Product Type -","0", true));
        result.ProductTypes.Add(new SelectListItem("Bill Payment", ((Int32)ProductType.BillPayment).ToString()));
        result.ProductTypes.Add(new SelectListItem("Mobile Topup", ((Int32)ProductType.MobileTopup).ToString()));
        result.ProductTypes.Add(new SelectListItem("Voucher", ((Int32)ProductType.Voucher).ToString()));
        return result;
    }
}