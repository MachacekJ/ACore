using Microsoft.Extensions.Internal;

namespace ACore.UnitTests.Core.Services.Cache.FakeClasses;

public class TestSystemClock : ISystemClock
{
  public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
}