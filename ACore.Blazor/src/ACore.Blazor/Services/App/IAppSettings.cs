using System.Reflection;
using ACore.Blazor.Abstractions;
using ACore.Blazor.Services.App.Manager.Models;

namespace ACore.Blazor.Services.App;

public interface IAppSettings
{
  IEnumerable<Assembly> Assemblies { get; }
  string AppName { get; }
  IEnumerable<AppMenuItem> PageHierarchyItems { get; }
  IEnumerable<IPageConfig> AllAvailablePages { get; }
  IPageConfig HomePage { get; }
  IPageConfig NotFoundPage { get; }
  //void ApplyTranslations();

  bool IsTest { get; }
  
  bool IsDebug { get; }
}