FROM stuartferguson/txnprocbase AS base
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install Node.js for Tailwind CSS build
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs

COPY ["EstateManagementUI.BlazorServer/NuGet.Config", "."]
COPY ["EstateManagementUI.BlazorServer/Certificates/*.*", "Certificates/"]
COPY ["EstateManagementUI.BlazorServer/EstateManagementUI.BlazorServer.csproj", "EstateManagementUI.BlazorServer/"]
RUN dotnet restore "./EstateManagementUI.BlazorServer/EstateManagementUI.BlazorServer.csproj"

COPY . .
WORKDIR "/src/EstateManagementUI.BlazorServer"

# Install npm packages and build Tailwind CSS
RUN npm install
RUN npm run css:build

RUN dotnet build "./EstateManagementUI.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EstateManagementUI.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstateManagementUI.BlazorServer.dll"]
