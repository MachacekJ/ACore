using ACore.Blazor.Components.Layouts;

namespace ACore.Blazor.Services.App.Manager.Models.Actions;

/// <summary>
/// General action for right menu.
/// </summary>
public class RightMenuActions
{
  /// <summary>
  /// It will be called if a visible right menu is required.
  /// Primary serves only <see cref="DashboardDrawer"/>.
  /// </summary>
  public event Func<Type, Task>? ShowRightMenuNotifier;

  /// <summary>
  /// It will be called if a hide right menu is required.
  /// Primary serves only <see cref="DashboardDrawer"/>.
  /// </summary>
  public event Func<Task>? HideRightMenuNotifier;

  /// <summary>
  /// Request to display the right menu.
  /// </summary>
  /// <param name="rightMenuType">Type of right menu component.</param>
  public void ShowRightMenu(Type rightMenuType)
  {
    ShowRightMenuNotifier?.Invoke(rightMenuType);
  }

  /// <summary>
  /// Request to hide the right menu.
  /// </summary>
  public void HideRightMenu()
  {
    HideRightMenuNotifier?.Invoke();
  }
}