using ACore.Services.Localization.Models;

namespace ACore.Services.Localization.Interfaces;

public interface IACoreLocalizationItem
{
    public string Key { get; }

    public string Translation { get; }

    public int Lcid { get; }

    public Type ContextId { get; }
    
    ACoreLocalizationKeyItem LocalizationKey { get; } 
}