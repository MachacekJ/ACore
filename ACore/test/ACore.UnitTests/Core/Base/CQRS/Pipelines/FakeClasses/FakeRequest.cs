using ACore.Models.Result;
using MediatR;

namespace ACore.UnitTests.Core.Base.CQRS.Pipelines.FakeClasses;

public class FakeRequest: IRequest<Result<FakeResponse>>
{
  
}