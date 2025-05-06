using ACore.Blazor.Abstractions;
using ACore.Services.Localization.Models;
using Microsoft.Extensions.Localization;
using Telerik.Blazor.Components.PanelBar.Models;
using Telerik.SvgIcons;

namespace ACore.Blazor.Services.App.Manager.Models;

/// <summary>
/// The items are part of the application's page hierarchy tree.
/// Properties are used for app menu and breadcrumb displaying.
/// </summary>
public class AppMenuItem
{
  public ACoreLocalizationKeyItem? LocalizationKey { get; }

  public string Id { get; }

  // public string Title { get; set; }
  public string? Url { get; }
  public ISvgIcon? Icon { get; }
  public IEnumerable<AppMenuItem> Children { get; } = Array.Empty<AppMenuItem>();
  public AppMenuItem? Parent { get; set; }

  public AppMenuItem(IPageConfig page)
  {
    Id = page.PageId;
    // Title = page.GetTitle();
    if (page is { LocX: not null, TitleLocalizationKey: not null })
      LocalizationKey = new ACoreLocalizationKeyItem(page.LocX, page.TitleLocalizationKey);

    var url = "/";

    if (!string.IsNullOrEmpty(page.PageUrl))
      if (!page.PageUrl.StartsWith("/"))
        url += page.PageUrl;

    Url = url;
    Icon = page.Icon;
  }

  public AppMenuItem(IPageConfig page, IEnumerable<AppMenuItem> children) : this(page)
  {
    Children = children;
  }
}

public static class PageMenuItemExtension
{
  public static PanelBarItem ToPanelBarItem(this AppMenuItem appMenuItem, IStringLocalizerFactory factory)
  {
    var title = appMenuItem.Id;
    if (appMenuItem.LocalizationKey != null)
      title = appMenuItem.LocalizationKey.GetString(factory);

    return new PanelBarItem
    {
      Id = appMenuItem.Id,
      Items = [],
      Text = title,
      Icon = appMenuItem.Icon,
      Url = appMenuItem.Url
    };
  }

  public static BreadcrumbItem ToBreadcrumbItem(this AppMenuItem appMenuItem, IStringLocalizerFactory factory)
  {
    var title = appMenuItem.Id;
    if (appMenuItem.LocalizationKey != null)
      title = appMenuItem.LocalizationKey.GetString(factory);
    return new BreadcrumbItem
    {
      Text = title,
      Title = title,
      Icon = appMenuItem.Icon,
      Url = appMenuItem.Url ?? string.Empty
    };
  }
}