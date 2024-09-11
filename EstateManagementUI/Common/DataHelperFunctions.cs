using System.Globalization;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleResults;

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

    public static async Task<OperatorListModel> GetOperators(String accessToken, Guid estateId, IMediator mediator)
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

    public static async Task<MerchantListModel> GetMerchants(String accessToken, Guid estateId, IMediator mediator)
    {
        Queries.GetMerchantsQuery query = new Queries.GetMerchantsQuery(accessToken, estateId);

        List<MerchantModel> response = await mediator.Send(query, CancellationToken.None);

        List<SelectListItem> resultList = new();
        foreach (MerchantModel merchantModel in response)
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
}