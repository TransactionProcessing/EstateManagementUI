@permissions @offline
Feature: Permissions Management

  Scenario: View Permissions List
    When I navigate to the Permissions page
    Then I should see a list of permissions
    And the permissions table should be displayed

  Scenario: View Permission Details
    When I navigate to the Permissions page
    And I click on a permission in the list
    Then I should see the permission details page
    And the permission information should be displayed

  Scenario: Edit Permission
    When I navigate to the Permissions page
    And I click on a permission in the list
    And I click the Edit button
    And I update the permission details
      | Field       | Value                    |
      | Name        | Updated Permission       |
      | Description | Updated description text |
    And I click the Save button
    Then the permission should be updated successfully
    And the updated details should be displayed

  Scenario: Filter Permissions by Role
    When I navigate to the Permissions page
    And I select "Estate" from the role filter
    And I click the Filter button
    Then only permissions for the Estate role should be displayed

  Scenario: Search Permissions by Name
    When I navigate to the Permissions page
    And I enter "Merchant" in the search box
    And I click the Search button
    Then only permissions matching "Merchant" should be displayed
