using ACore.Server.Configuration;
using ACore.Server.Storages.Configuration;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SampleServerPackage.Configuration;
using SampleServerPackage.ToDoModulePG.CQRS;
using SampleServerPackage.ToDoModulePG.Repositories;
using SampleServerPackage.ToDoModulePG.Repositories.SQL;

namespace SampleServerPackage.ToDoModulePG.Configuration;

public static class ToDoModuleServiceExtensions
{
  public static void AddToDoModule(this IServiceCollection services, ToDoModuleOptions options)
  {
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(ToDoModulePipelineBehavior<,>));
    
    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    
    services.AddDbPGStorage<ToDoRepositoryPGStorageImpl>(options.Storages);
  }

  public static void ConfigureAutofacTestModule(this ContainerBuilder containerBuilder)
  {

  }

  public static async Task UseToDoModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreServerOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(ACoreServerOptions)} is not configured.");

    StorageOptions? storageOptions = null;
    if (opt.DefaultStorages != null)
      storageOptions = opt.DefaultStorages;
    if (opt.ToDoModuleOptions.Storages != null)
      storageOptions = opt.ToDoModuleOptions.Storages;
    if (storageOptions == null)
      throw new ArgumentException($"{nameof(opt.ToDoModuleOptions)} is null. You can also use {nameof(opt.DefaultStorages)}.");
    
    await provider.ConfigurePGStorage<IToDoRepository, ToDoRepositoryPGStorageImpl>(storageOptions);
  }
}