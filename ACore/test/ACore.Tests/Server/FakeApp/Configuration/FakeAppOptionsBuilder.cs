using ACore.Server.Configuration;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Configuration;

namespace ACore.Tests.Server.FakeApp.Configuration;

public class FakeAppOptionsBuilder : ACoreServerOptionsBuilder
{
  private readonly Fake1ModuleOptionsBuilder _fake1ModuleOptionBuilder = Fake1ModuleOptionsBuilder.Empty();

  public static FakeAppOptionsBuilder Default() => new();

  private FakeAppOptionsBuilder()
  {
  }

  public FakeAppOptionsBuilder AddTestModule(Action<Fake1ModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_fake1ModuleOptionBuilder);
    _fake1ModuleOptionBuilder.Activate();
    return this;
  }

  public override FakeAppOptions Build()
  {
    var res = new FakeAppOptions();
    SetOptions(res);
    return res;
  }

  private void SetOptions(FakeAppOptions opt)
  {
    base.SetOptions(opt);
    opt.TestModuleOptions = _fake1ModuleOptionBuilder.IsActive ? _fake1ModuleOptionBuilder.Build(DefaultRepositoryOptionBuilder) : null;
  }
}