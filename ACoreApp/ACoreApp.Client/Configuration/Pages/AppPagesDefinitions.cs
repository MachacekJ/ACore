using ACore.Blazor.Abstractions;
using ACoreApp.Client.UI.Pages;
using ACoreApp.Client.UI.Pages.About;
using ACoreApp.Client.UI.Pages.Home;

namespace ACoreApp.Client.Configuration.Pages;

public static class AppPagesDefinitions
{
  public static readonly IPageConfig NotFound = new PageNotFoundConfig();
  public static readonly IPageConfig Home = new HomePageConfig();
  public static readonly IPageConfig About = new AboutPageConfig();
}