@base @shared @reporting
Feature: Reporting Tests

Background: 

	Given I create the following roles
	| Role Name  |
	| Estate |

	Given I create the following api scopes
	| Name                 | DisplayName                  | Description                        |
	| estateManagement | Estate Managememt REST Scope | A scope for Estate Managememt REST |
	| transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST |

	Given I create the following api resources
	| Name             | DisplayName            | Secret  | Scopes           | UserClaims               |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId,estateId,role |
	| transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |

	Given I create the following identity resources
	| Name    | DisplayName          | Description                                                 | UserClaims                                                             |
	| openid  | Your user identifier |                                                             | sub                                                                    |
	| profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
	| email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

	Given I create the following clients
	| ClientId       | Name            | Secret  | Scopes                                                                   | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
	| serviceClient  | Service Client  | Secret1 | estateManagement,transactionProcessor                                    | client_credentials |                                      |                                       |                |                    |                      |
	| estateUIClient | Merchant Client | Secret1 | estateManagement,transactionProcessor,openid,email,profile               | hybrid             | https://localhost:[port]/signin-oidc | https://localhost:[port]/signout-oidc | false          | true               | https://[url]:[port] |

	Given I have a token to access the estate management resource
	| ClientId      |
	| serviceClient |

	Given I have created the following estates
	| EstateName  |
	| Test Estate |

	And I have created the following security users
	| EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
	| estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

	Given I am on the application home page

	And I click on the Sign In Button
	
	Then I am presented with a login screen
	
	When I login with the username 'estateuser@testestate1.co.uk' and password '123456'

	Then I am presented with the Estate Administrator Dashboard

@PRTest
Scenario: View Transaction Detail Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen

@PRTest
Scenario: Filter Transaction Detail Report by Date Range
	When I click on the Reporting Sidebar option

	And I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen
	
	When I filter the report by date range
	| Field     | Value      |
	| StartDate | 2024-01-01 |
	| EndDate   | 2024-12-31 |

	And I click the Search button

	Then the transaction detail report should display results for the date range

@PRTest
Scenario: Filter Transaction Detail Report by Merchant
	When I click on the Reporting Sidebar option

	And I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen
	
	When I filter the report by merchant 'Test Merchant 1'

	And I click the Search button

	Then the transaction detail report should display results for merchant 'Test Merchant 1'

@PRTest
Scenario: Filter Transaction Detail Report by Operator
	When I click on the Reporting Sidebar option

	And I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen
	
	When I filter the report by operator 'Safaricom'

	And I click the Search button

	Then the transaction detail report should display results for operator 'Safaricom'

@PRTest
Scenario: Filter Transaction Detail Report by Transaction Status
	When I click on the Reporting Sidebar option

	And I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen
	
	When I filter the report by transaction status 'Successful'

	And I click the Search button

	Then the transaction detail report should display only successful transactions

@PRTest
Scenario: View Transaction Summary by Merchant Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Transaction Summary Merchant Report link

	Then I am presented with the Transaction Summary Merchant Report screen

@PRTest
Scenario: Filter Transaction Summary Merchant Report with Multiple Filters
	When I click on the Reporting Sidebar option

	And I click on the Transaction Summary Merchant Report link

	Then I am presented with the Transaction Summary Merchant Report screen
	
	When I apply multiple filters
	| Field     | Value          |
	| StartDate | 2024-01-01     |
	| EndDate   | 2024-12-31     |
	| Merchant  | Test Merchant 1|
	| GroupBy   | Day            |

	And I click the Search button

	Then the transaction summary merchant report should display aggregated results

@PRTest
Scenario: View Transaction Summary by Operator Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Transaction Summary Operator Report link

	Then I am presented with the Transaction Summary Operator Report screen

@PRTest
Scenario: Filter Transaction Summary Operator Report by Date and Operator
	When I click on the Reporting Sidebar option

	And I click on the Transaction Summary Operator Report link

	Then I am presented with the Transaction Summary Operator Report screen
	
	When I apply operator report filters
	| Field     | Value      |
	| StartDate | 2024-01-01 |
	| EndDate   | 2024-12-31 |
	| Operator  | Safaricom  |

	And I click the Search button

	Then the transaction summary operator report should display aggregated results for 'Safaricom'

