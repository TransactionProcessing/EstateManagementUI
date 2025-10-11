using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Database;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Hydro;
using Hydro.Configuration;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using Shared.Extensions;
using Shared.General;
using Shared.Logger;
using Shared.Middleware;
using SimpleResults;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Logger = Shared.Logger.Logger;


[ExcludeFromCodeCoverage]
public class Program {

    public static async Task Main(String[] args) {

        //string dbConnString = "Data Source=C:\\temp\\permissions.db";
        //var permissionsRepository = await CreatePermissionsRepository(dbConnString, CancellationToken.None);
        //await permissionsRepository.MigrateDatabase(CancellationToken.None);
        //await permissionsRepository.SeedDatabase(CancellationToken.None);
        //var addRoleResult = await permissionsRepository.AddRole("Administrator", CancellationToken.None);
        //if (addRoleResult.IsFailed)
        //{
        //    // TODO: Error
        //}
        //var permissionsList = await permissionsRepository.GetRolePermissions(addRoleResult.Data, CancellationToken.None);
        //List<ApplicationSection> applicationSections = permissionsList.Data.Select(p => p.Item1).Distinct().ToList();
        //await permissionsRepository.AddUserToRole(addRoleResult.Data, "estateuser@testestate1.co.uk", CancellationToken.None);

        //// Build up the role permissions
        //List<(int, string, int, string, bool)> Permissions = new();
        //foreach (ApplicationSection applicationSection in applicationSections)
        //{
        //    List<(Function, bool)> functionAccess = permissionsList.Data.Where(p =>
        //        p.Item1.ApplicationSectionId == applicationSection.ApplicationSectionId).Select(x => (x.Item2, x.Item3)).ToList();

        //    foreach ((Function, bool) function in functionAccess)
        //    {
        //        Permissions.Add((applicationSection.ApplicationSectionId, applicationSection.Name, function.Item1.FunctionId, function.Item1.Name, function.Item2));
        //    }
        //}

        ////List<(int, string, int, string, bool)> newPermissions = new List<(Int32, String, Int32, String, Boolean)>();

        ////foreach ((Int32, String, Int32, String, Boolean) permission in Permissions) {
        ////    newPermissions.Add(new() {
        ////        Item1 = permission.Item1,
        ////        Item2 = permission.Item2,
        ////        Item3 = permission.Item3,
        ////        Item4 = permission.Item4,
        ////        Item5 = true
        ////    });
        ////}

        //List<(int, int, bool)> newPermissions = Permissions.Select(p => (p.Item1, p.Item3, true)).ToList();

        //await permissionsRepository.UpdateRolePermissions(addRoleResult.Data, newPermissions, CancellationToken.None);

        Program.CreateHostBuilder(args).Build().Run();
    }

    private static IPermissionsRepository CreatePermissionsRepository(String dbConnString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PermissionsContext>();
        optionsBuilder.UseSqlite(dbConnString); // Configure for your database provider

        var serviceProvider = new ServiceCollection()
            .AddLogging(config => config.AddConsole())  // Add logging if needed
            .BuildServiceProvider();

        // Create the DbContextFactory instance
        var contextFactory = new DbContextFactory<PermissionsContext>(serviceProvider, optionsBuilder.Options, new DbContextFactorySource<PermissionsContext>());
        return new PermissionsRepository(contextFactory);
    }


