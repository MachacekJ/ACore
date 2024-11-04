using System.Collections.ObjectModel;
using ACore.Extensions;
using ACore.Models.Result;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Server.Storages.CQRS.Handlers.Models;
using ACore.Server.Storages.CQRS.Results.Models;

namespace ACore.Server.Storages.CQRS.Results;

public class EntityResult : Result
{
  public ReadOnlyDictionary<IStorage, EntityResultData> ReturnedValues { get; }

  private EntityResult(IDictionary<IStorage, EntityResultData> pkValues) : base(true, ResultErrorItem.None)
    => ReturnedValues = pkValues.AsReadOnly();

  public static EntityResult SuccessWithEntityData(IEnumerable<StorageEntityExecutorItem> data)
  {
    return SuccessWithValues(data.ToDictionary(
      k => k.Storage,
      v => new EntityResultData(
        v.Entity.PropertyValue(nameof(PKEntity<int>.Id)) ?? throw new Exception($"{nameof(PKEntity<int>.Id)} is null."),
        v.Task.Result
      )));
  }

  private static EntityResult SuccessWithValues(Dictionary<IStorage, EntityResultData> pkValues) => new(pkValues);

  /// <summary>
  /// Return the first PK value. Value must exist.
  /// </summary>
  public T SinglePrimaryKey<T>()
    => (T)SingleResult().PK;
  
  public DatabaseOperationResult SingleDatabaseOperationResult()
  => SingleResult().OperationResult;
  
  private EntityResultData SingleResult()
  {
    if (ReturnedValues.Count != 1)
      throw new Exception($"No suitable {nameof(ReturnedValues)} is available. Count of items is {ReturnedValues.Count}.");
    return ReturnedValues.First().Value;
  }
}