using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using MediatR;

namespace ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageGet;

public class LocalStorageGetQuery(LocalStorageCategoryEnum category, string key) : IRequest<LocalStorageData>
{
    public string Key => key;
    public LocalStorageCategoryEnum Category => category;
}