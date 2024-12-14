using ACore.CQRS.Pipelines;
using ACore.CQRS.Pipelines.Models;
using ACore.Results;
using ACore.Results.Models;
using ACore.UnitTests.Core.Base.CQRS.Pipelines.FakeClasses;
using ACore.UnitTests.TestImplementations;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using ValidationResult = ACore.Results.ValidationResult;

namespace ACore.UnitTests.Core.Base.CQRS.Pipelines;

public class LoggingPipelineBehaviorTests
{
  private const string FakeErrorCode = nameof(FakeErrorCode);
  private const string FakeErrorMessage = nameof(FakeErrorMessage);

  [Fact]
  public async Task NoErrorTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<LoggingPipelineBehavior<FakeRequest, Result>>();
    var sut = CreateLoggingPipelineBehaviorAsSut(loggerHelper.LoggerMocked);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return Result.Success();
    }, CancellationToken.None);

    // Assert
    loggerHelper.LogLevels.All(a => a.Equals(LogLevel.Debug)).Should().BeTrue();
  }

  [Fact]
  public async Task ValidationErrorTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<LoggingPipelineBehavior<FakeRequest, Result>>();
    var sut = CreateLoggingPipelineBehaviorAsSut(loggerHelper.LoggerMocked);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return ValidationResult.WithErrors(ValidationTypeEnum.ValidationInput, [new FluentValidationErrorItem(new ValidationFailure("fakeProp", FakeErrorMessage))]); // new ResultErrorItem(FakeErrorCode, FakeErrorMessage));
    }, CancellationToken.None);

    // Assert
    loggerHelper.LogLevels.All(a => a.Equals(LogLevel.Debug)).Should().BeTrue();
  }
  
  [Fact]
  public async Task ExceptionErrorTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<LoggingPipelineBehavior<FakeRequest, Result>>();
    var sut = CreateLoggingPipelineBehaviorAsSut(loggerHelper.LoggerMocked);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      throw new Exception(FakeErrorMessage);
    }, CancellationToken.None);

    // Assert
    response.Should().BeOfType(typeof(ExceptionResult));
    var index = loggerHelper.LogLevels.Select((logLevel, i) => new { logLevel, index = i }).First(a => a.logLevel.Equals(LogLevel.Error)).index;
    var errorMessage = loggerHelper.LogMessages[index];
    errorMessage.Should().Contain(FakeErrorMessage);
  }
  
  [Fact]
  public async Task ResultErrorTest()
  {
    // Arrange
    var loggerHelper = new MoqLogger<LoggingPipelineBehavior<FakeRequest, Result>>();
    var sut = CreateLoggingPipelineBehaviorAsSut(loggerHelper.LoggerMocked);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return Result.Failure(new ResultErrorItem(FakeErrorCode, FakeErrorMessage));
    }, CancellationToken.None);

    // Assert
    response.Should().BeOfType(typeof(Result));
    var index = loggerHelper.LogLevels.Select((logLevel, i) => new { logLevel, index = i }).First(a => a.logLevel.Equals(LogLevel.Error)).index;
    var errorMessage = loggerHelper.LogMessages[index];
    errorMessage.Should().Contain(FakeErrorMessage).And.Contain(FakeErrorCode);
  }

  private static LoggingPipelineBehavior<FakeRequest, Result> CreateLoggingPipelineBehaviorAsSut(ILogger<LoggingPipelineBehavior<FakeRequest, Result>> logger)
    => new(logger);
}