using ACore.Blazor.Components.SvgIcons;
using ACore.Blazor.Services.App.Models;

namespace BlazorApp.Client.Configuration.Localization;

public static class SupportedLanguage
{
    public static IEnumerable<BlazorLanguageItem> AllSupportedLanguages { get; } = new[] {
        new BlazorLanguageItem (1033,"en-US", "English", SvgNationalFlagIcons.EnUs ),
        new BlazorLanguageItem ( 1029, "cs-CZ", "Čeština", SvgNationalFlagIcons.CsCz ) };
}