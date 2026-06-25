# NuGet Source Mapping Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make package restore work consistently with central package management by using one root NuGet configuration with package source mapping in local builds, Docker builds, and GitHub Actions workflows.

**Architecture:** Keep a single `NuGet.Config` at the repository root so every restore path can discover the same package sources, credentials, and source mapping. Update Docker and workflow restore commands to use that file explicitly, then remove the project-local config so there is no competing restore policy.

**Tech Stack:** .NET SDK restore/publish, NuGet package source mapping, GitHub Actions, Docker

---

### Task 1: Add root NuGet policy

**Files:**
- Create: `NuGet.Config`
- Delete: `EstateManagementUI.BlazorServer/NuGet.Config`

- [ ] **Step 1: Add the root NuGet config**

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="Feedz" value="https://f.feedz.io/transactionprocessing/nugets/nuget/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="Feedz">
      <package pattern="ClientProxyBase" />
      <package pattern="FileProcessor.Client" />
      <package pattern="MessagingService.IntegrationTesting.Helpers" />
      <package pattern="SecurityService.Client" />
      <package pattern="SecurityService.IntegrationTesting.Helpers" />
      <package pattern="Shared" />
      <package pattern="Shared.Results" />
      <package pattern="Shared.IntegrationTesting" />
      <package pattern="TransactionProcessor.Client" />
      <package pattern="TransactionProcessor.IntegrationTesting.Helpers" />
    </packageSource>
    <packageSource key="nuget.org">
      <package pattern="AspNetCore.HealthChecks.UI.Client" />
      <package pattern="AspNetCore.HealthChecks.Uris" />
      <package pattern="bunit.web" />
      <package pattern="coverlet.collector" />
      <package pattern="coverlet.msbuild" />
      <package pattern="IdentityModel" />
      <package pattern="Lamar" />
      <package pattern="Lamar.Microsoft.DependencyInjection" />
      <package pattern="MediatR" />
      <package pattern="Microsoft.AspNetCore.Authentication.OpenIdConnect" />
      <package pattern="Microsoft.CodeAnalysis.NetAnalyzers" />
      <package pattern="Microsoft.EntityFrameworkCore.Sqlite" />
      <package pattern="Microsoft.Extensions.Hosting.WindowsServices" />
      <package pattern="Microsoft.NET.Test.Sdk" />
      <package pattern="Microsoft.Playwright" />
      <package pattern="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" />
      <package pattern="Moq" />
      <package pattern="NUnit" />
      <package pattern="NUnit3TestAdapter" />
      <package pattern="Reqnroll" />
      <package pattern="Reqnroll.NUnit" />
      <package pattern="Reqnroll.Tools.MsBuild.Generation" />
      <package pattern="Roslynator.Analyzers" />
      <package pattern="Sentry.AspNetCore" />
      <package pattern="Shouldly" />
      <package pattern="SimpleResults" />
      <package pattern="xunit.v3" />
      <package pattern="xunit.runner.visualstudio" />
    </packageSource>
  </packageSourceMapping>
  <packageSourceCredentials>
    <Feedz>
      <add key="Username" value="TestUser" />
      <add key="ClearTextPassword" value="T-jXeJSPLLjOdZvRZOQ5O2e1vrQwRI7uxC1U" />
    </Feedz>
  </packageSourceCredentials>
  <packageRestore>
    <add key="enabled" value="True" />
    <add key="automatic" value="True" />
  </packageRestore>
  <bindingRedirects>
    <add key="skip" value="False" />
  </bindingRedirects>
  <packageManagement>
    <add key="format" value="0" />
    <add key="disabled" value="False" />
  </packageManagement>
</configuration>
```

- [ ] **Step 2: Remove the project-local config**

Delete `EstateManagementUI.BlazorServer/NuGet.Config` so the root config becomes the only active NuGet policy in the repo.

- [ ] **Step 3: Verify the root config is discoverable**

Run:

```powershell
dotnet restore EstateManagementUI.sln --configfile NuGet.Config
```

Expected: restore uses the root config and no longer warns about multiple sources without mapping.

### Task 2: Point Docker and CI at the root config

**Files:**
- Modify: `EstateManagementUI.BlazorServer/Dockerfile`
- Modify: `EstateManagementUI.BlazorServer/Dockerfile.original`
- Modify: `.github/workflows/pullrequest.yml`
- Modify: `.github/workflows/codecoverage.yml`
- Modify: `.github/workflows/nightlybuild.yml`
- Modify: `.github/workflows/createrelease.yml`

- [ ] **Step 1: Make Docker restore use the root config**

```dockerfile
COPY ["NuGet.Config", "."]
COPY ["EstateManagementUI.BlazorServer/Certificates/*.*", "Certificates/"]
COPY ["EstateManagementUI.BlazorServer/EstateManagementUI.BlazorServer.csproj", "EstateManagementUI.BlazorServer/"]
COPY ["Directory.Packages.props", "."]
RUN dotnet restore "./EstateManagementUI.BlazorServer/EstateManagementUI.BlazorServer.csproj" --configfile NuGet.Config
```

- [ ] **Step 2: Make CI restore use the same config**

Use the repository root config explicitly in every restore step:

```yaml
run: dotnet restore EstateManagementUI.sln --configfile NuGet.Config
```

Remove any `--source ...` arguments from those restore steps so they do not bypass the mapped source policy.

- [ ] **Step 3: Re-run publish/build commands with no extra restore**

Keep the existing `--no-restore` build steps, and add `--no-restore` to any `dotnet publish` step that follows a restore in the same job.

- [ ] **Step 4: Verify in CI syntax and locally**

Run:

```powershell
dotnet restore EstateManagementUI.sln --configfile NuGet.Config
dotnet build EstateManagementUI.sln --configuration Release --no-restore
```

Expected: restore completes with the mapped config, and build reuses the restored assets.
