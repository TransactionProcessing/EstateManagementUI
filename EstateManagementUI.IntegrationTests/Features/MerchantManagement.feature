Feature: Merchant Management Integration Tests
    As a user of the Estate Management UI
    I want to access Merchant Management functionality based on my role
    So that I can view and manage merchant information according to my permissions

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

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can update merchant details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user updates the merchant name to "Updated Merchant Name"
    And the user updates the settlement schedule to "Weekly"
    And the user clicks the Save Changes button
    Then a success message is displayed
    And the success message contains "Merchant details updated successfully"

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can update merchant address
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "address" tab
    And the user updates the address line 1 to "456 Updated Street"
    And the user updates the town to "New Town"
    And the user updates the region to "New Region"
    And the user clicks the Save Changes button
    Then a success message is displayed
    And the success message contains "Merchant address updated successfully"

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can update merchant contact details
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "contact" tab
    And the user updates the contact name to "Jane Doe"
    And the user updates the contact email to "jane@updated.com"
    And the user updates the contact phone to "555-9999"
    And the user clicks the Save Changes button
    Then a success message is displayed
    And the success message contains "Merchant contact updated successfully"

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can add an operator to a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "operators" tab
    And the user clicks the Add Operator button
    And the user selects "Safaricom" from the operator dropdown
    And the user enters "12345" as the merchant number
    And the user clicks the Add button in the operator form
    Then a success message is displayed
    And the success message contains "Operator added successfully"
    And the operator "Safaricom" is listed in assigned operators

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can remove an operator from a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "operators" tab
    And the user clicks the Add Operator button
    And the user selects "Voucher" from the operator dropdown
    And the user clicks the Add button in the operator form
    Then a success message is displayed
    When the user removes the operator "Voucher"
    Then a success message is displayed
    And the success message contains "Operator removed successfully"
    And the operator "Voucher" is not listed in assigned operators

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can assign a contract to a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "contracts" tab
    And the user clicks the Assign Contract button
    And the user selects "Standard Transaction Contract" from the contract dropdown
    And the user clicks the Assign button in the contract form
    Then a success message is displayed
    And the success message contains "Contract assigned successfully"
    And the contract "Standard Transaction Contract" is listed in assigned contracts

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can remove a contract from a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "contracts" tab
    And the user clicks the Assign Contract button
    And the user selects "Voucher Sales Contract" from the contract dropdown
    And the user clicks the Assign button in the contract form
    Then a success message is displayed
    When the user removes the contract "Voucher Sales Contract"
    Then a success message is displayed
    And the success message contains "Contract removed successfully"
    And the contract "Voucher Sales Contract" is not listed in assigned contracts

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can add a device to a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "devices" tab
    And the user clicks the Add Device button
    And the user enters "DEVICE123" as the device identifier
    And the user clicks the Add button in the device form
    Then a success message is displayed
    And the success message contains "Device added successfully"
    And the device "DEVICE123" is listed in assigned devices

@MerchantManagementTests @EstateRole @EditOperations
Scenario: Estate user can remove a device from a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks edit for merchant "Test Merchant 1"
    Then the Edit Merchant page is displayed
    When the user clicks on the "devices" tab
    And the user clicks the Add Device button
    And the user enters "DEVICE999" as the device identifier
    And the user clicks the Add button in the device form
    Then a success message is displayed
    When the user removes the device "DEVICE999"
    Then a success message is displayed
    And the success message contains "Device removed successfully"
    And the device "DEVICE999" is not listed in assigned devices

@MerchantManagementTests @EstateRole @DepositOperations
Scenario: Estate user can make a deposit to a merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to Merchant Management
    And the user clicks make deposit for merchant "Test Merchant 1"
    Then the Make Deposit page is displayed
    When the user enters "1000" as the deposit amount
    And the user selects today as the deposit date
    And the user enters "TEST-DEPOSIT-001" as the deposit reference
    And the user clicks the Make Deposit button
    Then a success message is displayed
    And the success message contains "Deposit completed successfully"
