using System.Reflection;
using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App;
using ACore.Blazor.Services.App.Manager.Models;
using ACoreApp.Client.Configuration.Pages;

namespace ACoreApp.Client.Configuration;

public class AppSettings(IEnumerable<Assembly> assemblies) : AppSettingsBase(assemblies)
{
  public override string AppName => "ACore App";
  public override IPageConfig HomePage => AppPagesDefinitions.Home;

  public override IPageConfig NotFoundPage => AppPagesDefinitions.NotFound;

  // public override IEnumerable<IPageConfig> AllAvailablePages => PagesConfiguration.AllPages;
  public override IEnumerable<AppMenuItem> PageHierarchyItems => AppPagesHierarchyConfiguration.PageHierarchy;
}