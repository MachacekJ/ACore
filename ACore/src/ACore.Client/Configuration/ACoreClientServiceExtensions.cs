using ACore.Modules.LocalizationModule.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Client.Configuration;

public static class ACoreClientServiceExtensions
{
  
  public static void AddACoreClient(this IServiceCollection services, Action<ACoreClientOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreServerOptionBuilder = ACoreClientOptionsBuilder.Empty();
    optionsBuilder?.Invoke(aCoreServerOptionBuilder);
    var aCoreServerOptions = aCoreServerOptionBuilder.Build();
    AddACoreClient(services, aCoreServerOptions);
  }
  
  public static void AddACoreClient(this IServiceCollection services, ACoreClientOptions configureOptions)
  {
    services.AddACoreClientModules(configureOptions);
    // if ()
    //   services.AddACoreLocalization(new ACoreLocalizationOptions
    //   {
    //     LocalizationRepositories =
    //     [
    //       new MemoryLocalizationRepository([])
    //       //new ResXLocalizationRepository()
    //       // new MemoryLocalizationRepository([
    //       //   new ACoreACoreLocalizationItem(typeof(ACore.Client.Configuration.Localization.Contexts.Memory.ResXContext)), "Test", 1033, "TestEn"),
    //       //   new ACoreACoreLocalizationItem("ResXContext", "Test", 1029, "TestCZ"),
    //       //   new ACoreACoreLocalizationItem("ResXContext2", "Test", 1033, "TestEn2"),
    //       //   new ACoreACoreLocalizationItem("ResXContext2", "Test", 1029, "TestCZ2"),
    //       // ])
    //     ]
    //   });
  }

  private static void AddACoreClientModules(this IServiceCollection services, ACoreClientOptions options)
  {
    if (options.LocalizationClientModuleOptions.IsActive)
      services.AddLocalizationModule(options.LocalizationClientModuleOptions);
  }
}