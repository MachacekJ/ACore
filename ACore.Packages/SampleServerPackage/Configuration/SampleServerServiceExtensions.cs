using ACore.CQRS.Extensions;
using ACore.Server.Storages.Services.StorageResolvers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SampleServerPackage.ToDoModulePG.Configuration;

namespace SampleServerPackage.Configuration;

public static class SampleServerServiceExtensions
{
  public static void AddSampleServerModule(this IServiceCollection services, Action<SampleServerOptionBuilder>? optionsBuilder = null)
  {
    var aCoreServerOptionBuilder = SampleServerOptionBuilder.Empty();
    optionsBuilder?.Invoke(aCoreServerOptionBuilder);
    var aCoreServerOptions = aCoreServerOptionBuilder.Build();
    AddSampleServerModule(services, aCoreServerOptions);
  }

  public static void AddSampleServerModule(this IServiceCollection services, SampleServerOptions aCoreServerOptions)
  {
    var myOptionsInstance = Options.Create(aCoreServerOptions);
    services.AddSingleton(myOptionsInstance);

    // Adding CQRS from ACore.Server assembly.
    services.AddMediatR(c =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(SampleServerServiceExtensions));
      c.ParallelNotification();
    });
    services.AddValidatorsFromAssembly(typeof(SampleServerServiceExtensions).Assembly, includeInternalTypes: true);
    
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    
    
    if (aCoreServerOptions.ToDoModuleOptions.IsActive)
      services.AddToDoModule(aCoreServerOptions.ToDoModuleOptions);
    
  }

  public static async Task UseSampleServerModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<SampleServerOptions>>()?.Value ?? throw new Exception($"{nameof(SampleServerOptions)} is not registered.");

    if (opt.ToDoModuleOptions.IsActive)
      await provider.UseToDoModule();
  }
}