using ACore.CQRS.Base;
using ACore.Models.Result;
using MediatR;

namespace SampleServerPackage.ToDoModulePG.CQRS;

public class ToDoModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class ToDoModuleQueryRequest<TResponse> : HashEntityQueryRequest<TResponse>
  where TResponse : Result;

public class ToDoModuleRequestCommandRequest<TResponse>(string? sumHash) : EntityCommandRequest<TResponse>(sumHash)
  where TResponse : Result;