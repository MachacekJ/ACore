using ACore.Server.Configuration;
using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Services.ServerCache;
using MediatR;

namespace ACore.Server.Services.AppUser;

public interface IApp
{
  UserData CurrentUser { get; }
  ACoreServerOptions Options { get; }
  IServerCache ServerCache { get; }
  IMediator Mediator { get; }
}