using ACore.Server.Storages.Contexts.EF.Models;

namespace ACore.Server.Storages.CQRS.Results.Models;

public record EntityResultData(object PK, RepositoryOperationResult OperationResult);
