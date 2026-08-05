@base @background @estate @dashboard
Feature: Estate Management
  As an authenticated estate user
  I want to move through the estate management screens
  So that I can manage estate operators from one end-to-end journey

  Background:
    Given I create the following roles
      | Role Name     |
      | Administrator |
      | Estate        |

    Given I create the following api scopes
      | Name                 | DisplayName                      | Description                          |
      | transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST |
      | fileProcessor        | File Processor REST Scope        | Scope for File Processor REST        |
      | estateReporting      | Estate Reporting REST Scope      | Scope for Estate Reporting REST      |

    Given I create the following api resources
      | Name                 | DisplayName                | Secret  | Scopes               | UserClaims               |
      | transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |
      | fileProcessor        | File Processor REST        | Secret1 | fileProcessor        | merchantId,estateId,role |
      | estateReporting      | Estate Reporting REST      | Secret1 | estateReporting      | merchantId,estateId,role |

    Given I create the following identity resources
      | Name    | DisplayName          | Description                                                 | UserClaims                                                             |
      | openid  | Your user identifier |                                                             | sub                                                                    |
      | profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
      | email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

    Given I create the following clients
      | ClientId       | Name            | Secret  | Scopes                                                                  | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
      | serviceClient  | Service Client  | Secret1 | transactionProcessor,fileProcessor,estateReporting                      | client_credentials |                                      |                                       |                |                    |                      |
      | estateUIClient | Merchant Client | Secret1 | fileProcessor,transactionProcessor,estateReporting,openid,email,profile | hybrid             | https://127.0.0.1:[port]/signin-oidc | https://127.0.0.1:[port]/signout-oidc | false          | true               | https://127.0.0.1:[port] |

    Given I create the following users
      | Email Address             | Phone Number | Given Name | Middle Name | Family Name | Claims | Roles         | Password |
      | administrator@admin.co.uk |    123456789 | Test       |             | User 1      |        | Administrator | 123456   |

    Given I have a token to access the transaction Processor resource
      | ClientId      |
      | serviceClient |

    Given I have created the following estates
      | EstateName  |
      | Test Estate |

    And I have created the following operators
      | EstateName  | OperatorName     | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
      | Test Estate | Test Operator    | True                        | True                        |
      | Test Estate | Spare Operator    | False                       | False                       |

    And I have assigned the following operators to the estates
      | EstateName  | OperatorName   |
      | Test Estate | Test Operator  |

    And I have created the following security users
      | EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
      | estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

    Given the user navigates to the entry screen
    Then I should see the entry screen
    When I open the estate information page
    Then I should see the estate info page
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the dashboard heading

  Scenario: Estate users can navigate the estate screens and manage operators
    When I open the estate management screen
    Then I should see the estate management heading
    And I should see the estate overview summary
    When I switch to the operators tab
    Then I should see the assigned operators section
    And I should see the assigned operator 'Test Operator'
    When I add the operator 'Spare Operator' to the estate
    Then I should see the assigned operator 'Spare Operator'
    When I remove the operator 'Test Operator' from the estate
    Then I should not see the assigned operator 'Test Operator'
