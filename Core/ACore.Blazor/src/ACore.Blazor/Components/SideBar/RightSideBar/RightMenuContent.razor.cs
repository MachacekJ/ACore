using Microsoft.AspNetCore.Components;

namespace ACore.Blazor.Components.SideBar.RightSideBar;

public partial class RightMenuContent : ACoreComponentBase
{
  protected override bool LocalizationEnabled => true;
  
  [Parameter] public required Type? RightMenuType { get; set; }

  private RenderFragment? _menuComponent;

  protected override void OnInitialized()
  {
    base.OnInitialized();
    _menuComponent = (builder) =>
    {
      if (RightMenuType == null)
        return;
      
      builder.OpenComponent(0, RightMenuType);
      builder.CloseComponent();
    };
  }
}