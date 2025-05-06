using ACore.Blazor.Services.App.Manager.Models;

namespace BlazorApp.Client.Configuration.Pages;

public static class AppPagesHierarchyConfiguration
{
  public static readonly IEnumerable<AppMenuItem> PageHierarchy = new List<AppMenuItem>
  {
    new(AppPagesDefinitions.Home),
    new(AppPagesDefinitions.About)
  };
}