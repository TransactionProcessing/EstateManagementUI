# EstateManagementUI

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