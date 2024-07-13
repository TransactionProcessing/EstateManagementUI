using System.Net;
using System.Security.Cryptography.X509Certificates;
using EstateManagementUI.Bootstrapper;
using Hydro.Configuration;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.General;

var builder = WebApplication.CreateBuilder(args);

FileInfo fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(fi.Directory.FullName).AddJsonFile("hosting.json", optional: true)
    .AddJsonFile("hosting.development.json", optional: true).AddEnvironmentVariables().Build();

builder.Host.UseLamar((c,
                       r) => {
    r.AddRazorPages();
    r.AddHydro();
    r.IncludeRegistry<MiddlewareRegistry>();
    r.IncludeRegistry<AuthenticationRegistry>();
}).ConfigureAppConfiguration((hostingContext, configBuilder) => {
    
    configBuilder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("/home/txnproc/config/appsettings.json", true, true)
        .AddJsonFile($"/home/txnproc/config/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    ConfigurationReader.Initialise(configBuilder.Build());
});

builder.WebHost.UseConfiguration(config);
builder.WebHost.UseKestrel(options =>
    {
        var port = 5004;

        options.Listen(IPAddress.Any,
            port,
            listenOptions =>
            {
                try
                {
                    // Enable support for HTTP1 and HTTP2 (required if you want to host gRPC endpoints)
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
                    listenOptions.UseHttps(LoadCertificate(fi.Directory.FullName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else {
    app.UseDeveloperExceptionPage();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHydro(builder.Environment);

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();


static X509Certificate2 LoadCertificate(String path)
{
    //just to ensure that we are picking the right file! little bit of ugly code:
    var files = Directory.GetFiles(path);
    var certificateFile = files.First(name => name.Contains("pfx"));
    Console.WriteLine($"Certficate File: {certificateFile}");
    return new X509Certificate2(certificateFile, "password");
}