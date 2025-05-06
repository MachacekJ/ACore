using ACore.CQRS.Base;
using ACore.Results;
using MediatR;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS;

public class Fake1Request<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class Fake1QueryRequest<TResponse> : HashEntityQueryRequest<TResponse>
  where TResponse : Result;

public class Fake1CommandRequest<TResponse>(string? sumHash) : EntityCommandRequest<TResponse>(sumHash)
  where TResponse : Result;