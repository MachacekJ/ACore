﻿using ACore.Models.Result;
using MediatR;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;