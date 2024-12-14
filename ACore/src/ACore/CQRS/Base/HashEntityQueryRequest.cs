using ACore.Results;
using MediatR;

namespace ACore.CQRS.Base;

public class HashEntityQueryRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class EntityCommandRequest<TResponse>(string? sumHash): IRequest<TResponse>
  where TResponse : Result
{
  public string? SumHash => sumHash;
}
