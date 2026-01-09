@contract @offline
Feature: Contract Management

  Background: 
    When I navigate to the home page
    Then I should see the application dashboard

  Scenario: View Contracts List
    When I navigate to the Contracts page
    Then I should see a list of contracts
    And the contracts grid should be displayed

  Scenario: View Contract Details
    When I navigate to the Contracts page
    And I click the first contract view button
    Then the contract information should be displayed

  Scenario: Create New Contract
    When I navigate to the Contracts page
    And I click the create new contract button
    And I fill in the contract details form
    And I click the create contract submit button
    Then I should see a list of contracts

  Scenario: Add Product to Contract
    When I navigate to the Contracts page
    And I click the first contract view button
    And I click the add product button
    And I fill in the product details form
    And I click the add product submit button
    Then the contract information should be displayed

  Scenario: Add Transaction Fee to Product
    When I navigate to the Contracts page
    And I click the first contract view button
    And I click the add product button
    And I fill in the product details form
    And I click the add product submit button
    And I click the add fee button for the first product
    And I fill in the transaction fee details form
    And I click the add fee submit button
    Then the contract information should be displayed
