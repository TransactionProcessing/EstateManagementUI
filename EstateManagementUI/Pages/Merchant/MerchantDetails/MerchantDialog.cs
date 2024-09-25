using System.ComponentModel.DataAnnotations;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.PermissionManagement.Roles;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using Hydro.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleResults;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

public partial class MerchantDialog : SecureHydroComponent
{
    private readonly IMediator Mediator;

    [Display(Name = "Settlement Schedule")]
    public SettlementScheduleListModel SettlementSchedule { get; set; }

    public AddressViewModel Address { get; set; }
    public ContactViewModel Contact { get; set; }
    public MerchantDialog(IMediator mediator, IPermissionsService permissionsService, String merchantFunction) : base(ApplicationSections.Merchant, merchantFunction, permissionsService)
    {
        this.Mediator = mediator;
    }
    
    public override async Task MountAsync() {
        this.SettlementSchedule ??= new SettlementScheduleListModel {
            SettlementSchedule = new List<SelectListItem> {
                new SelectListItem { Text = "Immediate", Value = "0" },
                new SelectListItem { Text = "Weekly", Value = "1" },
                new SelectListItem { Text = "Monthly", Value = "2" }
            }
        };

        this.Address ??= new AddressViewModel();
        this.Contact??= new ContactViewModel();

        if (this.MerchantId != Guid.Empty)
        {
            await this.LoadMerchant(CancellationToken.None);
        }
    }

    private async Task LoadMerchant(CancellationToken cancellationToken)
    {
        await this.PopulateTokenAndEstateId();

        // TODO: this is a seperate
        //Queries.GetOperatorQuery query =
        //    new Queries.GetOperatorQuery(this.AccessToken, this.EstateId, this.OperatorId);
        //Result<OperatorModel> result = await this.Mediator.Send(query, cancellationToken);
        //if (result.IsFailed)
        //{
        //    // handle this
        //}

        //this.Name = result.Data.Name;
        //this.RequireCustomTerminalNumber = result.Data.RequireCustomTerminalNumber;
        //this.RequireCustomMerchantNumber = result.Data.RequireCustomMerchantNumber;
    }

    public Guid MerchantId { get; set; }

    [Required(ErrorMessage = "A name is required to create a Merchant")]
    public string Name { get; set; }

    public void Close() =>
        this.Dispatch(new MerchantPageEvents.HideNewMerchantDialog(), Scope.Global);

    public async Task Save()
    {
        if (!this.ModelState.IsValid)
        {
            return;
        }
        await this.PopulateTokenAndEstateId();

        if (this.MerchantId == Guid.Empty) {
            BusinessLogic.Models.CreateMerchantModel createMerchantModel = new() {
                MerchantName = this.Name,
                Address = new AddressModel
                {
                    AddressId = Guid.NewGuid(),
                    AddressLine1 = this.Address.AddressLine1,
                    AddressLine2 = this.Address.AddressLine2,
                    Country = this.Address.Country,
                    PostalCode = this.Address.PostCode,
                    Region = this.Address.Region,
                    Town = this.Address.Town,
                },
                Contact = new ContactModel
                {
                    ContactName = this.Contact.ContactName,
                    ContactEmailAddress = this.Contact.EmailAddress,
                    ContactPhoneNumber = this.Contact.PhoneNumber,
                    ContactId = Guid.NewGuid()
                },
                SettlementSchedule = this.SettlementSchedule.SettlementScheduleId switch {
                    1 => BusinessLogic.Models.SettlementSchedule.Weekly,
                    2 => BusinessLogic.Models.SettlementSchedule.Monthly,
                    _ => BusinessLogic.Models.SettlementSchedule.Immediate
                }
            };

            Commands.AddNewMerchantCommand command =
                new Commands.AddNewMerchantCommand(this.AccessToken, this.EstateId, createMerchantModel);

                Result result = await this.Mediator.Send(command, CancellationToken.None);

                if (result.IsFailed)
                {
                    this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                    return;
                }

                this.Dispatch(new MerchantPageEvents.MerchantCreatedEvent(), Scope.Global);
        }
        //else
        //{
        //    Commands.UpdateOperatorCommand command = new Commands.UpdateOperatorCommand(this.AccessToken, this.EstateId, this.OperatorId,
        //        this.Name, this.RequireCustomMerchantNumber, this.RequireCustomTerminalNumber);

        //    Result result = await this.Mediator.Send(command, CancellationToken.None);

        //    if (result.IsFailed)
        //    {
        //        this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
        //        return;
        //    }

        //    this.Dispatch(new OperatorPageEvents.OperatorUpdatedEvent(), Scope.Global);
        //}

        this.Close();
    }
}

public class AddressViewModel {
    [Display(Name = "Address Line 1")]
    public String AddressLine1 { get; set; }
    [Display(Name = "Address Line 2")]
    public String AddressLine2 { get; set; }
    [Display(Name = "Town")]
    public String Town { get; set; }
    [Display(Name = "Region")]
    public String Region { get; set; }
    [Display(Name = "Post Code")]
    public String PostCode { get; set; }
    [Display(Name = "Country")]
    public String Country { get; set; }
}

public class ContactViewModel {
    [Display(Name = "Contact Name")]
    public String ContactName { get; set; }
    [Display(Name = "Email Address")]
    public String EmailAddress { get; set; }
    [Display(Name = "Phone Number")]
    public String PhoneNumber{ get; set; }
}