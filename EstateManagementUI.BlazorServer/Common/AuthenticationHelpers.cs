using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BlazorServer.Common;

public static class AuthenticationHelpers
{
    /// <summary>
    /// Gets the security service addresses for authentication configuration.
    /// This method allows for port overrides to support integration testing scenarios
    /// where the security service may be running on different ports.
    /// </summary>
    /// <param name="authority">The base authority URL</param>
    /// <param name="securityServiceLocalPort">Optional local port override (defaults to 5001)</param>
    /// <param name="securityServicePort">Optional port override (defaults to 5001)</param>
    /// <returns>Tuple containing the authority address and issuer address</returns>
    [ExcludeFromCodeCoverage]
    public static (string authorityAddress, string issuerAddress) GetSecurityServiceAddresses(
        string authority, 
        string? securityServiceLocalPort, 
        string? securityServicePort)
    {
        Console.WriteLine($"authority is {authority}");
        Console.WriteLine($"securityServiceLocalPort is {securityServiceLocalPort}");
        Console.WriteLine($"securityServicePort is {securityServicePort}");

        if (string.IsNullOrEmpty(securityServiceLocalPort))
        {
            securityServiceLocalPort = "5001";
        }

        if (string.IsNullOrEmpty(securityServicePort))
        {
            securityServicePort = "5001";
        }

        Uri u = new Uri(authority);

        var authorityAddress = u.Port switch
        {
            _ when u.Port.ToString() != securityServiceLocalPort => $"{u.Scheme}://{u.Host}:{securityServiceLocalPort}{u.AbsolutePath}",
            _ => authority
        };

        var issuerAddress = u.Port switch
        {
            _ when u.Port.ToString() != securityServicePort => $"{u.Scheme}://{u.Host}:{securityServicePort}{u.AbsolutePath}",
            _ => authority
        };

        if (authorityAddress.EndsWith("/"))
        {
            authorityAddress = authorityAddress.Substring(0, authorityAddress.Length - 1);
        }
        if (issuerAddress.EndsWith("/"))
        {
            issuerAddress = issuerAddress.Substring(0, issuerAddress.Length - 1);
        }

        Console.WriteLine($"authorityAddress is {authorityAddress}");
        Console.WriteLine($"issuerAddress is {issuerAddress}");

        return (authorityAddress, issuerAddress);
    }
}
