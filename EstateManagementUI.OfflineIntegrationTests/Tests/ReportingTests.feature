@reporting @offline
Feature: Reporting

  Scenario: View Transaction Detail Report
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    Then the Transaction Detail report page should be displayed

  Scenario: Filter Transaction Detail Report by Date Range
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    And I set the date range filter
      | Field     | Value      |
      | Start Date| 2024-01-01 |
      | End Date  | 2024-12-31 |
    And I click the Generate Report button
    Then the report should display transactions for the date range

  Scenario: Filter Transaction Detail Report by Merchant
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    And I select "Test Merchant" from the merchant filter
    And I click the Generate Report button
    Then the report should display transactions for the selected merchant

  Scenario: Filter Transaction Detail Report by Operator
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    And I select "Test Operator" from the operator filter
    And I click the Generate Report button
    Then the report should display transactions for the selected operator

  Scenario: Filter Transaction Detail Report by Status
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    And I select "Successful" from the status filter
    And I click the Generate Report button
    Then the report should display only successful transactions

  Scenario: View Transaction Summary Merchant Report
    When I navigate to the Reporting page
    And I select the "Transaction Summary (Merchant)" report
    Then the Transaction Summary Merchant report page should be displayed

  Scenario: Filter Transaction Summary Merchant Report with Grouping
    When I navigate to the Reporting page
    And I select the "Transaction Summary (Merchant)" report
    And I set the report filters
      | Field     | Value           |
      | Start Date| 2024-01-01      |
      | End Date  | 2024-12-31      |
      | Merchant  | Test Merchant   |
      | Group By  | Day             |
    And I click the Generate Report button
    Then the report should display aggregated merchant transaction data

  Scenario: View Transaction Summary Operator Report
    When I navigate to the Reporting page
    And I select the "Transaction Summary (Operator)" report
    Then the Transaction Summary Operator report page should be displayed

  Scenario: Filter Transaction Summary Operator Report
    When I navigate to the Reporting page
    And I select the "Transaction Summary (Operator)" report
    And I set the report filters
      | Field     | Value         |
      | Start Date| 2024-01-01    |
      | End Date  | 2024-12-31    |
      | Operator  | Test Operator |
    And I click the Generate Report button
    Then the report should display aggregated operator transaction data

  Scenario: View Settlement Summary Report
    When I navigate to the Reporting page
    And I select the "Settlement Summary" report
    Then the Settlement Summary report page should be displayed

  Scenario: Filter Settlement Summary Report by Date Range
    When I navigate to the Reporting page
    And I select the "Settlement Summary" report
    And I set the date range filter
      | Field     | Value      |
      | Start Date| 2024-01-01 |
      | End Date  | 2024-12-31 |
    And I click the Generate Report button
    Then the report should display settlement data for the date range

  Scenario: View Settlement Reconciliation Report
    When I navigate to the Reporting page
    And I select the "Settlement Reconciliation" report
    Then the Settlement Reconciliation report page should be displayed

  Scenario: Filter Settlement Reconciliation Report by Merchant
    When I navigate to the Reporting page
    And I select the "Settlement Reconciliation" report
    And I select "Test Merchant" from the merchant filter
    And I click the Generate Report button
    Then the report should display reconciliation data for the selected merchant

  Scenario: View Merchant Settlement History Report
    When I navigate to the Reporting page
    And I select the "Merchant Settlement History" report
    Then the Merchant Settlement History report page should be displayed

  Scenario: Filter Merchant Settlement History Report
    When I navigate to the Reporting page
    And I select the "Merchant Settlement History" report
    And I set the report filters
      | Field     | Value          |
      | Merchant  | Test Merchant  |
      | Start Date| 2024-01-01     |
      | End Date  | 2024-12-31     |
    And I click the Generate Report button
    Then the report should display settlement history for the merchant

  Scenario: View Product Performance Report
    When I navigate to the Reporting page
    And I select the "Product Performance" report
    Then the Product Performance report page should be displayed

  Scenario: Filter Product Performance Report by Product Type
    When I navigate to the Reporting page
    And I select the "Product Performance" report
    And I set the report filters
      | Field       | Value        |
      | Product Type| Mobile Topup |
      | Start Date  | 2024-01-01   |
      | End Date    | 2024-12-31   |
    And I click the Generate Report button
    Then the report should display performance data for the product type

  Scenario: View Analytical Charts
    When I navigate to the Reporting page
    And I select the "Analytical Charts" option
    Then the analytical charts page should be displayed
    And charts should be visible

  Scenario: Filter Analytical Charts by Time Period
    When I navigate to the Reporting page
    And I select the "Analytical Charts" option
    And I select "Last 7 Days" from the time period filter
    Then the charts should update to show data for the last 7 days

  Scenario: Export Report to CSV
    When I navigate to the Reporting page
    And I select the "Transaction Detail" report
    And I set the date range filter
      | Field     | Value      |
      | Start Date| 2024-01-01 |
      | End Date  | 2024-12-31 |
    And I click the Generate Report button
    And I click the Export to CSV button
    Then the report should be prepared for download as CSV

  Scenario: Export Report to Excel
    When I navigate to the Reporting page
    And I select the "Settlement Summary" report
    And I set the date range filter
      | Field     | Value      |
      | Start Date| 2024-01-01 |
      | End Date  | 2024-12-31 |
    And I click the Generate Report button
    And I click the Export to Excel button
    Then the report should be prepared for download as Excel
