using ACore.Server.Configuration;
using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Services.ServerCache;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Services;

/// <summary>
/// It contains general information and interfaces that can be used in all parts of the application code.
/// Substitute many params in constructor for dependency injection.
/// Instance must be added in scope <see cref="ServiceCollectionServiceExtensions.AddScoped(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Type)"/> to DI.
/// </summary>
public interface IACoreServerApp
{
  UserData CurrentUser { get; }
  ACoreServerOptions Options { get; }
  IServerCache ServerCache { get; }
  IMediator Mediator { get; }
}