using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog.LayoutRenderers.Wrappers;
using SimpleResults;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

public class Merchant : SecureHydroComponent
{
    private readonly IMediator Mediator;
    public String ActiveTab { get; set; }
    [Display(Name = "Settlement Schedule")]
    public SettlementScheduleListModel SettlementSchedule { get; set; }

    public AddressViewModel Address { get; set; }
    public ContactViewModel Contact { get; set; }

    public List<MerchantOperatorViewModel> Operators { get; set; }
    public List<MerchantDeviceViewModel> Devices { get; set; }
    public List<MerchantContractModel> Contracts { get; set; }

    public Merchant(IMediator mediator, IPermissionsService permissionsService, String merchantFunction) : base(ApplicationSections.Merchant, merchantFunction, permissionsService)
    {
        this.Mediator = mediator;

        Subscribe<MerchantPageEvents.MerchantCreatedEvent>(Handle);
        Subscribe<MerchantPageEvents.MerchantUpdatedEvent>(Handle);
    }

    [ExcludeFromCodeCoverage]
    private async Task Handle(MerchantPageEvents.MerchantCreatedEvent obj)
    {
        this.Dispatch(new ShowMessage("Merchant Created Successfully", ToastType.Success), Scope.Global);
        await Task.Delay(1000); // TODO: might be a better way of handling this
        this.Close();
    }

