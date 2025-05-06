using ACore.CQRS.Extensions;
using ACore.Server.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;
using Autofac;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.FakeApp.Configuration;

public static class FakeAppServiceExtensions
{
  public static void AddFakeApp(this IServiceCollection services, Action<FakeAppOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreTestOptionsBuilder = FakeAppOptionsBuilder.Default();
    optionsBuilder?.Invoke(aCoreTestOptionsBuilder);
    var oo = aCoreTestOptionsBuilder.Build();
    AddFakeApp(services, oo);
  }

  private static void AddFakeApp(this IServiceCollection services, FakeAppOptions fakeAppOptions)
  {
    ValidateDependencyInConfiguration(fakeAppOptions);

    services.AddACoreServer(fakeAppOptions);

    var myOptionsInstance = Options.Create(fakeAppOptions);
    services.AddSingleton(myOptionsInstance);

    // Adding CQRS from ACore.Tests assembly.
    services.AddMediatR(c =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(FakeAppServiceExtensions));
      c.ParallelNotification();
    });
    services.AddValidatorsFromAssembly(typeof(FakeAppServiceExtensions).Assembly, includeInternalTypes: true);

    if (fakeAppOptions.TestModuleOptions is { IsActive: true })
      services.AddTestModule(fakeAppOptions.TestModuleOptions);
  }

  public static async Task UseFakeApp(this IApplicationBuilder applicationBuilder)
  {
    var provider = applicationBuilder.ApplicationServices;
    var opt = provider.GetService<IOptions<FakeAppOptions>>()?.Value ?? throw new Exception($"{nameof(FakeAppOptions)} is not registered.");
    await applicationBuilder.UseACoreServer();

    if (opt.TestModuleOptions is { IsActive: true })
      await provider.UseTestModule();
  }

  public static void RegisterFakeAppContainer(this ContainerBuilder containerBuilder)
  {
    containerBuilder.ContainerTestModule();
  }

  private static void ValidateDependencyInConfiguration(FakeAppOptions fakeAppOptions)
  {
    ValidateTestModuleOptions(fakeAppOptions);
  }

  private static void ValidateTestModuleOptions(FakeAppOptions fakeAppOptions)
  {
    if (fakeAppOptions.TestModuleOptions is { IsActive: false })
      return;

    if (fakeAppOptions.AuditModuleOptions is { IsActive: false })
      throw new Exception($"Module {nameof(ACore.Server.Modules.AuditModule)} must be activated.");
  }
}