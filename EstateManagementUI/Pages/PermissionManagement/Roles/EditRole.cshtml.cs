using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;
using static Hydro.Scope;

namespace EstateManagementUI.Pages.PermissionManagement.Roles;

public class EditRole : HydroComponent
{
    private readonly IPermissionsRepository PermissionsRepository;
    public Int32 Id { get; set; }
    public String Name { get; set; }

    public List<(ApplicationSection, List<(Function, Boolean)>)> Permissions { get; set; }

    public EditRole(IPermissionsRepository permissionsRepository) {
        this.PermissionsRepository = permissionsRepository;
    }

    public override async Task MountAsync() {
        Result<BusinessLogic.PermissionService.Database.Entities.Role> role = await this.PermissionsRepository.GetRole(this.Id, CancellationToken.None);

        Result<List<(ApplicationSection, Function, Boolean)>> permissionsList = await this.PermissionsRepository.GetRolePermissions(this.Id, CancellationToken.None);
        //this.
        List<ApplicationSection> applicationSections = permissionsList.Data.Select(p => p.Item1).Distinct().ToList();
        this.Permissions = new List<(ApplicationSection, List<(Function, Boolean)>)>();
        foreach (ApplicationSection applicationSection in applicationSections) {
            List<(Function, Boolean)> functionAccess = permissionsList.Data.Where(p =>
                p.Item1.ApplicationSectionId == applicationSection.ApplicationSectionId).Select(x => (x.Item2, x.Item3)).ToList();
            this.Permissions.Add((applicationSection, functionAccess));
        }
        
        this.Name = role.Data.Name;
        
    }

    public async Task Save()
    {
        if (!Validate())
        {
            return;
        }

        //var customer = await database.Query<Customer>(this.Id).SingleAsync();

        //customer.Edit(
        //    name: this.Name,
        //    taxId: this.TaxId,
        //    currencyCode: this.CurrencyCode,
        //    address: this.Address,
        //    city: this.City,
        //    countryCode: this.CountryCode,
        //    paymentTerms: this.PaymentTerms
        //);

        //await database.SaveChangesAsync();

        //this.Back();
        //this.Dispatch(new ShowMessage("Customer has been updated"), Global);
    }

    //public async Task Remove()
    //{
    //    var customer = await database.Query<Customer>(this.Id).SingleAsync();
    //    customer.Remove();
    //    await database.SaveChangesAsync();
    //    this.Back();
    //}

    [SkipOutput]
    public void Back() =>
        Location(Url.Page("/PermissionManagement/RoleList"));
}
