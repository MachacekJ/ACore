using ACore.Blazor.Abstractions;
using Telerik.SvgIcons;

namespace BlazorApp.Client.UI.Pages.PageNotFound;

public class PageNotFoundConfig : IPageConfig
{
  public const string Url = "/notfound";
  public string PageId => "notFound";
  public string PageUrl => PageId;

  public ISvgIcon? Icon => null;

  public string? TitleLocalizationKey => null;
  public Type? LocX => null;
}