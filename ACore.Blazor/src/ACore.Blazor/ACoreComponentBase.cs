using ACore.Blazor.Services.App;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace ACore.Blazor;

public abstract class ACoreComponentBase : ComponentBase
{
    [Inject]
    public IMediator Mediator { get; set; } = null!;

    [Inject]
    public IAppState AppState { get; set; } = null!;

    [Inject]
    public ILogger<ACoreComponentBase> Log { get; set; } = null!;
}