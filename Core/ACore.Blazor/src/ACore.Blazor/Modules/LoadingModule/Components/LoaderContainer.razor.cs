using Microsoft.AspNetCore.Components;

namespace ACore.Blazor.Modules.LoadingModule.Components;

public partial class LoaderContainer : ACoreComponentBase
{

  private bool _visible = false;

  [Parameter]
  public string Text { get; set; } = string.Empty;

  [Parameter]
  public double Percent { get; set; } = 10;

  protected override void OnInitialized()
  {
    base.OnInitialized();
    LoadingAppExtension.OnStart += LoadingAppExtensionOnOnStart;
    LoadingAppExtension.OnStop += LoadingAppExtensionOnOnStop;
    LoadingAppExtension.OnChangeText += LoadingAppExtensionOnOnChangeText;

  }

  private void LoadingAppExtensionOnOnChangeText(string text)
  {
    Text = text;
    StateHasChanged();
  }

  private void LoadingAppExtensionOnOnStop()
  {
    _visible = false;
    StateHasChanged();
  }

  private void LoadingAppExtensionOnOnStart(string text)
  {
    _visible = true;
    Text = text;
    StateHasChanged();
  }

  public override void Dispose()
  {
    LoadingAppExtension.OnStart -= LoadingAppExtensionOnOnStart;
    LoadingAppExtension.OnStop -= LoadingAppExtensionOnOnStop;
    base.Dispose();
  }
}