using ACore.UnitTests.Server.Repositories.Contexts.EF.EventNotification;

namespace ACore.UnitTests.Server.Repositories.Contexts.EF;

/// <summary>
/// <see cref="ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData"/>
/// </summary>
public class EfContextBaseSupportedDataTypesTests() : EfContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditableLongEntity)
{
  
}