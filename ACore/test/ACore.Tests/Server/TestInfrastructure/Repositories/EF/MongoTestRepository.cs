using ACore.Server.Configuration;
using ACore.Tests.Base.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.Tests.Server.TestInfrastructure.Repositories.EF;

public class MongoTestRepository : ITestRepository
{
  private ILogger<MongoTestRepository>? _log;
  private readonly string _dbName;
  private readonly string _dbConnectionString;

  public MongoTestRepository(TestData testData, IConfigurationRoot configuration)
  {
    _dbName = testData.GetDbName();
    _dbConnectionString = string.Format(configuration["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException(), _dbName);
  }

  public void SetupBuilder(ACoreServerOptionsBuilder builder)
  {
    builder.AddDefaultRepositories(storageOptionBuilder => storageOptionBuilder.AddMongo(_dbConnectionString, _dbName));
  }

  public void RegisterServices(ServiceCollection sc) { }

  public async Task CreateDb(IServiceProvider sp)
  {
    _log = sp.GetService<ILogger<MongoTestRepository>>() ?? throw new ArgumentException($"{nameof(ILogger<MongoTestRepository>)} is null.");
    await NewMongoDatabase();
  }
  
  public  async Task FinishedTestAsync()
  {
    await DropDatabase();
  }


  private async Task DropDatabase()
  {
    // if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
    //   return;

    var client = new MongoClient(_dbConnectionString);
    await client.DropDatabaseAsync(_dbName);
    _log?.LogInformation("Mongo database '{Dbname}' has been deleted", _dbName);
  }

  private async Task NewMongoDatabase()
  {
    // if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
    //   return;

    var client = new MongoClient(_dbConnectionString);
    await client.DropDatabaseAsync(_dbName);
    _log?.LogInformation("Mongo database '{Dbname}' has been created.", _dbName);
  }
}

