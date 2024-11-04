using ACore.CQRS.Results;
using ACore.Extensions;
using ACore.Models.Result;
using ACore.Server.Storages.CQRS.Results;
using FluentAssertions;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;

public static class AuditAssertTestHelper
{
  public static TPK AssertSinglePrimaryKeyWithResult<T, TPK>(Result? result, T[]? data)
    where T : class
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(data);

    if (result is ExceptionResult exceptionResult)
      throw exceptionResult.Exception;

    if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ExceptionResult<>))
      throw result.PropertyValue(nameof(ExceptionResult<int>.Exception)) as Exception ?? throw new Exception();

    result.Should().BeOfType<EntityResult>();
    var dbSaveResult = (EntityResult)result;
    dbSaveResult.IsSuccess.Should().BeTrue();
    dbSaveResult.Should().NotBeNull();
    dbSaveResult.ReturnedValues.Should().HaveCount(1);
    data.Should().HaveCount(1);

    var pk = dbSaveResult.SinglePrimaryKey<TPK>();
    var pkData = Convert.ChangeType(data.First().PropertyValue("Id"), typeof(TPK));
    pk.Should().Be(pkData);

    return pk;
  }
}