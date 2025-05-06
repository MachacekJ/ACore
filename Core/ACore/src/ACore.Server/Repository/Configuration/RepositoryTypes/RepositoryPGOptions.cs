namespace ACore.Server.Repository.Configuration.RepositoryTypes;

public class RepositoryPGOptions(string readWriteConnectionString, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
}