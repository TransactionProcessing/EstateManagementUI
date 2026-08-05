using System.Diagnostics;
using System.Net;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class LocalAppHost : IAsyncDisposable
{
    private readonly string _projectPath;
    private Process? _process;
    private readonly HttpClient _httpClient;

    public LocalAppHost(string projectPath)
    {
        _projectPath = projectPath;
        _httpClient = new HttpClient();
    }

    public Uri BaseUri { get; } = new("http://127.0.0.1:5004");

    public async Task StartAsync()
    {
        if (_process is not null && _process.HasExited == false)
        {
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --no-build --project \"{_projectPath}\"",
            WorkingDirectory = Path.GetDirectoryName(_projectPath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Test";
        startInfo.Environment["AppSettings__TestMode"] = "BackedByTestDataStore";
        startInfo.Environment["ASPNETCORE_URLS"] = BaseUri.ToString();

        void WriteProcessLine(object? _, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine($"[app host] {e.Data}");
            }
        }

        _process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start the Blazor app host.");
        _process.OutputDataReceived += WriteProcessLine;
        _process.ErrorDataReceived += WriteProcessLine;
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        await WaitForHealthAsync();
    }

    public async Task ResetAsync()
    {
        await _httpClient.PostAsync(new Uri(BaseUri, "/test-support/reset"), new StringContent(string.Empty));
    }

    public async Task StopAsync()
    {
        if (_process is null)
        {
            return;
        }

        try
        {
            if (_process.HasExited == false)
            {
                _process.Kill(entireProcessTree: true);
                await _process.WaitForExitAsync();
            }
        }
        catch
        {
            // Best effort cleanup.
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        _process?.Dispose();
        _httpClient.Dispose();
    }

    private async Task WaitForHealthAsync()
    {
        var deadline = DateTime.UtcNow.AddSeconds(120);
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                using var response = await _httpClient.GetAsync(new Uri(BaseUri, "/test-support/ping"));
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
                // keep waiting
            }

            await Task.Delay(1000);
        }

        throw new TimeoutException("The Blazor app did not become healthy in time.");
    }
}
