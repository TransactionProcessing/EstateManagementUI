using EstateManagementUI.BusinessLogic.Models;
using System.Collections.Concurrent;
using FileImportLogDetailsModel = EstateManagementUI.BusinessLogic.Models.FileProcessingModels.FileImportLogDetailsModel;

namespace EstateManagementUI.BlazorServer.Testing;

public sealed class TestSupportState
{
    private readonly ConcurrentDictionary<(Guid EstateId, Guid MerchantId, int Year), MerchantModels.MerchantScheduleModel> _merchantSchedules = new();
    private readonly ConcurrentDictionary<(Guid EstateId, Guid MerchantId), MerchantModels.MerchantOpeningHoursModel> _merchantOpeningHours = new();
    private readonly ConcurrentDictionary<(Guid EstateId, Guid MerchantId), List<MerchantModels.MerchantOperatorModel>> _merchantOperators = new();
    private readonly ConcurrentDictionary<(Guid EstateId, Guid MerchantId), List<MerchantModels.MerchantContractModel>> _merchantContracts = new();
    private readonly ConcurrentDictionary<(Guid EstateId, Guid MerchantId), List<MerchantModels.MerchantDeviceModel>> _merchantDevices = new();
    private readonly ConcurrentDictionary<Guid, FileImportLogDetailsModel> _fileImportLogs = new();

    public void Reset()
    {
        _merchantSchedules.Clear();
        _merchantOpeningHours.Clear();
        _merchantOperators.Clear();
        _merchantContracts.Clear();
        _merchantDevices.Clear();
        _fileImportLogs.Clear();
    }

    public bool TryGetMerchantSchedule(Guid estateId, Guid merchantId, int year, out MerchantModels.MerchantScheduleModel? schedule)
        => _merchantSchedules.TryGetValue((estateId, merchantId, year), out schedule);

    public void SetMerchantSchedule(Guid estateId, Guid merchantId, MerchantModels.MerchantScheduleModel schedule)
        => _merchantSchedules[(estateId, merchantId, schedule.Year)] = schedule;

    public bool TryGetMerchantOpeningHours(Guid estateId, Guid merchantId, out MerchantModels.MerchantOpeningHoursModel? openingHours)
        => _merchantOpeningHours.TryGetValue((estateId, merchantId), out openingHours);

    public void SetMerchantOpeningHours(Guid estateId, Guid merchantId, MerchantModels.MerchantOpeningHoursModel openingHours)
        => _merchantOpeningHours[(estateId, merchantId)] = openingHours;

    public List<MerchantModels.MerchantOperatorModel> GetMerchantOperators(Guid estateId, Guid merchantId)
        => _merchantOperators.GetOrAdd((estateId, merchantId), _ => new List<MerchantModels.MerchantOperatorModel>());

    public List<MerchantModels.MerchantContractModel> GetMerchantContracts(Guid estateId, Guid merchantId)
        => _merchantContracts.GetOrAdd((estateId, merchantId), _ => new List<MerchantModels.MerchantContractModel>());

    public List<MerchantModels.MerchantDeviceModel> GetMerchantDevices(Guid estateId, Guid merchantId)
        => _merchantDevices.GetOrAdd((estateId, merchantId), _ => new List<MerchantModels.MerchantDeviceModel>());

    public void SetFileImportLog(FileImportLogDetailsModel log) => _fileImportLogs[log.FileImportLogId] = log;

    public List<FileImportLogDetailsModel> GetFileImportLogs() => _fileImportLogs.Values.OrderByDescending(log => log.ImportLogDate).ToList();

    public FileImportLogDetailsModel? GetFileImportLog(Guid fileImportLogId)
        => _fileImportLogs.TryGetValue(fileImportLogId, out var log) ? log : null;
}
