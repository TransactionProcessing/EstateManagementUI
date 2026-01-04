#!/bin/bash

# Setup script for Estate Management UI Playwright Tests
# This script helps set up the test environment

set -e

echo "================================================"
echo "Estate Management UI - Playwright Tests Setup"
echo "================================================"
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found. Please install .NET 10.0 SDK or later."
    exit 1
fi

echo "✓ .NET SDK found: $(dotnet --version)"
echo ""

# Check if Docker is installed and running
if ! command -v docker &> /dev/null; then
    echo "ERROR: Docker not found. Please install Docker."
    exit 1
fi

if ! docker ps &> /dev/null; then
    echo "ERROR: Docker daemon is not running. Please start Docker."
    exit 1
fi

echo "✓ Docker found and running"
echo ""

# Check for required Docker images
echo "Checking for required Docker images..."

if docker images | grep -q "securityservice.*latest"; then
    echo "✓ securityservice:latest found"
else
    echo "✗ securityservice:latest NOT found"
    echo "  You need to build this image from the existing integration tests."
    MISSING_IMAGES=true
fi

if docker images | grep -q "estatemanagementui.*latest"; then
    echo "✓ estatemanagementui:latest found"
else
    echo "✗ estatemanagementui:latest NOT found"
    echo "  You can build this image by running:"
    echo "  docker build -t estatemanagementui:latest -f EstateManagementUI.BlazorServer/Dockerfile ."
    MISSING_IMAGES=true
fi

if [ "$MISSING_IMAGES" = true ]; then
    echo ""
    echo "WARNING: Some required Docker images are missing."
    echo "Tests will fail without these images."
    echo "Please build the required images before running tests."
    echo ""
fi

# Restore NuGet packages
echo "Restoring NuGet packages..."
dotnet restore EstateManagementUI.PlaywrightTests/EstateManagementUI.PlaywrightTests.csproj
echo ""

# Build the test project
echo "Building test project..."
dotnet build EstateManagementUI.PlaywrightTests/EstateManagementUI.PlaywrightTests.csproj
echo ""

# Install Playwright browsers
echo "Installing Playwright browsers..."
if command -v pwsh &> /dev/null; then
    pwsh -c "playwright install chromium"
elif command -v playwright &> /dev/null; then
    playwright install chromium
else
    echo "ERROR: playwright command not found."
    echo "After building, you can install it with:"
    echo "  dotnet tool install --global Microsoft.Playwright.CLI"
    echo "  playwright install chromium"
    exit 1
fi
echo ""

echo "================================================"
echo "Setup Complete!"
echo "================================================"
echo ""
echo "To run the tests:"
echo "  cd EstateManagementUI.PlaywrightTests"
echo "  dotnet test"
echo ""
echo "To run tests in verbose mode:"
echo "  dotnet test --logger \"console;verbosity=detailed\""
echo ""
