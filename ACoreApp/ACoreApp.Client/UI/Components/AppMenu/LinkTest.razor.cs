using ACore.Blazor;
using ACoreApp.Client.UI.Components.RightMenu;
using Microsoft.AspNetCore.Components.Web;

namespace ACoreApp.Client.UI.Components.AppMenu;

public partial class LinkTest : ACoreComponentBase
{
    private void ShowContextMenu(MouseEventArgs e)
    {
        AppManager.RightMenu.ShowRightMenu(typeof(AccountRightMenu));
    }
}