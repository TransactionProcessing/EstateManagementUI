﻿using System.ComponentModel;
using System.Security.Claims;

namespace EstateManagementUI.Common
{
    public static class Helpers {
        public const String EstateIdClaimType = "estateId";

        public static T GetClaimValue<T>(ClaimsIdentity claimsIdentity,
                                         String claimType) {
            if (!claimsIdentity.HasClaim(x => x.Type.ToLower() == claimType.ToLower())) {
                throw new InvalidOperationException($"User {claimsIdentity.Name} does not have Claim [{claimType}]");
            }

            Claim claim = claimsIdentity.Claims.Single(x => x.Type.ToLower() == claimType.ToLower());
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(claim.Value);
        }

        public static (String authorityAddress, String issuerAddress) GetSecurityServiceAddresses(String authority, String securityServiceLocalPort, String securityServicePort)
        {
            Console.WriteLine($"authority is {authority}");
            Console.WriteLine($"securityServiceLocalPort is {securityServiceLocalPort}");
            Console.WriteLine($"securityServicePort is {securityServicePort}");

            if (String.IsNullOrEmpty(securityServiceLocalPort))
            {
                securityServiceLocalPort = "5001";
            }

            if (String.IsNullOrEmpty(securityServicePort))
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
                authorityAddress = $"{authorityAddress.Substring(0, authorityAddress.Length - 1)}";
            }
            if (issuerAddress.EndsWith("/"))
            {
                issuerAddress = $"{issuerAddress.Substring(0, issuerAddress.Length - 1)}";
            }

            Console.WriteLine($"authorityAddress is {authorityAddress}");
            Console.WriteLine($"issuerAddress is {issuerAddress}");

            return (authorityAddress, issuerAddress);
        }
    }
}
