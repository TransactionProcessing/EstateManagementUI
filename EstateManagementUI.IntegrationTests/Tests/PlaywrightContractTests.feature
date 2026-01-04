@base @shared @playwright
Feature: Playwright Contract Tests

Background:

	Given I create the following roles
	| Role Name |
	| Estate    |
	| Merchant  |

	Given I create the following api scopes
	| Name                 | DisplayName                      | Description                            |
	| estateManagement     | Estate Managememt REST Scope     | A scope for Estate Managememt REST     |
	| transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST   |
	| fileProcessor        | File Processor REST Scope        | Scope for File Processor REST          |

	Given I create the following api resources
	| Name                 | DisplayName                | Secret  | Scopes               | UserClaims               |
	| estateManagement     | Estate Managememt REST     | Secret1 | estateManagement     | merchantId,estateId,role |
	| transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |
	| fileProcessor        | File Processor REST        | Secret1 | fileProcessor        | merchantId,estateId,role |

	Given I create the following identity resources
	| Name    | DisplayName          | Description                                                 | UserClaims                                                             |
	| openid  | Your user identifier |                                                             | sub                                                                    |
	| profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
	| email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

	Given I create the following clients
	| ClientId       | Name            | Secret  | Scopes                                                                   | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
	| serviceClient  | Service Client  | Secret1 | estateManagement,transactionProcessor                                    | client_credentials |                                      |                                       |                |                    |                      |
	| estateUIClient | Merchant Client | Secret1 | estateManagement,fileProcessor,transactionProcessor,openid,email,profile | hybrid             | https://localhost:[port]/signin-oidc | https://localhost:[port]/signout-oidc | false          | true               | https://[url]:[port] |

	Given I have a token to access the estate management resource
	| ClientId      |
	| serviceClient |

	Given I have created the following estates
	| EstateName  |
	| Test Estate |

	And I have created the following operators
	| EstateName  | OperatorName     | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate | Test Operator    | True                        | True                        |
	
	And I have assigned the following operators to the estates
	| EstateName  | OperatorName     |
	| Test Estate | Test Operator    |

	Given I create the following contracts
	| EstateName  | OperatorName     | Description        |
	| Test Estate | Test Operator    | Test Contract 1    |
	| Test Estate | Test Operator    | Test Contract 2    |

	Given I create the following Products for 'Test Contract 1'
	| EstateName  | OperatorName     | ProductName     | DisplayText | Value  |
	| Test Estate | Test Operator    | Test Product 1  | Test        | 100.00 |
	| Test Estate | Test Operator    | Test Product 2  | Test        | 50.00  |

	Given I create the following Products for 'Test Contract 2'
	| EstateName  | OperatorName     | ProductName     | DisplayText | Value  |
	| Test Estate | Test Operator    | Test Product 3  | Test        | 75.00  |
	
	And I have created the following security users
	| EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
	| estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

	Given I am on the application home page

	And I click on the Sign In Button
	
	Then I am presented with a login screen
	
	When I login with the username 'estateuser@testestate1.co.uk' and password '123456'

	Then I am presented with the Estate Administrator Dashboard

@PRTest
Scenario: Contract List and Navigation

	Given I click on the My Contracts sidebar option
	Then I am presented with the Contracts List Screen
	# Flexible validation - checks that contracts are displayed but allows for backend changes
	And the contract list contains at least 2 contracts
	And the contract list displays contract descriptions
	And the contract list displays operator information
	When I click on the New Contract Button
	Then the Add New Contract Screen is displayed

@PRTest
Scenario: View Contract Products

	Given I click on the My Contracts sidebar option
	Then I am presented with the Contracts List Screen
	When I click on the View Products Button for contract 'Test Contract 1'
	Then the Contract Products Screen is displayed
	# Flexible validation for products
	And the product list contains at least 2 products
	And the product list displays product names
	And the product list displays product values