    [ExcludeFromCodeCoverage]
    private async Task Handle(MerchantPageEvents.MerchantUpdatedEvent obj) {
        this.Dispatch(new ShowMessage("Merchant Updated Successfully", ToastType.Success), Scope.Global);
        await Task.Delay(1000); // TODO: might be a better way of handling this
        this.Close();
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

    protected async Task LoadMerchant(CancellationToken cancellationToken)
    {
        await this.PopulateTokenAndEstateId();

        Queries.GetMerchantQuery query = new(this.AccessToken, this.EstateId, this.MerchantId);
        Result<MerchantModel> result = await this.Mediator.Send(query, cancellationToken);
        if (result.IsFailed)
        {
            // handle this
        }
        
        // We need to now get all the contracts here to populate the names on the Merchant Model
        Queries.GetContractsQuery contractsQuery = new Queries.GetContractsQuery(this.AccessToken, this.EstateId);
        var contractsResult = await this.Mediator.Send(contractsQuery, cancellationToken);
        if (result.IsFailed) {
            // TODO: Handle this
        }


        this.Name = result.Data.MerchantName;
        this.Reference = result.Data.MerchantReference;
        //this.SettlementSchedule.SettlementScheduleId = result.Data.SettlementSchedule switch {
            
        //}
        BusinessLogic.Models.SettlementSchedule settlementSchedule =
            Enum.Parse<SettlementSchedule>(result.Data.SettlementSchedule);
        this.SettlementSchedule.SettlementScheduleId = (Int32)settlementSchedule;
        this.Address = new AddressViewModel {
            AddressLine1 = result.Data.Address.AddressLine1,
            AddressLine2 = result.Data.Address.AddressLine2,
            Region = result.Data.Address.Region,
            Country = result.Data.Address.Country,
            PostCode = result.Data.Address.PostalCode,
            Town = result.Data.Address.Town,
            AddressId = result.Data.Address.AddressId,
        };
        this.Contact = new ContactViewModel {
            ContactName = result.Data.Contact.ContactName,
            EmailAddress = result.Data.Contact.ContactEmailAddress,
            PhoneNumber = result.Data.Contact.ContactPhoneNumber,
            ContactId = result.Data.Contact.ContactId
        };
        this.Operators = new List<MerchantOperatorViewModel>();
        foreach (MerchantOperatorModel merchantOperatorModel in result.Data.Operators) {
            this.Operators.Add(new MerchantOperatorViewModel {
                IsDeleted = merchantOperatorModel.IsDeleted,
                MerchantNumber = String.IsNullOrEmpty(merchantOperatorModel.MerchantNumber) ? "None": merchantOperatorModel.MerchantNumber,
                Name = merchantOperatorModel.Name,
                OperatorId = merchantOperatorModel.OperatorId,
                TerminalNumber = String.IsNullOrEmpty(merchantOperatorModel.TerminalNumber) ? "None" : merchantOperatorModel.TerminalNumber,
            });
        }

        this.Contracts = new List<MerchantContractModel>();
        foreach (MerchantContractModel merchantContractModel in result.Data.Contracts) {
            ContractModel contract = contractsResult.SingleOrDefault(c => c.ContractId == merchantContractModel.ContractId);
            this.Contracts.Add(new MerchantContractModel {
                ContractId = merchantContractModel.ContractId,
                IsDeleted = merchantContractModel.IsDeleted,
                Name = contract == null ? "N/A" : contract.Description
            });
        }

        this.Devices = new List<MerchantDeviceViewModel>();
        foreach (KeyValuePair<Guid, String> device in result.Data.Devices) {
            this.Devices.Add(new MerchantDeviceViewModel {
                DeviceId = device.Key,
                DeviceIdentifier = device.Value
            });
        }
    }

    public Guid MerchantId { get; set; }

    [Required(ErrorMessage = "A name is required to create a Merchant")]
    public string Name { get; set; }

    public string Reference { get; set; }

    public void Close() => this.Location("/Merchant/Index");

    private async Task CreateNewMerchant() {
        BusinessLogic.Models.CreateMerchantModel createMerchantModel = new()
        {
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
            SettlementSchedule = this.SettlementSchedule.SettlementScheduleId switch
            {
                1 => BusinessLogic.Models.SettlementSchedule.Weekly,
                2 => BusinessLogic.Models.SettlementSchedule.Monthly,
                _ => BusinessLogic.Models.SettlementSchedule.Immediate
            },
        };

        Commands.AddMerchantCommand command = new(this.AccessToken, this.EstateId, createMerchantModel);

        Result result = await this.Mediator.Send(command, CancellationToken.None);

        if (result.IsFailed)
        {
            this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
            return;
        }

        this.Dispatch(new MerchantPageEvents.MerchantCreatedEvent(), Scope.Global);
    }

    private async Task UpdateExitingMerchant() {
        List<IRequest<Result>> commands = new();
        
        UpdateMerchantModel updateMerchantModel = new() {
            SettlementSchedule = this.SettlementSchedule.SettlementScheduleId switch
            {
                1 => BusinessLogic.Models.SettlementSchedule.Weekly,
                2 => BusinessLogic.Models.SettlementSchedule.Monthly,
                _ => BusinessLogic.Models.SettlementSchedule.Immediate
            },
            MerchantName = this.Name
        };
        commands.Add(new Commands.UpdateMerchantCommand(this.AccessToken, this.EstateId, this.MerchantId, updateMerchantModel));


        AddressModel updateAddressModel = new() {
            Region = this.Address.Region,
            AddressLine1 = this.Address.AddressLine1,
            AddressLine2 = this.Address.AddressLine2,
            Country = this.Address.Country,
            Town = this.Address.Town,
            PostalCode = this.Address.PostCode,
            AddressId = this.Address.AddressId,
        };
        commands.Add(new Commands.UpdateMerchantAddressCommand(this.AccessToken, this.EstateId, this.MerchantId, updateAddressModel));

        ContactModel updateContactModel = new() {
            ContactName = this.Contact.ContactName,
            ContactEmailAddress = this.Contact.EmailAddress,
            ContactPhoneNumber = this.Contact.PhoneNumber,
            ContactId = this.Contact.ContactId
        };
        commands.Add(new Commands.UpdateMerchantContactCommand(this.AccessToken, this.EstateId, this.MerchantId,
            updateContactModel));

        foreach (IRequest<Result> command in commands) {
            Result result = await this.Mediator.Send(command, CancellationToken.None);

            if (result.IsFailed)
            {
                this.Dispatch(new ShowMessage(result.Errors.Single(), ToastType.Error), Scope.Global);
                return;
            }    
        }
        this.Dispatch(new MerchantPageEvents.MerchantUpdatedEvent(), Scope.Global);
    }

    public async Task Save() {
        if (!this.ModelState.IsValid) {
            return;
        }
        await this.PopulateTokenAndEstateId();

        Task t = this.MerchantId switch {
            _ when this.MerchantId == Guid.Empty => this.CreateNewMerchant(),
            _ => this.UpdateExitingMerchant(),

        };

        await t;
    }

    public async Task RemoveOperator(Guid merchantId,
                               Guid operatorId)
    {
        await this.PopulateTokenAndEstateId();

        Commands.RemoveOperatorFromMerchantCommand removeOperatorFromMerchantCommand =
            new(this.AccessToken, this.EstateId, merchantId, operatorId);
        await this.Mediator.Send(removeOperatorFromMerchantCommand, CancellationToken.None);

        // TODO: handle result
        this.Dispatch(new MerchantPageEvents.OperatorRemovedFromMerchantEvent(), Scope.Global);
    }

    public async Task RemoveContract(Guid merchantId,
                               Guid contractId)
    {
        await this.PopulateTokenAndEstateId();

        Commands.RemoveContractFromMerchantCommand removeContractFromMerchantCommand =
            new(this.AccessToken, this.EstateId, merchantId, contractId);
        await this.Mediator.Send(removeContractFromMerchantCommand, CancellationToken.None);

        // TODO: handle result
        this.Dispatch(new MerchantPageEvents.ContractRemovedFromMerchantEvent(), Scope.Global);
    }
}

[ExcludeFromCodeCoverage]
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
    public Guid AddressId { get; set; }
}

[ExcludeFromCodeCoverage]
public class ContactViewModel {
    [Display(Name = "Contact Name")]
    public String ContactName { get; set; }
    [Display(Name = "Email Address")]
    public String EmailAddress { get; set; }
    [Display(Name = "Phone Number")]
    public String PhoneNumber{ get; set; }
    public Guid ContactId { get; set; }
}

[ExcludeFromCodeCoverage]
public class MerchantOperatorViewModel
{
    [Display(Name = "Operator Name")]
    public String Name { get; set; }

    public Guid OperatorId { get; set; }
    [Display(Name = "Merchant Number")]
    public string MerchantNumber { get; set; }
    [Display(Name = "Terminal Number")]
    public string TerminalNumber { get; set; }
    [Display(Name = "Deleted")]
    public bool IsDeleted { get; set; }
}

[ExcludeFromCodeCoverage]
public class MerchantDeviceViewModel
{
    public Guid DeviceId { get; set; }
    [Display(Name = "Device Identifier")]
    public string DeviceIdentifier { get; set; }
}

[ExcludeFromCodeCoverage]
public class MerchantContractViewModel
{
    public Guid ContractId { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}