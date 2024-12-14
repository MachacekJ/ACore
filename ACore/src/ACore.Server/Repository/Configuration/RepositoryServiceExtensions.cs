using ACore.Repository;
using ACore.Server.Repository.Contexts.Mongo;
using ACore.Server.Repository.Services.RepositoryResolvers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Repository.Configuration;

public static class RepositoryServiceExtensions
{
  private const string MemoryConnectionString = "memory";

  public static void AddDbMongoRepository<T>(this IServiceCollection services, ServerRepositoryOptions serverRepositoryOptions)
  //  where TI : class
    where T : MongoContextBase
  {
    if (serverRepositoryOptions.MongoDb == null)
      return;
    
    services.AddScoped<T>();    
    MongoConventions.RegisterConventions();
  }

  public static void AddDbPGRepository<T>(this IServiceCollection services, ServerRepositoryOptions serverRepositoryOptions)
    where T : DbContext
  {
    if (serverRepositoryOptions.PGDb != null)
      services.AddDbContext<T>(opt => opt.UseNpgsql(serverRepositoryOptions.PGDb.ReadWriteConnectionString));
  }

  public static void AddDbMemoryRepository<T>(this IServiceCollection services, ServerRepositoryOptions serverRepositoryOptions, string name)
    where T : DbContext
  {
    if (serverRepositoryOptions.MemoryDb != null)
    {
      services.AddDbContext<T>(dbContextOptionsBuilder => { dbContextOptionsBuilder.UseInMemoryDatabase(MemoryConnectionString + name + Guid.NewGuid()); });
    }
  }

  public static async Task ConfigureMongoRepository<TIRepository, TImplementation>(this IServiceProvider provider, ServerRepositoryOptions serverRepositoryOptions)
    where TImplementation : class, TIRepository
    where TIRepository : IDbRepository
  {
    var repositoryResolver = GetRepositoryResolver(provider);
    if (serverRepositoryOptions.MongoDb != null)
    {
      var mongoImpl = provider.GetService<TImplementation>() as IDbRepository ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await repositoryResolver.ConfigureRepository<TIRepository>(new RepositoryImplementation(mongoImpl));
    }
  }

  public static async Task ConfigurePGRepository<TIRepository, TImplementation>(this IServiceProvider provider, ServerRepositoryOptions serverRepositoryOptions)
    where TImplementation : DbContext
    where TIRepository : IRepository
  {
    var repositoryResolver = GetRepositoryResolver(provider);
    if (serverRepositoryOptions.PGDb != null)
    {
      var pgImpl = provider.GetService<TImplementation>() as IDbRepository ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await repositoryResolver.ConfigureRepository<TIRepository>(new RepositoryImplementation(pgImpl));
    }
  }

  public static async Task ConfigureMemoryRepository<TIRepository, TImplementation>(this IServiceProvider provider, ServerRepositoryOptions serverRepositoryOptions)
    where TImplementation : DbContext
    where TIRepository : IRepository
  {
    var repositoryResolver = GetRepositoryResolver(provider);
    if (serverRepositoryOptions.MemoryDb != null)
    {
      var memoryImpl = provider.GetService<TImplementation>() as IDbRepository ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await repositoryResolver.ConfigureRepository<TIRepository>(new RepositoryImplementation(memoryImpl));
    }
  }

  private static IRepositoryResolver GetRepositoryResolver(IServiceProvider provider)
    => provider.GetService<IRepositoryResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IRepositoryResolver)}.");
}