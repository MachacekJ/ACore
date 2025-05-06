using ACore.Blazor;
using BlazorApp.Client.UI.Components.RightMenu;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp.Client.UI.Components.AppMenu;

public partial class LinkTest : ACoreComponentBase
{
    private void ShowContextMenu(MouseEventArgs e)
    {
        AppManager.RightMenu.ShowRightMenu(typeof(AccountRightMenu));
    }
}