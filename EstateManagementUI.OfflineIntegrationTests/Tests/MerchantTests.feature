@merchant @offline
Feature: Merchant Management

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
    And I click the Create Merchant button
    And I fill in the merchant details
      | Field             | Value                |
      | Name              | Test Merchant        |
      | Contact Name      | John Smith           |
      | Contact Email     | john@testmerchant.com|
      | Address Line 1    | 123 Test Street      |
      | Town              | Test Town            |
      | Postcode          | TS1 1TS              |
    And I click the Save button
    Then the merchant should be created successfully
    And I should see the new merchant in the list

  Scenario: Edit Merchant Details
    When I navigate to the Merchants page
    And I click on a merchant in the list
    And I click the Edit button
    And I update the merchant name to "Updated Merchant Name"
    And I click the Save button
    Then the merchant details should be updated
    And the updated name should be displayed

  Scenario: Make Merchant Deposit
    When I navigate to the Merchants page
    And I click on a merchant in the list
    And I click the Make Deposit button
    And I enter the deposit amount "100.00"
    And I click the Confirm Deposit button
    Then the deposit should be processed
    And the merchant balance should be updated
