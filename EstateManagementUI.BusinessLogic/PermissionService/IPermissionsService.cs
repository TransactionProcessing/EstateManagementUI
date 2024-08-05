using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients
{
    public interface IPermissionsService {
        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName,
                                        String function);

        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName);

        Task<Result> LoadPermissionsData();

    }
}
