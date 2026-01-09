@base @shared @permissions
Feature: Permissions Tests

Background: 

	Given I create the following roles
	| Role Name  |
	| Estate |

	Given I create the following api scopes
	| Name                 | DisplayName                  | Description                        |
	| estateManagement | Estate Managememt REST Scope | A scope for Estate Managememt REST |

	Given I create the following api resources
	| Name             | DisplayName            | Secret  | Scopes           | UserClaims               |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId,estateId,role |

	Given I create the following identity resources
	| Name    | DisplayName          | Description                                                 | UserClaims                                                             |
	| openid  | Your user identifier |                                                             | sub                                                                    |
	| profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
	| email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

	Given I create the following clients
	| ClientId       | Name            | Secret  | Scopes                                                                   | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
	| serviceClient  | Service Client  | Secret1 | estateManagement                                                         | client_credentials |                                      |                                       |                |                    |                      |
	| estateUIClient | Merchant Client | Secret1 | estateManagement,openid,email,profile                                    | hybrid             | https://localhost:[port]/signin-oidc | https://localhost:[port]/signout-oidc | false          | true               | https://[url]:[port] |

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
Scenario: View Permissions List
	When I click on the Permissions Sidebar option

	Then I am presented with the Permissions List screen
	
	And the permissions list should be displayed

@PRTest
Scenario: View Permission Details
	When I click on the Permissions Sidebar option

	Then I am presented with the Permissions List screen
	
	When I click on a permission entry

	Then I am presented with the Permission Details screen
	
	And the permission details should be displayed

@PRTest
Scenario: Edit Permission
	When I click on the Permissions Sidebar option

	Then I am presented with the Permissions List screen
	
	When I click on a permission entry

	Then I am presented with the Permission Details screen
	
	When I click the Edit Permission button

	Then I am presented with the Edit Permission screen
	
	When I update the permission details
	| Field             | Value                    |
	| PermissionName    | Updated Permission Name  |
	| Description       | Updated description      |

	And I click the Save Permission button

	Then the permission should be updated successfully

@PRTest
Scenario: Search Permissions by Name
	When I click on the Permissions Sidebar option

	Then I am presented with the Permissions List screen
	
	When I search for permissions by name 'Merchant'

	Then the permissions list should show results containing 'Merchant'

@PRTest
Scenario: Filter Permissions by Role
	When I click on the Permissions Sidebar option

	Then I am presented with the Permissions List screen
	
	When I filter permissions by role 'Estate'

	Then the permissions list should show only permissions for role 'Estate'
