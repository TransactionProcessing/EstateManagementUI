@base @background @estate @fileprocessing @dashboard
Feature: File Processing
  As an authenticated estate user
  I want to upload a batch topup file
  So that I can prove the upload flow works end to end

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

    Given the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the dashboard heading
    And I have created the following merchants
      | MerchantName    | SettlementSchedule | AddressLine1       | AddressLine2 | Town      | Region      | PostCode | Country         | ContactName | EmailAddress             | PhoneNumber  |
      | Test Merchant 1 | Immediate          | 1 Integration Road | Suite 100    | Test Town | Test Region | TE1 1ST  | United Kingdom  | Test Contact | test.contact@example.com | 01234567890  |
    And I have created the following file profiles
      | FileProfileId                        | Name           | ListeningDirectory        | RequestType | OperatorName  | LineTerminator         | FileFormatHandler   |
      | 11111111-1111-1111-1111-111111111111 | SafaricomTopup | /var/lib/fileprocessor/in | BatchTopup  | Test Operator | CarriageReturnLineFeed | CsvFileFormatHandler |

  Scenario: Estate users can upload a batch topup file successfully
    When I open the file processing screen
    Then I should see the file processing heading
    When I open the file upload page
    Then I should see the file upload page
    And the upload dropdowns should default to the placeholder option
    When I upload a batch topup file
      | PhoneNumber    | Amount |
      | 254701000001   | 500    |
      | 254701000002   | 500    |
      | 254701000003   | 500    |
    Then I should see the upload success message
