@base @background @dashboard @estate
Feature: Merchant Management
  As an authenticated estate user
  I want to move through the merchant management screens
  So that I can create, inspect, edit, schedule, and deposit against a merchant from one journey

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
      | EstateName  | OperatorName  | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
      | Test Estate | Test Operator | True                        | True                        |

    And I have assigned the following operators to the estates
      | EstateName  | OperatorName  |
      | Test Estate | Test Operator |

    And I have created the following security users
      | EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
      | estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

  Scenario: Estate users can complete the full merchant screen flow
    Given the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the dashboard heading
    When I open the merchant management screen
    Then I should see the merchant management heading
    When I create the following merchants
      | MerchantName           | SettlementSchedule | AddressLine1       | AddressLine2 | Town      | Region      | PostCode | Country        | ContactName  | EmailAddress             | PhoneNumber  |
      | Integration Merchant 1 | Immediate          | 1 Integration Road | Suite 100    | Test Town | Test Region | TE1 1ST  | United Kingdom | Test Contact | test.contact@example.com | 01234567890  |
    Then I should see the merchant in the list
    When I view the merchant
    Then I should see the merchant view page
    When I switch to the address tab
    Then I should see the merchant address details
    When I switch to the contact tab
    Then I should see the merchant contact details
    When I switch to the opening hours tab
    Then I should see the merchant opening hours details
    When I switch to the merchant operators tab
    Then I should see no operators assigned
    When I switch to the contracts tab
    Then I should see no contracts assigned
    When I switch to the devices tab
    Then I should see no devices assigned
    When I open the merchant schedule from the view page
    Then I should see the read-only merchant schedule page
    When I return to the merchant view page
    Then I should see the merchant view page
    When I open the merchant edit page
    Then I should see the merchant edit page
    When I switch to the opening hours editor
    Then I should see the merchant opening hours editor
    When I save merchant opening hours
      | MondayOpening | MondayClosing | TuesdayOpening | TuesdayClosing | WednesdayOpening | WednesdayClosing | ThursdayOpening | ThursdayClosing | FridayOpening | FridayClosing | SaturdayOpening | SaturdayClosing | SundayOpening | SundayClosing |
      | 0800          | 1700          | 0800           | 1700           | 0800             | 1700             | 0800            | 1700            | 0800          | 1700          | 0900            | 1600            | 1000          | 1500          |
    Then I should see merchant opening hours updated successfully
    When I switch to the merchant operators editor
    And I add the operator to the merchant
      | OperatorName  | MerchantNumber | TerminalNumber |
      | Test Operator | 12345678       | 87654321       |
    Then I should see the operator in the merchant list
    When I switch to the contracts editor
    Then I should see no contracts assigned
    When I switch to the devices editor
    And I add the device to the merchant
      | DeviceIdentifier |
      | DEVICE-001       |
    Then I should see the device in the merchant list
    When I open the merchant schedule from the edit page
    Then I should see the editable merchant schedule page
    When I save the merchant schedule
      | Year | ClosedDays |
      | 2027 | 1, 2, 15   |
    Then I should see schedule saved successfully
    When I return to the merchant edit page
    Then I should see the merchant edit page
    When I open the merchant deposit page
    Then I should see the merchant deposit page
    When I submit the merchant deposit
      | Amount | Date       | Reference |
      | 100    | 2026-07-22 | DEP-001   |
    Then I should be back on the merchant list
