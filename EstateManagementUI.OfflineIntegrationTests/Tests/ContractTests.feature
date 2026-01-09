@contract @offline
Feature: Contract Management

  Background: 
    When I navigate to the home page
    Then I should see the application dashboard

  Scenario: View Contracts List
    When I navigate to the Contracts page
    Then I should see a list of contracts
    And the contracts table should be displayed

  Scenario: View Contract Details
    When I navigate to the Contracts page
    And I click on a contract in the list
    Then I should see the contract details page
    And the contract information should be displayed

  Scenario: Create New Contract
    When I navigate to the Contracts page
    And I click the Create Contract button
    And I fill in the contract details
      | Field       | Value                |
      | Description | Test Contract        |
      | Operator    | Test Operator        |
    And I click the Save button
    Then the contract should be created successfully
    And I should see the new contract in the list

  Scenario: Add Product to Contract
    When I navigate to the Contracts page
    And I click on a contract in the list
    And I click the Add Product button
    And I fill in the product details
      | Field       | Value          |
      | Name        | Mobile Topup   |
      | Display Text| 100 KES        |
      | Value       | 100.00         |
    And I click the Save Product button
    Then the product should be added to the contract
    And I should see the product in the contract

  Scenario: Add Transaction Fee to Product
    When I navigate to the Contracts page
    And I click on a contract in the list
    And I click on a product in the contract
    And I click the Add Transaction Fee button
    And I fill in the transaction fee details
      | Field          | Value            |
      | Description    | Service Fee      |
      | Calculation Type| Fixed           |
      | Fee Value      | 5.00             |
    And I click the Save Fee button
    Then the transaction fee should be added to the product
