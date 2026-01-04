@base @shared @playwright
Feature: Playwright Estate Tests

Background:

	Given I create the following roles
	| Role Name |
	| Estate    |
	| Merchant  |

	Given I create the following api scopes
	| Name                 | DisplayName                      | Description                            |
	| estateManagement     | Estate Managememt REST Scope     | A scope for Estate Managememt REST     |
	| transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST   |

	Given I create the following api resources
	| Name                 | DisplayName                | Secret  | Scopes               | UserClaims               |
	| estateManagement     | Estate Managememt REST     | Secret1 | estateManagement     | merchantId,estateId,role |
	| transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |

	Given I create the following identity resources
	| Name    | DisplayName          | Description                                                 | UserClaims                                                             |
	| openid  | Your user identifier |                                                             | sub                                                                    |
	| profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
	| email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

	Given I create the following clients
	| ClientId       | Name            | Secret  | Scopes                                                                   | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
	| serviceClient  | Service Client  | Secret1 | estateManagement,transactionProcessor                                    | client_credentials |                                      |                                       |                |                    |                      |
	| estateUIClient | Merchant Client | Secret1 | estateManagement,transactionProcessor,openid,email,profile                | hybrid             | https://localhost:[port]/signin-oidc | https://localhost:[port]/signout-oidc | false          | true               | https://[url]:[port] |

	Given I have a token to access the estate management resource
	| ClientId      |
	| serviceClient |

	Given I have created the following estates
	| EstateName  |
	| Test Estate |

	And I have created the following operators
	| EstateName  | OperatorName     | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate | Test Operator 1  | True                        | True                        |
	| Test Estate | Test Operator 2  | False                       | False                       |
	
	And I have assigned the following operators to the estates
	| EstateName  | OperatorName     |
	| Test Estate | Test Operator 1  |
	| Test Estate | Test Operator 2  |
	
	And I have created the following security users
	| EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
	| estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

	Given I am on the application home page

	And I click on the Sign In Button
	
	Then I am presented with a login screen
	
	When I login with the username 'estateuser@testestate1.co.uk' and password '123456'

	Then I am presented with the Estate Administrator Dashboard

@PRTest
Scenario: View Estate Details

	Given I click on the My Estate sidebar option
	Then I am presented with the View Estate Page
	# Flexible validation - checks that estate information is displayed
	And the estate name is displayed
	And the estate reference is displayed

@PRTest
Scenario: View Estate Operators

	Given I click on the My Estate sidebar option
	Then I am presented with the View Estate Page
	# Flexible validation for operators list
	And the estate operators list contains at least 2 operators
	And the estate operators list displays operator names
