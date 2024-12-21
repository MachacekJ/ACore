using ACore.Server.Configuration;
using ACore.Tests.Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Server.TestInfrastructure.Storages.EF;

public class PGTestStorage : ITestStorage
{
  private readonly string _dbName;
  private MasterDb? _masterDb;
  private ILogger<PGTestStorage>? _log;
  private readonly string _dbConnectionString;
  private readonly string _dbConnectionStringMaster;

  public PGTestStorage(TestData testData, IConfigurationRoot configuration)
  {
    _dbName = testData.GetDbName();
    _dbConnectionString = string.Format(configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), _dbName);
    _dbConnectionStringMaster = string.Format(configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), "postgres");
  }


  public void RegisterServices(ServiceCollection sc)
  {
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(_dbConnectionStringMaster));
  }

  public void ConfigureStorage(ACoreServerOptionsBuilder builder)
  {
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddPG(_dbConnectionString));
  }

  public async Task GetServices(IServiceProvider sp)
  {
    _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(PGTestStorage)}.{nameof(MasterDb)} is null.");
    _log = sp.GetService<ILogger<PGTestStorage>>() ?? throw new ArgumentException($"{nameof(ILogger<PGTestStorage>)} is null.");
    await NewPGDatabase();
  }

  public async Task FinishedTestAsync()
  {
    await DropPGDatabase();
  }

  private async Task NewPGDatabase()
  {
    // if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
    //   return;

    string sql = @"
DROP DATABASE IF EXISTS " + _dbName + @" WITH (FORCE);

CREATE DATABASE " + _dbName + @"
    WITH OWNER = 'user'
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";

    if (_masterDb != null)
      await _masterDb.Database.ExecuteSqlRawAsync(sql);

    _log?.LogInformation("Database '{Dbname}' has been created", _dbName);
  }

  private async Task DropPGDatabase()
  {
    // if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
    //   return;

    var sql = "DROP DATABASE IF EXISTS " + _dbName + " WITH (FORCE);";

    if (_masterDb != null)
      await _masterDb.Database.ExecuteSqlRawAsync(sql);

    _log?.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }
}

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);