using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Results;
using SimpleResults;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class Edit
    {
        [Parameter]
        public Guid MerchantId { get; set; }

        private MerchantModel? merchant;
        private bool isLoading = true;
        private bool isSaving = false;
        private string? errorMessage;
        private string? successMessage;
        private string activeTab = "details";

        // Unified model for editing
        private MerchantEditModel merchantEditModel = new();

        // Operators
        private List<OperatorDropDownModel>? availableOperators;
        private List<MerchantOperatorModel> assignedOperators = new();
        private bool showAddOperator = false;
        private string? selectedOperatorId;
        private OperatorModel? selectedOperator;
        private string? merchantNumber;
        private string? terminalNumber;
        private string? merchantNumberError;
        private string? terminalNumberError;

        // Contracts
        private List<ContractDropDownModel>? availableContracts;
        private List<MerchantContractModel> assignedContracts = new();
        private bool showAddContract = false;
        private string? selectedContractId;

        // Devices
        private List<MerchantDeviceModel> assignedDevices = new();
        private bool showAddDevice = false;
        private string? deviceIdentifier;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            try
            {
                await base.OnInitializedAsync();

                await RequirePermission(PermissionSection.Merchant, PermissionFunction.Edit);

                var loadMerchantResult = await LoadMerchant();

                if (loadMerchantResult.IsFailed) {
                    this.NavigationManager.NavigateToErrorPage();
                }

                var loadOperatorsTask = LoadOperators();
                var loadContractsTask = LoadContracts();

                await Task.WhenAll(loadOperatorsTask, loadContractsTask);

                if (loadOperatorsTask.Result.IsFailed || loadContractsTask.Result.IsFailed) {
                    this.NavigationManager.NavigateToErrorPage();
                }

            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private async Task<Result> LoadMerchant()
        {
            try
            {
                isLoading = true;
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var getMerchantResult = await Mediator.Send(new MerchantQueries.GetMerchantQuery(correlationId, estateId, MerchantId));

                if (getMerchantResult.IsFailed)
                    return ResultHelpers.CreateFailure(getMerchantResult);

                merchant = ModelFactory.ConvertFrom(getMerchantResult.Data);

                // Initialize unified model with current values
                merchantEditModel = new MerchantEditModel
                {
                    MerchantName = merchant.MerchantName,
                    SettlementSchedule = merchant.SettlementSchedule ?? "Immediate",
                    AddressId = this.merchant.AddressId,
                    AddressLine1 = merchant.AddressLine1,
                    AddressLine2 = merchant.AddressLine2,
                    Town = merchant.Town,
                    Region = merchant.Region,
                    PostalCode = merchant.PostalCode,
                    Country = merchant.Country,
                    ContactId = this.merchant.ContactId,
                    ContactName = merchant.ContactName,
                    ContactEmailAddress = merchant.ContactEmailAddress,
                    ContactPhoneNumber = merchant.ContactPhoneNumber,
                };

                var operatorsResultTask = Mediator.Send(new MerchantQueries.GetMerchantOperatorsQuery(correlationId, estateId, MerchantId));
                var contractsResultTask = Mediator.Send(new MerchantQueries.GetMerchantContractsQuery(correlationId, estateId, MerchantId));
                var devicesResultTask = Mediator.Send(new MerchantQueries.GetMerchantDevicesQuery(correlationId, estateId, MerchantId));

                await Task.WhenAll(operatorsResultTask, contractsResultTask, devicesResultTask);

                if (operatorsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(operatorsResultTask.Result);

                if (contractsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(contractsResultTask.Result);

                if (devicesResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(devicesResultTask.Result);

                assignedOperators = ModelFactory.ConvertFrom(operatorsResultTask.Result.Data);
                assignedContracts = ModelFactory.ConvertFrom(contractsResultTask.Result.Data);
                this.assignedDevices = ModelFactory.ConvertFrom(devicesResultTask.Result.Data);

                return Result.Success();
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task<Result> LoadOperators() {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await Mediator.Send(new OperatorQueries.GetOperatorsForDropDownQuery(correlationId, estateId));

            if (result.IsFailed) {
                return ResultHelpers.CreateFailure(result);
            }

            var unfiltered = ModelFactory.ConvertFrom(result.Data);
            this.availableOperators = unfiltered.Where(u => this.assignedOperators.Select(a => a.OperatorId).Contains(u.OperatorId) == false).ToList();

            return Result.Success();
        }

        private async Task<Result> LoadContracts()
        {
            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await Mediator.Send(new ContractQueries.GetContractsForDropDownQuery(correlationId,  estateId));
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }
            
            var unfiltered = ModelFactory.ConvertFrom(result.Data);
            this.availableContracts = unfiltered.Where(u => this.assignedContracts.Select(a => a.ContractId).Contains(u.ContractId) == false).ToList();
            
            return Result.Success();
        }

        private void SetActiveTab(string tab)
        {
            activeTab = tab;
            ClearMessages();
        }

        private string GetTabClass(string tab)
        {
            var baseClass = "px-6 py-3 text-sm font-medium border-b-2 transition-colors ";
            return tab == activeTab
                ? baseClass + "border-blue-600 text-blue-600"
                : baseClass + "border-transparent text-gray-600 hover:text-gray-800 hover:border-gray-300";
        }

        private void HandleInvalidSubmit(EditContext editContext)
        {
            // Aggregate validation messages so the user sees why the submit didn't run
            var errors = editContext.GetValidationMessages().ToList();
            if (errors.Any())
            {
                errorMessage = string.Join(" \u2014 ", errors); // em dash separator
            }
            else
            {
                errorMessage = "Form is invalid. Please check the highlighted fields.";
            }

            // Ensure UI updates
            StateHasChanged();
        }

        private async Task SaveAllChanges()
        {
            isSaving = true;
            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var address = new MerchantCommands.MerchantAddress(this.merchantEditModel.AddressId, this.merchantEditModel.AddressLine1, this.merchantEditModel.Town, this.merchantEditModel.Region, this.merchantEditModel.PostalCode, this.merchantEditModel.Country);
                var contact = new MerchantCommands.MerchantContact(this.merchantEditModel.ContactId, this.merchantEditModel.ContactName, this.merchantEditModel.ContactEmailAddress, this.merchantEditModel.ContactPhoneNumber);
                var updateMerchantCommand = new MerchantCommands.UpdateMerchantCommand(correlationId, estateId, this.MerchantId, this.merchantEditModel.MerchantName, this.merchantEditModel.SettlementSchedule, address, contact);

                var updateResult = await Mediator.Send(updateMerchantCommand);
                if (updateResult.IsFailed) {
                    errorMessage = updateResult.Message ?? "Failed to update merchant";
                    return;
                }

                // Show success message briefly before navigating away
                successMessage = "Merchant details updated successfully";
                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await Task.Delay(2500);
                
                // Navigate to edit page
                NavigationManager.NavigateTo($"/merchants");
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                isSaving = false;
            }
        }

        private async Task OnOperatorSelected()
        {
            if (string.IsNullOrEmpty(selectedOperatorId))
            {
                selectedOperator = null;
                merchantNumber = null;
                terminalNumber = null;
                merchantNumberError = null;
                terminalNumberError = null;
            }
            else
            {
                var operatorId = Guid.Parse(selectedOperatorId);
                OperatorQueries.GetOperatorQuery query = new OperatorQueries.GetOperatorQuery(CorrelationIdHelper.New(), await this.GetEstateId(), operatorId);
                var getOperatorResult = await this.Mediator.Send(query);
                if (getOperatorResult.IsFailed)
                    NavigationManager.NavigateToErrorPage();

                selectedOperator = ModelFactory.ConvertFrom(getOperatorResult.Data);
                merchantNumber = null;
                terminalNumber = null;
                merchantNumberError = null;
                terminalNumberError = null;
            }
        }

        private bool ValidateOperatorFields()
        {
            bool isValid = true;
            merchantNumberError = null;
            terminalNumberError = null;

            if (selectedOperator != null)
            {
                if (selectedOperator.RequireCustomMerchantNumber)
                {
                    if (string.IsNullOrWhiteSpace(merchantNumber))
                    {
                        merchantNumberError = "Merchant number is required";
                        isValid = false;
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(merchantNumber, @"^\d+$"))
                    {
                        merchantNumberError = "Merchant number must be numeric";
                        isValid = false;
                    }
                }

                if (selectedOperator.RequireCustomTerminalNumber)
                {
                    if (string.IsNullOrWhiteSpace(terminalNumber))
                    {
                        terminalNumberError = "Terminal number is required";
                        isValid = false;
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(terminalNumber, @"^\d+$"))
                    {
                        terminalNumberError = "Terminal number must be numeric";
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private async Task AddOperatorToMerchant()
        {
            if (string.IsNullOrEmpty(selectedOperatorId)) return;

            ClearMessages();

            // Validate fields
            if (!ValidateOperatorFields())
            {
                return;
            }

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();
                var operatorId = Guid.Parse(selectedOperatorId);

                var command = new MerchantCommands.AddOperatorToMerchantCommand(
                    correlationId,
                    estateId,
                    MerchantId,
                    operatorId,
                    merchantNumber,
                    terminalNumber
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Operator added successfully";
                    selectedOperatorId = null;
                    selectedOperator = null;
                    merchantNumber = null;
                    terminalNumber = null;
                    merchantNumberError = null;
                    terminalNumberError = null;
                    showAddOperator = false;

                    // Add to assigned list (in real implementation, reload from server)
                    var op = availableOperators?.FirstOrDefault(o => o.OperatorId == operatorId);
                    if (op != null && !assignedOperators.Any(a => a.OperatorId == operatorId))
                    {
                        assignedOperators.Add(new MerchantOperatorModel() {
                            OperatorId = op.OperatorId,
                            OperatorName = op.OperatorName,
                        });
                    }
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to add operator";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task RemoveOperatorFromMerchant(Guid operatorId)
        {
            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var command = new MerchantCommands.RemoveOperatorFromMerchantCommand(
                    correlationId,
                    estateId,
                    MerchantId,
                    operatorId
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Operator removed successfully";
                    assignedOperators.RemoveAll(o => o.OperatorId == operatorId);
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to remove operator";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task AssignContractToMerchant()
        {
            if (string.IsNullOrEmpty(selectedContractId)) return;

            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();
                var contractId = Guid.Parse(selectedContractId);

                var command = new MerchantCommands.AssignContractToMerchantCommand(
                    correlationId,
                    estateId,
                    MerchantId,
                    contractId
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Contract assigned successfully";
                    selectedContractId = null;
                    showAddContract = false;

                    // Add to assigned list (in real implementation, reload from server)
                    var contract = availableContracts?.FirstOrDefault(c => c.ContractId == contractId);
                    if (contract != null && !assignedContracts.Any(a => a.ContractId == contractId))
                    {
                        assignedContracts.Add(new MerchantContractModel() {
                            ContractName = contract.Description,
                            ContractId = contract.ContractId
                            // TODO: products
                        });
                    }
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to assign contract";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task RemoveContractFromMerchant(Guid contractId)
        {
            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var command = new MerchantCommands.RemoveContractFromMerchantCommand(
                    correlationId,
                    estateId,
                    MerchantId,
                    contractId
                );

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    successMessage = "Contract removed successfully";
                    assignedContracts.RemoveAll(c => c.ContractId == contractId);
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to remove contract";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task AddDeviceToMerchant()
        {
            if (string.IsNullOrEmpty(deviceIdentifier)) return;

            ClearMessages();

            try
            {
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                IRequest<Result> command = new MerchantCommands.AddMerchantDeviceCommand(correlationId, estateId, MerchantId, deviceIdentifier);
                var result = await Mediator.Send(command);

                if (result.IsSuccess) {
                    successMessage = "Device added successfully";
                    assignedDevices.Add(new MerchantDeviceModel() {
                        DeviceIdentifier = this.deviceIdentifier
                    });
                    deviceIdentifier = null;
                    showAddDevice = false;
                }
                else
                {
                    errorMessage = result.Message ?? "Failed to add device";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

    // inside partial class Edit (add the following fields and methods)

        // Swap device UI state
        private string? selectedDeviceToSwap;
    private string? swapDeviceIdentifier;
    private string? swapDeviceError;

    private void StartSwapDevice(string device)
    {
        ClearMessages();
        selectedDeviceToSwap = device;
        swapDeviceIdentifier = string.Empty;
        swapDeviceError = null;
    }

    private void CancelSwapDevice()
    {
        swapDeviceIdentifier = null;
        swapDeviceError = null;
        selectedDeviceToSwap = null;
    }

    private async Task SwapDeviceConfirm(string originalDevice)
    {
        ClearMessages();

        var newId = swapDeviceIdentifier?.Trim();

        // Validation
        if (string.IsNullOrWhiteSpace(newId)) {
            swapDeviceError = "New device identifier is required.";
            return;
        }

        // Case-insensitive comparison for equality/duplicates
        if (string.Equals(originalDevice?.Trim(), newId, StringComparison.OrdinalIgnoreCase)) {
            swapDeviceError = "New device identifier cannot be the same as the current device.";
            return;
        }

        if (assignedDevices.Any(d => string.Equals(d.DeviceIdentifier.Trim(), newId, StringComparison.OrdinalIgnoreCase))) {
            swapDeviceError = "The specified device identifier is already assigned.";
            return;
        }

        var correlationId = new CorrelationId(Guid.NewGuid());
        var estateId = await this.GetEstateId();
        var command = new MerchantCommands.SwapMerchantDeviceCommand(correlationId, estateId, this.MerchantId, this.assignedDevices.Single().DeviceIdentifier, newId);

        var result = await Mediator.Send(command);

        if (result.IsSuccess) {
                successMessage = $"Device {originalDevice} swapped for {newId}.";
            }
            else {
                swapDeviceError = "Original device not found.";
            }

            // Reset swap UI
            CancelSwapDevice();
            StateHasChanged();
    }


    private void ClearMessages()
        {
            errorMessage = null;
            successMessage = null;
        }

        private void BackToList()
        {
            NavigationManager.NavigateTo("/merchants");
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
    }
}
