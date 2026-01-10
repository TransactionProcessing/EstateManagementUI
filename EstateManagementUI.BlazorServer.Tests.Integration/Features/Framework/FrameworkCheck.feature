@framework @smoke
Feature: Framework Check
    As a test automation engineer
    I want to verify that the Testcontainers-based testing framework is working correctly
    So that I can run integration tests against the containerized Blazor application

Background:
    Given the application is running in a container

Scenario: Home page is accessible
    When I navigate to the home page
    Then the page title should be visible
