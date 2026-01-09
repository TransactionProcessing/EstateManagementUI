@fileprocessing @offline
Feature: File Processing

  Scenario: View File Processing List
    When I navigate to the File Processing page
    Then I should see a list of processed files
    And the file processing table should be displayed

  Scenario: View File Details
    When I navigate to the File Processing page
    And I click on a file in the list
    Then I should see the file processing details
    And the file information should be displayed

  Scenario: Filter Files by Date Range
    When I navigate to the File Processing page
    And I set the date range filter
      | Field     | Value      |
      | Start Date| 2024-01-01 |
      | End Date  | 2024-12-31 |
    And I click the Filter button
    Then the file list should be filtered by date range
    And only files within the date range should be displayed

  Scenario: Search Files by Name
    When I navigate to the File Processing page
    And I enter "test-file" in the search box
    And I click the Search button
    Then the file list should be filtered by name
    And only matching files should be displayed
