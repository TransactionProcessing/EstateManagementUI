Feature: Contract Management Integration Tests
    As a user of the Estate Management UI
    I want to manage contracts based on my role
    So that I can view, create, and edit contracts according to my permissions

Background:
    Given the user navigates to the Contract Management page

@ContractManagement @AdminRole
Scenario: Administrator user cannot see Contract Management menu
    Given the user is authenticated as an "Administrator" user
    When the user navigates to the home page
    Then the Contract Management menu item is not visible

@ContractManagement @AdminRole
Scenario: Administrator user cannot access Contract Management page directly
    Given the user is authenticated as an "Administrator" user
    When the user navigates to the Contract Management page
    Then an access denied message is displayed

@ContractManagement @EstateRole
Scenario: Estate user can see Contract Management menu
    Given the user is authenticated as an "Estate" user
    When the user navigates to the home page
    Then the Contract Management menu item is visible

@ContractManagement @EstateRole
Scenario: Estate user can view contracts list
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    Then the Contract Management page is displayed
    And the page title is "Contract Management"
    And "2" contracts are displayed in the list
    And the "Add New Contract" button is visible

@ContractManagement @EstateRole
Scenario: Estate user can view contract details
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    And the user clicks on the "View" button for the first contract
    Then the View Contract page is displayed
    And the contract description is "Standard Transaction Contract"
    And the contract operator is "Safaricom"
    And the contract has "2" products
    And the first product is "Mobile Topup"
    And the first product has "2" transaction fees

@ContractManagement @EstateRole
Scenario: Estate user can view product transaction fee details
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    And the user clicks on the "View" button for the first contract
    Then the View Contract page is displayed
    And the product "Mobile Topup" has a transaction fee "Merchant Commission" with value "0.50"
    And the product "Mobile Topup" has a transaction fee "Service Provider Fee" with value "2.50"

@ContractManagement @EstateRole
Scenario: Estate user can navigate to create new contract page
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    And the user clicks on the "Add New Contract" button
    Then the Create New Contract page is displayed
    And the contract form is displayed
    And the "Description" field is visible
    And the "Operator" dropdown is visible

@ContractManagement @EstateRole
Scenario: Estate user can create a new contract
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Create New Contract page
    And the user enters "Test Contract" in the "Description" field
    And the user selects "Safaricom" from the "Operator" dropdown
    And the user clicks on the "Create Contract" button
    Then the contract is created successfully
    And the user is redirected to the Contract Management page

@ContractManagement @EstateRole
Scenario: Estate user sees validation error when creating contract without required fields
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Create New Contract page
    And the user clicks on the "Create Contract" button
    Then validation errors are displayed
    And the "Description is required" error is shown
    And the "Operator is required" error is shown

@ContractManagement @EstateRole
Scenario: Estate user can navigate to edit contract page
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    And the user clicks on the "Edit" button for the first contract
    Then the Edit Contract page is displayed
    And the contract description field contains "Standard Transaction Contract"
    And the operator name is displayed as "Safaricom"

@ContractManagement @EstateRole
Scenario: Estate user can view products on edit page
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Contract Management page
    And the user clicks on the "Edit" button for the first contract
    Then the Edit Contract page is displayed
    And the products section is displayed
    And "2" products are listed
    And the "Add Product" button is visible

@ContractManagement @EstateRole
Scenario: Estate user can add a product to contract
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Edit Contract page for the first contract
    And the user clicks on the "Add Product" button
    Then the Add Product modal is displayed
    When the user enters "New Product" in the product name field
    And the user enters "New Product Display" in the display text field
    And the user enters "100" in the value field
    And the user clicks on the "Add Product" button in the modal
    Then the product is added successfully
    And the Add Product modal is closed

@ContractManagement @EstateRole
Scenario: Estate user can add a variable value product to contract
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Edit Contract page for the first contract
    And the user clicks on the "Add Product" button
    Then the Add Product modal is displayed
    When the user enters "Variable Product" in the product name field
    And the user enters "Variable Display" in the display text field
    And the user checks the "Variable Value" checkbox
    And the user clicks on the "Add Product" button in the modal
    Then the product is added successfully

@ContractManagement @EstateRole
Scenario: Estate user can add a transaction fee to a product
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Edit Contract page for the first contract
    And the user clicks on the "Add Fee" button for the first product
    Then the Add Transaction Fee modal is displayed
    When the user enters "Test Fee" in the fee description field
    And the user selects "Fixed" from the calculation type dropdown
    And the user selects "Merchant" from the fee type dropdown
    And the user enters "5.00" in the fee value field
    And the user clicks on the "Add Fee" button in the modal
    Then the transaction fee is added successfully
    And the Add Transaction Fee modal is closed

@ContractManagement @ViewerRole
Scenario: Viewer user can see Contract Management menu
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the home page
    Then the Contract Management menu item is visible

@ContractManagement @ViewerRole
Scenario: Viewer user can view contracts list
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Contract Management page
    Then the Contract Management page is displayed
    And the page title is "Contract Management"
    And "2" contracts are displayed in the list
    And the "Add New Contract" button is not visible

@ContractManagement @ViewerRole
Scenario: Viewer user can view contract details
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Contract Management page
    And the user clicks on the "View" button for the first contract
    Then the View Contract page is displayed
    And the contract description is "Standard Transaction Contract"
    And the contract operator is "Safaricom"
    And the contract has "2" products

@ContractManagement @ViewerRole
Scenario: Viewer user cannot access create new contract page
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Create New Contract page directly
    Then an access denied message is displayed for contract creation

@ContractManagement @ViewerRole
Scenario: Viewer user cannot see Edit button for contracts
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Contract Management page
    Then the Contract Management page is displayed
    And the "Edit" button is not visible for contracts

@ContractManagement @ViewerRole
Scenario: Viewer user cannot access edit contract page directly
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Edit Contract page directly
    Then an access denied message is displayed for contract editing
