using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Results;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using static FastExpressionCompiler.ExpressionCompiler;
using Result = SimpleResults.Result;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants;
    public partial class Edit {
        [Parameter] public Guid MerchantId { get; set; }

        private MerchantModels.MerchantModel? merchant;
        private bool isLoading = true;
        private bool isSaving = false;

        // Unified model for editing
        private MerchantModels.MerchantEditModel merchantEditModel = new();

        // Operators
        private List<OperatorModels.OperatorDropDownModel>? availableOperators;
        private List<MerchantModels.MerchantOperatorModel> assignedOperators = new();
        private bool showAddOperator = false;
        private string? selectedOperatorId;
        private OperatorModels.OperatorModel? selectedOperator;
        private string? merchantNumber;
        private string? terminalNumber;
        private string? merchantNumberError;
        private string? terminalNumberError;

        // Contracts
        private List<ContractModels.ContractDropDownModel>? availableContracts;
        private List<MerchantModels.MerchantContractModel> assignedContracts = new();
        private bool showAddContract = false;
        private string? selectedContractId;

        // Devices
        private List<MerchantModels.MerchantDeviceModel> assignedDevices = new();
        private bool showAddDevice = false;
        private string? deviceIdentifier;

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            this.SetActiveTab("details");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (!firstRender) {
                return;
            }

            Result result = await OnAfterRender(PermissionSection.Merchant, PermissionFunction.Edit, this.LoadMerchant);
            if (result.IsFailed) {
                return;
            }

            isLoading = false;
            this.StateHasChanged();
        }

        private async Task<Result> LoadMerchant() {
            try {
                isLoading = true;
                var correlationId = new CorrelationId(Guid.NewGuid());
                var estateId = await this.GetEstateId();

                var getMerchantResult = await this.MerchantUiService.GetMerchant(correlationId, estateId, MerchantId);

                if (getMerchantResult.IsFailed)
                    return ResultHelpers.CreateFailure(getMerchantResult);

                merchant = getMerchantResult.Data;

                // Initialize unified model with current values
                merchantEditModel = new MerchantModels.MerchantEditModel {
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

                var operatorsResultTask = this.MerchantUiService.GetMerchantOperators(correlationId, estateId, MerchantId);
                var contractsResultTask = this.MerchantUiService.GetMerchantContracts(correlationId, estateId, MerchantId);
                var devicesResultTask = this.MerchantUiService.GetMerchantDevices(correlationId, estateId, MerchantId);

                await Task.WhenAll(operatorsResultTask, contractsResultTask, devicesResultTask);

                if (operatorsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(operatorsResultTask.Result);

                if (contractsResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(contractsResultTask.Result);

                if (devicesResultTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(devicesResultTask.Result);

                assignedOperators = operatorsResultTask.Result.Data;
                assignedContracts = contractsResultTask.Result.Data;
                this.assignedDevices = devicesResultTask.Result.Data;

                Task<Result<List<OperatorModels.OperatorDropDownModel>>> operatorsDropDownTask = this.OperatorUiService.GetOperatorsForDropDown(correlationId, estateId);
                Task<Result<List<ContractModels.ContractDropDownModel>>> contractsDropDownTask = this.ContractUiService.GetContractsForDropDown(correlationId, estateId);

                await Task.WhenAll(operatorsDropDownTask, contractsDropDownTask);

                if (operatorsDropDownTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(operatorsResultTask.Result);

                if (contractsDropDownTask.Result.IsFailed)
                    return ResultHelpers.CreateFailure(contractsResultTask.Result);

                var unfilteredOperator = operatorsDropDownTask.Result.Data;
                this.availableOperators = unfilteredOperator.Where(u => this.assignedOperators.Select(a => a.OperatorId).Contains(u.OperatorId) == false).ToList();

                var unfilteredContracts = contractsDropDownTask.Result.Data;
                this.availableContracts = unfilteredContracts.Where(u => this.assignedContracts.Select(a => a.ContractId).Contains(u.ContractId) == false).ToList();

                return Result.Success();
            }
            finally {
                isLoading = false;
            }
        }

        private string GetTabClass(string tab) {
            var baseClass = "px-6 py-3 text-sm font-medium border-b-2 transition-colors ";
            return tab == activeTab ? baseClass + "border-blue-600 text-blue-600" : baseClass + "border-transparent text-gray-600 hover:text-gray-800 hover:border-gray-300";
        }

        private void HandleInvalidSubmit(EditContext editContext) {
            // Aggregate validation messages so the user sees why the submit didn't run
            var errors = editContext.GetValidationMessages().ToList();
            if (errors.Any()) {
                errorMessage = string.Join(" \u2014 ", errors); // em dash separator
            }
            else {
                errorMessage = "Form is invalid. Please check the highlighted fields.";
            }

            // Ensure UI updates
            StateHasChanged();
        }

        private async Task SaveAllChanges() {
            isSaving = true;
            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.MerchantUiService.UpdateMerchant(correlationId, estateId, this.MerchantId, this.merchantEditModel);

            if (result.IsSuccess) {
                successMessage = "Merchant details updated successfully";

                StateHasChanged();

                // Small delay so user sees confirmation (adjust duration as needed)
                await this.WaitOnUIRefresh();

                // Navigate to contracts list with success
                NavigationManager.NavigateTo("/merchants");
            }
            else {
                this.errorMessage = "Failed to update merchant";
            }

            isSaving = false;
        }

        private async Task OnOperatorSelected() {
            if (string.IsNullOrEmpty(selectedOperatorId)) {
                selectedOperator = null;
                merchantNumber = null;
                terminalNumber = null;
                merchantNumberError = null;
                terminalNumberError = null;
            }
            else {
                var operatorId = Guid.Parse(selectedOperatorId);
                var result = await this.OperatorUiService.GetOperator(CorrelationIdHelper.New(), await this.GetEstateId(), operatorId);
                if (result.IsFailed)
                    NavigationManager.NavigateToErrorPage();

                selectedOperator = result.Data;
                merchantNumber = null;
                terminalNumber = null;
                merchantNumberError = null;
                terminalNumberError = null;
            }
        }

        private bool ValidateOperatorFields() {
            bool isValid = true;
            merchantNumberError = null;
            terminalNumberError = null;

            if (selectedOperator != null) {
                if (selectedOperator.RequireCustomMerchantNumber) {
                    if (string.IsNullOrWhiteSpace(merchantNumber)) {
                        merchantNumberError = "Merchant number is required";
                        isValid = false;
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(merchantNumber, @"^\d+$")) {
                        merchantNumberError = "Merchant number must be numeric";
                        isValid = false;
                    }
                }

                if (selectedOperator.RequireCustomTerminalNumber) {
                    if (string.IsNullOrWhiteSpace(terminalNumber)) {
                        terminalNumberError = "Terminal number is required";
                        isValid = false;
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(terminalNumber, @"^\d+$")) {
                        terminalNumberError = "Terminal number must be numeric";
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private async Task AddOperatorToMerchant() {
            if (string.IsNullOrEmpty(selectedOperatorId)) return;

            ClearMessages();

            // Validate fields
            if (!ValidateOperatorFields()) {
                return;
            }

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();
            var operatorId = Guid.Parse(selectedOperatorId);

            var result = await this.MerchantUiService.AddOperatorToMerchant(correlationId, estateId, MerchantId, operatorId, merchantNumber, terminalNumber);

            if (result.IsSuccess) {
                successMessage = "Operator added successfully";
                selectedOperatorId = null;
                selectedOperator = null;
                merchantNumber = null;
                terminalNumber = null;
                merchantNumberError = null;
                terminalNumberError = null;
                showAddOperator = false;

                //// Add to assigned list (in real implementation, reload from server)
                //var op = availableOperators?.FirstOrDefault(o => o.OperatorId == operatorId);
                //if (op != null && !assignedOperators.Any(a => a.OperatorId == operatorId))
                //{
                //    assignedOperators.Add(new MerchantModels.MerchantOperatorModel() {
                //        OperatorId = op.OperatorId,
                //        OperatorName = op.OperatorName,
                //    });
                //}
                await this.LoadMerchant();
            }
            else {
                errorMessage = "Failed to add operator";
            }
        }

        private async Task RemoveOperatorFromMerchant(Guid operatorId) {
            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.MerchantUiService.RemoveOperatorFromMerchant(correlationId, estateId, MerchantId, operatorId);

            if (result.IsSuccess) {
                successMessage = "Operator removed successfully";
                await this.LoadMerchant();
            }
            else {
                errorMessage = "Failed to remove operator";
            }
        }

        private async Task AssignContractToMerchant() {
            if (string.IsNullOrEmpty(selectedContractId)) return;

            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();
            var contractId = Guid.Parse(selectedContractId);

            var result = await this.MerchantUiService.AssignContractToMerchant(correlationId, estateId, MerchantId, contractId);

            if (result.IsSuccess) {
                successMessage = "Contract assigned successfully";
                selectedContractId = null;
                showAddContract = false;

                // Add to assigned list (in real implementation, reload from server)
                await this.LoadMerchant();
            }
            else {
                errorMessage = "Failed to assign contract";
            }
        }

        private async Task RemoveContractFromMerchant(Guid contractId) {
            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.MerchantUiService.RemoveContractFromMerchant(correlationId, estateId, MerchantId, contractId);


            if (result.IsSuccess) {
                successMessage = "Contract removed successfully";
                await this.LoadMerchant();
            }
            else {
                errorMessage = "Failed to remove contract";
            }
        }

        private async Task AddDeviceToMerchant() {
            if (string.IsNullOrEmpty(deviceIdentifier)) return;

            ClearMessages();

            var correlationId = new CorrelationId(Guid.NewGuid());
            var estateId = await this.GetEstateId();

            var result = await this.MerchantUiService.AddMerchantDevice(correlationId, estateId, MerchantId, deviceIdentifier);

            if (result.IsSuccess) {
                successMessage = "Device added successfully";

                await this.LoadMerchant();

                deviceIdentifier = null;
                showAddDevice = false;
            }
            else {
                errorMessage = "Failed to add device";
            }
        }

        // inside partial class Edit (add the following fields and methods)

        // Swap device UI state
        private string? selectedDeviceToSwap;
        private string? swapDeviceIdentifier;
        private string? swapDeviceError;

        private void StartSwapDevice(string device) {
            ClearMessages();
            selectedDeviceToSwap = device;
            swapDeviceIdentifier = string.Empty;
            swapDeviceError = null;
        }

        private void CancelSwapDevice() {
            swapDeviceIdentifier = null;
            swapDeviceError = null;
            selectedDeviceToSwap = null;
        }

        private async Task SwapDeviceConfirm(string originalDevice) {
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

            var result = await this.MerchantUiService.SwapMerchantDevice(correlationId, estateId, this.MerchantId, this.assignedDevices.Single().DeviceIdentifier, newId);

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

        private void BackToList() {
            NavigationManager.NavigateTo("/merchants");
        }
    }
