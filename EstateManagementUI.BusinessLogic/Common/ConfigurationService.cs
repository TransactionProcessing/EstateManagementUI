using Shared.General;

namespace EstateManagementUI.BusinessLogic.Common;

public class ConfigurationService : IConfigurationService
{
    public Boolean GetPermissionsBypass()
    {
        return ConfigurationReader.GetValueOrDefault<Boolean>("AppSettings", "PermissionsBypass", false);
    }
}