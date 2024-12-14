using ACore.Repository;
using ACore.Services.Localization.Interfaces;
using ACore.Services.Localization.Models;

namespace ACore.Modules.LocalizationModule.Repositories;

/// <summary>
/// Interface for retrieving localization record from repository.
/// </summary>
public interface ILocalizationRepository : IRepository
{
    IACoreLocalizationItem? GetLocalizationRecord(ACoreLocalizationKeyItem localizationKey, int lcid);
    IEnumerable<IACoreLocalizationItem> GetAllRecords(Type contextId, int lcid);
}