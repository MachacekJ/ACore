using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace ACore.Blazor.Services.App.Manager.Models.Actions;

public class PageActions(IAppSettings appSettings, NavigationManager? navigationManager, ILogger<AppManager> logger)
{
  /// <summary>
  /// It will be called if the page changes. e.g. the application needs to highlight a menu item.
  /// </summary>
  public event Func<IPageConfig, Task>? OnPageChange;

  /// <summary>
  /// Current displayed page.
  /// </summary>
  public IPageConfig Current { get; private set; } = appSettings.HomePage;

  //public IEnumerable<BreadcrumbItem> Breadcrumbs { get; private set; } = new List<BreadcrumbItem>();

  /// <summary>
  /// Which life cycle phase is page.
  /// </summary>
  public PageStateEnum PageState { get; private set; } = PageStateEnum.Initialize;

  /// <summary>
  /// Display other page.
  /// </summary>
  /// <param name="page"></param>
  /// <exception cref="InvalidOperationException"></exception>
  public void ChangePage(IPageConfig page)
  {
    if (navigationManager == null)
      throw new InvalidOperationException("NavigationManager is not initialized.");

    navigationManager.NavigateTo(page.PageUrl);
  }

  /// <summary>
  /// Set page according to url path, set <see cref="Current"/> and call <see cref="OnPageChange"/>
  /// </summary>
  public void SetPage(string path)
  {
    IPageConfig page;
    if (path is "" or "/")
      page = appSettings.HomePage;
    else
    {
      var url = path.ToLower();

      if (url.Contains("?"))
        url = url.Remove(url.IndexOf('?'));

      if (url == "_framework/debug/ws-proxy")
        return;

      if (url.StartsWith("/"))
        url = url.Substring(1);

      var foundPage = appSettings.AllAvailablePages.FirstOrDefault(appSettingsPage => url.StartsWith(appSettingsPage.PageId));
      if (foundPage == null)
      {
        logger.LogWarning("Page for path '{path}' has not been found.", path);
        foundPage = appSettings.NotFoundPage;
      }

      page = foundPage;
    }

    if (Current.PageId == page.PageId)
      return;

    Current = page;

    if (PageState != PageStateEnum.Rendered)
      return;

    OnPageChange?.Invoke(Current);
  }

  /// <summary>
  /// Set life page cycle phase.
  /// </summary>
  public void SetPageState(PageStateEnum pageState)
  {
    PageState = pageState;
  }
}