@PRTest
Scenario: View Settlement Summary Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Settlement Summary Report link

	Then I am presented with the Settlement Summary Report screen

@PRTest
Scenario: Filter Settlement Summary Report by Date Range
	When I click on the Reporting Sidebar option

	And I click on the Settlement Summary Report link

	Then I am presented with the Settlement Summary Report screen
	
	When I filter the settlement report by date range
	| Field     | Value      |
	| StartDate | 2024-01-01 |
	| EndDate   | 2024-12-31 |

	And I click the Search button

	Then the settlement summary report should display results for the date range

@PRTest
Scenario: View Settlement Reconciliation Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Settlement Reconciliation Report link

	Then I am presented with the Settlement Reconciliation Report screen

@PRTest
Scenario: Filter Settlement Reconciliation Report
	When I click on the Reporting Sidebar option

	And I click on the Settlement Reconciliation Report link

	Then I am presented with the Settlement Reconciliation Report screen
	
	When I filter the reconciliation report by merchant 'Test Merchant 1'

	And I click the Search button

	Then the settlement reconciliation report should display results for 'Test Merchant 1'

@PRTest
Scenario: View Merchant Settlement History Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Merchant Settlement History Report link

	Then I am presented with the Merchant Settlement History Report screen

@PRTest
Scenario: Filter Merchant Settlement History by Merchant and Date
	When I click on the Reporting Sidebar option

	And I click on the Merchant Settlement History Report link

	Then I am presented with the Merchant Settlement History Report screen
	
	When I filter merchant settlement history
	| Field     | Value           |
	| Merchant  | Test Merchant 1 |
	| StartDate | 2024-01-01      |
	| EndDate   | 2024-12-31      |

	And I click the Search button

	Then the merchant settlement history report should display results

@PRTest
Scenario: View Product Performance Report
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Product Performance Report link

	Then I am presented with the Product Performance Report screen

@PRTest
Scenario: Filter Product Performance Report by Product and Date Range
	When I click on the Reporting Sidebar option

	And I click on the Product Performance Report link

	Then I am presented with the Product Performance Report screen
	
	When I filter product performance by
	| Field       | Value        |
	| ProductType | MobileTopup  |
	| StartDate   | 2024-01-01   |
	| EndDate     | 2024-12-31   |

	And I click the Search button

	Then the product performance report should display results for 'MobileTopup'

@PRTest
Scenario: View Analytical Charts
	When I click on the Reporting Sidebar option

	Then I am presented with the Reporting Index screen
	
	When I click on the Analytical Charts link

	Then I am presented with the Analytical Charts screen
	
	And analytical charts should be displayed

@PRTest
Scenario: Filter Analytical Charts by Time Period
	When I click on the Reporting Sidebar option

	And I click on the Analytical Charts link

	Then I am presented with the Analytical Charts screen
	
	When I select time period 'Last 7 Days' for charts

	Then the analytical charts should update with data for 'Last 7 Days'

@PRTest
Scenario: Export Transaction Detail Report to CSV
	When I click on the Reporting Sidebar option

	And I click on the Transaction Detail Report link

	Then I am presented with the Transaction Detail Report screen
	
	When I filter the report by date range
	| Field     | Value      |
	| StartDate | 2024-01-01 |
	| EndDate   | 2024-12-31 |

	And I click the Search button

	And I click the Export to CSV button

	Then the report should be downloaded as a CSV file

@PRTest
Scenario: Export Settlement Summary Report to Excel
	When I click on the Reporting Sidebar option

	And I click on the Settlement Summary Report link

	Then I am presented with the Settlement Summary Report screen
	
	When I filter the settlement report by date range
	| Field     | Value      |
	| StartDate | 2024-01-01 |
	| EndDate   | 2024-12-31 |

	And I click the Search button

	And I click the Export to Excel button

	Then the report should be downloaded as an Excel file
