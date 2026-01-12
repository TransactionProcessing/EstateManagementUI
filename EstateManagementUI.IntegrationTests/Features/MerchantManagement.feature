Feature: Merchant Management Integration Tests
    As a user of the Estate Management UI
    I want to access Merchant Management functionality based on my role
    So that I can view and manage merchant information according to my permissions

Background:
    Given the user navigates to Merchant Management

@MerchantManagementTests @AdminRole
Scenario: Administrator user cannot access Merchant Management
    Given the user is authenticated as an "Administrator" user
    When the user navigates to Merchant Management
    Then the Merchant Management menu is not visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant list
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the page title is "Merchant Management"
    And the merchant list contains "3" merchants

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant list with correct data
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the merchant "Test Merchant 1" with reference "MERCH001" is listed
    And the merchant "Test Merchant 2" with reference "MERCH002" is listed
    And the merchant "Test Merchant 3" with reference "MERCH003" is listed

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant balance information
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the merchant "Test Merchant 1" shows balance "£10,000.00"
    And the merchant "Test Merchant 1" shows available balance "£8,500.00"
    And the merchant "Test Merchant 1" shows settlement schedule "Immediate"

@MerchantManagementTests @EstateRole
Scenario: Estate user can see Add New Merchant button
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the Add New Merchant button is visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can view a specific merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    Then the View Merchant page is displayed
    And the merchant name is "Test Merchant 1"
    And the merchant reference is "MERCH001"

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant details tab
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    Then the View Merchant page is displayed
    And the merchant balance is "£10,000.00"
    And the merchant available balance is "£8,500.00"
    And the merchant settlement schedule is "Immediate"

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant address details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "address" tab
    Then the address tab content is displayed
    And the address line 1 is "123 Main Street"
    And the town is "Test Town"
    And the region is "Test Region"
    And the postal code is "12345"
    And the country is "Test Country"

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant contact details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "contact" tab
    Then the contact tab content is displayed
    And the contact name is "John Smith"
    And the contact email is "john@testmerchant.com"
    And the contact phone is "555-1234"

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant assigned operators tab
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "operators" tab
    Then the operators tab content is displayed
    And the Assigned Operators section is visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant assigned contracts tab
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "contracts" tab
    Then the contracts tab content is displayed
    And the Assigned Contracts section is visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can view merchant assigned devices tab
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "devices" tab
    Then the devices tab content is displayed
    And the Assigned Devices section is visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can navigate to Create New Merchant page
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks the Add New Merchant button
    Then the Create New Merchant page is displayed
    And the merchant name field is visible
    And the settlement schedule field is visible
    And the address line 1 field is visible
    And the contact name field is visible

@MerchantManagementTests @EstateRole
Scenario: Estate user can navigate to Edit Merchant page
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    And the merchant name field contains "Test Merchant 1"

@MerchantManagementTests @EstateRole
Scenario: Estate user can see Make Deposit button
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the Make Deposit button is visible for merchant "Test Merchant 1"

@MerchantManagementTests @EstateRole
Scenario: Estate user can navigate to Make Deposit page
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks make deposit for merchant "Test Merchant 1"
    Then the Make Deposit page is displayed
    And the deposit amount field is visible
    And the deposit date field is visible
    And the deposit reference field is visible

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view merchant list
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the page title is "Merchant Management"
    And the merchant list contains "3" merchants

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view merchant list with correct data
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the merchant "Test Merchant 1" with reference "MERCH001" is listed
    And the merchant "Test Merchant 2" with reference "MERCH002" is listed
    And the merchant "Test Merchant 3" with reference "MERCH003" is listed

@MerchantManagementTests @ViewerRole
Scenario: Viewer user cannot see Add New Merchant button
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the Add New Merchant button is not visible

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view a specific merchant
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    Then the View Merchant page is displayed
    And the merchant name is "Test Merchant 1"
    And the merchant reference is "MERCH001"

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view merchant details tab
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    Then the View Merchant page is displayed
    And the merchant balance is "£10,000.00"
    And the merchant available balance is "£8,500.00"

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view merchant address details
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "address" tab
    Then the address tab content is displayed
    And the address line 1 is "123 Main Street"

@MerchantManagementTests @ViewerRole
Scenario: Viewer user can view merchant contact details
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    And the user clicks on merchant "Test Merchant 1"
    And the user clicks on the "contact" tab
    Then the contact tab content is displayed
    And the contact name is "John Smith"

@MerchantManagementTests @ViewerRole
Scenario: Viewer user cannot see Edit button
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the Edit button is not visible for merchant "Test Merchant 1"

@MerchantManagementTests @ViewerRole
Scenario: Viewer user cannot see Make Deposit button
    Given the user is authenticated as a "Viewer" user
    When the user navigates to Merchant Management
    Then the Merchant Management page is displayed
    And the Make Deposit button is not visible for merchant "Test Merchant 1"
