﻿using ACore.Tests.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public class MongoStorageRegistrationT(TestData testData) : IStorageRegistrationT
{
  private ILogger<MongoStorageRegistrationT> _log = null!;
  private string _dbName = testData.GetDbName();

  private string ConnectionStringMongo { get; set; } = null!;
  
  
  public void RegisterServices(ServiceCollection sc)
  {
    ConnectionStringMongo = config.MongoDb?.ReadWrite ?? throw new InvalidOperationException();
  //  storageResolver.RegisterStorage(sc, new MongoTestStorageConfiguration(ConnectionStringMongo, _dbName, requiredBaseStorageModules));
  }

  public void GetServices(IServiceProvider sp)
  {
    _log = sp.GetService<ILogger<MongoStorageRegistrationT>>() ?? throw new ArgumentException($"{nameof(ILogger<MongoStorageRegistrationT>)} is null.");
    NewMongoDatabase();
  }

  public void FinishedTest()
  {
    if (!testData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
      return;
    
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(_dbName);
    _log.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }

  private void NewMongoDatabase()
  {
    if (!testData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
      return;
    
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(_dbName);
    _log.LogInformation("Database '{Dbname}' has been created.", _dbName);
  }
}