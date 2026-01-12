Feature: Reporting Integration Tests
    As a user of the Estate Management UI
    I want to access reporting functionality based on my role
    So that I can view and analyze transaction and settlement data

Background:
    Given the user navigates to the Reporting Dashboard

@ReportingTests @AdminRole
Scenario: Administrator user can access the Reporting Dashboard
    Given the user is authenticated as an "Administrator" user
    When the user navigates to the Reporting Dashboard
    Then the Reporting Dashboard page is displayed
    And the page title is "Reporting Dashboard"

@ReportingTests @EstateRole
Scenario: Estate user can access the Reporting Dashboard
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Reporting Dashboard
    Then the Reporting Dashboard page is displayed
    And the page title is "Reporting Dashboard"
    And the Transaction Reporting section is displayed
    And the Settlement Reporting section is displayed
    And the Reconciliation section is displayed
    And the KPI Reporting section is displayed

@ReportingTests @ViewerRole
Scenario: Viewer user can access the Reporting Dashboard
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Reporting Dashboard
    Then the Reporting Dashboard page is displayed
    And the page title is "Reporting Dashboard"
    And the Transaction Reporting section is displayed
    And the Settlement Reporting section is displayed

# Transaction Detail Report Tests

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can view Transaction Detail Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    Then the Transaction Detail Report page is displayed
    And the page title is "Transaction Detail Report"
    And the filters section is displayed
    And the transaction details grid is displayed

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can filter Transaction Detail Report by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user sets the start date to "7" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the transaction details grid displays filtered results
    And the Total Transactions KPI is greater than "0"

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can filter Transaction Detail Report by merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    Then the transaction details grid displays results for the selected merchant
    And all displayed transactions match the selected merchant

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can filter Transaction Detail Report by operator
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user selects an operator from the filter dropdown
    And the user clicks Apply Filters
    Then the transaction details grid displays results for the selected operator
    And all displayed transactions match the selected operator

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can filter Transaction Detail Report by product
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user selects a product from the filter dropdown
    And the user clicks Apply Filters
    Then the transaction details grid displays results for the selected product
    And all displayed transactions match the selected product

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can sort Transaction Detail Report columns
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user clicks on the "Merchant" column header
    Then the transaction details grid is sorted by "Merchant" in ascending order

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can paginate through Transaction Detail Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the transaction details grid has multiple pages
    When the user clicks the Next Page button
    Then the transaction details grid displays the next page of results

@ReportingTests @EstateRole @TransactionDetail
Scenario: Estate user can clear filters on Transaction Detail Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Detail Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    And the user clicks Clear Filters
    Then all filter selections are reset to default values
    And the transaction details grid displays unfiltered results

# Transaction Summary by Merchant Tests

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user can view Transaction Summary by Merchant Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    Then the Transaction Summary by Merchant page is displayed
    And the page title is "Transaction Summary by Merchant"
    And the merchant summary grid is displayed

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user sees merchant summary KPIs
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    Then the Total Merchants KPI is displayed
    And the Total Transactions KPI is displayed
    And the Total Value KPI is displayed
    And the Average Transaction KPI is displayed

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user can filter Transaction Summary by Merchant by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the merchant summary grid displays filtered results

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user can filter Transaction Summary by Merchant by merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    Then the merchant summary grid displays results for the selected merchant

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user can filter Transaction Summary by Merchant by operator
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    And the user selects an operator from the filter dropdown
    And the user clicks Apply Filters
    Then the merchant summary grid displays results for the selected operator

@ReportingTests @EstateRole @TransactionSummaryMerchant
Scenario: Estate user can drill down from Merchant Summary to Transaction Detail
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Merchant Report
    And the user clicks View Details for a merchant
    Then the user is navigated to the Transaction Detail Report
    And the merchant filter is pre-populated

# Transaction Summary by Operator Tests

@ReportingTests @EstateRole @TransactionSummaryOperator
Scenario: Estate user can view Transaction Summary by Operator Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Operator Report
    Then the Transaction Summary by Operator page is displayed
    And the page title is "Transaction Summary by Operator"
    And the operator summary grid is displayed

@ReportingTests @EstateRole @TransactionSummaryOperator
Scenario: Estate user can filter Transaction Summary by Operator by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Operator Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the operator summary grid displays filtered results

@ReportingTests @EstateRole @TransactionSummaryOperator
Scenario: Estate user can filter Transaction Summary by Operator by operator
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Transaction Summary by Operator Report
    And the user selects an operator from the filter dropdown
    And the user clicks Apply Filters
    Then the operator summary grid displays results for the selected operator

# Product Performance Report Tests

@ReportingTests @EstateRole @ProductPerformance
Scenario: Estate user can view Product Performance Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Product Performance Report
    Then the Product Performance Report page is displayed
    And the page title is "Product Performance Report"
    And the product performance grid is displayed

@ReportingTests @EstateRole @ProductPerformance
Scenario: Estate user sees product performance KPIs
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Product Performance Report
    Then the Total Products KPI is displayed
    And the Total Transactions KPI is displayed
    And the Total Value KPI is displayed
    And the Average per Product KPI is displayed

