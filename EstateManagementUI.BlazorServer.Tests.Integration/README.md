# EstateManagementUI.BlazorServer.Tests.Integration

**Testcontainers-based integration test project** for the Blazor Server UI using Playwright for browser automation and Reqnroll for BDD-style scenarios.

## Overview

This project provides a "zero-process" testing environment where the Blazor application is automatically built from its Dockerfile, deployed in a Docker container, and tested using BDD-style scenarios.

### Key Features

1. **Zero-Process Testing**: Application automatically built from Dockerfile and deployed in container - no manual setup
2. **Testcontainers Integration**: Uses official Testcontainers for .NET to manage full container lifecycle
3. **Browser Automation**: Microsoft Playwright for cross-browser testing (Chromium, Firefox, WebKit)
4. **BDD Framework**: Reqnroll (modern SpecFlow successor) with NUnit test runner
5. **Parallel Execution**: Tests run in parallel at Children level (4 concurrent scenarios)
6. **Test Environment**: Application runs in "Test" mode with isolated data (no external API calls)

## Technical Architecture

### Container Management

The `DockerHelper` class:
- Builds Docker image from `EstateManagementUI.BlazorServer/Dockerfile` using `ImageFromDockerfileBuilder`
- Starts container with Test environment configuration
- Manages lifecycle from setup to teardown
- Implements `IAsyncDisposable` for proper resource cleanup

```csharp
var imageBuilder = new ImageFromDockerfileBuilder()
    .WithDockerfileDirectory(repoRoot)
    .WithDockerfile("EstateManagementUI.BlazorServer/Dockerfile")
    .WithCleanUp(true);

var container = new ContainerBuilder()
    .WithImage(await imageBuilder.Build())
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Test")
    .WithEnvironment("AppSettings:TestMode", "true")
    .Build();
```

### Test Lifecycle (Hooks)

Reqnroll hooks manage the complete test lifecycle:

- **BeforeTestRun (Order 0)**: Start Docker containers
- **BeforeTestRun (Order 100)**: Initialize Playwright  
- **BeforeScenario**: Create browser page, register DockerHelper
- **AfterScenario**: Take screenshots on failure, close page
- **AfterTestRun (Order 0)**: Close Playwright
- **AfterTestRun (Order 100)**: Stop containers and cleanup

### Parallel Execution

Configured in `Properties/AssemblyInfo.cs`:

```csharp
[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]
```

## Project Structure

```
EstateManagementUI.BlazorServer.Tests.Integration/
├── Support/
│   ├── DockerHelper.cs          # Testcontainers container management
│   └── Hooks.cs                 # Reqnroll lifecycle hooks
├── Features/
│   └── Framework/
│       ├── FrameworkCheck.feature        # Smoke test
│       └── FrameworkCheckSteps.cs        # Step definitions
├── Properties/
│   └── AssemblyInfo.cs          # NUnit parallel configuration
└── EstateManagementUI.BlazorServer.Tests.Integration.csproj
```

## Running Tests

### Prerequisites

1. **.NET 10.0 SDK** or later
2. **Docker** (Docker Desktop on Windows/Mac, Docker Engine on Linux)

### Quick Start

```bash
# Navigate to the test project
cd EstateManagementUI.BlazorServer.Tests.Integration

# Run all tests (builds container automatically)
dotnet test

# Run only framework smoke tests
dotnet test --filter "Category=framework"

# Run with specific browser
Browser=Firefox dotnet test

# Run in headless mode (CI)
IsCI=true dotnet test
```

### Test Execution Flow

1. **Build Phase**: Testcontainers builds Docker image from Dockerfile
2. **Container Start**: Built image started with test-specific configuration
3. **Browser Setup**: Playwright initializes configured browser(s)
4. **Test Run**: Tests execute in parallel (up to 4 scenarios)
5. **Cleanup**: Containers stopped, browsers closed, Docker resources cleaned up

### Environment Variables

- **`Browser`**: Browser type (`Chrome`, `Firefox`, `WebKit`) - defaults to Chrome
- **`IsCI`**: Set to `true` for headless mode - defaults to false (headed mode)

## Framework Check Feature

The `FrameworkCheck.feature` validates the infrastructure:

```gherkin
@framework @smoke
Feature: Framework Check
Background:
    Given the application is running in a container

Scenario: Home page is accessible
    When I navigate to the home page
    Then the page title should be visible
```

This test validates:
- ✓ Docker image builds from Dockerfile
- ✓ Container starts with proper configuration
- ✓ Playwright connects to containerized application
- ✓ Basic navigation works

## Package Dependencies

- **Testcontainers** 4.2.0 - Official Testcontainers for .NET
- **Microsoft.Playwright** 1.49.0 - Browser automation
- **Reqnroll** 3.2.1 - BDD framework (SpecFlow successor)
- **NUnit** 4.4.0 - Test runner
- **Shouldly** 4.3.0 - Assertion library

## Troubleshooting

### Docker Build Fails

1. Check Docker is running: `docker ps`
2. Verify Dockerfile exists: `ls ../EstateManagementUI.BlazorServer/Dockerfile`
3. Ensure Node.js available in Docker build (required for Tailwind CSS)

### Container Won't Start

1. Check Docker logs: `docker logs <container-name>`
2. Verify port 5004 not already in use
3. Check Docker has sufficient resources

### Playwright Issues

1. Browsers installed automatically in BeforeTestRun
2. Check container accessible: `curl -k https://localhost:<port>`
3. Verify SSL certificate handling configured

## Future Expansion

The project structure supports adding more feature files:

```
Features/
├── Framework/          # Infrastructure tests
├── Authentication/     # Auth flow tests
├── Estates/           # Estate management tests
└── Reporting/         # Reporting feature tests
```

Each feature area can have its own `.feature` files and step definitions.
