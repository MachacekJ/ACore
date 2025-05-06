using ACore.Extensions;
using ACore.Results;
using ACore.Server.Repository.Results;
using FluentAssertions;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;

public static class AuditAssertTestHelper
{
  public static TPK AssertSinglePrimaryKeyWithResult<T, TPK>(Result? result, T[]? data)
    where T : class
  {
    ArgumentNullException.ThrowIfNull(result);
    if (result is ExceptionResult exceptionResult)
      throw exceptionResult.Exception;
    
    ArgumentNullException.ThrowIfNull(data);
    
    if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ExceptionResult<>))
      throw result.PropertyValue(nameof(ExceptionResult<int>.Exception)) as Exception ?? throw new Exception();

    result.Should().BeOfType<RepositoryResult>();
    var dbSaveResult = (RepositoryResult)result;
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