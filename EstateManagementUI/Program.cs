using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using EstateManagementUI.Bootstrapper;
using Hydro.Configuration;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using Shared.Extensions;
using Shared.General;

public class Startup {
    private static IWebHostEnvironment WebHostEnvironment;
    public static Container Container;

    public static IConfigurationRoot Configuration { get; set; }

    public Startup(IWebHostEnvironment webHostEnvironment)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
            .AddJsonFile("/home/txnproc/config/appsettings.json", true, true)
            .AddJsonFile($"/home/txnproc/config/appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Startup.Configuration = builder.Build();
        Startup.WebHostEnvironment = webHostEnvironment;
        
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureContainer(ServiceRegistry services)
    {
        ConfigurationReader.Initialise(Startup.Configuration);

        services.AddHttpContextAccessor();
        
        services.IncludeRegistry<MiddlewareRegistry>();
        if (Startup.WebHostEnvironment.IsEnvironment("IntegrationTest") == false){
            services.IncludeRegistry<AuthenticationRegistry>();
        }
        services.IncludeRegistry<ClientRegistry>();
        services.IncludeRegistry<MediatorRegistry>();

        Startup.Container = new Container(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                          ILoggerFactory loggerFactory) {

        // TODO: where should the logging be configured??

        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();

        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHydro(env);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}


public class Program {

    public static void Main(String[] args)
    {
        Program.CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(String[] args) {
        FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(fi.Directory.FullName)
            .AddJsonFile("hosting.json", optional: true).AddJsonFile("hosting.development.json", optional: true)
            .AddEnvironmentVariables().Build();

        IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.UseWindowsService();
        hostBuilder.UseLamar();
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
                services.AddRazorPages();
                services.AddHydro();
            });

            webBuilder.UseConfiguration(config);
            webBuilder.ConfigureLogging((context,
                                         loggingBuilder) => {
                // NLog: Setup NLog for Dependency injection

                //loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); // TODO: Config

                String nlogConfigFilename = "nlog.config";

                if (context.HostingEnvironment.IsDevelopment()) {
                    var developmentNlogConfigFilename = "nlog.development.config";
                    if (File.Exists(Path.Combine(context.HostingEnvironment.ContentRootPath,
                            developmentNlogConfigFilename))) {
                        nlogConfigFilename = developmentNlogConfigFilename;
                    }
                }

                NLog.LogManager.Setup().LoadConfigurationFromFile(nlogConfigFilename);
                loggingBuilder.AddNLog();

                var loggerFactory = new NLog.Extensions.Logging.NLogLoggerFactory();
                var l = loggerFactory.CreateLogger("");
                Shared.Logger.Logger.Initialise(l);
                //Startup.Configuration.LogConfiguration(Shared.Logger.Logger.LogWarning);
            });
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
        var files = Directory.GetFiles($"{path}\\Certificates");
        var certificateFile = files.First(name => name.Contains("pfx"));
        Console.WriteLine($"Certficate File: {certificateFile}");
        
        var x509 = new X509Certificate2(certificateFile, "password");
        
        return x509;
    }
}
