using System.Diagnostics.CodeAnalysis;
using Shared.General;

namespace EstateManagementUI.BusinessLogic.Common;

[ExcludeFromCodeCoverage]
public class ConfigurationService : IConfigurationService
{
    public Boolean GetPermissionsBypass()
    {
        return ConfigurationReader.GetValueOrDefault<Boolean>("AppSettings", "PermissionsBypass", false);
    }
}