namespace ACore.Server.Repository.Configuration.RepositoryTypes;

public class RepositoryMongoOptions(string readWriteConnectionString, string collectionName, string? readOnlyConnectionString = null, bool enableTransactions = true)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString;
  public string ReadWriteConnectionString => readWriteConnectionString;
  public string CollectionName => collectionName;
  public bool EnableTransactions => enableTransactions;
}