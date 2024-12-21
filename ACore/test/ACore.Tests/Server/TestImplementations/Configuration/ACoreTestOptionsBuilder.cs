using ACore.Server.Configuration;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Configuration;

namespace ACore.Tests.Server.TestImplementations.Configuration;

public class ACoreTestOptionsBuilder : ACoreServerOptionsBuilder
{
  private readonly TestModuleOptionsBuilder _testModuleOptionBuilder = TestModuleOptionsBuilder.Empty();

  public static ACoreTestOptionsBuilder Empty() => new();

  private ACoreTestOptionsBuilder()
  {
  }
  
  public ACoreTestOptionsBuilder AddTestModule(Action<TestModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_testModuleOptionBuilder);
    _testModuleOptionBuilder.Activate();
    return this;
  }

  public override ACoreTestOptions Build()
  {
    var res = new ACoreTestOptions();
    SetOptions(res);
    return res; 
  }

  private void SetOptions(ACoreTestOptions opt)
  {
    base.SetOptions(opt);
    opt.TestModuleOptions = _testModuleOptionBuilder.Build(DefaultStorageOptionBuilder);
  }
}