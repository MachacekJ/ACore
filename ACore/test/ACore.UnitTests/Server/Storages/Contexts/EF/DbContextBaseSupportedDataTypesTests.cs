using ACore.UnitTests.Server.Storages.Contexts.EF.CRUDTests;

namespace ACore.UnitTests.Server.Storages.Contexts.EF;

/// <summary>
/// <see cref="ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData"/>
/// </summary>
public class DbContextBaseSupportedDataTypesTests() : DbContextBaseCRUDTests(CRUDEntityTypeEnum.FakeNotAuditableLongEntity)
{
  
}