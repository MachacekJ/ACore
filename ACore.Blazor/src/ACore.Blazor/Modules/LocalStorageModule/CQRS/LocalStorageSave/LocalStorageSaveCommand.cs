using System.Text.Json;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using MediatR;

namespace ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageSave;

// ReSharper disable once ClassNeverInstantiated.Global
public class LocalStorageSaveCommand(LocalStorageCategoryEnum category, string key, object value, Type type) : IRequest
{
    public LocalStorageCategoryEnum Category => category;
    public string Key => key;
    public string Value => JsonSerializer.Serialize(value, type);
}