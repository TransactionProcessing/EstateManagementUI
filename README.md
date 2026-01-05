# EstateManagementUI

## Overview

This repository contains the Estate Management User Interface applications and associated test projects.

### UI Projects

- **EstateManagementUI**: Legacy ASP.NET Core web application with Razor Pages (Hydro)
- **EstateManagementUI.BlazorServer**: Modern Blazor Server application (current/future UI)

### Test Projects

- **EstateManagementUI.IntegrationTests**: Integration tests for the legacy UI using Selenium WebDriver
- **EstateManagementUI.BlazorIntegrationTests**: Integration tests for the Blazor UI using Playwright (NEW!)
- **EstateManagementUI.UITests**: Unit tests for UI components
- **EstateManagementUI.BusinessLogic.Tests**: Unit tests for business logic

## Integration Testing

### Blazor Integration Tests (NEW)

The `EstateManagementUI.BlazorIntegrationTests` project provides comprehensive integration tests for the Blazor Server UI using **Microsoft Playwright** for browser automation.

**Key Features:**
- Targets the Blazor Server UI (`EstateManagementUI.BlazorServer`)
- Uses Playwright for modern, reliable browser automation
- Full Docker infrastructure support for testing
- BDD-style tests using Reqnroll (SpecFlow successor)

See [EstateManagementUI.BlazorIntegrationTests/README.md](EstateManagementUI.BlazorIntegrationTests/README.md) for detailed documentation.

### Legacy Integration Tests

The `EstateManagementUI.IntegrationTests` project provides integration tests for the legacy UI using Selenium WebDriver. These tests target the old ASP.NET Core application.

## Building the Blazor Server Application

The `EstateManagementUI.BlazorServer` project uses Tailwind CSS for styling. The CSS must be built before running the application.

### Prerequisites
- .NET 10.0 SDK
- Node.js and npm

### Building the Project

The Tailwind CSS is automatically built when you build the project:

```bash
cd EstateManagementUI.BlazorServer
dotnet build
```

This will:
1. Install npm dependencies (if not already installed)
2. Build Tailwind CSS from `Styles/app.css` to `wwwroot/css/app.css`
3. Build the .NET project

### Running the Application

```bash
cd EstateManagementUI.BlazorServer
dotnet run
```

The application will be available at `https://localhost:5004` (or the configured port).

### Development Workflow

For active development with auto-rebuild of CSS:

```bash
cd EstateManagementUI.BlazorServer
npm run css:watch
```

This will watch for changes to Tailwind CSS source files and automatically rebuild the CSS.

## Solution Structure

```
EstateManagementUI/
├── EstateManagementUI/                    # Legacy ASP.NET Core UI
├── EstateManagementUI.BlazorServer/       # Modern Blazor Server UI
├── EstateManagementUI.BusinessLogic/      # Shared business logic
├── EstateManagementUI.ViewModels/         # Shared view models
├── EstateManagementUI.Testing/            # Shared testing utilities
├── EstateManagementUI.IntegrationTests/   # Legacy UI integration tests (Selenium)
├── EstateManagementUI.BlazorIntegrationTests/  # Blazor UI integration tests (Playwright)
├── EstateManagementUI.UITests/            # UI component unit tests
└── EstateManagementUI.BusinessLogic.Tests/# Business logic unit tests
```