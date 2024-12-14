namespace ACore.Server.Repository.Results.Models;

/// <summary>
/// The type of CRUD repository operation that should have been or has been performed.
/// </summary>
public enum RepositoryOperationTypeEnum
{
  Unknown,
  Added,
  Modified,
  Deleted,
  UnModified,
  Failed, 
  Error
}