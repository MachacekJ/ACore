using FluentValidation.Results;

namespace ACore.CQRS.Pipelines.Models;

public class FluentValidationErrorItem(ValidationFailure validationFailure)
{
  public ValidationFailure ValidationFailure => validationFailure;
  
  public static FluentValidationErrorItem Create(ValidationFailure vf)
  {
    return new FluentValidationErrorItem(vf);
  }
}