@ReportingTests @EstateRole @ProductPerformance
Scenario: Estate user can filter Product Performance Report by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Product Performance Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Refresh
    Then the product performance grid displays filtered results

@ReportingTests @EstateRole @ProductPerformance
Scenario: Estate user can toggle between grid and chart view
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Product Performance Report
    And the user clicks Chart View button
    Then the product performance chart is displayed
    When the user clicks Grid View button
    Then the product performance grid is displayed

# Settlement Summary Report Tests

@ReportingTests @EstateRole @SettlementSummary
Scenario: Estate user can view Settlement Summary Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Summary Report
    Then the Settlement Summary Report page is displayed
    And the page title is "Settlement Summary Report"
    And the settlement summary grid is displayed

@ReportingTests @EstateRole @SettlementSummary
Scenario: Estate user sees settlement summary KPIs
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Summary Report
    Then the Total Merchants KPI is displayed
    And the Gross Value KPI is displayed
    And the Total Fees KPI is displayed
    And the Net Settlement Value KPI is displayed

@ReportingTests @EstateRole @SettlementSummary
Scenario: Estate user can filter Settlement Summary Report by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Summary Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the settlement summary grid displays filtered results

@ReportingTests @EstateRole @SettlementSummary
Scenario: Estate user can filter Settlement Summary Report by merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Summary Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    Then the settlement summary grid displays results for the selected merchant

@ReportingTests @EstateRole @SettlementSummary
Scenario: Estate user can filter Settlement Summary Report by settlement status
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Summary Report
    And the user selects "settled" from the status filter dropdown
    And the user clicks Apply Filters
    Then the settlement summary grid displays only settled settlements

# Settlement Reconciliation Report Tests

@ReportingTests @EstateRole @SettlementReconciliation
Scenario: Estate user can view Settlement Reconciliation Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Reconciliation Report
    Then the Settlement Reconciliation Report page is displayed
    And the page title is "Settlement vs Transaction Reconciliation Report"
    And the reconciliation grid is displayed

@ReportingTests @EstateRole @SettlementReconciliation
Scenario: Estate user can filter Settlement Reconciliation Report by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Reconciliation Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the reconciliation grid displays filtered results

@ReportingTests @EstateRole @SettlementReconciliation
Scenario: Estate user can filter Settlement Reconciliation Report by merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Settlement Reconciliation Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    Then the reconciliation grid displays results for the selected merchant

# Merchant Settlement History Tests

@ReportingTests @EstateRole @MerchantSettlementHistory
Scenario: Estate user can view Merchant Settlement History Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Merchant Settlement History Report
    Then the Merchant Settlement History page is displayed
    And the page title is "Merchant Settlement History"
    And the settlement history grid is displayed

@ReportingTests @EstateRole @MerchantSettlementHistory
Scenario: Estate user can filter Merchant Settlement History by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Merchant Settlement History Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Apply Filters
    Then the settlement history grid displays filtered results

@ReportingTests @EstateRole @MerchantSettlementHistory
Scenario: Estate user can filter Merchant Settlement History by merchant
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Merchant Settlement History Report
    And the user selects a merchant from the filter dropdown
    And the user clicks Apply Filters
    Then the settlement history grid displays results for the selected merchant

# Analytical Charts Tests

@ReportingTests @EstateRole @AnalyticalCharts
Scenario: Estate user can view Analytical Charts Report
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Analytical Charts Report
    Then the Analytical Charts page is displayed
    And the page title is "Analytical Charts (Volume & Value)"
    And the analytical charts are displayed

@ReportingTests @EstateRole @AnalyticalCharts
Scenario: Estate user can filter Analytical Charts by date range
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Analytical Charts Report
    And the user sets the start date to "30" days ago
    And the user sets the end date to today
    And the user clicks Refresh
    Then the analytical charts display filtered results

@ReportingTests @EstateRole @AnalyticalCharts
Scenario: Estate user can toggle between volume and value charts
    Given the user is authenticated as an "Estate" user
    When the user navigates to the Analytical Charts Report
    And the user selects the Volume chart type
    Then the volume chart is displayed
    When the user selects the Value chart type
    Then the value chart is displayed

# Viewer Role Tests

@ReportingTests @ViewerRole @TransactionDetail
Scenario: Viewer user can view Transaction Detail Report
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Transaction Detail Report
    Then the Transaction Detail Report page is displayed
    And the transaction details grid is displayed

@ReportingTests @ViewerRole @TransactionSummaryMerchant
Scenario: Viewer user can view Transaction Summary by Merchant Report
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Transaction Summary by Merchant Report
    Then the Transaction Summary by Merchant page is displayed
    And the merchant summary grid is displayed

@ReportingTests @ViewerRole @SettlementSummary
Scenario: Viewer user can view Settlement Summary Report
    Given the user is authenticated as a "Viewer" user
    When the user navigates to the Settlement Summary Report
    Then the Settlement Summary Report page is displayed
    And the settlement summary grid is displayed
