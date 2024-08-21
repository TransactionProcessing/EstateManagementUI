﻿using System.Runtime.CompilerServices;
using Azure;
using EstateManagement.DataTransferObjects.Responses.Contract;
using EstateManagement.DataTransferObjects.Responses.Estate;
using EstateManagement.DataTransferObjects.Responses.Merchant;
using EstateManagement.DataTransferObjects.Responses.Operator;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using SimpleResults;

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

    public static List<MerchantModel> ConvertFrom(List<MerchantResponse> source) {
        if (source == null || source.Any() == false) {
            return new List<MerchantModel>();
        }

        List<MerchantModel> models = new List<MerchantModel>();
        foreach (MerchantResponse merchantResponse in source) {
            MerchantModel model = new MerchantModel {
                MerchantId = merchantResponse.MerchantId, 
                MerchantName = merchantResponse.MerchantName,
                MerchantReference = merchantResponse.MerchantReference,
                SettlementSchedule = merchantResponse.SettlementSchedule.ToString(),
                AddressLine1 = merchantResponse.Addresses.FirstOrDefault().AddressLine1,
                ContactName = merchantResponse.Contacts.FirstOrDefault().ContactName,
                Town = merchantResponse.Addresses.FirstOrDefault().Town
            };
            models.Add(model);
        }
        
        return models;
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

    public static List<OperatorModel> ConvertFrom(List<OperatorResponse> operators) {
        if (operators == null || operators.Any() == false)
        {
            return new List<OperatorModel>();
        }

        List<OperatorModel> models = new List<OperatorModel>();
        foreach (OperatorResponse @operator in operators)
        {
            models.Add(ConvertFrom(@operator));
        }

        return models;
    }

    public static List<ContractModel> ConvertFrom(List<ContractResponse> contracts)
    {
        if (contracts == null || contracts.Any() == false)
        {
            return new List<ContractModel>();
        }

        List<ContractModel> models = new List<ContractModel>();
        foreach (ContractResponse contract in contracts)
        {
            models.Add(new ContractModel
            {
                Description = contract.Description,
                NumberOfProducts = contract.Products.Count(),
                OperatorName = contract.OperatorName,
                ContractId = contract.ContractId
            });
        }

        return models;
    }

    public static OperatorModel ConvertFrom(OperatorResponse @operator) {

        if (@operator == null)
            return null;

        OperatorModel model = new OperatorModel {
            Name = @operator.Name,
            RequireCustomTerminalNumber = @operator.RequireCustomTerminalNumber,
            RequireCustomMerchantNumber = @operator.RequireCustomMerchantNumber,
            OperatorId = @operator.OperatorId
        };
        return model;
    }
}