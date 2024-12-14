using ACore.Server.Modules.SecurityModule.CQRS;
using ACore.Server.Modules.SecurityModule.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ACore.Server.Modules.SecurityModule.Configuration;

public static class SecurityModuleServiceExtensions
{
  public static void AddSecurityModule(this IServiceCollection services, SecurityModuleOptions options)
  {
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SecurityModulePipelineBehavior<,>));
    services.TryAddScoped<ISecurityModule, Services.SecurityModule>();
  }
}