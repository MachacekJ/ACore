using ACore.Blazor.Services.App.Manager.Extensions;

namespace ACore.Blazor.Modules.LoadingModule;

// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Is registered in <see cref="Services.App.Manager.AppManager.RegisterAllAppExtensions"/>
/// </summary>
public class LoadingAppExtension : IAppExtension
{
  public event Action<string>? OnStart;
  public event Action? OnStop;
  public event Action<string>? OnChangeText;
  public void Start(string text)
  {
    OnStart?.Invoke(text);
  }

  public void Text(string text)
  {
    OnChangeText?.Invoke(text);
  }

  public void Stop()
  {
    OnStop?.Invoke();
  }
}