@merchant @offline
Feature: Merchant Management

  Background: 
    When I navigate to the home page
    Then I should see the application dashboard

  Scenario: View Merchants List
    When I navigate to the Merchants page
    Then I should see a list of merchants
    And the merchants table should be displayed

  Scenario: View Merchant Details
    When I navigate to the Merchants page
    And I click on a merchant in the list
    Then I should see the merchant details page
    And the merchant information should be displayed

  Scenario: Create New Merchant
    When I navigate to the Merchants page
    And I click the Add New Merchant button
    And I fill in the merchant details
      | Field             | Value                   |
      | MerchantName      | Test Merchant           |
      | ContactName       | John Smith              |
      | EmailAddress      | john@testmerchant.com   |
      | PhoneNumber       | 01234567890             |
      | AddressLine1      | 123 Test Street         |
      | Town              | Test Town               |
      | Region            | Test Region             |
      | PostCode          | TS1 1TS                 |
      | Country           | United Kingdom          |
      | SettlementSchedule| Immediate               |
    And I click the Create Merchant button
    Then the merchant should be created successfully

  Scenario: Edit Merchant Details
    When I navigate to the Merchants page
    And I click the edit button for a merchant
    And I update the merchant name to "Updated Merchant Name"
    And I click the Save button
    Then the merchant details should be updated

  Scenario: Make Merchant Deposit
    When I navigate to the Merchants page
    And I click the make deposit button for a merchant
    And I enter the deposit amount "100"
    And I enter the deposit reference "TEST-DEP-001"
    And I click the Make Deposit button
    Then the deposit should be processed
