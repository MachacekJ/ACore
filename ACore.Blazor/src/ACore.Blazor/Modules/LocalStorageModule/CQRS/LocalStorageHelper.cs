using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;

namespace ACore.Blazor.Modules.LocalStorageModule.CQRS;

public static class LocalStorageHelper
{
    public static string GetKey(LocalStorageCategoryEnum category, string key) => $"{category}-{key}";
}