    public static IHostBuilder CreateHostBuilder(String[] args) {
        FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(fi.Directory.FullName)
            .AddJsonFile("hosting.json", optional: true).AddJsonFile("hosting.development.json", optional: true)
            .AddEnvironmentVariables().Build();

        String contentRoot = Directory.GetCurrentDirectory();
        String nlogConfigPath = Path.Combine(contentRoot, "nlog.config");

        LogManager.Setup(b =>
        {
            b.SetupLogFactory(setup =>
            {
                setup.AddCallSiteHiddenAssembly(typeof(NlogLogger).Assembly);
                setup.AddCallSiteHiddenAssembly(typeof(Shared.Logger.Logger).Assembly);
                setup.AddCallSiteHiddenAssembly(typeof(TenantMiddleware).Assembly);
            });
            b.LoadConfigurationFromFile(nlogConfigPath);
        });

        IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.UseWindowsService();
        hostBuilder.UseLamar();
        hostBuilder.ConfigureLogging((context,
                                     loggingBuilder) => {
            loggingBuilder.AddNLog();
        });
        hostBuilder.ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();

            webBuilder.ConfigureServices(services =>
            {
                // This is important, the call to AddControllers()
                // cannot be made before the usage of ConfigureWebHostDefaults
                //services.AddNewtonsoftJson(options =>
                //{
                //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //    options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                //    options.SerializerSettings.Formatting = Formatting.Indented;
                //    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //});
                services.AddControllers();
                services.AddRazorPages();
                services.AddHydro();
            });

            webBuilder.UseConfiguration(config);
            
            webBuilder.UseKestrel(options =>
            {
                var port = 5004;
                
                options.Listen(IPAddress.Any, port, listenOptions =>
                {
                    try
                    {
                        // Enable support for HTTP1 and HTTP2 (required if you want to host gRPC endpoints)
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                        // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
                        listenOptions.UseHttps(Program.LoadCertificate(fi.Directory.FullName));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                });
            });
        });
        return hostBuilder;
    }

    private static X509Certificate2 LoadCertificate(String path) {
        //just to ensure that we are picking the right file! little bit of ugly code:
        var files = Directory.GetFiles($"{path}/Certificates");

        var certificateFile = files.First(name => name.Contains("pfx"));
        Console.WriteLine($"Certficate File: {certificateFile}");

        var x509 = new X509Certificate2(certificateFile, "password");
        return x509;
    }
}

[ExcludeFromCodeCoverage]
public static class Extensions {
    public static async Task PreWarm(this IApplicationBuilder applicationBuilder) {
        IPermissionsRepository permissionsRepository = Startup.Container.GetService<IPermissionsRepository>();
        Shared.Logger.Logger.LogWarning($"Before Migrate and Seed");
        Shared.Logger.Logger.LogWarning($"About to Migrate");
        await permissionsRepository.MigrateDatabase(CancellationToken.None);

        Shared.Logger.Logger.LogWarning($"About to Seed");
        // TODO: dont do this if data already present...
        await permissionsRepository.SeedDatabase(CancellationToken.None);
        
        try {
            var tables = await permissionsRepository.GetTableList(CancellationToken.None);
            Logger.LogWarning($"About  to log out all tables");
            foreach (String table in tables) {
                Logger.LogWarning($"{table}");
            }
            Logger.LogWarning($"About  to log out all permissions");
            // Log out all the permissions
            var rolesResult = await permissionsRepository.GetRoles(CancellationToken.None);
            if (rolesResult.IsFailed) {
                Logger.LogWarning($"Failed getting roles {rolesResult.Message}");
            }

            foreach (Role role in rolesResult.Data) {
                Logger.LogWarning($"Get users for Role {role.Name}");
                Result<List<UserRole>> roleUsersResult =
                    await permissionsRepository.GetRoleUsers(role.RoleId, CancellationToken.None);
                if (roleUsersResult.IsFailed) {
                    Logger.LogWarning($"Failed getting users for role {role.Name} {roleUsersResult.Message}");
                }

                Logger.LogWarning($"Get permissions for Role {role.Name}");
                var permissionsResult =
                    await permissionsRepository.GetRolePermissions(role.RoleId, CancellationToken.None);
                if (permissionsResult.IsFailed) {
                    Logger.LogWarning($"Failed getting permisisons for role {role.Name} {permissionsResult.Message}");
                }

                foreach ((ApplicationSection appSection, Function function, Boolean hasAccess) valueTuple in
                         permissionsResult.Data) {
                    Logger.LogWarning(
                        $"Application Section [{valueTuple.appSection.Name}] Function [{valueTuple.function.Name}] Has Access [{valueTuple.hasAccess}]");
                }
            }
        }
        catch (Exception ex) {
            Logger.LogWarning($"Error logging out permissions");
            Logger.LogError(ex);
        }
    }
}
