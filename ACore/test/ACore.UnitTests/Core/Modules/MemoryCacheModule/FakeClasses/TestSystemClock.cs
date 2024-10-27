using Microsoft.Extensions.Internal;

namespace ACore.UnitTests.Core.Modules.MemoryCacheModule.FakeClasses;

public class TestSystemClock : ISystemClock
{
  public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
}