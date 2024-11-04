using ACore.Models.Result;

namespace ACore.Server.Storages.Contexts.EF.Models;

public enum DatabaseOperationTypeEnum
{
  Unknown,
  Added,
  Modified,
  Deleted,
  UnModified,
}

public class DatabaseOperationResult : Result
{
  public DatabaseOperationTypeEnum DatabaseOperationType { get; }
  
  public string? SumHash { get; }

  private DatabaseOperationResult(DatabaseOperationTypeEnum operationTypeEnum, bool isSuccess, ResultErrorItem resultErrorItem, string? sumHash = null) : base(isSuccess, resultErrorItem)
  {
    DatabaseOperationType = operationTypeEnum;
    SumHash = sumHash;
  }

  public static DatabaseOperationResult Success(DatabaseOperationTypeEnum operationTypeEnum, string? checkSumHash = null) => new(operationTypeEnum, true, ResultErrorItem.None, checkSumHash);

  public static DatabaseOperationResult ErrorEntityNotExists(string entityName, string id) => new (DatabaseOperationTypeEnum.Unknown, false, new ResultErrorItem("entityId", $"Entity '{entityName}' with Id '{id}' does not exist."));
  public static DatabaseOperationResult ErrorConcurrency(string entityName, string id) => new (DatabaseOperationTypeEnum.Unknown, false, new ResultErrorItem("concurrency", $"Entity '{entityName}' with Id '{id}' has been modified, check sum hash code is incorrect."));
  
}