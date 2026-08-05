@base @background @reporting @dashboard
Feature: Reporting
  As an authenticated estate user
  I want to inspect the reporting screens
  So that I can verify seeded transaction and settlement data is surfaced correctly

  Background:
    Given I create the following roles
      | Role Name     |
      | Administrator |
      | Estate        |

    And I create the following api scopes
      | Name                 | DisplayName                      | Description                          |
      | transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST |
      | fileProcessor        | File Processor REST Scope        | Scope for File Processor REST        |
      | estateReporting      | Estate Reporting REST Scope      | Scope for Estate Reporting REST      |

    And I create the following api resources
      | Name                 | DisplayName                | Secret  | Scopes               | UserClaims               |
      | transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |
      | fileProcessor        | File Processor REST        | Secret1 | fileProcessor        | merchantId,estateId,role |
      | estateReporting      | Estate Reporting REST      | Secret1 | estateReporting      | merchantId,estateId,role |

    And I create the following identity resources
      | Name    | DisplayName          | Description                                                 | UserClaims                                                             |
      | openid  | Your user identifier |                                                             | sub                                                                    |
      | profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
      | email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

    And I create the following clients
      | ClientId      | Name            | Secret  | Scopes                                                                  | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
      | serviceClient | Service Client  | Secret1 | transactionProcessor,fileProcessor,estateReporting                      | client_credentials |                                      |                                       |                |                    |                      |
      | estateUIClient | Merchant Client | Secret1 | fileProcessor,transactionProcessor,estateReporting,openid,email,profile | hybrid             | https://127.0.0.1:[port]/signin-oidc | https://127.0.0.1:[port]/signout-oidc | false          | true               | https://127.0.0.1:[port] |

    And I create the following users
      | Email Address             | Phone Number | Given Name | Middle Name | Family Name | Claims | Roles         | Password |
      | administrator@admin.co.uk |    123456789 | Test       |             | User 1      |        | Administrator | 123456   |

    And I have a token to access the transaction Processor resource
      | ClientId      |
      | serviceClient |

    And I have created the following estates
      | EstateName  |
      | Test Estate |

    And I have created the following operators
      | EstateName  | OperatorName  | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
      | Test Estate | Test Operator | True                        | True                        |

    And I have assigned the following operators to the estates
      | EstateName  | OperatorName  |
      | Test Estate | Test Operator |

    And I have created the following security users
      | EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
      | estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

    And the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'

    And I have created the following reporting operators
      | Operator Name          | Require Custom Merchant Number | Require Custom Terminal Number |
      | Safaricom              | True                           | True                           |
      | Voucher                | True                           | True                           |

    And I have created the following reporting merchant setups
      | Merchant Name          | Settlement Schedule | Operator Name         | Merchant Number | Terminal Number | Device Identifier | Contract Description   | Product Name          | Product Display Text  | Product Type | Product Value | Fee Description          | Fee Type | Calculation Type | Fee Value |
      | Reporting Merchant A   | Immediate           | Safaricom             | 700001          | 900001          | REPORT-DEVICE-A   | Reporting Contract A   | Reporting Product A   | Reporting Product A   | BillPayment  | 10.00         | Reporting Product A Fee  | Merchant | Fixed            | 1.25      |
      | Reporting Merchant B   | Immediate           | Voucher               | 700002          | 900002          | REPORT-DEVICE-B   | Reporting Contract B   | Reporting Product B   | Reporting Product B   | BillPayment  | 10.00         | Reporting Product B Fee  | Merchant | Fixed            | 1.25      |

    And I have made the following reporting merchant deposits
      | Merchant Name          | Amount | Date       | Reference           |
      | Reporting Merchant A   | 100.00 | Today | REPORT-DEPOSIT-A    |
      | Reporting Merchant B   | 100.00 | Today | REPORT-DEPOSIT-B    |

    And I have seeded the following reporting sales
      | Transaction Number | Merchant Name        | Product Name        | Operator Name | Device Identifier | Transaction Date Time | Transaction Type | Amount | Account Number | Hours |
      |               1001 | Reporting Merchant A | Reporting Product A | Safaricom     | REPORT-DEVICE-A   | Today                 | sale             |  10.00 |      123456789 |     9 |
      |               1002 | Reporting Merchant A | Reporting Product A | Safaricom     | REPORT-DEVICE-A   | Today                 | sale             |  10.00 |      123456789 |    13 |
      |               1003 | Reporting Merchant B | Reporting Product B | Voucher       | REPORT-DEVICE-B   | Today                 | sale             |  10.00 |      123456789 |    15 |
      |               1004 | Reporting Merchant A | Reporting Product A | Safaricom     | REPORT-DEVICE-A   | Yesterday             | sale             |  10.00 |      123456789 |    10 |
      |               1005 | Reporting Merchant B | Reporting Product B | Voucher       | REPORT-DEVICE-B   | Yesterday             | sale             |  10.00 |      123456789 |    14 |

    And I have processed the following reporting settlements
      | Merchant Name          | Settlement Date |
      | Reporting Merchant A   | Yesterday      |
      | Reporting Merchant B   | Yesterday      |

    And I run the todays summary stored procedures for 'Yesterday'
    And I run the historic summary stored procedures for 'Yesterday'
    And I run the todays summary stored procedures for 'Today'

  Scenario: Reporting dashboard exposes all report links
    When I open the reporting dashboard
    Then I should see the reporting dashboard heading
    And I should see the reporting dashboard links

  Scenario: Transaction detail report shows the seeded sales
    When I open the transaction detail report
    Then I should see the transaction detail report heading
    And the transaction detail report should show these summary values
      | Label              | Value |
      | Total Transactions | 5     |
      | Gross Amount       | 50.00 |
      | Total Fees         | 6.25  |
      | Net Amount         | 43.75 |
    And the transaction detail report should list these transactions
      | Transaction Number | Merchant              | Product              | Type | Status      |
      | 1001               | Reporting Merchant A  | Reporting Product A  | sale | Authorised  |
      | 1002               | Reporting Merchant A  | Reporting Product A  | sale | Authorised  |
      | 1003               | Reporting Merchant B  | Reporting Product B  | sale | Authorised  |
      | 1004               | Reporting Merchant A  | Reporting Product A  | sale | Authorised  |
      | 1005               | Reporting Merchant B  | Reporting Product B  | sale | Authorised  |

  Scenario: Transaction summary by merchant aggregates seeded sales
    When I open the transaction summary by merchant report
    Then I should see the transaction summary by merchant report heading
    And the merchant summary report should show these summary values
      | Label              | Value |
      | Total Merchants    | 2     |
      | Total Transactions | 5     |
      | Total Value        | 50.00 |
      | Average Transaction | 10.00 |
    And the merchant summary report should list these merchant rows
      | Merchant             | Total Count | Total Value | Average Value | Successful | Failed | Success Rate |
      | Reporting Merchant A | 3           | 30.00       | 30.00         | 3          | 0      | 100.0        |
      | Reporting Merchant B | 2           | 20.00       | 20.00         | 2          | 0      | 100.0        |

  Scenario: Transaction summary by operator aggregates seeded sales
    When I open the transaction summary by operator report
    Then I should see the transaction summary by operator report heading
    And the operator summary report should show these summary values
      | Label              | Value |
      | Total Operators    | 2     |
      | Total Transactions | 5     |
      | Total Value        | 50.00 |
      | Average Transaction | 10.00 |
    And the operator summary report should list these operator rows
      | Operator          | Transaction Count | Transaction Value | Average Value | Successful | Failed | Success Rate |
      | Safaricom         | 3                 | 30.00             | 30.00         | 3          | 0      | 100.0        |
      | Voucher           | 2                 | 20.00             | 20.00         | 2          | 0      | 100.0        |

  Scenario: Product performance report shows the seeded product split
    When I open the product performance report
    Then I should see the product performance report heading
    And the product performance report should show these summary values
      | Label                | Value |
      | Total Products       | 2     |
      | Total Transactions   | 5     |
      | Total Value          | 50.00 |
      | Average per Product  | 25.00 |
    And the product performance report should list these product rows
      | Product              | Transaction Count | Transaction Value | Percentage |
      | Reporting Product A  | 3                 | 30.00             | 60.00      |
      | Reporting Product B  | 2                 | 20.00             | 40.00      |
    And the product performance report should validate the percentage split
      | Validation Message |
      | Percentages sum to 100% - Data validated |

  Scenario: Settlement summary report still renders the placeholder state
    When I open the settlement summary report
    Then I should see the settlement summary report heading
    And I should see the settlement summary placeholder

  Scenario: Merchant settlement history report still renders the placeholder state
    When I open the merchant settlement history report
    Then I should see the merchant settlement history report heading
    And I should see the merchant settlement history placeholder

  Scenario: Settlement reconciliation report still renders the placeholder state
    When I open the settlement reconciliation report
    Then I should see the settlement reconciliation report heading
    And I should see the settlement reconciliation placeholder

  Scenario: Analytical charts report shows seeded count and value data
    When I open the analytical charts report
    Then I should see the analytical charts report heading
    And the analytical charts report should show these summary values
      | Label         | Value |
      | Total Count   | 3     |
      | Total Value   | 30.00 |
      | Average Value | 10.00 |
      | Net Settlement | 3.75  |
    And the analytical charts report should use this comparison date
      | Comparison Date |
      | Yesterday      |
    And the analytical charts report should compare these chart totals
      | Chart   | Today | Comparison |
      | Volume  | 3     | 2          |
      | Value   | 30.00 | 20.00      |
    And the analytical charts report should compare these hourly chart points
      | Chart  | Hour  | Today | Comparison |
      | Volume | 09:00 | 1     | 0          |
      | Volume | 10:00 | 0     | 1          |
      | Volume | 13:00 | 1     | 0          |
      | Volume | 14:00 | 0     | 1          |
      | Volume | 15:00 | 1     | 0          |
      | Value  | 09:00 | 10.00 | 0.00       |
      | Value  | 10:00 | 0.00  | 10.00      |
      | Value  | 13:00 | 10.00 | 0.00       |
      | Value  | 14:00 | 0.00  | 10.00      |
      | Value  | 15:00 | 10.00 | 0.00       |
