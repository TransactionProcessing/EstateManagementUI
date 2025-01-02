using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Azure;
using Hydro;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;
using Shared.Logger;

namespace EstateManagementUI.Pages.PermissionManagement.Components
{
    [ExcludeFromCodeCoverage]
    public class EnterUserCode : HydroComponent
    {
        public String UserCode { get; set; }
        
        public EnterUserCode()
        {
        }

        public void Submit() {
            var permissionKey = ConfigurationReader.GetValue("AppSettings", "PermissionKey");
            var expectedValue = ConfigurationReader.GetValue("AppSettings", "PermissionHash");
            
            string hashedMessage = ComputeHMACSHA256Hash(this.UserCode, permissionKey);

            if (hashedMessage != expectedValue) {
                // return an error...
                Location(Url.Page("/Index"));
                return;
            }

            // Need a way to record the user has been authorised but also this must expire
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                Secure = true
            };
            var permissionCookieValue = ConfigurationReader.GetValue("AppSettings", "PermissionCookieValue");
            
            this.HttpContext.Response.Cookies.Append("PermissionManagementCookie", permissionCookieValue, cookieOptions);
            
            Location(Url.Page("/PermissionManagement/Roles/Index"));
        }
        
        public static string ComputeHMACSHA256Hash(string message, string key)
        {
            // Convert key and message to byte arrays
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // Compute the HMACSHA256 hash
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);

                // Convert the hash to a hex string
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.AppendFormat("{0:x2}", b);
                }
                return hashString.ToString();
            }
        }
    }


}
