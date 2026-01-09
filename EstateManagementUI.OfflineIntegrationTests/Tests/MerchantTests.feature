@merchant @offline
Feature: Merchant Management

  Background: 
    When I navigate to the home page
    Then I should see the application dashboard

  Scenario: View Merchants List
    When I navigate to the Merchants page
    Then I should see the merchants table
    And the merchant list table should be visible

  Scenario: View Single Merchant Details
    When I navigate to the Merchants page
    And I click the view button for the first merchant
    Then I should see the merchant details page
    And the merchant information should be displayed

  Scenario: Edit Single Merchant
    When I navigate to the Merchants page
    And I click the edit button for the first merchant in the table
    Then I should see the merchant edit page
    When I update the merchant name field
    And I click the save changes button
    Then the merchant should be updated successfully

  Scenario: Make Merchant Deposit
    When I navigate to the Merchants page
    And I click the make deposit button for the first merchant in the table
    Then I should see the deposit page
    When I enter deposit amount "100"
    And I enter deposit date "2026-01-09"
    And I enter deposit reference "TEST-DEP-001"
    And I click the submit deposit button
    Then the deposit should be processed successfully

  Scenario: Create New Merchant Record
    When I navigate to the Merchants page
    And I click the new merchant button
    Then I should see the create merchant page
    When I enter merchant name "Test Merchant"
    And I select settlement schedule "Immediate"
    And I enter address line 1 "123 Test Street"
    And I enter town "Test Town"
    And I enter region "Test Region"
    And I enter postcode "TS1 1TS"
    And I enter country "United Kingdom"
    And I enter contact name "John Smith"
    And I enter email address "john@testmerchant.com"
    And I enter phone number "01234567890"
    And I click the submit create merchant button
    Then the merchant should be created successfully
