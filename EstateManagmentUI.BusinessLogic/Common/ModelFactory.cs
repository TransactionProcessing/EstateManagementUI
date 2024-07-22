using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagementUI.BusinessLogic.Common;

public static class ModelFactory
{
    public static EstateModel ConvertFrom(EstateResponse source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        EstateModel model = new EstateModel
        {
            EstateId = source.EstateId,
            EstateName = source.EstateName,
            Operators = ConvertOperators(source.Operators),
            SecurityUsers = ConvertSecurityUsers(source.SecurityUsers)
        };
        return model;
    }

    public static List<EstateOperatorModel> ConvertOperators(List<EstateOperatorResponse> estateResponseOperators)
    {
        if (estateResponseOperators == null || estateResponseOperators.Any() == false)
        {
            return null;
        }

        List<EstateOperatorModel> models = new List<EstateOperatorModel>();
        foreach (EstateOperatorResponse estateOperatorResponse in estateResponseOperators)
        {
            models.Add(new EstateOperatorModel
            {
                Name = estateOperatorResponse.Name,
                OperatorId = estateOperatorResponse.OperatorId,
                RequireCustomMerchantNumber = estateOperatorResponse.RequireCustomMerchantNumber,
                RequireCustomTerminalNumber = estateOperatorResponse.RequireCustomTerminalNumber
            });
        }

        return models;
    }

    public static List<SecurityUserModel> ConvertSecurityUsers(List<SecurityUserResponse> estateResponseSecurityUsers)
    {
        if (estateResponseSecurityUsers == null || estateResponseSecurityUsers.Any() == false)
        {
            return null;
        }

        List<SecurityUserModel> models = new List<SecurityUserModel>();
        foreach (SecurityUserResponse estateResponseSecurityUser in estateResponseSecurityUsers)
        {
            models.Add(new SecurityUserModel
            {
                EmailAddress = estateResponseSecurityUser.EmailAddress,
                SecurityUserId = estateResponseSecurityUser.SecurityUserId
            });
        }

        return models;
    }
}