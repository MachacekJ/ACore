using ACore.CQRS.Pipelines;
using ACore.CQRS.Pipelines.Models;
using ACore.Results;
using ACore.UnitTests.Core.Base.CQRS.Pipelines.FakeClasses;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ACore.UnitTests.Core.Base.CQRS.Pipelines;

public class FluentValidationPipelineBehaviorTests
{
  private const string FakeProp = nameof(FakeProp);
  private const string FakeErrorMessage = nameof(FakeErrorMessage);

  [Fact]
  public async Task ValidationSuccessTest()
  {
    // Arrange
    var mv = new Mock<IValidator<FakeRequest>>();
    mv.Setup(m => m.Validate(It.IsAny<FakeRequest>())).Returns(new ValidationResult());
    var sut = CreateFluentValidationPipelineBehaviorNotGenericResultAsSut([mv.Object]);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return Result.Success();
    }, CancellationToken.None);

    // Assert
    response.IsSuccess.Should().BeTrue();
    response.IsFailure.Should().BeFalse();
    response.ResultErrorItem.Code.Should().Be(string.Empty);
  }

  [Fact]
  public async Task ValidationNotSuccessNotGenericResultTest()
  {
    // Arrange
    var mv = new Mock<IValidator<FakeRequest>>();
    mv.Setup(m => m.Validate(It.IsAny<FakeRequest>())).Returns(new ValidationResult([new ValidationFailure(FakeProp, FakeErrorMessage)]));
    var sut = CreateFluentValidationPipelineBehaviorNotGenericResultAsSut([mv.Object]);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return Result.Success();
    }, CancellationToken.None);

    // Assert
    response.Should().BeOfType(typeof(Results.ValidationResult));
    var validationResult = response as Results.ValidationResult ?? throw new Exception();
    AssertNotSuccess(response, validationResult.ValidationErrors);

  }

  [Fact]
  public async Task ValidationNotSuccessGenericResultTest()
  {
    // Arrange
    var mv = new Mock<IValidator<FakeRequest>>();
    mv.Setup(m => m.Validate(It.IsAny<FakeRequest>())).Returns(new ValidationResult([new ValidationFailure(FakeProp, FakeErrorMessage)]));
    var sut = CreateFluentValidationPipelineBehaviorGenericResultAsSut([mv.Object]);
    var req = new FakeRequest();

    // Act
    var response = await sut.Handle(req, async (ct) =>
    {
      await Task.CompletedTask;
      return Result.Success(new FakeResponse());
    }, CancellationToken.None);

    // Assert
    response.Should().BeOfType(typeof(ValidationResult<FakeResponse>));
    var validationResult = response as ValidationResult<FakeResponse> ?? throw new Exception();
    AssertNotSuccess(validationResult, validationResult.ValidationErrors);
  }

  private static FluentValidationPipelineBehavior<FakeRequest, Result> CreateFluentValidationPipelineBehaviorNotGenericResultAsSut(List<IValidator<FakeRequest>> validators)
    => new(validators);

  private static FluentValidationPipelineBehavior<FakeRequest, Result<FakeResponse>> CreateFluentValidationPipelineBehaviorGenericResultAsSut(List<IValidator<FakeRequest>> validators)
    => new(validators);

  private void AssertNotSuccess(Result response, FluentValidationErrorItem[] validationErrors)
  {
    response.IsFailure.Should().BeTrue();
    response.IsSuccess.Should().BeFalse();
    response.ResultErrorItem.Code.Should().Be(Results.ValidationResult.ResultErrorItemValidationInput.Code);
    response.ResultErrorItem.Message.Should().Be(Results.ValidationResult.ResultErrorItemValidationInput.Message);
    validationErrors.Should().HaveCount(1);
    validationErrors[0].ValidationFailure.ErrorMessage.Should().Be(FakeErrorMessage);
    validationErrors[0].ValidationFailure.PropertyName.Should().Be(FakeProp);
    validationErrors[0].ValidationFailure.Severity.Should().Be(Severity.Error);
    
  }
}