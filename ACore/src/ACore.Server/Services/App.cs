using ACore.Server.Configuration;
using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Modules.SecurityModule.Services;
using ACore.Server.Services.AppUser;
using ACore.Server.Services.ServerCache;
using MediatR;

namespace ACore.Server;

public class App(ISecurityModule currentUser, ACoreServerOptions options, IServerCache serverCache, IMediator mediator)
  : IApp
{
  public UserData CurrentUser => currentUser.CurrentUser;
  public ACoreServerOptions Options => options;
  public IServerCache ServerCache => serverCache;
  public IMediator Mediator => mediator;
}