using ACore.Modules.LocalizationModule.Models;
using Telerik.SvgIcons;

namespace ACore.Blazor.Services.App.Models;
public class BlazorLanguageItem(int lcid, string name, string text, ISvgIcon icon) : LanguageItem(lcid, name)
{
    public ISvgIcon Icon => icon;
    public string Text => text;
}
