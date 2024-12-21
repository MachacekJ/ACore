using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;

namespace ACore.Blazor.Services.App;

public interface IAppPagesConfiguration
{
  string AppName { get; }
  IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; }
  IEnumerable<IPageData> AllPages { get; }
  IPageData HomePage { get; }
  void ApplyTranslations();

  bool IsTest { get; }
}