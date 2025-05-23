﻿using ACore.CQRS.Extensions;
using ACore.CQRS.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Configuration.CQRS;

public static class CQRSExtensions
{
  public static void AddACoreMediatr(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(CQRSExtensions));
      c.ParallelNotification();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));
    services.AddValidatorsFromAssembly(typeof(CQRSExtensions).Assembly, includeInternalTypes: true);
  }
}