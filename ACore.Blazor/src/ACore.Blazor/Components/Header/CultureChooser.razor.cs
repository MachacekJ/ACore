using System.Globalization;
using ACore.Blazor.Configuration;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ACore.Blazor.Components.Header;

public partial class CultureChooser : ACoreComponentBase
{
    private LanguageItem _value = ACoreBlazorAvailableLanguage.AllSupportedLanguages.First();

    private static CultureInfo Culture => CultureInfo.CurrentUICulture;

    protected override void OnInitialized()
    {
        var result = ACoreBlazorAvailableLanguage.AllSupportedLanguages.FirstOrDefault(a => a.Id == Culture.Name);
        if (result == null)
        {
            _value = ACoreBlazorAvailableLanguage.AllSupportedLanguages.First();
            return;
        }
        _value = result;
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        AppState.ShowRightMenu(RightMenuTypeEnum.Language);
    }
}