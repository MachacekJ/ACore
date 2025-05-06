using ACore.Blazor.Abstractions;
using Telerik.SvgIcons;

namespace BlazorApp.Client.UI.Pages.About;

public class AboutPageConfig : IPageConfig
{
  public const string Url = "about";
  public Type LocX => typeof(AboutPageResX);
  public string PageId => "about";
  public string PageUrl => Url;
  public string? TitleLocalizationKey => nameof(AboutPageResX.PageTitle);
  public ISvgIcon? Icon => SvgIcon.InfoCircle;
}