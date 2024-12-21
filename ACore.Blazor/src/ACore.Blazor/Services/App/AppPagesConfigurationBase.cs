using System.Globalization;
using System.Resources;
using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;

namespace ACore.Blazor.Services.App;

public abstract class AppPagesConfigurationBase : IAppPagesConfiguration
{
  public abstract string AppName { get; }
  public abstract IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; }
  public abstract IEnumerable<IPageData> AllPages { get; }
  public abstract IPageData HomePage { get; }
 

  public void ApplyTranslations()
  {
    foreach (var pageData in AllPages)
    {
      if (pageData.ResX.HasValue)
        pageData.Title = GetLocalizedTitle(pageData.ResX.Value);
    }

    foreach (var menuHierarchyItem in LeftMenuHierarchy)
    {
      HierarchyRecurrence(menuHierarchyItem);
    }
  }

  public bool IsTest => false;

  private static void HierarchyRecurrence(MenuHierarchyItem menuHierarchyItem)
  {
    if (menuHierarchyItem.ResX.HasValue)
      menuHierarchyItem.Title = GetLocalizedTitle(menuHierarchyItem.ResX.Value);

    foreach (var hierarchyItem in menuHierarchyItem.Children)
    {
      HierarchyRecurrence(hierarchyItem);
    }
  }

  private static string GetLocalizedTitle((ResourceManager Type, string Name) resX)
  {
    var lcid = CultureInfo.DefaultThreadCurrentUICulture == null ? 1033 : CultureInfo.DefaultThreadCurrentUICulture.LCID;
    var translation = resX.Type.GetString(resX.Name);
    return string.IsNullOrEmpty(translation) ? 
      $"{lcid}-{resX.Name}-{resX.Name}" 
      : translation;
  }
}