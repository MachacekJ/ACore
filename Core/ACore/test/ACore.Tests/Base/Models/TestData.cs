using System.Reflection;

namespace ACore.Tests.Base.Models;

public class TestData
{
  private const int MaximumLengthOfDb = 63;

  private readonly string[] _replaceLetter = [".", "<", ">", "+"];
  private readonly string[] _replaceDbLetter = ["_", "-"];

  /// <summary>
  /// Name of test important for DB name and log file name.
  /// Name is derived by <see cref="MemberInfo"/> from ctor.
  /// </summary>
  public string TestName { get; }

  public TestData(MemberInfo method)
  {
    ArgumentNullException.ThrowIfNull(method);
    TestName = method.DeclaringType?.FullName ?? throw new ArgumentNullException(nameof(method));
    TestName = _replaceLetter.Aggregate(TestName, (current, letter) => current.Replace(letter, "_"));
  }

  public string GetDbName()
  {
    var testName = TestName.ToLower();
    testName += Guid.NewGuid();

    if (testName.Length > MaximumLengthOfDb)
      testName = testName.Substring(testName.Length - MaximumLengthOfDb);

    testName = _replaceDbLetter.Aggregate(testName, (current, letter) => current.Replace(letter, "_"));

    return testName;
  }
}