using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App.Manager.Models;
using ACore.Services.Localization.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Telerik.SvgIcons;

namespace ACore.Blazor.Components.Layouts;

public partial class Breadcrumb(IStringLocalizerFactory localizerFactory, ILogger<Breadcrumb> logger) : ACoreComponentBase
{
  private IEnumerable<BreadcrumbItem> _breadCrumbItems = new List<BreadcrumbItem>();

  protected override void OnInitialized()
  {
    base.OnInitialized();
    LocalizationAppExtension.OnLocalizationChangeAsync += Reload;
    AppManager.Page.OnPageChange += PageChange;
  }

  protected override async Task OnInitializedAsync() => await Reload();

  public override void Dispose()
  {
    base.Dispose();
    LocalizationAppExtension.OnLocalizationChangeAsync -= Reload;
    AppManager.Page.OnPageChange -= PageChange;
  }

  private Task Reload()
  {
    _breadCrumbItems = GetBreadcrumbsForPage();
    StateHasChanged();
    return Task.CompletedTask;
  }

  private async Task PageChange(IPageConfig pageConfig)
  {
    await Reload();
  }

  private IEnumerable<BreadcrumbItem> GetBreadcrumbsForPage()
  {
    var title = string.Empty;
    var page = AppManager.Page.Current;
    var homePage = AppManager.AppSettings.HomePage;

    if (homePage is { LocX: not null, TitleLocalizationKey: not null })
    {
      var localizationKey = new ACoreLocalizationKeyItem(homePage.LocX, homePage.TitleLocalizationKey);
      title = localizationKey.GetString(localizerFactory);
    }
    
    var res = new List<BreadcrumbItem>
    {
      new()
      {
        Text = title,
        Title = title,
        Icon = SvgIcon.Home,
        Url = homePage.PageId
      }
    };
    if (page.PageId == homePage.PageId)
      return res;

    var find = FindMenuItem(AppManager.AppSettings.PageHierarchyItems, page);
    if (find == null)
    {
      logger.LogError("Page id '{PageId}' and title '{title}' cannot be found in {_leftMenuFlatItems}.", page.PageId, title, nameof(AppManager.AppSettings.PageHierarchyItems));
      return res;
    }

    if (find.Url == AppManager.AppSettings.HomePage.PageId)
      return res;

    BreadcrumbsRek(find, res);

    res.Last().Disabled = true;

    return res;
  }

  private void BreadcrumbsRek(AppMenuItem item, ICollection<BreadcrumbItem> list)
  {
    if (item.Parent != null)
      BreadcrumbsRek(item.Parent, list);
    list.Add(item.ToBreadcrumbItem(localizerFactory));
  }

  private static AppMenuItem? FindMenuItem(IEnumerable<AppMenuItem> items, IPageConfig page)
  {
    return items.FirstOrDefault(i => i.Id == page.PageId);
  }
}