Feature: Dashboard Integration Tests
    As a user of the Estate Management UI
    I want to see appropriate dashboard content based on my role
    So that I can access the information relevant to my permissions

Background:
    Given the user navigates to the Dashboard

@DashboardTests @AdminRole
Scenario: Administrator user sees limited dashboard view
    Given the user is authenticated as an "Administrator" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the Administrator welcome message is displayed
    And no merchant KPI cards are displayed
    And no sales data cards are displayed

@DashboardTests @EstateRole
Scenario: Estate user sees full dashboard with merchant KPIs
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the merchant KPI cards are displayed
    And the Merchants with Sales in Last Hour shows "45"
    And the Merchants with No Sales Today shows "12"
    And the Merchants with No Sales in Last 7 Days shows "5"

@DashboardTests @EstateRole
Scenario: Estate user sees sales data on dashboard
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the Today's Sales card is displayed
    And the Today's Sales card shows "523" transactions
    And the Today's Sales card shows a value greater than $0
    And the Failed Sales card is displayed
    And the Failed Sales card shows "18" transactions

@DashboardTests @EstateRole
Scenario: Estate user sees comparison date selector
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the comparison date selector is displayed

@DashboardTests @EstateRole
Scenario: Estate user sees recently created merchants
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the Recently Created Merchants section is displayed
    And at least "1" merchant is shown in Recently Created Merchants

@DashboardTests @ViewerRole
Scenario: Viewer user sees full dashboard with merchant KPIs
    Given the user is authenticated as an "Viewer" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the merchant KPI cards are displayed
    And the Merchants with Sales in Last Hour shows "45"
    And the Merchants with No Sales Today shows "12"
    And the Merchants with No Sales in Last 7 Days shows "5"

@DashboardTests @ViewerRole
Scenario: Viewer user sees sales data on dashboard
    Given the user is authenticated as an "Viewer" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the Today's Sales card is displayed
    And the Today's Sales card shows "523" transactions
    And the Today's Sales card shows a value greater than $0
    And the Failed Sales card is displayed
    And the Failed Sales card shows "18" transactions

@DashboardTests @ViewerRole
Scenario: Viewer user sees comparison date selector
    Given the user is authenticated as an "Viewer" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the comparison date selector is displayed

@DashboardTests @ViewerRole
Scenario: Viewer user sees recently created merchants
    Given the user is authenticated as an "Viewer" user
    When the user navigates to the Dashboard
    Then the Dashboard page is displayed
    And the Recently Created Merchants section is displayed
    And at least "1" merchant is shown in Recently Created Merchants
