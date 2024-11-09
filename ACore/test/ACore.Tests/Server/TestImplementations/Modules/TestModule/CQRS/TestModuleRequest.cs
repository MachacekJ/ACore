using ACore.CQRS.Base;
using ACore.Models.Result;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class TestModuleQueryRequest<TResponse> : HashEntityQueryRequest<TResponse>
  where TResponse : Result;

public class TestModuleCommandRequest<TResponse>(string? sumHash) : EntityCommandRequest<TResponse>(sumHash)
  where TResponse : Result;