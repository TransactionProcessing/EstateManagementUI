using EstateManagementUI.Bootstrapper;
using Hydro.Configuration;
using Lamar;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
using Shared.General;
using Shared.Middleware;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

[ExcludeFromCodeCoverage]
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
        if (Startup.WebHostEnvironment.IsEnvironment("IntegrationTest") == false)
        {
            services.IncludeRegistry<AuthenticationRegistry>();
        }
        services.IncludeRegistry<ClientRegistry>();
        services.IncludeRegistry<MediatorRegistry>();

        Startup.Container = new Container(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                          ILoggerFactory loggerFactory) {

        ILogger logger = loggerFactory.CreateLogger("MessagingService");

        Shared.Logger.Logger.Initialise(logger);
        Startup.Configuration.LogConfiguration(Shared.Logger.Logger.LogWarning);

        // TODO: where should the logging be configured??

        //// Configure the HTTP request pipeline.
        //if (!env.IsDevelopment())
        //{
        //    app.UseExceptionHandler("/Error");
        //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //    app.UseHsts();
        //}
        //else
        //{
        //    app.UseDeveloperExceptionPage();

        //}

        //app.UseExceptionHandler(b => b.Run(async context =>
        //{
        //    if (!context.IsHydro())
        //    {
        //        return;
        //    }

        //    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //    switch (contextFeature?.Error)
        //    {
        //        // custom cases for custom exception types if needed

        //        default:
        //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        //            await context.Response.WriteAsJsonAsync(new UnhandledHydroError(
        //                Message: "There was a problem with this operation and it wasn't finished",
        //                Data: null
        //            ));

        //            return;
        //    }
        //}));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseMiddleware<TenantMiddleware>();
        app.AddRequestLogging();
        app.AddResponseLogging();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHydro(env);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.PreWarm().Wait();
    }
}