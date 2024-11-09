using ACore.Models.Result;

namespace ACore.Server.Storages.Contexts.EF.Models;

public enum RepositoryOperationTypeEnum
{
  Unknown,
  Added,
  Modified,
  Deleted,
  UnModified,
}

public class RepositoryOperationResult : Result
{
  /// <summary>
  /// Repository primary key id
  /// </summary>
  public object? Id { get; set; }
  public RepositoryOperationTypeEnum RepositoryOperationType { get; }
  
  public string? SumHash { get; }

  private RepositoryOperationResult(RepositoryOperationTypeEnum operationTypeEnum, bool isSuccess, ResultErrorItem resultErrorItem, string? sumHash = null) : base(isSuccess, resultErrorItem)
  {
    RepositoryOperationType = operationTypeEnum;
    SumHash = sumHash;
  }

  public static RepositoryOperationResult Success(RepositoryOperationTypeEnum operationTypeEnum, string? checkSumHash = null) => new(operationTypeEnum, true, ResultErrorItem.None, checkSumHash);

  public static RepositoryOperationResult ErrorEntityNotExists(string entityName, string id) => new (RepositoryOperationTypeEnum.Unknown, false, new ResultErrorItem("entityId", $"Entity '{entityName}' with Id '{id}' does not exist."));
  public static RepositoryOperationResult ErrorConcurrency(string entityName, string id) => new (RepositoryOperationTypeEnum.Unknown, false, new ResultErrorItem("concurrency", $"Entity '{entityName}' with Id '{id}' has been modified, check sum hash code is incorrect."));
  
}