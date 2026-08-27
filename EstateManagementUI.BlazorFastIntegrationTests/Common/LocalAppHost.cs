using System.Diagnostics;
using System.Net;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class LocalAppHost : IAsyncDisposable
{
    private readonly string _projectPath;
    private Process? _process;
    private readonly HttpClient _httpClient;
    private readonly List<string> _standardOutput = new();
    private readonly List<string> _standardError = new();

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
            WorkingDirectory = Path.GetDirectoryName(_projectPath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--no-build");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(_projectPath);

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Test";
        startInfo.Environment["AppSettings__TestMode"] = "BackedByTestDataStore";
        startInfo.Environment["ASPNETCORE_URLS"] = BaseUri.ToString();

        void WriteProcessLine(object? _, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine($"[app host] {e.Data}");
                lock (_standardOutput)
                {
                    _standardOutput.Add(e.Data);
                }
            }
        }

        void WriteProcessError(object? _, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine($"[app host:err] {e.Data}");
                lock (_standardError)
                {
                    _standardError.Add(e.Data);
                }
            }
        }

        _process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start the Blazor app host.");
        _process.OutputDataReceived += WriteProcessLine;
        _process.ErrorDataReceived += WriteProcessError;
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
            if (_process is not null && _process.HasExited)
            {
                throw new InvalidOperationException(BuildStartupFailureMessage("The Blazor app exited before it became healthy."));
            }

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

        throw new TimeoutException(BuildStartupFailureMessage("The Blazor app did not become healthy in time."));
    }

    private string BuildStartupFailureMessage(string headline)
    {
        var exitCode = _process?.HasExited == true ? _process.ExitCode.ToString() : "still running";
        var stdout = string.Join(Environment.NewLine, GetTail(_standardOutput, 40));
        var stderr = string.Join(Environment.NewLine, GetTail(_standardError, 40));

        return $"{headline}{Environment.NewLine}" +
               $"Process state: {exitCode}{Environment.NewLine}" +
               $"Recent stdout:{Environment.NewLine}{stdout}{Environment.NewLine}" +
               $"Recent stderr:{Environment.NewLine}{stderr}";
    }

    private static IEnumerable<string> GetTail(List<string> lines, int maxLines)
    {
        if (lines.Count <= maxLines)
        {
            return lines.ToArray();
        }

        return lines.Skip(lines.Count - maxLines).ToArray();
    }
}
