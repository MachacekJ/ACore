using System.Collections.ObjectModel;
using ACore.Extensions;
using ACore.Repository.Models;
using ACore.Results;
using ACore.Results.Models;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.CQRS.Handlers.Models;
using ACore.Server.Repository.Results.Models;

namespace ACore.Server.Repository.Results;

public class RepositoryResult : Result
{
  public const string ErrorCodeFailed = "Repository-Failed";

  public ReadOnlyDictionary<RepositoryInfo, RepositoryResultData> ReturnedValues { get; }

  private RepositoryResult(IDictionary<RepositoryInfo, RepositoryResultData> pkValues, bool success, ResultErrorItem resultErrorItem) : base(success, resultErrorItem)
    => ReturnedValues = pkValues.AsReadOnly();


  public static RepositoryResult CreateResultWithEntity(List<RepositoryEntityExecutorItem> data)
  {
    var resultData = data.ToDictionary(
      k => k.Repository.RepositoryInfo,
      v => new RepositoryResultData(
        v.Entity.PropertyValue(nameof(PKEntity<int>.Id)) ?? throw new Exception($"{nameof(PKEntity<int>.Id)} is null."),
        v.DatabaseExecutableTask.Result
      ));

    if (!resultData.Values.Any(e => e.OperationResult.IsFailure))
      return SuccessWithValues(resultData);

    var allMess = resultData.Values.Where(e => e.OperationResult.IsFailure).Select(e =>e.OperationResult.ResultErrorItem).ToList();
    var ms = string.Join("-->", allMess.Select(e => $"{e.Code}:{e.Message}").ToList());
    return ErrorWithValues(resultData, ms);
  }

  private static RepositoryResult SuccessWithValues(Dictionary<RepositoryInfo, RepositoryResultData> pkValues) => new(pkValues, true, ResultErrorItem.None);
  private static RepositoryResult ErrorWithValues(Dictionary<RepositoryInfo, RepositoryResultData> pkValues, string message) => new(pkValues, false, new ResultErrorItem(ErrorCodeFailed, message));

  /// <summary>
  /// Return the first PK value. Value must exist.
  /// </summary>
  public T SinglePrimaryKey<T>()
    => (T)SingleResult().PK;

  public RepositoryOperationResult SingleDatabaseOperationResult()
    => SingleResult().OperationResult;

  private RepositoryResultData SingleResult()
  {
    if (ReturnedValues.Count != 1)
      throw new Exception($"No suitable {nameof(ReturnedValues)} is available. Count of items is {ReturnedValues.Count}.");
    return ReturnedValues.First().Value;
  }
}