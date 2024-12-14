using ACore.Server.Configuration;
using ACore.Server.Modules.SecurityModule.Models;
using ACore.Server.Services.Security;
using ACore.Server.Services.ServerCache;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Services;

public class ACoreServerCurrentScope(ISecurity currentUser, IOptions<ACoreServerOptions> options, IServerCache serverCache, IMediator mediator)
  : IACoreServerCurrentScope
{
  public UserData CurrentUser => currentUser.CurrentUser;
  public ACoreServerOptions Options => options.Value;
  public IServerCache ServerCache => serverCache;
  public IMediator Mediator => mediator;
}