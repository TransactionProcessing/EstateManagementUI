@operator @offline
Feature: Operator Management

  Scenario: View Operators List
    When I navigate to the Operators page
    Then I should see a list of operators
    And the operators table should be displayed

  Scenario: View Operator Details
    When I navigate to the Operators page
    And I click on an operator in the list
    Then I should see the operator details page
    And the operator information should be displayed

  Scenario: Create New Operator
    When I navigate to the Operators page
    And I click the Create Operator button
    And I fill in the operator details
      | Field                        | Value         |
      | Name                         | Test Operator |
      | Require Custom Merchant Number | Yes         |
      | Require Custom Terminal Number | No          |
    And I click the Save button
    Then the operator should be created successfully
    And I should see the new operator in the list

  Scenario: Edit Operator Details
    When I navigate to the Operators page
    And I click on an operator in the list
    And I click the Edit button
    And I update the operator configuration
    And I click the Save button
    Then the operator details should be updated
