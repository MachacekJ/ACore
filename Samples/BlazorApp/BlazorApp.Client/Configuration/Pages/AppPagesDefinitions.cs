using ACore.Blazor.Abstractions;
using BlazorApp.Client.UI.Pages.About;
using BlazorApp.Client.UI.Pages.Home;
using BlazorApp.Client.UI.Pages.PageNotFound;
  
namespace BlazorApp.Client.Configuration.Pages;

public static class AppPagesDefinitions
{
  public static readonly IPageConfig NotFound = new PageNotFoundConfig();
  public static readonly IPageConfig Home = new HomePageConfig();
  public static readonly IPageConfig About = new AboutPageConfig();
}