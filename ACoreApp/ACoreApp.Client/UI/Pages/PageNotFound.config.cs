using ACore.Blazor.Abstractions;
using Telerik.SvgIcons;

namespace ACoreApp.Client.UI.Pages;

public class PageNotFoundConfig : IPageConfig
{
  public string PageId => "notFound";
  public string PageUrl => PageId;

  public ISvgIcon? Icon => null;

  public string? TitleLocalizationKey => null;
  public Type? LocX => null;
}