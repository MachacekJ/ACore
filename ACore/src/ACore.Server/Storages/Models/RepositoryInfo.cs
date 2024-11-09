using ACore.Server.Storages.Definitions;

namespace ACore.Server.Storages.Models;

public record RepositoryInfo(string ModuleName, StorageDefinition StorageDefinition);
