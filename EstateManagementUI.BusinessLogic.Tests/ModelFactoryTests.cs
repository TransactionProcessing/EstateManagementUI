using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateManagementUI.BusinessLogic.Models;
using Shouldly;
using EstateManagementUI.BusinessLogic.Common;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_EmptyOperators_ModelIsConverted()
        {
            EstateResponse response = TestData.EstateResponse;
            response.Operators = new List<EstateOperatorResponse>();

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.ShouldBeNull();
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_EmptySecurityUsers_ModelIsConverted()
        {
            EstateResponse response = TestData.EstateResponse;
            response.SecurityUsers = new List<SecurityUserResponse>();

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            model.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_ModelIsConverted()
        {
            EstateResponse response = TestData.EstateResponse;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullOperators_ModelIsConverted()
        {
            EstateResponse response = TestData.EstateResponse;
            response.Operators = null;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.ShouldBeNull();
            response.SecurityUsers.ForEach(u => {
                SecurityUserModel securityUserModel = model.SecurityUsers.SingleOrDefault(su => su.SecurityUserId == u.SecurityUserId);
                securityUserModel.ShouldNotBeNull();
                securityUserModel.EmailAddress.ShouldBe(u.EmailAddress);
            });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullResponse_ErrorThrown()
        {
            EstateResponse response = null;

            Should.Throw<ArgumentNullException>(() => { ModelFactory.ConvertFrom(response); });
        }

        [Fact]
        public void ModelFactory_ConvertFrom_EstateResponse_NullSecurityUsers_ModelIsConverted()
        {
            EstateResponse response = TestData.EstateResponse;
            response.SecurityUsers = null;

            EstateModel model = ModelFactory.ConvertFrom(response);

            model.EstateName.ShouldBe(response.EstateName);
            model.EstateId.ShouldBe(response.EstateId);
            model.Operators.Count.ShouldBe(response.Operators.Count);
            response.Operators.ForEach(o => {
                EstateOperatorModel operatorModel = model.Operators.SingleOrDefault(mo => mo.OperatorId == o.OperatorId);
                operatorModel.ShouldNotBeNull();
                operatorModel.RequireCustomMerchantNumber.ShouldBe(o.RequireCustomMerchantNumber);
                operatorModel.RequireCustomTerminalNumber.ShouldBe(o.RequireCustomTerminalNumber);
                operatorModel.Name.ShouldBe(o.Name);
            });
            model.SecurityUsers.ShouldBeNull();
        }
    }
}
