using System.Reflection;
using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App.Manager.Models;

namespace ACore.Blazor.Services.App;

public abstract class AppSettingsBase : IAppSettings
{
  private readonly List<IPageConfig> _allAvailablePages = [];

  public abstract string AppName { get; }
  public abstract IEnumerable<AppMenuItem> PageHierarchyItems { get; }
  public abstract IPageConfig HomePage { get; }
  public abstract IPageConfig NotFoundPage { get; }
  
  public IEnumerable<IPageConfig> AllAvailablePages => _allAvailablePages;
  public bool IsTest => false;
  public bool IsDebug => true;

  public IEnumerable<Assembly> Assemblies { get; }

  protected AppSettingsBase(IEnumerable<Assembly> assemblies)
  {
    Assemblies = assemblies;
    
    var baseType = typeof(IPageConfig);
    var allComponents = AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(s => s.GetTypes())
      .Where(p => baseType.IsAssignableFrom(p) && p is { IsInterface: false, IsAbstract: false, IsClass: true });

    foreach (var type in allComponents)
    {
      if (Activator.CreateInstance(type) is IPageConfig pageConfig)
        _allAvailablePages.Add(pageConfig);
    }
  }
}