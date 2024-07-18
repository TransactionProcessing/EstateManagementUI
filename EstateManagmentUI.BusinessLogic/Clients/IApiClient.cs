using EstateManagmentUI.BusinessLogic.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses.Estate;
using Shared.Logger;
using SimpleResults;

namespace EstateManagmentUI.BusinessLogic.Clients
{
    public interface IApiClient
    {
        Task<EstateModel> GetEstate(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CancellationToken cancellationToken);
    }

    public class ApiClient : IApiClient {
        private readonly IEstateClient EstateClient;

        public ApiClient(IEstateClient estateClient) {
            this.EstateClient = estateClient;
        }
        
        public async Task<EstateModel> GetEstate(String accessToken,
                                           Guid actionId,
                                           Guid estateId,
                                           CancellationToken cancellationToken) {
            async Task<Result<EstateModel>> ClientMethod()
            {
                EstateResponse? estate = await this.EstateClient.GetEstate(accessToken, estateId, cancellationToken);

                return ModelFactory.ConvertFrom(estate);
            }

            return await this.CallClientMethod(ClientMethod, cancellationToken);
        }

        private async Task<Result<T>> CallClientMethod<T>(Func<Task<Result<T>>> clientMethod, CancellationToken cancellationToken)
        {
            try
            {
                Result<T> clientResult = await clientMethod();
                return Result.Success(clientResult);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                return Result.Failure(e.Message);

            }
        }
    }


    public static class ModelFactory {
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


}
