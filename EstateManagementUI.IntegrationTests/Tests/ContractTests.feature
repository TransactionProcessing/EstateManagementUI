﻿@base @shared @uigeneral
Feature: Contract Tests

Background: 

	Given I create the following roles
	| Role Name  |
	| Estate |

	Given I create the following api scopes
	| Name                 | DisplayName                  | Description                        |
	| estateManagement | Estate Managememt REST Scope | A scope for Estate Managememt REST |
	| transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST |
	| fileProcessor | File Processor REST Scope | Scope for File Processor REST |

	Given I create the following api resources
	| Name             | DisplayName            | Secret  | Scopes           | UserClaims               |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId,estateId,role |
	| transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |
	| fileProcessor | File Processor REST | Secret1 | fileProcessor | merchantId,estateId,role |

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
	| EstateName  | OperatorName    | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate | Test Operator 1 | True                        | True                        |
	| Test Estate | Test Operator 2 | True                        | False                       |
	| Test Estate | Test Operator 3 | False                       | True                        |

	And I have assigned the following operators to the estates
	| EstateName  | OperatorName    |
	| Test Estate | Test Operator 1 |
	| Test Estate | Test Operator 2 |
	| Test Estate | Test Operator 3 |

	And I have created the following security users
	| EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
	| estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

	Given I have created the following contracts
	| EstateName | OperatorName | ContractDescription |
	| Test Estate | Test Operator 1 | Operator 1 Contract |
	| Test Estate | Test Operator 2 | Operator 2 Contract |
	| Test Estate | Test Operator 3 | Operator 3 Contract |

	Given I have created the following contract products
	| EstateName  | OperatorName     | ContractDescription | ProductName      | DisplayText | Value  | ProductType |
	| Test Estate | Test Operator 1  | Operator 1 Contract | 100 KES Topup    | 100 KES     | 100.00 | MobileTopup |
	| Test Estate | Test Operator 1  | Operator 1 Contract | Variable Topup 1 | Custom      |        | MobileTopup |
	| Test Estate | Test Operator 2  | Operator 2 Contract | 200 KES Topup    | 200 KES     | 500.00 | MobileTopup |
	| Test Estate | Test Operator 2  | Operator 2 Contract | 500 KES Topup    | 500 KES     | 500.00 | MobileTopup |
	| Test Estate | Test Operator 2  | Operator 2 Contract | Variable Topup 1 | Custom      |        | MobileTopup |
	| Test Estate | Test Operator 3 | Operator 3 Contract | 50 KES Topup     | 50 KES      | 50.00  | MobileTopup |
	
	Given I am on the application home page

	And I click on the Sign In Button
	
	Then I am presented with a login screen
	
	When I login with the username 'estateuser@testestate1.co.uk' and password '123456'

	Then I am presented with the Estate Administrator Dashboard

@PRTest
Scenario: View Contract List

	Given I click on the My Contracts sidebar option
	Then I am presented with the Contracts List Screen
	And the following contract details are in the list
	| Description         | OperatorName    | Products |
	| Operator 1 Contract | Test Operator 1 | 2        |
	| Operator 2 Contract | Test Operator 2 | 3        |
	| Operator 3 Contract | Test Operator 3 | 1        |