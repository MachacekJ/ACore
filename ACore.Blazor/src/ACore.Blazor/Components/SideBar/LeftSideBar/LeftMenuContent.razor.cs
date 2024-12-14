using ACore.Blazor.Abstractions;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageGet;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.LocalStorageSave;
using ACore.Blazor.Modules.LocalStorageModule.CQRS.Models;
using ACore.Blazor.Services.App.Manager.Models;
using Microsoft.Extensions.Localization;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.PanelBar.Models;

namespace ACore.Blazor.Components.SideBar.LeftSideBar;

public partial class LeftMenuContent(IStringLocalizerFactory localizerFactory) : ACoreComponentBase
{
  private List<PanelBarItem> _rootItems = [];

  private IEnumerable<object> _expandedItems = new List<PanelBarItem>();

  private List<string> _allExp = [];

  protected override void OnInitialized()
  {
    base.OnInitialized();
    AppManager.Page.OnPageChange += AppPageChange;
    LocalizationAppExtension.OnLocalizationChangeAsync += LocalizationAppLocalizationLocalizationAppChangeAsync;
  }

  public override void Dispose()
  {
    base.Dispose();
    AppManager.Page.OnPageChange -= AppPageChange;
    LocalizationAppExtension.OnLocalizationChangeAsync -= LocalizationAppLocalizationLocalizationAppChangeAsync;
  }

  protected override async Task OnInitializedAsync()
  {
    var expanded = new List<PanelBarItem>();
    var memoryExpanded = new List<string>();

    var expandedHistory = await Mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent)));
    if (expandedHistory.IsValue)
      memoryExpanded = expandedHistory.GetValue<List<string>>() ?? [];

    _expandedItems = expanded;

    LoadItems(memoryExpanded, expanded);
  }

  private void LoadItems(List<string> memoryExpanded, List<PanelBarItem> expanded)
  {
    var rootItems = AppManager.AppSettings.PageHierarchyItems.Select(menuItem => LoadPanelBarItems(menuItem, memoryExpanded, expanded)).ToList();
    _rootItems = rootItems;
  }
  
  private Task OnExpand(PanelBarExpandEventArgs item)
  {
    var id = ((PanelBarItem)item.Item).Id.ToString() ?? throw new Exception("");
    if (!_allExp.Contains(id))
      _allExp.Add(id);
    return Mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent),
      _allExp.ToList(), _allExp.GetType()));
  }

  private Task OnCollapse(PanelBarCollapseEventArgs item)
  {
    var id = ((PanelBarItem)item.Item).Id.ToString() ?? throw new Exception("");
    if (_allExp.Contains(id))
      _allExp.Remove(id);
    return Mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent),
      _allExp.ToList(), _allExp.GetType()));
  }

  private void ExpandedItemsChanged(IEnumerable<object> expandedItems)
  {
    _expandedItems = expandedItems;
  }

  private Task AppPageChange(IPageConfig pageConfig)
  {
    StateHasChanged();
    return Task.CompletedTask;
  }

  private async Task LocalizationAppLocalizationLocalizationAppChangeAsync()
  {
    await OnInitializedAsync();
    StateHasChanged();
  }

  private void SelectActivePage(PanelBarItemRenderEventArgs itemRender)
  {
    itemRender.Class = null;
    if (itemRender.Item is not PanelBarItem panelBar)
      return;

    if (panelBar.Id == null)
      return;

    if (panelBar.Id.ToString() == AppManager.Page.Current.PageId)
    {
      itemRender.Class = "k-level-selected";
    }
  }

  private PanelBarItem LoadPanelBarItems(AppMenuItem menuItem, List<string> expandedHistory, List<PanelBarItem> expanded)
  {
    var panelItem = menuItem.ToPanelBarItem(localizerFactory) ?? throw new Exception("panelItem is null");

    if (!menuItem.Children.Any())
      return panelItem;

    panelItem.Items = [];
    panelItem.HasChildren = true;
    var panelId = panelItem.Id.ToString() ?? throw new NullReferenceException("panel id is null.");
    if (expandedHistory.Contains(panelId))
    {
      expanded.Add(panelItem);
      _allExp.Add(panelId);
      panelItem.Expanded = true;
    }

    foreach (var children in menuItem.Children)
    {
      var subItem = LoadPanelBarItems(children, expandedHistory, expanded);
      panelItem.Items.Add(subItem);
    }

    return panelItem;
  }
}