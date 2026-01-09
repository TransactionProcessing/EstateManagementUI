@estate @offline
Feature: Estate Management

  Scenario: View Estate Information
    When I navigate to the Estate page
    Then I should see the estate details
    And the estate name should be displayed

  Scenario: View Estate Dashboard
    When I navigate to the home page
    Then I should see the estate dashboard
    And the dashboard should show estate statistics
