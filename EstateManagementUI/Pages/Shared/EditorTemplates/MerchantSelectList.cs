using System.Collections.ObjectModel;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagmentUI.BusinessLogic.Requests;
using JasperFx.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EstateManagementUI.Pages.Shared.EditorTemplates;

public class MerchantSelectList : DynamicSelect {
    public static List<MerchantModel> Merchants = new List<MerchantModel> {
        new MerchantModel {
            MerchantId = Guid.Parse("9A39A50A-6B47-430F-B84F-CCC7DF051C55"),
            MerchantName = "Test Merchant 1",
            MerchantReference = "Merch1"
        },
        new MerchantModel {
            MerchantId = Guid.Parse("367250CA-9EC9-4432-A314-F86D15E0D8A6"),
            MerchantName = "Test Merchant 2",
            MerchantReference = "Merch2"
        },
        new MerchantModel {
            MerchantId = Guid.Parse("A30B5E1F-C944-4A7E-BC1C-C105DD6FC640"),
            MerchantName = "Test Merchant 3",
            MerchantReference = "Merch3"
        },
    };

    private readonly IMediator Mediator;

    public MerchantSelectList(IMediator mediator) {
        this.Mediator = mediator;
    }

    public override string ItemPartial => this.GetViewPath("MerchantSelectItem");

    protected override Task<List<SelectItem>> GetItemsAsync() {
        IQueryable<MerchantModel> query = Merchants.AsQueryable();

        return Task.FromResult(query.OrderBy(m => m.MerchantName)
            .Select(m => new SelectItem(m.MerchantId.ToString(), m.MerchantName, m.MerchantReference, null))
            .ToList());
    }

    protected override Task<string> FindValueByTextAsync(string text) {
        IQueryable<MerchantModel> query = Merchants.AsQueryable();

        return Task.FromResult(query.OrderBy(m => m.MerchantName)
            .Where(m=> m.MerchantName ==text)
            .Select(m => m.MerchantId.ToString()).SingleOrDefault());

    }

    protected override Task<string> GetTextFromValueAsync(string value) {
        IQueryable<MerchantModel> query = Merchants.AsQueryable();

        return Task.FromResult(query.OrderBy(m => m.MerchantName)
            .Where(m => m.MerchantId.ToString() == value)
            .Select(m => m.MerchantName).SingleOrDefault());
    }
        
}