using ACore.Blazor.Abstractions;
using Telerik.SvgIcons;

namespace ACoreApp.Client.UI.Pages.Home;

public class HomePageConfig : IPageConfig
{
  public const string Url = "/";
  public string PageUrl => Url;
  public string TitleLocalizationKey => nameof(HomePageLoc.Title);
  public string PageId => "home";

  public ISvgIcon? Icon => SvgIcon.Home;
  public Type LocX => typeof(HomePageLoc);
}