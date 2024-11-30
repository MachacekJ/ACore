using ACore.CQRS.Extensions;
using ACore.Server.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SampleServerPackage.ToDoModulePG.Configuration;

namespace SampleServerPackage.Configuration;

public static class SampleServerPackageServiceExtensions
{
  // public static void AddSampleServerPackage(this IServiceCollection services, Action<SampleServerPackageOptionBuilder>? optionsBuilder = null)
  // {
  //  // services.Where(s=>s.)
  //   var sampleServerOptionBuilder = SampleServerPackageOptionBuilder.Empty();
  //   optionsBuilder?.Invoke(sampleServerOptionBuilder);
  //   
  //   if (sampleServerOptionBuilder.DefaultStorageOptionBuilder != null)
  //   {
  //     var def =  services.FirstOrDefault(s => s.ServiceType == typeof(IOptions<ACoreServerOptions>));
  //     var aa = def.ImplementationInstance as ACoreServerOptions;
  //     sampleServerOptionBuilder.AddDefaultStorageOption(aa.DefaultStorages);
  //   }
  //
  //   var sampleServerOptions = sampleServerOptionBuilder.Build();
  //   AddSampleServerPackage(services, sampleServerOptions);
  // }

  // public static void AddSampleServerPackage(this IServiceCollection services, SampleServerPackageOptions aCoreServerPackageOptions)
  // {
  //   var myOptionsInstance = Options.Create(aCoreServerPackageOptions);
  //   services.AddSingleton(myOptionsInstance);
  //
  //   // Adding CQRS from ACore.Server assembly.
  //   services.AddMediatR(c =>
  //   {
  //     c.RegisterServicesFromAssemblyContaining(typeof(SampleServerPackageServiceExtensions));
  //     c.ParallelNotification();
  //   });
  //   services.AddValidatorsFromAssembly(typeof(SampleServerPackageServiceExtensions).Assembly, includeInternalTypes: true);
  //   
  //   services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
  //   
  //   
  //   if (aCoreServerPackageOptions.ToDoModuleOptions.IsActive)
  //     services.AddToDoModule(aCoreServerPackageOptions.ToDoModuleOptions);
  //   
  // }

  // public static async Task UseACoreSampleServerPackage(this IServiceProvider provider)
  // {
  //   var opt = provider.GetService<IOptions<SampleServerPackageOptions>>()?.Value ?? throw new Exception($"{nameof(SampleServerPackageOptions)} is not registered.");
  //
  //   if (opt.ToDoModuleOptions.IsActive)
  //     await provider.UseToDoModule();
  // }
}