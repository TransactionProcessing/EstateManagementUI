@estate @offline
Feature: Estate Management

  Scenario: View Application Dashboard
    When I navigate to the home page
    Then I should see the application dashboard
    And the dashboard should show key metrics

  Scenario: View Estate Information  
    When I navigate to the Estate details page
    Then I should see the estate information
    And the estate name should be displayed
