Feature: Operator Management Integration Tests
    As a user of the Estate Management UI
    I want to access Operator Management functionality based on my role
    So that I can view and manage operator information according to my permissions

@OperatorManagementTests @AdminRole
Scenario: Administrator user cannot access Operator Management
    Given the user is authenticated as an "Administrator" user
    When the user navigates to Operator Management
    Then the Operator Management menu is not visible

@OperatorManagementTests @EstateRole
Scenario: Estate user can view operator list
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the page title is "Operator Management"
    And the operator list contains "2" operators

@OperatorManagementTests @EstateRole
Scenario: Estate user can view operator list with correct data
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the operator "Safaricom" is listed
    And the operator "Voucher" is listed

@OperatorManagementTests @EstateRole
Scenario: Estate user can view operator custom merchant number requirements
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the operator "Safaricom" shows custom merchant number as "Required"
    And the operator "Voucher" shows custom merchant number as "Not Required"

@OperatorManagementTests @EstateRole
Scenario: Estate user can view operator custom terminal number requirements
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the operator "Safaricom" shows custom terminal number as "Not Required"
    And the operator "Voucher" shows custom terminal number as "Not Required"

@OperatorManagementTests @EstateRole
Scenario: Estate user can see Add New Operator button
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the Add New Operator button is visible

@OperatorManagementTests @EstateRole
Scenario: Estate user can view a specific operator
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the operator name is "Safaricom"

@OperatorManagementTests @EstateRole
Scenario: Estate user can view operator requirement details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the operator custom merchant number requirement is "Required"
    And the operator custom terminal number requirement is "Not Required"

@OperatorManagementTests @EstateRole
Scenario: Estate user can see Edit Operator button on view page
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the Edit Operator button is visible

@OperatorManagementTests @EstateRole
Scenario: Estate user can navigate to Create New Operator page
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks the Add New Operator button
    Then the Create New Operator page is displayed
    And the operator name field is visible
    And the custom merchant number checkbox is visible
    And the custom terminal number checkbox is visible

@OperatorManagementTests @EstateRole
Scenario: Estate user can navigate to Edit Operator page from list
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks edit for operator "Safaricom"
    Then the Edit Operator page is displayed
    And the operator name field contains "Safaricom"

@OperatorManagementTests @EstateRole
Scenario: Estate user can navigate to Edit Operator page from view
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    And the user clicks the Edit Operator button
    Then the Edit Operator page is displayed
    And the operator name field contains "Safaricom"

@OperatorManagementTests @ViewerRole
Scenario: Viewer user can view operator list
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the page title is "Operator Management"
    And the operator list contains "2" operators

@OperatorManagementTests @ViewerRole
Scenario: Viewer user can view operator list with correct data
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the operator "Safaricom" is listed
    And the operator "Voucher" is listed

@OperatorManagementTests @ViewerRole
Scenario: Viewer user cannot see Add New Operator button
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the Add New Operator button is not visible

@OperatorManagementTests @ViewerRole
Scenario: Viewer user can view a specific operator
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the operator name is "Safaricom"

@OperatorManagementTests @ViewerRole
Scenario: Viewer user can view operator requirement details
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the operator custom merchant number requirement is "Required"
    And the operator custom terminal number requirement is "Not Required"

@OperatorManagementTests @ViewerRole
Scenario: Viewer user cannot see Edit button on list
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    Then the Operator Management page is displayed
    And the Edit button is not visible for operator "Safaricom"

@OperatorManagementTests @ViewerRole
Scenario: Viewer user can see Edit Operator button on view page but navigating to edit is blocked
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Operator Management
    And the user clicks on operator "Safaricom"
    Then the View Operator page is displayed
    And the Edit Operator button is visible

@OperatorManagementTests @EstateRole @EditOperations
Scenario: Estate user can update operator details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks edit for operator "Safaricom"
    Then the Edit Operator page is displayed
    When the user updates the operator name to "Updated Operator Name"
    And the user updates the custom merchant number requirement to checked
    And the user updates the custom terminal number requirement to checked
    And the user clicks the Save Changes button
    Then a success message is displayed
    And the success message contains "Operator updated successfully"

@OperatorManagementTests @EstateRole @CreateOperations
Scenario: Estate user can create a new operator
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks the Add New Operator button
    Then the Create New Operator page is displayed
    When the user enters "New Test Operator" as the operator name
    And the user updates the custom merchant number requirement to checked
    And the user clicks the Create Operator button
    Then a success message is displayed or user is redirected to operator list
    And the operator "New Test Operator" is listed

@OperatorManagementTests @EstateRole @CreateOperations
Scenario: Estate user cannot create operator with empty name
    Given the user is authenticated as an "Estate" user
    When the user navigates to Operator Management
    And the user clicks the Add New Operator button
    Then the Create New Operator page is displayed
    When the user clicks the Create Operator button
    Then a validation error is displayed for operator name

@OperatorManagementTests @ViewerRole @EditOperations
Scenario: Viewer user cannot access Edit Operator page directly
    Given the user is authenticated as a "Viewer" user
    When the user navigates directly to Edit Operator page for "Safaricom"
    Then an access denied message is displayed
    And the message indicates no permission to edit operators
