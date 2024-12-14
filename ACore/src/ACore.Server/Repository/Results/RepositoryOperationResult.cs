using ACore.Results;
using ACore.Results.Models;
using ACore.Server.Repository.Results.Models;

namespace ACore.Server.Repository.Results;

/// <summary>
/// Repository result for one CRUD repository operation. see <see cref="RepositoryOperationTypeEnum"/>
/// </summary>
public class RepositoryOperationResult : Result
{
  public const string ErrorCodeErrorEntityNotExists = "RepositoryOperation-EntityNotExists";
  public const string ErrorCodeConcurrency = "RepositoryOperation-Concurrency";
  
  public RepositoryOperationTypeEnum RepositoryOperationType { get; }

  public string? SumHash { get; }

  private RepositoryOperationResult(RepositoryOperationTypeEnum operationTypeEnum, bool isSuccess, ResultErrorItem resultErrorItem, string? sumHash = null) : base(isSuccess, resultErrorItem)
  {
    RepositoryOperationType = operationTypeEnum;
    SumHash = sumHash;
  }

  public static RepositoryOperationResult Success(RepositoryOperationTypeEnum operationTypeEnum, string? checkSumHash = null) => new(operationTypeEnum, true, ResultErrorItem.None, checkSumHash);
  public static RepositoryOperationResult ErrorEntityNotExists(string entityName, string id) => new(RepositoryOperationTypeEnum.Failed, false, new ResultErrorItem(ErrorCodeErrorEntityNotExists, $"Entity '{entityName}' with Id '{id}' does not exist."));
  public static RepositoryOperationResult ErrorConcurrency(string entityName, string id) => new(RepositoryOperationTypeEnum.Failed, false, new ResultErrorItem(ErrorCodeConcurrency, $"Entity '{entityName}' with Id '{id}' has been modified, check sum hash code is incorrect."));
  public static RepositoryOperationResult InternalError(Exception ex) => new(RepositoryOperationTypeEnum.Error, false, ResultErrorItem.Exception(ex));
}