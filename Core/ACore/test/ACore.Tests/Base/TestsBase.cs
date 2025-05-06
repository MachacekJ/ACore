using System.Globalization;
using System.Reflection;
using ACore.Extensions;
using ACore.Models.Cache;
using ACore.Tests.Base.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;

namespace ACore.Tests.Base;

public abstract class TestsBase
{
  private IMediator? _mediator;
  private ServiceCollection _services = [];

  protected IConfigurationRoot? Configuration { get; private set; }
  protected TestData TestData { get; private set; } = new(MethodBase.GetCurrentMethod() ?? throw new ArgumentException($"Method name is null {nameof(RunTestAsync)}"));
  protected IMediator Mediator
  {
    get => _mediator ?? throw new NullReferenceException();
    private set => _mediator = value;
  }
  protected InMemorySink LogInMemorySink { get; set; } = new();
  private ILogger<TestsBase>? Log { get; set; }
  private string RootDir { get; set; } = string.Empty;

  protected async Task RunTestAsync(MemberInfo? method, Func<Task> testCode)
  {
    if (method == null)
      throw new ArgumentException($"Method name is null {nameof(RunTestAsync)}");

    await RunTestAsync(new TestData(method), testCode);
  }

  protected Task RunTestAsync<T>(MemberInfo method, T param, Func<T, Task> testCode)
    => RunTestAsync(new TestData(method), param, testCode);

  protected async Task RunTestAsync(TestData testData, Func<Task> testCode)
  {
    await RunTestAsync(testData, 0, async (_) => { await testCode(); });
  }

  protected async Task RunTestAsync<T>(TestData testData, T param, Func<T, Task> testCode)
  {
    Thread.CurrentThread.CurrentCulture = new CultureInfo(1033);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(1033);

    TestData = testData;
    await StartTest(_services);

    ArgumentNullException.ThrowIfNull(Log);
    Log.LogInformation("Start test Test {TestName}", TestData.TestName);

    try
    {
      await testCode(param);
    }
    catch (Exception ex)
    {
      Log.LogError("RunTestAsync-{Ex}", ex.MessageRecursive());
      throw;
    }
    finally
    {
      await ClearTestAsync();
    }

    Log.LogInformation("End test Test {TestName}", TestData.TestName);
  }

  protected virtual string JsonSettingPath()
  {
    var dic = Directory.GetCurrentDirectory();
    dic = dic.Replace("\\bin\\Debug\\net6.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net6.0", string.Empty);
    dic = dic.Replace("\\bin\\Debug\\net7.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net7.0", string.Empty);
    dic = dic.Replace("\\bin\\Debug\\net8.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net8.0", string.Empty);
    return dic;
  }

  protected virtual void SetAutofacContainer(ContainerBuilder containerBuilder)
  {
  }

  protected virtual void RegisterServices(ServiceCollection services)
  {
    RootDir = JsonSettingPath();

    var builder = new ConfigurationBuilder()
      .SetBasePath(RootDir)
      .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
      .AddJsonFile("appsettings.Test.Debug.json", optional: true, reloadOnChange: true)
      .AddEnvironmentVariables();
    Configuration = builder.Build();
    services.AddSingleton<IConfiguration>(Configuration);


    var logDir = Path.Combine(RootDir, "Logs");
    var serilog = new LoggerConfiguration()
      .MinimumLevel.Verbose()
      .WriteTo.File(Path.Combine(logDir, TestData.TestName) + ".txt", retainedFileTimeLimit: TimeSpan.FromDays(1), retainedFileCountLimit: 1)
      .WriteTo.Sink(LogInMemorySink)
      .WriteTo.InMemory(restrictedToMinimumLevel: LogEventLevel.Debug)
      .CreateLogger();

    services.AddLogging(logBuilder => { logBuilder.AddSerilog(logger: serilog, dispose: true); });
  }

  protected virtual async Task<IServiceProvider> UseServices(IApplicationBuilder applicationBuilder)
  {
    var sp = applicationBuilder.ApplicationServices;
    Log = sp.GetService<ILogger<TestsBase>>() ?? throw new ArgumentException($"{nameof(ILogger<TestsBase>)} is null.");
    Mediator = sp.GetService<IMediator>() ?? throw new ArgumentException($"{nameof(IMediator)} is null.");
    return await Task.FromResult(sp);
  }

  protected virtual async Task ClearTestAsync()
  {
    await Task.CompletedTask;
  }

  private async Task StartTest(ServiceCollection services)
  {
    _services = [];

    // The Microsoft.Extensions.Logging package provides this one-liner
    // to add logging services.
    // serviceCollection.AddLogging();

    var containerBuilder = new ContainerBuilder();

    SetAutofacContainer(containerBuilder);
    RegisterServices(services);

    var configuration = MediatRConfigurationBuilder
      .Create(typeof(TestsBase).Assembly, typeof(CacheKey).Assembly)
      .WithAllOpenGenericHandlerTypesRegistered()
      .Build();
    containerBuilder.RegisterMediatR(configuration);

    // Once you've registered everything in the ServiceCollection, call
    // Populate to bring those registrations into Autofac. This is
    // just like a foreach over the list of things in the collection
    // to add them to Autofac.
    containerBuilder.Populate(services);

    // Creating a new AutofacServiceProvider makes the container
    // available to your app using the Microsoft IServiceProvider
    // interface so you can use those abstractions rather than
    // binding directly to Autofac.
    var container = containerBuilder.Build();
    var serviceProvider = new AutofacServiceProvider(container);
    var appBuilderMock = new Mock<IApplicationBuilder>();

    appBuilderMock.SetupGet(p => p.ApplicationServices).Returns(serviceProvider);
    await UseServices(appBuilderMock.Object);
  }
}