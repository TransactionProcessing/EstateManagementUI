@base @shared @fileprocessing
Feature: File Processing Tests

Background: 

	Given I create the following roles
	| Role Name  |
	| Estate |

	Given I create the following api scopes
	| Name                 | DisplayName                  | Description                        |
	| estateManagement | Estate Managememt REST Scope | A scope for Estate Managememt REST |
	| fileProcessor | File Processor REST Scope | Scope for File Processor REST |

	Given I create the following api resources
	| Name             | DisplayName            | Secret  | Scopes           | UserClaims               |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId,estateId,role |
	| fileProcessor | File Processor REST | Secret1 | fileProcessor | merchantId,estateId,role |

	Given I create the following identity resources
	| Name    | DisplayName          | Description                                                 | UserClaims                                                             |
	| openid  | Your user identifier |                                                             | sub                                                                    |
	| profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
	| email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

	Given I create the following clients
	| ClientId       | Name            | Secret  | Scopes                                                                   | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
	| serviceClient  | Service Client  | Secret1 | estateManagement,fileProcessor                                           | client_credentials |                                      |                                       |                |                    |                      |
	| estateUIClient | Merchant Client | Secret1 | estateManagement,fileProcessor,openid,email,profile                      | hybrid             | https://localhost:[port]/signin-oidc | https://localhost:[port]/signout-oidc | false          | true               | https://[url]:[port] |

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
Scenario: View File Processing List
	When I click on the File Processing Sidebar option

	Then I am presented with the File Processing List screen
	
	And the File Processing list should be displayed

@PRTest
Scenario: View File Processing Details
	When I click on the File Processing Sidebar option

	Then I am presented with the File Processing List screen
	
	When I click on a file processing entry

	Then I am presented with the File Processing Details screen
	
	And the file processing details should be displayed

@PRTest
Scenario: Filter File Processing by Date Range
	When I click on the File Processing Sidebar option

	Then I am presented with the File Processing List screen
	
	When I filter file processing by date range from '2024-01-01' to '2024-12-31'

	Then the file processing list should be filtered by the date range

@PRTest
Scenario: Search File Processing by File Name
	When I click on the File Processing Sidebar option

	Then I am presented with the File Processing List screen
	
	When I search for file processing by name 'test-file.csv'

	Then the file processing list should show results for 'test-file.csv'
