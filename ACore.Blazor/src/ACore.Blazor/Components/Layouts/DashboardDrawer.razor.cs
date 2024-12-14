using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ACore.Blazor.Components.Layouts;

public partial class DashboardDrawer : ACoreComponentBase
{
  private TelerikDrawer<int>? _leftMenuDrawer;
  private TelerikDrawer<int>? _rightMenuDrawer;
  
  private DrawerMode _mode = DrawerMode.Push;


  private string _currentPageId = string.Empty;
  private bool _leftMenuExpanded = true;

  private Type? _rightMenuType;

  private List<int> _leftDrawerData = [0];
  private List<int> _rightDrawerData = [0];


  [Parameter] public RenderFragment? Body { get; set; }
  [Parameter] public RenderFragment? AppBar { get; set; }

  private bool RightMenuExpanded => _rightMenuType != null;

  protected override void OnInitialized()
  {
    base.OnInitialized();
    //AppManager.Page.OnPageChange += PageChange;
    AppManager.RightMenu.ShowRightMenuNotifier += ShowRightMenuNotifier;
    AppManager.RightMenu.HideRightMenuNotifier += HideRightMenuNotifier;
  }

  public override void Dispose()
  {
    base.Dispose();
    //AppManager.Page.OnPageChange -= PageChange;
    AppManager.RightMenu.ShowRightMenuNotifier -= ShowRightMenuNotifier;
    AppManager.RightMenu.HideRightMenuNotifier -= HideRightMenuNotifier;
  }

  protected override async Task OnInitializedAsync()
  {
    await PageChange(AppManager.Page.Current);
  }

  private async Task PageChange(IPageConfig pageConfig)
  {
    if (_currentPageId == pageConfig.PageId)
      return;

    _currentPageId = pageConfig.PageId;

    if (_leftMenuDrawer != null && AppManager.ResponsiveType == ResponsiveTypeEnum.Mobile)
      await _leftMenuDrawer.CollapseAsync();

    //StateHasChanged();
  }

  private async Task ToggleMenuDrawer()
  {
    if (_leftMenuDrawer != null)
    {
      if (_leftMenuDrawer.Expanded)
        await _leftMenuDrawer.CollapseAsync();
      else
        await _leftMenuDrawer.ExpandAsync();
    }
  }

  private void LeftMenuExpandedChangedHandler(bool expanded)
  {
    _leftMenuExpanded = expanded;
  }

  private async Task MediaQueryChange(bool isSmall)
  {
    AppManager.SetResponsiveType(isSmall ? ResponsiveTypeEnum.Mobile : ResponsiveTypeEnum.Desktop);
    if (AppManager.ResponsiveType == ResponsiveTypeEnum.Mobile)
    {
      _mode = DrawerMode.Overlay;
      if (_leftMenuDrawer != null)
        await _leftMenuDrawer.CollapseAsync();
    }
    else
    {
      _mode = DrawerMode.Push;
      if (_leftMenuDrawer != null)
        await _leftMenuDrawer.ExpandAsync();
    }
  }

  #region RightMenu

  private async Task ShowRightMenuNotifier(Type rightMenuType)
  {
    _rightMenuType = rightMenuType;
    if (_rightMenuDrawer != null)
      await _rightMenuDrawer.ExpandAsync();
  }

  private async Task HideRightMenuNotifier()
  {
    if (_rightMenuDrawer != null)
      await _rightMenuDrawer.CollapseAsync();
  }

  private void RightMenuExpandedChangedHandler(bool expanded)
  {
    if (!expanded)
      _rightMenuType = null;
  }

  #endregion
}