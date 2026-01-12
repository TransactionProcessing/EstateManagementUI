Feature: Estate Management Integration Tests
    As a user of the Estate Management UI
    I want to access Estate Management functionality based on my role
    So that I can view and manage estate information according to my permissions

Background:
    Given the user navigates to Estate Management

@EstateManagementTests @AdminRole
Scenario: Administrator user cannot access Estate Management
    Given the user is authenticated as an "Administrator" user
    When the user navigates to Estate Management
    Then the Estate Management menu is not visible

@EstateManagementTests @EstateRole
Scenario: Estate user can view estate overview
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the page title is "Estate Management"
    And the estate name is "Test Estate"
    And the estate reference is "Test Estate"

@EstateManagementTests @EstateRole
Scenario: Estate user can view estate statistics
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Total Merchants count is "3"
    And the Total Operators count is "2"
    And the Total Contracts count is "2"
    And the Total Users count is "5"

@EstateManagementTests @EstateRole
Scenario: Estate user can view recent merchants section
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Recent Merchants section is displayed
    And at least "1" merchant is shown in Recent Merchants
    And the merchant "Test Merchant 1" with reference "MERCH001" is visible

@EstateManagementTests @EstateRole
Scenario: Estate user can view contracts section
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Contracts section is displayed
    And at least "1" contract is shown
    And the contract "Standard Transaction Contract" for operator "Safaricom" is visible

@EstateManagementTests @EstateRole
Scenario: Estate user can switch to operators tab
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the Assigned Operators section is visible
    And at least "2" operators are assigned

@EstateManagementTests @EstateRole
Scenario: Estate user can view assigned operators
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the operator "Safaricom" is listed
    And the operator "Voucher" is listed

@EstateManagementTests @EstateRole
Scenario: Estate user can see Add Operator button
    Given the user is authenticated as an "Estate" user
    When the user navigates to Estate Management
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the Add Operator button is visible

@EstateManagementTests @ViewerRole
Scenario: Viewer user can view estate overview
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the page title is "Estate Management"
    And the estate name is "Test Estate"
    And the estate reference is "Test Estate"

@EstateManagementTests @ViewerRole
Scenario: Viewer user can view estate statistics
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Total Merchants count is "3"
    And the Total Operators count is "2"
    And the Total Contracts count is "2"
    And the Total Users count is "5"

@EstateManagementTests @ViewerRole
Scenario: Viewer user can view recent merchants section
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Recent Merchants section is displayed
    And at least "1" merchant is shown in Recent Merchants

@EstateManagementTests @ViewerRole
Scenario: Viewer user can view contracts section
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    Then the Estate Management page is displayed
    And the Contracts section is displayed
    And at least "1" contract is shown

@EstateManagementTests @ViewerRole
Scenario: Viewer user can switch to operators tab
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the Assigned Operators section is visible

@EstateManagementTests @ViewerRole
Scenario: Viewer user cannot see Add Operator button
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Estate Management
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the Add Operator button is not visible
